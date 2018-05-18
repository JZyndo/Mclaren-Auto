#if UNITY_PURCHASING
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityEngine.Purchasing
{
	public static class IAPButtonMenu
	{
		[MenuItem ("Zyndo/Store/Create IAP Button", false, 5050)]
		public static void CreateZyndoIAPButton()
		{
			// Create Button
			EditorApplication.ExecuteMenuItem("GameObject/UI/Button");

			// Get GameObject of Button
			GameObject gO = Selection.activeGameObject;

			// Add IAP Button component to GameObject
			ZyndoIAPButton iapButton = null;
			if (gO) {
				iapButton = gO.AddComponent<ZyndoIAPButton>();
			}

			if (iapButton != null) {
				UnityEditorInternal.ComponentUtility.MoveComponentUp(iapButton);
				UnityEditorInternal.ComponentUtility.MoveComponentUp(iapButton);
				UnityEditorInternal.ComponentUtility.MoveComponentUp(iapButton);
			}
		}

		[MenuItem ("Zyndo/Store/Create Or Update Store From Catalog", false, 5051)]
		public static void CreateStore()
		{
			GameObject controlsRoot = GameObject.Find ("Controls");
			GameObject storeRoot;

			ProductCatalog catalog = ProductCatalog.LoadDefaultCatalog();

			if (controlsRoot.transform.Find ("Store") == null) {
				GameObject storePrefab = AssetDatabase.LoadAssetAtPath ("Assets/Zyndo/Prefabs/Store/Store.prefab", typeof(GameObject)) as GameObject;
				//create the root
				storeRoot = GameObject.Instantiate (storePrefab, controlsRoot.transform) as GameObject;
				storeRoot.transform.SetAsLastSibling ();
				storeRoot.name = "Store";
				RectTransform rectTransform = (RectTransform)storeRoot.transform;
				rectTransform.offsetMin = new Vector2 (0, 0);
				rectTransform.offsetMax = new Vector2 (0, 0);
				/*if (EditorUtility.DisplayDialog ("Create Store From Catalog", string.Format("A store hasn't existed in this scene thus far, do you want to automatically connect into into the NavBar?."), "Okay", "No"))
				{

					ToolbarNavigation toolbarNav = controlsRoot.transform.GetComponentInChildren<ToolbarNavigation>(true);
					if (toolbarNav != null)
					{
						//Create Menu Item

						GameObject storeNavBarPF = AssetDatabase.LoadAssetAtPath("Assets/Zyndo/Prefabs/Store/StoreNavBar.prefab", typeof(GameObject)) as GameObject;
						GameObject storeNavBarItem = GameObject.Instantiate(storeNavBarPF);
						Toggle toggle = storeNavBarItem.GetComponent<Toggle> ();
						Text text = storeNavBarItem.GetComponent<Text> ();
						text.text = "Store";
						storeNavBarItem.name = "Store";
						UnityEngine.Events.UnityAction action1 = (val) => { toolbarNav.SetActive(storeRoot); };

						toggle.onValueChanged.AddListener (action1);
						//Add to NavBar
						storeNavBarItem.transform.SetParent(toolbarNav.transform, false);
						storeNavBarItem.transform.SetSiblingIndex (toolbarNav.transform.childCount - 2);
						toolbarNav.AddToolBarMember (storeNavBarItem, toolbarNav.toolbarMembers.Length - 2);
					}
				}*/
			} 
			else 
			{
				storeRoot = controlsRoot.transform.Find ("Store").gameObject;
			}
			storeRoot.SetActive (true);
			GameObject contentRoot = GameObject.Find ("Content");
			int numProducts = catalog.allProducts.Count;
			int newItem = 0;
			int catID = 0;
			foreach (var product in catalog.allProducts)
			{			
				catID++;
				string updatedText = String.Format ("{0}/{1} Products Complete", catID,  numProducts);
				if (EditorUtility.DisplayCancelableProgressBar ("Progress", updatedText, (float)catID / numProducts))
				{
					EditorUtility.ClearProgressBar ();
					return;				
				}

				if (contentRoot.transform.Find("StoreItem_" + product.id) == null)
				{
					GameObject storeItemPF = AssetDatabase.LoadAssetAtPath("Assets/Zyndo/Prefabs/Store/StoreItem.prefab", typeof(GameObject)) as GameObject;
					GameObject storeItem = GameObject.Instantiate (storeItemPF, contentRoot.transform) as GameObject;
					storeItem.name = "StoreItem_" + product.id;
					ZyndoIAPButton zyndoIAPButton = storeItem.GetComponentInChildren<ZyndoIAPButton> ();
					zyndoIAPButton.productId = product.id;

					PurchaseAccessToScene purchaseAccessToScene = storeItem.GetComponentInChildren<PurchaseAccessToScene> ();
					if (purchaseAccessToScene != null)
					{
						purchaseAccessToScene.SceneToBuy = "Need To Fill In";
					}
					newItem++;
				}
			}
			Transform restoreGO = contentRoot.transform.Find ("StoreItemRestore");
			if (restoreGO == null)
			{				
				GameObject restorePF = AssetDatabase.LoadAssetAtPath ("Assets/Zyndo/Prefabs/Store/StoreItemRestore.prefab", typeof(GameObject)) as GameObject;
				//create the root
				restoreGO =( GameObject.Instantiate (restorePF, contentRoot.transform) as GameObject).transform;
				restoreGO.gameObject.name = "StoreItemRestore";
			}
			restoreGO.SetAsLastSibling ();

			storeRoot.SetActive (false);

			EditorUtility.ClearProgressBar ();
			EditorUtility.DisplayDialog ("Create Store From Catalog", string.Format("Store created with {0} StoreItem(s) created, please goto each button and set SceneToBuy on the PurchaseAccessToScene component.", newItem), "Okay");
		}
	}


	[CustomEditor(typeof(ZyndoIAPButton))]
	[CanEditMultipleObjects]
	public class ZyndoIAPButtonEditor : Editor 
	{
		private static readonly string[] excludedFields = new string[] { "m_Script", "titleText", "descriptionText", "priceText" };
		private static readonly string[] restoreButtonExcludedFields = new string[] { "m_Script", "consumePurchase", "onPurchaseComplete", "onPurchaseFailed", "titleText", "descriptionText", "priceText", "titleTMP", "descriptionTMP", "priceTMP"  };
		private const string kNoProduct = "<None>";

		private List<string> m_ValidIDs = new List<string>();
		private SerializedProperty m_ProductIDProperty;

		public void OnEnable()
		{
			m_ProductIDProperty = serializedObject.FindProperty("productId");
		}

		public override void OnInspectorGUI()
		{
			ZyndoIAPButton button = (ZyndoIAPButton)target;

			serializedObject.Update();

			if (button.buttonType == Zyndo.Store.IAPButton.ButtonType.Purchase) {
				EditorGUILayout.LabelField(new GUIContent("Product ID:", "Select a product from the IAP catalog"));

				var catalog = ProductCatalog.LoadDefaultCatalog();

				m_ValidIDs.Clear();
				m_ValidIDs.Add(kNoProduct);
				foreach (var product in catalog.allProducts) {
					m_ValidIDs.Add(product.id);
				}

				int currentIndex = string.IsNullOrEmpty(button.productId) ? 0 : m_ValidIDs.IndexOf(button.productId);
				int newIndex = EditorGUILayout.Popup(currentIndex, m_ValidIDs.ToArray());
				if (newIndex > 0 && newIndex < m_ValidIDs.Count) {
					m_ProductIDProperty.stringValue = m_ValidIDs[newIndex];
				} else {
					m_ProductIDProperty.stringValue = string.Empty;
				}

				if (GUILayout.Button("IAP Catalog...")) {
					ProductCatalogEditor.ShowWindow();
				}
			}

			DrawPropertiesExcluding(serializedObject, button.buttonType == Zyndo.Store.IAPButton.ButtonType.Restore ? restoreButtonExcludedFields : excludedFields);

			serializedObject.ApplyModifiedProperties();
		}
	}
}
#endif
