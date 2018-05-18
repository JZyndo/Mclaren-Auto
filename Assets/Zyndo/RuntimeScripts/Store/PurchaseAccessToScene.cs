using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class PurchaseAccessToScene : MonoBehaviour {
	
	public string SceneToBuy = "";
	[System.Serializable]
	public class CheckIfBoughten : UnityEvent {};
	public CheckIfBoughten CallOnBoughten;
	public CheckIfBoughten CallOnNotBoughten;
	public void Start ()
	{
		if (EpicPrefs.GetInt (SceneToBuy, -1, true) != -1) {
			SetSceneAsInaccessible ();
		}
		if (HasPurchased())
		{
			CallOnBoughten.Invoke ();
		}
		else
		{
			CallOnNotBoughten.Invoke ();
		}
		if(!DoesSceneExistInBuild())
		{
			Debug.LogWarning (SceneToBuy + "Doesn't exist by is unlockable in the store", gameObject);
		}
	}

	public bool DoesSceneExistInBuild()
	{
		#if UNITY_EDITOR
		foreach(EditorBuildSettingsScene buildScene in EditorBuildSettings.scenes)
		{
			if(buildScene.path.Contains(SceneToBuy))
			{
				return true;
			}
		}
		return false;
		#endif

		return true;
	}

	public void SetSceneAsInaccessible()
	{
		EpicPrefs.SetInt (SceneToBuy, 0, true);
	}

	public void PurchaseScene()
	{
		EpicPrefs.SetInt (SceneToBuy, 1, true);
	}

	public bool HasPurchased()
	{
		return EpicPrefs.GetInt (SceneToBuy, true) == 1;
	}
}
