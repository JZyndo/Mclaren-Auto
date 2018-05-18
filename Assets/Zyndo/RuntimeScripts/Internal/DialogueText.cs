using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class DialogueText : MonoBehaviour
{

	private TextMeshProUGUI textObj;

	public string internal_id;
    private bool resized = false;

    void OnEnable()
    {

    }

    // Use this for initialization
    void Start()
    {
		if (internal_id == null || internal_id == "")
		{
			Debug.LogWarning(string.Format("GameObject with name {0} has a DialogueText without an internal_id set", gameObject.name));
		}
    }

    // Update is called once per frame
    void Update()
    {
        if(!resized)
        {
           // ResizeBubble();
        }     
    }

    public string GetText()
    {

       	var text = "";
		textObj = GetComponentInChildren<TextMeshProUGUI>();
        if (textObj != null)
            text = textObj.text;

        return text;
    }

    public void SetText(string localText)
    {
		textObj = GetComponentInChildren<TextMeshProUGUI>();
		if (textObj != null) {
			textObj.text = localText;
			textObj.SetAllDirty ();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }

    void ResizeBubble()
    {
		TextMeshProUGUI textComp = GetComponentInChildren<TextMeshProUGUI>();
		textComp.autoSizeTextContainer = true;
//        int vizC = t.characterCountVisible;
//        int totalC = textComp.text.Length - 1;
//        if (vizC < totalC)
//        {
//            textComp.rectTransform.sizeDelta += new Vector2(50, 50);
//            textComp.rectTransform.parent.GetComponent<RectTransform>().sizeDelta += new Vector2(50, 50);
//        }
//        else
//            resized = true;
    }

	public string SetInternalID(string id = "")
	{
		if(id == "")
		{
			internal_id = Guid.NewGuid ().ToString();
			return internal_id;
		}
		internal_id = id;
		return internal_id;
	}
}
