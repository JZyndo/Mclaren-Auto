using UnityEngine;
using System.Collections;
using System;
using Jacovone;
using UnityEngine.SceneManagement;

public class SceneTransition : PageEventBase
{
	public string SceneToLoad = "";
	private bool SceneNeedsToBeUnlocked = false;
    bool sceneAlreadyLoaded = false;

    public override void ForceEvent()
    {
        if(SceneToLoad == "")
		    SceneToLoad = SceneManager.GetSceneAt ((1 + SceneManager.GetActiveScene ().buildIndex) % SceneManager.sceneCountInBuildSettings).name;

		ProcessEvent ();
    }

    public override void ProcessEvent()
	{
		//Open Scene if you can
		if (!sceneAlreadyLoaded && !SceneNeedsToBeUnlocked) {
			if (SceneToLoad != "") {
                sceneAlreadyLoaded = true;

                if (PageEventsManager.instance.rememberLastPage)
                {
                    PlayerPrefs.SetInt("LastVisitedPage", 0);
                    PlayerPrefs.SetString("LastSceneLoaded", SceneToLoad);
                }

                PlayerPrefs.SetInt("TransitionPage", PageEventsManager.currentPage.gameObject.transform.GetSiblingIndex());
                PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                PlayerPrefs.SetInt("LoadedBack", 0);

                SceneManager.LoadSceneAsync (SceneToLoad);
			} else {
				LoadNextScene ();
			}
			return;
		}

		//Inform the player that they need to buy the chapter to countinue.
		//TODO: either open the store, or create a popup
		if (SceneNeedsToBeUnlocked)
		{
			
		}
    }
		
	public void LoadNextScene()
	{
		SceneManager.LoadSceneAsync((1 + SceneManager.GetActiveScene().buildIndex) % SceneManager.sceneCountInBuildSettings);
	}
}