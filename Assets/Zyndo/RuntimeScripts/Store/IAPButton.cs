#if (UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS) && UNITY_PURCHASING
// You must obfuscate your secrets using Window > Unity IAP > Receipt Validation Obfuscator
// before receipt validation will compile in this sample.
// #define RECEIPT_VALIDATION



#endif
#if RECEIPT_VALIDATION
using UnityEngine.Purchasing.Security;
#endif

#if UNITY_PURCHASING
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

namespace Zyndo.Store
{
	[RequireComponent (typeof (Button))]
	public class IAPButton : MonoBehaviour
	{
		public enum ButtonType
		{
			Purchase,
			Restore
		}

		[System.Serializable]
		public class OnPurchaseCompletedEvent : UnityEvent<Product> {};

		[System.Serializable]
		public class OnPurchaseFailedEvent : UnityEvent<Product, PurchaseFailureReason> {};

		[HideInInspector]
		public string productId;

		[Tooltip("The type of this button, can be either a purchase or a restore button")]
		public ButtonType buttonType = ButtonType.Purchase;

		[Tooltip("Consume the product immediately after a successful purchase")]
		public bool consumePurchase = true;

		[Tooltip("Event fired after a successful purchase of this product")]
		public OnPurchaseCompletedEvent onPurchaseComplete;

		[Tooltip("Event fired after a failed purchase of this product")]
		public OnPurchaseFailedEvent onPurchaseFailed;

		[Tooltip("[Optional] Displays the localized title from the app store")]
		public Text titleText;

		[Tooltip("[Optional] Displays the localized description from the app store")]
		public Text descriptionText;

		[Tooltip("[Optional] Displays the localized price from the app store")]
		public Text priceText;

		public virtual void Start ()
		{
			Button button = GetComponent<Button>();

			if (buttonType == ButtonType.Purchase) {
				if (button) {
					button.onClick.AddListener(PurchaseProduct);
				}

				if (string.IsNullOrEmpty(productId)) {
					Debug.LogError("IAPButton productId is empty");
				}

				if (!IAPButtonStoreManager.Instance.HasProductInCatalog(productId)) {
					Debug.LogWarning("The product catalog has no product with the ID \"" + productId + "\"");
				}
			} else if (buttonType == ButtonType.Restore) {
				if (button) {
					button.onClick.AddListener(Restore);
				}
			}
		}

		void OnEnable()
		{
			if (buttonType == ButtonType.Purchase) {
				IAPButtonStoreManager.Instance.AddButton(this);
				UpdateText();
			}
		}

		void OnDisable()
		{
			if (buttonType == ButtonType.Purchase) {
				IAPButtonStoreManager.Instance.RemoveButton(this);
			}
		}

		void PurchaseProduct()
		{
			if (buttonType == ButtonType.Purchase) {
				Debug.Log("IAPButton.PurchaseProduct() with product ID: " + productId);

				IAPButtonStoreManager.Instance.InitiatePurchase(productId);
			}
		}

		void Restore()
		{
			if (buttonType == ButtonType.Restore) {
				if (Application.platform == RuntimePlatform.WSAPlayerX86 || Application.platform == RuntimePlatform.WSAPlayerX64 || Application.platform == RuntimePlatform.WSAPlayerARM) {
					IAPButtonStoreManager.Instance.ExtensionProvider.GetExtension<IMicrosoftExtensions>().RestoreTransactions();
				} else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.tvOS) {
					IAPButtonStoreManager.Instance.ExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(OnTransactionsRestored);
				} else if (Application.platform == RuntimePlatform.Android && StandardPurchasingModule.Instance().androidStore == AndroidStore.SamsungApps) {
					IAPButtonStoreManager.Instance.ExtensionProvider.GetExtension<ISamsungAppsExtensions>().RestoreTransactions(OnTransactionsRestored); 
				} else if (Application.platform == RuntimePlatform.Android && StandardPurchasingModule.Instance().androidStore == AndroidStore.CloudMoolah) {
					IAPButtonStoreManager.Instance.ExtensionProvider.GetExtension<IMoolahExtension>().RestoreTransactionID((restoreTransactionIDState) => { 
						OnTransactionsRestored(restoreTransactionIDState != RestoreTransactionIDState.RestoreFailed && restoreTransactionIDState != RestoreTransactionIDState.NotKnown);
					});
				} else {
					Debug.LogWarning(Application.platform.ToString() + " is not a supported platform for the Codeless IAP restore button");
				}
			}
		}

		void OnTransactionsRestored(bool success)
		{
			Debug.Log("Transactions restored: " + success);
		}

		/**
		 *  Invoked to process a purchase of the product associated with this button
		 */
		public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e)
		{
			Debug.Log(string.Format("IAPButton.ProcessPurchase(PurchaseEventArgs {0} - {1})", e, e.purchasedProduct.definition.id));

			onPurchaseComplete.Invoke(e.purchasedProduct);

			return (consumePurchase) ? PurchaseProcessingResult.Complete : PurchaseProcessingResult.Pending;
		}

		/**
		 *  Invoked on a failed purchase of the product associated with this button
		 */
		public void OnPurchaseFailed (Product product, PurchaseFailureReason reason)
		{
			Debug.Log(string.Format("IAPButton.OnPurchaseFailed(Product {0}, PurchaseFailureReason {1})", product, reason));

			onPurchaseFailed.Invoke(product, reason);
		}

		public virtual void UpdateText()
		{
			var product = IAPButtonStoreManager.Instance.GetProduct(productId);
			if (product != null) {
				if (titleText != null) {
					titleText.text = product.metadata.localizedTitle;
				}

				if (descriptionText != null) {
					descriptionText.text = product.metadata.localizedDescription;
				}

				if (priceText != null) {
					priceText.text = product.metadata.localizedPriceString;
				}
			}
		}

		public class IAPButtonStoreManager : IStoreListener
		{
			private static IAPButtonStoreManager instance = new IAPButtonStoreManager();
			private ProductCatalog catalog;
			private List<IAPButton> activeButtons = new List<IAPButton>();

			private IAppleExtensions m_AppleExtensions;
			protected IStoreController controller;
			protected IExtensionProvider extensions;

			private string m_LastTransationID;
			private string m_LastReceipt;
#if RECEIPT_VALIDATION
private CrossPlatformValidator validator;
#endif
			private IAPButtonStoreManager()
			{
				catalog = ProductCatalog.LoadDefaultCatalog();

				StandardPurchasingModule module = StandardPurchasingModule.Instance();
				module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

				ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);
				//This seems to be outdated/unneeded, the value should be set in unity services
				//builder.Configure<IGooglePlayConfiguration>().SetPublicKey("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA2O/9/H7jYjOsLFT/uSy3ZEk5KaNg1xx60RN7yWJaoQZ7qMeLy4hsVB3IpgMXgiYFiKELkBaUEkObiPDlCxcHnWVlhnzJBvTfeCPrYNVOOSJFZrXdotp5L0iS2NVHjnllM+HA1M0W2eSNjdYzdLmZl1bxTpXa4th+dVli9lZu7B7C2ly79i/hGTmvaClzPBNyX+Rtj7Bmo336zh2lYbRdpD5glozUq+10u91PMDPH+jqhx10eyZpiapr8dFqXl5diMiobknw9CgcjxqMTVBQHK6hS0qYKPmUDONquJn280fBs1PTeA6NMG03gb9FLESKFclcuEZtvM8ZwMMRxSLA9GwIDAQAB");

				foreach (var product in catalog.allProducts) {
					if (product.allStoreIDs.Count > 0) {
						var ids = new IDs();
						foreach (var storeID in product.allStoreIDs) {
							ids.Add(storeID.id, storeID.store);
						}
						builder.AddProduct(product.id, product.type, ids);
					} else {
						builder.AddProduct(product.id, product.type);
					}
				}		
				#if RECEIPT_VALIDATION
				validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.bundleIdentifier);
				#endif
				UnityPurchasing.Initialize (this, builder);
			}

			public static IAPButtonStoreManager Instance {
				get {
					return instance;
				}
			}

			public IStoreController StoreController {
				get {
					return controller;
				}
			}

			public IExtensionProvider ExtensionProvider {
				get {
					return extensions;
				}
			}

			public bool HasProductInCatalog(string productID)
			{
				foreach (var product in catalog.allProducts) {
					if (product.id == productID) {
						return true;
					}
				}
				return false;
			}

			public Product GetProduct(string productID)
			{
				if (controller != null) {
					return controller.products.WithID(productID);
				}
				return null;
			}

			public void AddButton(IAPButton button)
			{
				activeButtons.Add(button);
			}

			public void RemoveButton(IAPButton button)
			{
				activeButtons.Remove(button);
			}

			public void InitiatePurchase(string productID)
			{
				if (controller == null) {
					Debug.LogError("Purchase failed because Purchasing was not initialized correctly");
					return;
				}

				controller.InitiatePurchase(productID);
			}

			public void OnInitialized (IStoreController controller, IExtensionProvider extensions)
			{
				this.controller = controller;
				this.extensions = extensions;

				m_AppleExtensions = extensions.GetExtension<IAppleExtensions> ();

				// On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
				// On non-Apple platforms this will have no effect; OnDeferred will never be called.
				m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);

				foreach (var button in activeButtons) {
					button.UpdateText();
				}
			}

			private void OnDeferred(Product item)
			{
				Debug.Log("Purchase deferred: " + item.definition.id);
			}

			public void OnInitializeFailed (InitializationFailureReason error)
			{
				Debug.LogError(string.Format("Purchasing failed to initialize. Reason: {0}", error.ToString()));
			}

			public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e)
			{		
				Debug.Log("Purchase OK: " + e.purchasedProduct.definition.id);
				Debug.Log("Receipt: " + e.purchasedProduct.receipt);

				m_LastTransationID = e.purchasedProduct.transactionID;
				m_LastReceipt = e.purchasedProduct.receipt;

				#if RECEIPT_VALIDATION
				// Local validation is available for GooglePlay and Apple stores
				if (m_IsGooglePlayStoreSelected ||
				Application.platform == RuntimePlatform.IPhonePlayer ||
				Application.platform == RuntimePlatform.OSXPlayer ||
				Application.platform == RuntimePlatform.tvOS) {
					try {
						var result = validator.Validate(e.purchasedProduct.receipt);
						Debug.Log("Receipt is valid. Contents:");
						foreach (IPurchaseReceipt productReceipt in result) {
							Debug.Log(productReceipt.productID);
							Debug.Log(productReceipt.purchaseDate);
							Debug.Log(productReceipt.transactionID);

							GooglePlayReceipt google = productReceipt as GooglePlayReceipt;
							if (null != google) {
								Debug.Log(google.purchaseState);
								Debug.Log(google.purchaseToken);
							}

							AppleInAppPurchaseReceipt apple = productReceipt as AppleInAppPurchaseReceipt;
							if (null != apple) {
								Debug.Log(apple.originalTransactionIdentifier);
								Debug.Log(apple.subscriptionExpirationDate);
								Debug.Log(apple.cancellationDate);
								Debug.Log(apple.quantity);
							}
						}
					} catch (IAPSecurityException) {
					Debug.Log("Invalid receipt, not unlocking content");
					return PurchaseProcessingResult.Complete;
					}
				}
				#endif
				
				foreach (var button in activeButtons) {
					if (button.productId == e.purchasedProduct.definition.id) {
						return button.ProcessPurchase(e);
					}
				}		

				return PurchaseProcessingResult.Complete; // TODO: Maybe this shouldn't return complete
			}

			public void OnPurchaseFailed (Product product, PurchaseFailureReason reason)
			{ 
				foreach (var button in activeButtons) {
					if (button.productId == product.definition.id) {
						button.OnPurchaseFailed(product, reason);
					}
				} 
			}
		}
	}
}
#endif
