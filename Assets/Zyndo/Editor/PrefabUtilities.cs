using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class PrefabUtilities : MonoBehaviour {

	[MenuItem("Zyndo/Prefab Utilities/Remove All Links To Selected Prefab", false, 5042)]
	static void RemovePrefabLinks()
	{		
		if (PrefabType.None != PrefabUtility.GetPrefabType (Selection.activeGameObject)) {
			if (EditorUtility.DisplayDialog ("Remove Prefab Links", String.Format ("This will unlink all instances of this prefab in all scenes. If the current scene has unsaved changes, please save it before running this."), "Okay", "Cancel")) {
				string[] paths = System.IO.Directory.GetFiles (Application.dataPath, "*.unity", SearchOption.AllDirectories);
				Debug.Log (paths.Length);
				for (int sceneID = 0; sceneID < paths.Length; sceneID++) {
					//Debug.Log (paths [sceneID]);
					Scene openScene = EditorSceneManager.OpenScene (paths [sceneID]);
					GameObject[] obInScene = openScene.GetRootGameObjects ();
					bool dirty = false;
					foreach (GameObject goRoot in obInScene) {
						Transform[] perRootBasedObjs = goRoot.GetComponentsInChildren<Transform> ();
						foreach (Transform GO in perRootBasedObjs) {
							if (PrefabUtility.GetPrefabType (GO.gameObject) == PrefabType.PrefabInstance) {
								UnityEngine.Object GO_prefab = PrefabUtility.GetPrefabParent (GO.gameObject);
								if (Selection.activeGameObject == GO_prefab) {
									PrefabUtility.DisconnectPrefabInstance (GO.gameObject);
									//Debug.Log (GO.gameObject.name);
									dirty = true;
								}								
							}
						}
					}
					if (dirty)
					{
						EditorSceneManager.SaveScene (openScene, paths [sceneID]);
					}
				}
				if (EditorUtility.DisplayDialog ("Remove Prefab Links", String.Format ("This has unlinked all GameObjects in all scenes that were linked with this Prefab, however unity still keeps a connection so the prefab can be relinked later. To fully seperate them, the prefab itself must be delete (It will be re-introduced when you imoport the newest package). Do you want to Delete the prefab now?"), "Yes", "No")) {
					string path = Application.dataPath.Replace ("Assets", "") + AssetDatabase.GetAssetPath (Selection.activeGameObject);
					DestroyImmediate (Selection.activeGameObject, true);
					Debug.Log (path);
					File.Delete (path);
					EditorUtility.DisplayDialog ("Remove Prefab Links", String.Format ("The prefab has been deleted, but you may have to right click and refresh the folder it was in to see the prefab dissapear in the Project window"), "Okay");
				}
			}
		}
	}
}
