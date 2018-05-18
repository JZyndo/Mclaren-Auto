using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    [Header("UI")]
    public Image[] pageProgressPips;
    public Color currentPipPageColor;

    private Color defaultPipColor;
    private int curPip;

	// Use this for initialization
	void Awake () {

        if (PlayerPrefs.GetInt("TutorialComplete", 0) == 1)
            SceneManager.LoadScene(1);

        defaultPipColor = pageProgressPips[0].color;
        pageProgressPips[0].color = currentPipPageColor;
    }
	
	// Update is called once per frame
	void Update () {
        if (curPip != PageEventsManager.currentPage.transform.GetSiblingIndex())
        {
            pageProgressPips[curPip].color = defaultPipColor;
            curPip = PageEventsManager.currentPage.transform.GetSiblingIndex();
            pageProgressPips[curPip].color = currentPipPageColor;
        }
	}

    public void LeaveTutorial()
    {
        PlayerPrefs.SetInt("TutorialComplete", 1);
        SceneManager.LoadScene(1);
    }
}
