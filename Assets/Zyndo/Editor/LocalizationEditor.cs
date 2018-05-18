using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Localization))]
public class LocalizationEditor : Editor {
	int selected = 0;
	List<string> langs = new List<string>();
	public override void OnInspectorGUI()
	{
		Localization localization = (Localization) target;
		UpdateList (localization);
		try {
			serializedObject.Update ();
			DrawDefaultInspector ();
			int newSelection = EditorGUILayout.Popup ("Language", selected, langs.ToArray());
			string tempLang = langs[newSelection]; 
			if (newSelection != selected)
			{
				if (langs.Contains(localization.selectedLang))
				{
					if (EditorUtility.DisplayDialog("Change Localization", string.Format("This will save the {0} data, and load the {1} data.", localization.selectedLang, tempLang), "Okay", "Cancel"))
					{
						Localization.CreateLocalizationFile (localization.selectedLang);
						localization.selectedLang = tempLang;
						localization.SetLocalText ();
						selected = newSelection;
					}
				}
				else
				{
					if (EditorUtility.DisplayDialog("Change Localization", string.Format("This will NOT SAVE the {0} data (as this language is no longer in the localization file), but will OVERWRITE local data with the {1} data.", localization.selectedLang, tempLang), "Okay", "Cancel"))
					{
						localization.selectedLang = tempLang;
						localization.SetLocalText ();
						selected = newSelection;
					}
				}
			}
			if (langs.Contains(localization.selectedLang) && GUILayout.Button("Save Language"))
			{
				Localization.CreateLocalizationFile (localization.selectedLang);
			}	
		}
		catch
		{
			
		}		

		if (langs.Contains(localization.selectedLang) && GUILayout.Button("Load Language") && EditorUtility.DisplayDialog("Load Language", "This will overwrite all text in your scene, overwriting any unsaved changes. Are you sure?", "Okay", "Cancel"))
		{
			if(!langs.Contains(localization.selectedLang))
			{	
				localization.selectedLang = "English";
			}
			localization.SetLocalText ();
			langs = localization.GetLanguages ();
			SetSelectedFromCulture (localization.selectedLang);
		}

		serializedObject.ApplyModifiedProperties ();
	}

	public void SetSelectedFromCulture(string lang)
	{
		selected = langs.FindIndex ((val) => val == lang);
	}

	public void UpdateList(Localization loc)
	{
		langs = loc.GetLanguages ();
		SetSelectedFromCulture (loc.selectedLang);
	}
}