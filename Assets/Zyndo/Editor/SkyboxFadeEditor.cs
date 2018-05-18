using UnityEngine;
using System.Collections;
using System;
using UnityEditor;
public static class SkyboxMenuOptions
{	
	/*[MenuItem ("Zyndo/Skybox/Activate Auto Skyboxes", true, 1)]
	public static bool ShowActivateAutoSkyboxes()
	{
		return EditorPrefs.GetInt ("AutoSkyboxChange") == 0;
	}	*/

	[MenuItem ("Zyndo/Skybox/Activate Auto Skyboxes", false, 5045)]
	public static void ActivateAutoSkyboxes()
	{
		EditorPrefs.SetInt ("AutoSkyboxChange", 1);
	}	
	/*
	[MenuItem ("Zyndo/Skybox/Activate Auto Skyboxes", true, 1)]
	public static bool ShowDeactivateAutoSkyboxes()
	{
		return EditorPrefs.GetInt ("AutoSkyboxChange") == 1;
	}*/

	[MenuItem ("Zyndo/Skybox/Deactivate Auto Skyboxes", false, 5046)]
	public static void DeactivateAutoSkyboxes()
	{
		EditorPrefs.SetInt ("AutoSkyboxChange", 0);
	}
}

[CustomEditor(typeof(SkyboxFade))]
public class SkyboxFadeEditor : Editor {

	public override void OnInspectorGUI()
	{
		SkyboxFade fade = (SkyboxFade) target;
		serializedObject.Update ();
		if (fade.UseAmbientColor) {
			DrawDefaultInspector ();
		} else {
			string[] exlcudeVars = new string[1];
			exlcudeVars [0] = "AmbientColor";
			DrawPropertiesExcluding(serializedObject, exlcudeVars);

		}
		if (GUILayout.Button("Set Scene Skybox"))
		{
			fade.SetToSkybox ();
		}		
		if (GUILayout.Button("Set As Starting Skybox"))
		{
			if (EditorUtility.DisplayDialog("Set As Starting Skybox", "This will set the starting skybox to the skybox attatched to this Skybox Fade. Are you sure you want to make this change?", "Okay", "Cancel"))
			{
				fade.SetAsStartingSkybox ();
			}
		}
		serializedObject.ApplyModifiedProperties ();
	}
}