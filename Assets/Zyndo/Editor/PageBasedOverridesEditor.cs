using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PageOverrides))]
public class PageBasedOverridesEditor : Editor {
	SerializedProperty overrideZoom;	
	SerializedProperty zoomVal;
	SerializedProperty overrideboundsMulti;
	SerializedProperty boundsMulti;
	SerializedProperty overrideCamDelay;
	SerializedProperty zoomOutCamDelay;
    SerializedProperty overrideSnag;
    SerializedProperty snapOnRelease;
    SerializedProperty overridePageTilt;
    SerializedProperty pageTiltOn;
    SerializedProperty overrideCameraFOV;
    SerializedProperty cameraFOV;

    private void OnEnable()
	{
		overrideZoom = serializedObject.FindProperty ("OverrideZoomAmount");
		zoomVal = serializedObject.FindProperty ("ZoomVal");

		overrideboundsMulti = serializedObject.FindProperty ("OverrideBoundsMulti");
		boundsMulti = serializedObject.FindProperty ("BoundsMulti");

		overrideCamDelay = serializedObject.FindProperty ("OverrideCamDelay");
		zoomOutCamDelay = serializedObject.FindProperty ("ZoomOutCamDelay");

		overrideSnag = serializedObject.FindProperty ("OverrideDragSnap");
		snapOnRelease = serializedObject.FindProperty ("SnapOnDragRelease");

        overridePageTilt = serializedObject.FindProperty("OverridePageTilt");
        pageTiltOn = serializedObject.FindProperty("PageTiltOn");

        overridePageTilt = serializedObject.FindProperty("OverridePageTilt");
        pageTiltOn = serializedObject.FindProperty("PageTiltOn");

        overrideCameraFOV = serializedObject.FindProperty("OverrideCameraFOV");
        cameraFOV = serializedObject.FindProperty("CameraFOV");
    }

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(overrideZoom);
		if (overrideZoom.boolValue)
		{
			EditorGUILayout.PropertyField(zoomVal);
		}
		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(overrideboundsMulti);		
		if (overrideboundsMulti.boolValue)
		{	
			EditorGUILayout.PropertyField(boundsMulti);	
		}		
		EditorGUILayout.Separator();

		EditorGUILayout.PropertyField(overrideCamDelay);		
		if (overrideCamDelay.boolValue)
		{	
			EditorGUILayout.PropertyField(zoomOutCamDelay);	
		}		
		EditorGUILayout.Separator();

		EditorGUILayout.PropertyField(overrideSnag);		
		if (overrideSnag.boolValue)
		{	
			EditorGUILayout.PropertyField(snapOnRelease);	
		}
        EditorGUILayout.Separator();

        EditorGUILayout.PropertyField(overridePageTilt);
        if (overridePageTilt.boolValue)
        {
            EditorGUILayout.PropertyField(pageTiltOn);
        }

        EditorGUILayout.Separator();

        EditorGUILayout.PropertyField(overrideCameraFOV);
        if (overrideCameraFOV.boolValue)
        {
            EditorGUILayout.PropertyField(cameraFOV);
        }
        serializedObject.ApplyModifiedProperties();
	}
}
