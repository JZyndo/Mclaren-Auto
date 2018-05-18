using UnityEngine;
using System.Collections;

public class LoadNextChapter : PageEventBase {

	public string chapterToLoad;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void ProcessEvent ()
	{
		Application.LoadLevel (chapterToLoad);
	}
}
