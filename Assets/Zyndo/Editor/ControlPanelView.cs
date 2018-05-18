using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(ControlPanel))]
[CanEditMultipleObjects]
public class ControlPanelView : Editor
{

    private ReorderableList arrivalList;
    private ReorderableList nextList;
    private ReorderableList departureList;

    private string[] propNames = {"eventType", "duration" , "delay", "curve", "rank", "ignoreEmptyPage"};

    private void OnEnable()
    {
		arrivalList = CreateList("allArrivalData", "Page Arrival Events");
		nextList = CreateList("allNextData", "Ranked Events");
		departureList = CreateList("allDepartureData", "Page Departure Events");
    }

	ReorderableList CreateList(string listPropertyName, string listTitle)
	{
        var list = new ReorderableList(serializedObject,
                serializedObject.FindProperty(listPropertyName),
                true, true, true, true);

        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, listTitle);
        };

        list.elementHeightCallback = (index) => 
		{ 
			var element = list.serializedProperty.GetArrayElementAtIndex(index); 
			return 3.0f * EditorGUI.GetPropertyHeight(element); 
		};

        list.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            var cellSize = rect.width/(float)propNames.Length;
            var offset = new RectOffset(2, 5, 0, 0);
            //var dataItem = element.objectReferenceValue as System.Object as ZEventCreationData;

            //find all properties associated with the type
			int enumIndex = element.FindPropertyRelative("eventType").enumValueIndex;
			ZEventType eType = (ZEventType)enumIndex;
			Type compType = Type.GetType(eType.ToString());


            //draw
            for(int i = 0; i < propNames.Length; i++)
            {
            	var cellRect = new Rect(rect.x + i * cellSize, rect.y, cellSize/2, EditorGUIUtility.singleLineHeight);
            	cellRect = offset.Remove(cellRect);
            	EditorGUI.LabelField(cellRect, propNames[i]);

            	cellRect = new Rect(rect.x + i * cellSize + cellSize/2, rect.y, cellSize/2, EditorGUIUtility.singleLineHeight);
            	cellRect = offset.Remove(cellRect);
            	EditorGUI.PropertyField(
                	cellRect, element.FindPropertyRelative(propNames[i]), GUIContent.none);
            }

        };

        return list;
	}

    public override void OnInspectorGUI()
    {
        ControlPanel panel = (ControlPanel)target;
        serializedObject.Update();

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Page Arrival Events:");
        arrivalList.DoLayoutList();

   		EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Ranked Events:");
        nextList.DoLayoutList();

   		EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Page Departure Events:");
        departureList.DoLayoutList();


        serializedObject.ApplyModifiedProperties();
    }
}

