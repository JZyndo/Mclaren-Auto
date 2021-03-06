using UnityEngine;
using System.Collections;
using System;
using Jacovone;
using UnityEngine.SceneManagement;

public class LeaveTutorialTransition : PageEventBase
{

    public override void ForceEvent()
    {
		ProcessEvent ();
    }

    public override void ProcessEvent()
	{
    	LoadNextScene ();
    }
		
	public void LoadNextScene()
	{
        PlayerPrefs.SetInt("TutorialComplete", 1);
        SceneManager.LoadScene(1);
	}
}