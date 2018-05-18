using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SetPositionBasedOnPage : MonoBehaviour {
	// Use this for initialization
	void OnEnable () {
		//Reset Animations
		GameObject cloesetEarlierPage = FindClosestPage();
		if(cloesetEarlierPage != null)
		{
			Debug.Log ("-cloesetEarlierPage.transform.position.x" + (-cloesetEarlierPage.transform.position.x));
			Vector3 pos = transform.position;
			pos.x = -cloesetEarlierPage.transform.position.x;
			transform.position = pos;
		}
	}

	public GameObject FindClosestPage()
	{
		Page currPage = PageEventsManager.currentPage;
		int currPageIndex = currPage.transform.GetSiblingIndex ();
		GameObject closestEarlierPage = null;
		for (int i = 0; i < transform.childCount; i ++)
		{
			GoToChapterDirectly goToChapterDirectly = transform.GetChild (i).GetComponent<GoToChapterDirectly>();
			if (SceneManager.GetActiveScene ().name == goToChapterDirectly.SceneName && goToChapterDirectly.SceneName != "")
			{
				GameObject thisPage = GameObject.Find(goToChapterDirectly.PageName);
				int thisPageIndex = thisPage.transform.GetSiblingIndex();
				//Page's in the menu selection will need to be linear, so we get assume that the currecnt thisPageIndex is greater then the last one
				if (thisPageIndex <= currPageIndex)
				{
					closestEarlierPage = goToChapterDirectly.gameObject;
					Debug.Log ("closestEarlierPage", closestEarlierPage);
				}
				else
				{
					break;
				}
			}
		}
		Debug.Log ("Fianl:", closestEarlierPage);
		return closestEarlierPage;
	}
}
