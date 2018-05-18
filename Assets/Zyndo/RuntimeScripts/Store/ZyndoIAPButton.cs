using UnityEngine;
using System.Collections;
using TMPro;

#if UNITY_PURCHASING
using UnityEngine.Purchasing;
using Zyndo.Store;

public class ZyndoIAPButton : Zyndo.Store.IAPButton {
	[Tooltip("[Optional] Displays the localized title from the app store")]
	public TextMeshProUGUI TitleTMP;

	[Tooltip("[Optional] Displays the localized description from the app store")]
	public TextMeshProUGUI DescriptionTMP;

	[Tooltip("[Optional] Displays the localized price from the app store")]
	public TextMeshProUGUI PriceTMP;

	[Tooltip("[Optional] Changes the text on the localized text shown in priceTMP")]
	public string PriceString = "Buy for {0}";
	public override void Start()
	{
		base.Start ();
		#if !UNITY_IOS
		if (buttonType == ButtonType.Restore) {
			transform.parent.gameObject.SetActive (false);
		}
		#endif
	}

	public override void UpdateText()
	{
		var product = IAPButtonStoreManager.Instance.GetProduct(productId);
		if (product != null) {
			if (TitleTMP != null) {
				TitleTMP.text = product.metadata.localizedTitle;
			}

			if (DescriptionTMP != null) {
				DescriptionTMP.text = product.metadata.localizedDescription;
			}

			if (PriceTMP != null) {
				if (PriceString != null)
				{
					PriceTMP.text = string.Format(PriceString, product.metadata.localizedPriceString);
				}
				else
				{
					PriceTMP.text = product.metadata.localizedPriceString;
				}
			}
		}
	}
}
#endif