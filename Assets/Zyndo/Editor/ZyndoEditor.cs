using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System;
using TMPro;
using Colorful;
using BlendModes;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Jacovone;

public class ZyndoEditor : MonoBehaviour
{
	[MenuItem("Zyndo/Initialize", false, 1)]
	static void ZyndoInitialize()
	{
		Debug.Log("initializing Zyndo...");
		if (!GameObject.Find("Main"))
		{
			if (Camera.main != null)
				DestroyImmediate(Camera.main.gameObject);
			var start_obj = AssetDatabase.LoadAssetAtPath("Assets/Zyndo/Prefabs/Start.prefab", typeof(GameObject)) as GameObject;
			Instantiate(start_obj).name = "Main";
			var skybox_mat = AssetDatabase.LoadAssetAtPath("Assets/Zyndo/Materials/BlendableSkybox.mat", typeof(Material)) as Material;
			RenderSettings.skybox = skybox_mat;
			//create resources folder if not already created
			var dir_path = "Assets/Resources/";
			if (!AssetDatabase.IsValidFolder("Assets/Resources"))
				AssetDatabase.CreateFolder("Assets", "Resources");
			if (!AssetDatabase.IsValidFolder(dir_path + "Mixers"))
				AssetDatabase.CreateFolder("Assets/Resources", "Mixers");
			if (!AssetDatabase.IsValidFolder(dir_path + "Animations"))
				AssetDatabase.CreateFolder("Assets/Resources", "Animations");
			//if (!AssetDatabase.IsValidFolder(dir_path + "Images"))
			//    AssetDatabase.CreateFolder("Assets/Resources", "Images");

			//create some organizational folders
			if (!AssetDatabase.IsValidFolder("Assets/PSD"))
				AssetDatabase.CreateFolder("Assets", "PSD");
			if (!AssetDatabase.IsValidFolder("Assets/Audio"))
				AssetDatabase.CreateFolder("Assets", "Audio");
			if (!AssetDatabase.IsValidFolder("Assets/Materials"))
				AssetDatabase.CreateFolder("Assets", "Materials");
			if (!AssetDatabase.IsValidFolder("Assets/Models"))
				AssetDatabase.CreateFolder("Assets", "Models");
			if (!AssetDatabase.IsValidFolder("Assets/Images"))
				AssetDatabase.CreateFolder("Assets", "Images");
			//create a new language file
			CreateNewLanguageFile();
		}
		else
			Debug.Log("A Main object already exists. Delete it manually before running Initialize again.");

	}

	[MenuItem("Zyndo/Add Dialogue %#q", false, 4050)]
	static void AddDialogue()
	{
		Debug.Log("adding Dialogue...");
		GameObject dialoguePF = AssetDatabase.LoadAssetAtPath("Assets/Zyndo/Prefabs/Dialogue.prefab", typeof(GameObject)) as GameObject;
		for (int i = 0; i < Selection.gameObjects.Count(); i++) {

			GameObject panel = GameObject.Instantiate(dialoguePF) as GameObject;
			panel.transform.SetParent(Selection.gameObjects [i].transform);
			panel.name = "Dialogue";
			PathMagic pm = panel.transform.parent.GetComponentInChildren<PathMagic> ();
			if (pm != null)
			{
				panel.transform.SetSiblingIndex (pm.transform.GetSiblingIndex());
			}

			var dialogue = panel.GetComponent<DialogueText>();
			dialogue.internal_id = dialogue.SetInternalID ();

			Transform[] rectTrans = panel.GetComponentsInChildren<RectTransform> ();
			foreach(Transform rect in rectTrans)
			{
				rect.localPosition = Vector3.zero;
				rect.localScale = Vector3.one;
			}
		}
	}

	[MenuItem("Zyndo/Add Button", false, 5035)]
	static void AddVRReadyButton()
	{
		Debug.Log("adding Button...");
		GameObject dialoguePF = AssetDatabase.LoadAssetAtPath("Assets/Zyndo/Prefabs/BasicButton.prefab", typeof(GameObject)) as GameObject;
		for (int i = 0; i < Selection.gameObjects.Count(); i++) {
			GameObject panel = GameObject.Instantiate(dialoguePF) as GameObject;
			panel.transform.SetParent(Selection.gameObjects [i].transform);
			panel.name = "Button";
			panel.transform.localScale = Vector3.one;
			panel.transform.position = Vector3.zero;
		}
	}

	[MenuItem("Zyndo/Add Scene Transition On Last Page", false, 5036)]
	static void AddSceneTransitionOnLastPage()
	{
		GameObject pagesGO = GameObject.Find ("Pages");
		int childCount = pagesGO.transform.childCount;
		if (childCount >= 1)
		{
			SceneTransition sceneTransition = pagesGO.transform.GetChild(childCount - 1).GetComponent<SceneTransition> ();
			if (sceneTransition == null)
			{
				sceneTransition = pagesGO.transform.GetChild (childCount - 1).gameObject.AddComponent<SceneTransition> ();
				sceneTransition.linkedEvent = LinkedEvent.OnTopRank;
			}
		}
	}

	[MenuItem("Zyndo/Create or Update New Language File", false, 5040)]
	static void CreateNewLanguageFile()
	{
		Debug.Log("Create New Language File...");
		if (!AssetDatabase.IsValidFolder("Assets/Resources"))
			AssetDatabase.CreateFolder("Assets", "Resources");
		Localization.CreateLocalizationFile();
	}

	[MenuItem("Zyndo/Create Ordered Network", false, 2055)]
	static void CreateOrderedNetwork()
	{
		var orderedObj = Selection.gameObjects.OrderBy(x => x.transform.GetSiblingIndex()).ToList();
		for (int i = 0; i < orderedObj.Count - 1; i++)
		{
			//iterate through hiearchy and hook up the next pages
			var page = orderedObj[i].GetComponent<Page>();
			var nextPage = orderedObj[i + 1].GetComponent<Page>();
			if (page != null && nextPage != null)
			{
				page.connectedPages = new List<PageConnection>();
				page.connectedPages.Add(new PageConnection(nextPage));
			}

		}
	}

	[MenuItem("Zyndo/Create Pages from PSD", false, 3025)]
	static void PanelFromPSD()
	{
		Debug.Log("creating panels...");
		if (!AssetDatabase.IsValidFolder("Assets/Images"))
			AssetDatabase.CreateFolder("Assets", "Images");
		PanelEditor.CreatePanelsFromPSD();

	}

	[MenuItem("Zyndo/Create Page Preview %g", false, 3030)]
	static void CreatePagePreviews()
	{
		Camera.main.GetComponent<ThumbnailCreator>().DoCapture();
	}


	//[MenuItem("Zyndo/Force Update Pages")]
	static void ForceUpdatePanels()
	{
		Debug.Log("updating panels...");
		if (!AssetDatabase.IsValidFolder("Assets/Images"))
			AssetDatabase.CreateFolder("Assets", "Images");
		PanelEditor.UpdatePanels(true);

	}
	[MenuItem("Zyndo/Go To Selected Page %#a", false, 3035)]
	static void GoToSelectedPage()
	{
		var pageRoot = GameObject.Find("Pages");
		var pages = pageRoot.GetComponentsInChildren<Page>();
		var obj = Selection.activeGameObject;
		var pivot = GameObject.Find("Pivot");
		for (int i = 0; i < pages.Length; i++)
		{
			if (pages[i].gameObject == obj)
			{
				if (Application.isPlaying)
				{
					SessionManager.instance.GoToPage(i);
				}

				Vector3 position = SceneView.lastActiveSceneView.pivot;
				position.z = 0;
				SceneView.lastActiveSceneView.LookAt(obj.transform.position, obj.transform.rotation, 1.5f, false);
				pivot.transform.position = obj.transform.position;
				pivot.transform.localRotation = obj.transform.rotation;
				//SceneView.lastActiveSceneView.LookAtDirect(obj.transform.position, obj.transform.rotation);

				break;
			}
		}
		SceneView.lastActiveSceneView.Repaint();
		UnityEditorInternal.InternalEditorUtility.RepaintAllViews ();
	}

	[MenuItem("Zyndo/Import Audio", false, 4040)]
	static void ImportAudio()
	{
		Debug.Log("import audio...");
		//PanelEditor.CreatePanelsFromPSD();
		AudioEditor.AddAudio();
	}

	[MenuItem("Zyndo/Initialize Camera Paths", false, 2060)]
	static void InitializeCameraPaths()
	{
		var pageRoot = GameObject.Find("Pages");
		if (pageRoot != null) {
			var pages = pageRoot.GetComponentsInChildren<Page> ();
			for (int i = 0; i < pages.Length - 1; i++) {
				for (int j = 0; j < pages [i].connectedPages.Count; j++) {
					var name = "CameraPath_" + j;
					var test = pages [i].transform.Find (name);
					if (test == null) {
						//add a camera path rig
						var camPath = new GameObject ("CameraPath_" + j);
						camPath.transform.position = pages [i].transform.position;
						camPath.transform.parent = pages [i].transform;
						var bSpline = camPath.AddComponent<Jacovone.PathMagic> ();
						var transition = camPath.AddComponent<PanelTransition> ();
						transition.connectedID = j;
						transition.path = bSpline;
						transition.duration = 2.0f;
						transition.linkedEvent = LinkedEvent.OnTopRank;
						transition.UpdateSpline (true);
					} else {
					}
				}
				if (pages [i].connectedPages.Count == 0)
				{
					EditorUtility.DisplayDialog("Intialize Camera Paths", String.Format ("There are no connected pages on {0}, consider using 'Create Ordered Network'.", pages [i].name), "Okay");
				}
			}
			if (pages.Length == 0)
			{
				EditorUtility.DisplayDialog("Intialize Camera Paths", String.Format ("There are no pages in the scene, consider using 'Create Pages from PSD'."), "Okay");
			}
		} else {
			EditorUtility.DisplayDialog("Intialize Camera Paths", String.Format ("There is no gameObject named pages in the scene, consider pressing 'Initialize' and then 'Create Pages from PSD'."), "Okay");
		}
	}

	//[MenuItem("Zyndo/Insert Path Point %#e")]
	//static void InsertPathPoint()
	//{

	//    var obj = Selection.activeGameObject;
	//    var bezier = obj.GetComponent<BezierSpline>();
	//    if(bezier != null)
	//    {
	//        //bezier.InsertPoint(2);
	//        bezier.AddCurve();
	//    }
	//}

	[MenuItem("Zyndo/Insert Blank Page", false, 3036)]
	static void InsertBlankPage()
	{
		var obj = Selection.activeGameObject;
		GameObject panel_root_prefab = AssetDatabase.LoadAssetAtPath("Assets/Zyndo/Prefabs/PanelRoot.prefab", typeof(GameObject)) as GameObject;
		GameObject page_root = GameObject.Instantiate(panel_root_prefab);
		page_root.transform.parent = GameObject.Find("Pages").transform;
		//insert this page between the connected pages
		//if (obj != null)
		//{

		//}
		//otherwise, just add it to the pages object
		//else
		//{
		//add page component and details
		var page = page_root.AddComponent<Page>();

		//calc unique id
		page.uniquePageID = GameObjectFromTextures.hash6432shift(System.DateTime.Now.Ticks);

		//place at origin
		page_root.transform.position = Vector3.zero;

		//}

		page_root.name = "Blank_Page_Object";
	}


	[MenuItem("Zyndo/Replace All Unity Text", false, 6000)]
	static void ReplaceUnityDialogue()
	{
		Text[] allText = GameObject.Find("Pages").GetComponentsInChildren<Text>();

		foreach(var t in allText)
		{
			var color = t.color;
			var fontSize = t.fontSize;
			var text = t.text;
			var gameObject = t.gameObject;

			DestroyImmediate(t);

			var tm = gameObject.AddComponent<TextMeshProUGUI>();
			tm.enableWordWrapping = true;
			tm.color = color;
			tm.fontSize = fontSize;
			tm.text = text;

		}
	}

	[MenuItem("Zyndo/Reset Camera Paths", false, 2060)]
	static void ResetCameraPaths()
	{
		foreach (var o in Selection.gameObjects)
		{
			var camPaths = o.GetComponents<PanelTransition>();
			foreach(var c in camPaths)
			{
				c.GetComponent<Jacovone.PathMagic>();
				c.UpdateSpline(true);
				SceneView.RepaintAll();
			}
		}
	}

	[MenuItem("Zyndo/Update To VR Ready Camera", false, 6015)]
	static void UpdateToVRReadyCamera()
	{	
		Debug.Log("adding VR Ready Camera...");
		GameObject newCamPF = AssetDatabase.LoadAssetAtPath("Assets/Zyndo/Prefabs/CameraRoot.prefab", typeof(GameObject)) as GameObject;
		GameObject pivotGO = GameObject.Find ("Pivot");
		if (pivotGO != null)
		{		
			Transform originalCameraGO = pivotGO.transform.Find ("Camera");
			if (originalCameraGO != null)
			{
				originalCameraGO.gameObject.SetActive (false);
			}
		}

		GameObject newCamera = GameObject.Instantiate(newCamPF) as GameObject;
		newCamera.transform.SetParent(pivotGO.transform);
		newCamera.transform.localScale = Vector3.one;
		newCamera.transform.localPosition = Vector3.zero;
		newCamera.name = "CameraRoot";
	}

	[MenuItem("Zyndo/Set Mobile Friendly Settings", false, 6005)]
	static void SetMobileFriendlySettings()
	{			
		string question = string.Format ("This will set the following global settings:\n-RenderingPath = Forward\n-SpritePackerMode = SpritePackerMode.BuildTimeOnly\n\nThis will iterate through all {0} scene(s) in build settings and save them with:\n-Turn Off Camera Effects\n-Turn Off Reflection Probes", EditorBuildSettings.scenes.Count());
		if (!EditorUtility.DisplayDialog("Set Mobile Friendly Settings", question, "Okay", "Cancel"))
		{
			return;
		}

		//Forward rendering
		PlayerSettings.renderingPath = RenderingPath.Forward;
		EditorSettings.spritePackerMode = SpritePackerMode.BuildTimeOnly;

		for (int sceneInd = 0; sceneInd < EditorBuildSettings.scenes.Count(); sceneInd++)
		{
			string updatedText = String.Format ("{0}/{1} Scenes Complete", sceneInd, EditorBuildSettings.scenes.Count());
			if (EditorUtility.DisplayCancelableProgressBar ("Progress", updatedText, (float)sceneInd / EditorBuildSettings.scenes.Count()))
			{
				EditorUtility.ClearProgressBar ();
				return;				
			}
			string path = EditorBuildSettings.scenes [sceneInd].path;
			Scene openScene = EditorSceneManager.OpenScene (path);

			GameObject[] obInScene = openScene.GetRootGameObjects();
			Transform pivot;
			Transform main;
			int changes = 0;
			for (int i = 0; i < obInScene.Length; i++)
			{
				//turn off camera effects	
				pivot = obInScene[i].transform.Find("Pivot");
				if (pivot != null)
				{			
					BaseEffect[] effects = pivot.GetComponentsInChildren<BaseEffect> ();
					foreach(BaseEffect effect in effects)
					{
						effect.enabled = false;
					}
					changes++;
				}
				//Turn off reflection Probes
				main = obInScene[i].name == "Main" ? obInScene[i].transform : obInScene[i].transform.Find("Main");
				if (main != null)
				{			
					ReflectionProbe proby = main.GetComponent<ReflectionProbe> ();
					if (proby != null)
					{
						changes++;
						proby.enabled = false;
					}

					//Update All Pages
					Page[] allPages = main.GetComponentsInChildren<Page> ();
					for (int pageIndex = 0; pageIndex < allPages.Length; pageIndex++)
					{
						DisableAtDistFromCamera camScript = allPages [pageIndex].gameObject.GetComponent<DisableAtDistFromCamera> ();
						if (camScript == null)
						{
							allPages [pageIndex].gameObject.AddComponent<DisableAtDistFromCamera> ();
						}
					}
					Debug.Log (path + "  " + allPages.Count());
				}
			}


			//Disable all Skybox fades
			/*
			var pageRE = GameObject.Find("Pages");
			SkyboxFade[] effectsBM = pageRE.GetComponentsInChildren<SkyboxFade>();
			foreach(SkyboxFade effect in effectsBM)
			{
				effect.enabled = false;
			} 
			*/

			//Turn off all BlenModes
			/*
			BlendModeEffect[] effectsBM = pageRE.GetComponentsInChildren<BlendModeEffect>();
			foreach(BlendModeEffect effect in effectsBM)
			{
				effect.enabled = false;
			}
			*/
			if (changes != 2 || !EditorSceneManager.SaveScene (openScene, path)) {

				Debug.Log ("Scene Failed: " + path + " " + openScene.isDirty.ToString() + " " + changes);
			}
		}					
		EditorUtility.ClearProgressBar ();
		EditorUtility.DisplayDialog("Update Scenes", String.Format ("{0} Scene(s) Complete", EditorBuildSettings.scenes.Count()), "Okay");
	}

	[MenuItem("Zyndo/Update Pages", false, 3040)]
	static void UpdatePanels()
	{
		Debug.Log("updating panels...");
		if (!AssetDatabase.IsValidFolder("Assets/Images"))
			AssetDatabase.CreateFolder("Assets", "Images");
		PanelEditor.UpdatePanels();
	}

    [MenuItem("Zyndo/Clear Remebered Page Data", false, 7000)]
    static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteKey("LastSceneLoaded");
        PlayerPrefs.DeleteKey("LastVisitedPage");
    }

    [MenuItem("Zyndo/Clear Tutorial Data", false, 7001)]
    static void ClearTutData()
    {
        PlayerPrefs.SetInt("TutorialComplete", 0);
    }

    [MenuItem("Zyndo/v20170728", false, 1000000)]
	static void AboutMe()
	{
		EditorUtility.DisplayDialog("About Me", "Version: 20170728", "Okay");
	}
}
