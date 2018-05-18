using UnityEngine;
using System.Collections;

public class VRMenuControls : MonoBehaviour {
	public string OpenMenuButton = "Menu";
	public VRChapterMenu VRChapterMenu;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown(OpenMenuButton))
		{
			VRChapterMenu.gameObject.SetActive (false);
		}
	}

	/*public IEnumerator FadeOutPagesAndFadInMenu()
	{
		//Fade Out
		//Turn Pages Off
		GameObject pageRoot = GameObject.Find("Pages");
		if (pageRoot != null)
		{
			pageRoot.SetActive(false);
		}
		//Fade In
	}*/
}
