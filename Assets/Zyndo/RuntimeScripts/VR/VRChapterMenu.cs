using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class VRChapterMenu : MonoBehaviour {
	public float MoveMulti = 1f;
	public Transform MinPosition;
	public Transform MaxPosition;
	public float TimePerFlyIn = .5f;
	public void Start()
	{
		
	}

	void  OnEnable () {
		StartCoroutine (OnEnableDelayed ());
	}

	// Use this for initialization
	IEnumerator OnEnableDelayed () {
		//Reset Animations
		yield return null;
		GameObject cloesetEarlierPage = FindClosestPage();
		if(cloesetEarlierPage != null)
		{
			Debug.Log ("-cloesetEarlierPage.transform.position.x" + (-cloesetEarlierPage.transform.position.x));
			Debug.Log ("-cloesetEarlierPage.transform.position.x" + (-cloesetEarlierPage.transform.localPosition.x));
			Vector3 pos = transform.position;
			pos.x = -cloesetEarlierPage.transform.position.x;
			transform.position = pos;
		}
		StartCoroutine(SetTriggers("FlyIn", TimePerFlyIn));
	}

	public IEnumerator SetTriggers(string triggerName, float waitForEach)
	{

		for (int i = 0; i < transform.childCount; i ++)
		{
			Animator anim = transform.GetChild (i).GetComponent<Animator>();
			if (anim != null)
			{
				anim.SetTrigger (triggerName);
				yield return new WaitForSeconds(waitForEach);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (MinPosition.position.x < 0 && Input.GetAxis("Horizontal") < 0)
		{
			//Go Left
			transform.Translate(-Vector3.left * MoveMulti);
		}
		if (MaxPosition.position.x > 0 && Input.GetAxis("Horizontal") > 0)
		{
			//Go Right
			transform.Translate(-Vector3.right * MoveMulti);
		}
	}

	public IEnumerator CloseMenu()
	{
		return SetTriggers("FlyOut", TimePerFlyIn);
	}

	public GameObject FindClosestPage()
	{
		GameObject closestEarlierPage = null;
		string lastSceneName = EpicPrefs.GetString ("LastSceneName", true);
		if (lastSceneName != "") {
			for (int i = 0; i < transform.childCount; i++) {
				GoToChapterDirectly goToChapterDirectly = transform.GetChild (i).GetComponent<GoToChapterDirectly> ();
				if (lastSceneName == goToChapterDirectly.SceneName) {
					closestEarlierPage = goToChapterDirectly.gameObject;
					break;
				}
			}
		}
		Debug.Log ("FindClosestPage: ", closestEarlierPage);
		return closestEarlierPage;
	}
}
