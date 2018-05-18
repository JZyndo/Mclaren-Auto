using UnityEngine;
using UnityEditor;
using System;
using NUnit.Framework;

public class UpdateImportSettingsWindow : EditorWindow { 
	ResolutionOptions selectedRes = ResolutionOptions.NEXT_SMALLEST;
	Platform selectedPlatform = Platform.iPhone_Android;

	bool usePackingTags = true;
	bool useAssetBundles = false;

	[MenuItem("Zyndo/Update Image Import Settings", false, 6010)]
	static void UpdateImportSettings()
	{
		UpdateImportSettingsWindow window = ScriptableObject.CreateInstance<UpdateImportSettingsWindow>();
		window.position = new Rect(Screen.width / 2, Screen.height / 2, 400, 0);
		window.ShowUtility();
	}

	void OnGUI()
	{	     
		GUILayout.Label ("Image Import Settings", EditorStyles.boldLabel);

		//Platform[] opt2 = (ResolutionOptions[]) Enum.GetValues(typeof(Platform));
		selectedPlatform = (Platform) EditorGUILayout.EnumPopup ("Platform", selectedPlatform);

		usePackingTags = EditorGUILayout.Toggle ("Packing Tags?", usePackingTags);
		//useAssetBundles = EditorGUILayout.Toggle ("Asset Bundles?", useAssetBundles);

		//ResolutionOptions[] opt = (ResolutionOptions[]) Enum.GetValues(typeof(ResolutionOptions));
		selectedRes = (ResolutionOptions) EditorGUILayout.EnumPopup ("Max Res", selectedRes);

		GUILayout.Space(70);
		if (GUILayout.Button ("Update")) {
			this.Close ();
			ActuallyUpdateImages ();
		}
	}

	public void ActuallyUpdateImages()
	{
		PanelEditor.UpdateImportSettings(true, selectedRes, usePackingTags, useAssetBundles, selectedPlatform);
	}
}