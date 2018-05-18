using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Page))]
[CanEditMultipleObjects]
public class PageEditor : Editor
{
	private ReorderableList list;
	private void OnEnable()
	{
		if (EditorPrefs.GetInt("AutoSkyboxChange") == 1)
		{
			Selection.selectionChanged += OnSelectionChange;
		}
		list = new ReorderableList(serializedObject,
			serializedObject.FindProperty("connectedPages"),
			true, true, true, true);

		list.drawHeaderCallback = (Rect rect) =>
		{
			EditorGUI.LabelField(rect, "Connected Pages");
		};

		list.drawElementCallback =
			(Rect rect, int index, bool isActive, bool isFocused) =>
		{
			var element = list.serializedProperty.GetArrayElementAtIndex(index);
			rect.y += 2;
			EditorGUI.PropertyField(
				new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("page"), GUIContent.none);
			EditorGUI.PropertyField(
				new Rect(rect.x + 100 + 10, rect.y, 100, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("text"), GUIContent.none);
			EditorGUI.LabelField(
				new Rect(rect.x + 200 + 20, rect.y, rect.width - 200 - 30, EditorGUIUtility.singleLineHeight), "Index: " + index.ToString());
		};
	}

	public void OnDisable()
	{
		Selection.selectionChanged -= OnSelectionChange;
	}

	public void SetClosestSkybox()
	{
		Page page = (Page)target;
		if (page != null && Selection.activeObject == page.gameObject)
		{
			int siblingIndex = page.transform.GetSiblingIndex ();
			SkyboxFade isThereSb = page.GetComponent<SkyboxFade> ();
			while (siblingIndex >= 1 && isThereSb == null)
			{
				isThereSb = page.transform.parent.GetChild (--siblingIndex).GetComponent<SkyboxFade> ();
			}
			if (isThereSb) {
				isThereSb.SetToSkybox ();
				if (!isThereSb.UseAmbientColor)
				{
					isThereSb = null;

					//Continue until you can set the previous ambient light
					while (siblingIndex >= 1 && isThereSb == null)
					{
						Debug.Log (siblingIndex);
						isThereSb = page.transform.parent.GetChild (--siblingIndex).GetComponent<SkyboxFade> ();
						if(isThereSb != null && !isThereSb.UseAmbientColor)
						{
							//Only accept this script if it is using ambient color;
							isThereSb = null;
						}
					}
					//If there is a preivious Fade Skybox that uses an ambient light, use that light, otherwise use Startup's default light
					if (isThereSb) {
						isThereSb.SetToAmbientLight ();
					} else {
						GameObject.Find ("Main").GetComponent<StartUp> ().SetStartupAmbientLightData ();
					}
				}
			} else {
				GameObject.Find ("Main").GetComponent<StartUp> ().SetStartupSkyboxData ();
			}
		}
	}

	void OnSelectionChange()
	{
		//Debug.Log ("OnSelectionChange");
		SetClosestSkybox ();
	}

	public override void OnInspectorGUI()
	{
		Page panel = (Page)target;
		serializedObject.Update();
		EditorGUILayout.LabelField("Attach connected pages: ");
		list.DoLayoutList();
		//panel.connectedPages = EditorGUILayout.ObjectField("Next Page", panel.connectedPages, typeof(Page)) as Page;
		panel.psdAsset = EditorGUILayout.TextField("PSD Filepath: ", panel.psdAsset);
		//panel.pagePreview = EditorGUILayout.ObjectField("Page Preview", panel.pagePreview, typeof(Sprite)) as Sprite;
		EditorGUILayout.LabelField("MD5 Checksum: " + panel.md5);
		EditorGUILayout.LabelField("UniqueID: " + panel.uniquePageID.ToString());
		panel.parallaxFactor = EditorGUILayout.FloatField("Parallax Factor", panel.parallaxFactor);
		serializedObject.ApplyModifiedProperties();
	}

	void OnDestroy()
	{
		if (Application.isEditor)
		{

			if (((Page)target) == null)
			{


			}
		}
	}

	void OnSceneGUI()
	{

	}
}
