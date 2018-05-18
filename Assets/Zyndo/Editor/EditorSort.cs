using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SortChildObjects : EditorWindow
{

	//[MenuItem("Zyndo/Sort By Name", false, 3037)]
    public static void SortGameObjectsByName()
    {
        if (Selection.activeGameObject == null)
        {
            EditorUtility.DisplayDialog("Error", "You must select an item to sort", "Okay");
            return;
        }

        GameObject parentObject = (GameObject)Selection.activeGameObject;

        SortChildrenByName(parentObject);


    }

    public static void SortChildrenByName(GameObject parentObject)
    {

        // Build a list of all the Transforms in this player's hierarchy
        Transform[] objectTransforms = new Transform[parentObject.transform.childCount];
        for (int i = 0; i < objectTransforms.Length; i++)
            objectTransforms[i] = parentObject.transform.GetChild(i);

        int sortTime = System.Environment.TickCount;

        bool sorted = false;
        // Perform a bubble sort on the objects
        while (sorted == false)
        {
            sorted = true;
            for (int i = 0; i < objectTransforms.Length - 1; i++)
            {
                // Compare the two strings to see which is sooner
                int comparison = objectTransforms[i].name.CompareTo(objectTransforms[i + 1].name);

                if (comparison > 0) // 1 means that the current value is larger than the last value
                {
                    objectTransforms[i].transform.SetSiblingIndex(objectTransforms[i + 1].GetSiblingIndex());
                    sorted = false;
                }
            }

            // resort the list to get the new layout
            for (int i = 0; i < objectTransforms.Length; i++)
                objectTransforms[i] = parentObject.transform.GetChild(i);
        }

        Debug.Log("Sort took " + (System.Environment.TickCount - sortTime) + " milliseconds");
    }
}