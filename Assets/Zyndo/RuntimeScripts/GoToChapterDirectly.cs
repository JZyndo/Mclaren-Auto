using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VR;
using UnityEngine.Events;

public class GoToChapterDirectly : MonoBehaviour {
	public string SceneName;
	public string PageName;
	public Image Thumbnail;
	private GameObject _navBar;

	public UnityEvent OnNeedsPurchase;

	public void Start()
	{
		_navBar = GameObject.Find ("NavBar");

		if (EpicPrefs.GetInt(SceneName, -1, true) == 0)
		{
			//Needs purchase, and doesn't have it.
			OnNeedsPurchase.Invoke ();
		}
	}

	public void GoToChapterPage()
	{		
		StartCoroutine (GoToChapterCoroutine());
	}

	public IEnumerator GoToChapterCoroutine()
	{		
		//Put it back out there in the world
		//Get rid of menu
		yield return CloseMenu();
		Debug.Log ("GoToChapterDirectly: " + SceneManager.GetActiveScene ().name);
		if (SceneManager.GetActiveScene ().name != SceneName && SceneName != "") 
		{
			EpicPrefs.SetString ("StartPage", PageName);
			yield return GoToChapter ();
		}
		Debug.Log ("yield return GoToPage();");
		yield return GoToPage();
	}

	public IEnumerator GoToChapter()
	{
		//Open Scene if you can
		int keyVal = EpicPrefs.GetInt(SceneName, -1, true);
		//The scene either is NOT restricted OR has a lock and IS UNLCOKED
		if (keyVal == -1 || keyVal == 1) {
			if (SceneName != "") {
				AsyncOperation op = SceneManager.LoadSceneAsync (SceneName);
				while (!op.isDone)
				{
					yield return null;
				}
			} else {
				LoadNextScene ();
			}
		}
		else
		{
			//Needs to purchase, but hasn't. Inform player?? Should never hit this ATM, as the botton should be disabled on start
		}
	}

	public IEnumerator GoToPage()
	{
		Debug.Log ("In  GoToPage()");
		var pivot = GameObject.Find("Pivot");
		var orgPos = pivot.transform.position;
		var orgRot = pivot.transform.localRotation;
		float moveDuration = 0;
		GameObject pageGO = GameObject.Find (PageName);
		if (pageGO == null) {
			Debug.LogError (string.Format ("Error in GoToChapterDirectly transition, page doesn't appear to exit in the scene. Scene: '{0}' Page '{1}'", SceneName, PageName));
		} else {
			//Move there.
			float startTime = Time.time;
			while (Time.time - startTime < moveDuration) {
				Debug.Log ("Object.Fi");
				var lerpVal = Mathf.Clamp01 ((Time.time - startTime) / moveDuration);
				Debug.Log (lerpVal);
				pivot.transform.position = Vector3.Lerp (orgPos, pageGO.transform.position, lerpVal);
				pivot.transform.localRotation = Quaternion.Lerp (orgRot, pageGO.transform.localRotation, lerpVal);
				yield return null;
			}
			pivot.transform.position = pageGO.transform.position;
			pivot.transform.localRotation = pageGO.transform.localRotation;
		}
	}

	public IEnumerator CloseMenu()
	{
		if (VRSettings.enabled) {
			GameObject go = GameObject.Find ("PageSelection");
			yield return go.GetComponent<VRChapterMenu> ().CloseMenu ();
		}
		else
		{
			ToolbarNavigation toolBar = _navBar.GetComponent<ToolbarNavigation> ();
			if (toolBar != null)
			{
				toolBar.TurnOffActive ();
			}
			UIPanelOperations panelOperations = _navBar.GetComponent<UIPanelOperations> ();
			if (panelOperations != null)
			{
				panelOperations.ToggleActive ();
			}
			yield return null;
		}
	}

	public void LoadNextScene()
	{
		SceneManager.LoadSceneAsync((1 + SceneManager.GetActiveScene().buildIndex) % SceneManager.sceneCountInBuildSettings);
	}
}
