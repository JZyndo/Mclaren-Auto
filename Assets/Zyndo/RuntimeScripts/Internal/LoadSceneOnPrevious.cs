using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneOnPrevious : MonoBehaviour {

    public string SceneToLoad = "";

    public void Load()
    {
        if (SceneToLoad != "")
        {
            PlayerPrefs.SetInt("LoadedBack", 1);

            SceneManager.LoadSceneAsync(SceneToLoad);
        }
    }
}
