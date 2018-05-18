using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetUIText : MonoBehaviour {

    public Selectable valueObject;
    public Text targetText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if(targetText != null && valueObject.GetType().GetProperty("value") != null)
        {
            targetText.text = valueObject.GetType().GetProperty("value").GetValue(valueObject, null).ToString();
        }
	
	}
}
