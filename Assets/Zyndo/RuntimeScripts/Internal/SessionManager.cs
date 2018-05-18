using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class SessionManager : MonoBehaviour
{

    public Stack<Page> visitedPages;
    private AnimationCurve curve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
    private float undoTransitionTime = 2.0f;
    private bool moving = false;
    private bool processingPreviousMove = false;

    public static SessionManager instance;
    void OnEnable()
    {
        PageEventsManager.PageDeparture += OnPageDeparture;
        instance = this;
    }

    void OnDisable()
    {
        PageEventsManager.PageDeparture -= OnPageDeparture;
        instance = null;
    }

    // Use this for initialization
    void Awake()
    {
        if (visitedPages == null)
            visitedPages = new Stack<Page>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToPrevious()
    {

        if (!moving && PageEventsManager.currentPage.gameObject.GetComponent<LoadSceneOnPrevious>())
        {
            PageEventsManager.currentPage.gameObject.GetComponent<LoadSceneOnPrevious>().Load();
            return;
        }

        if (!moving && visitedPages.Count > 0)
        {
            var target = visitedPages.Pop();

            while (target == null && visitedPages.Count > 0)
                target = visitedPages.Pop();

            if(target != null)
            {
                var page = target.gameObject;
                processingPreviousMove = true;
                PageEventsManager.DisableEvents();
                PageEventsManager.ForcePageDeparture();
                StartCoroutine(MovePivotToTarget(page));
            }


        }
    }

    public void GoToPage(int id)
    {
        var pageRoot = GameObject.Find("Pages");
        if (pageRoot != null)
        {
            var pages = pageRoot.GetComponentsInChildren<Page>();
            if (id < pages.Length)
            {
                PageEventsManager.DisableEvents();
				PageEventsManager.ForcePageDeparture ();
                //manually push the last page to stack
                visitedPages.Push(PageEventsManager.currentPage);
                StartCoroutine(MovePivotToTarget(pages[id].gameObject));
            }
        }
    }

    public virtual void OnPageDeparture(object sender, EventArgs e)
    {
        if (!processingPreviousMove)
            visitedPages.Push(PageEventsManager.currentPage);
    }

    IEnumerator MovePivotToTarget(GameObject target)
    {
        PageEventsManager.busy = true;
        var steps = Mathf.Ceil(undoTransitionTime / Time.deltaTime);
        var pivot = PageEventsManager.pivot;
        if (pivot == null)
            pivot = GameObject.Find("Pivot");
        var org_pos = pivot.transform.position;
        var org_rot = pivot.transform.localRotation;

        float targetFOV = Camera.main.fieldOfView;
        float lastFOV = targetFOV;

        var overrides = target.GetComponent<PageOverrides>();
        if (overrides != null && overrides.OverrideCameraFOV)
            targetFOV = overrides.CameraFOV;

        moving = true;
        for (int i = 0; i <= steps; i++)
        {
            var lerpVal = curve.Evaluate(i / steps);
            pivot.transform.position = Vector3.Lerp(org_pos, target.transform.position, lerpVal);
            pivot.transform.localRotation = Quaternion.Lerp(org_rot, target.transform.localRotation, lerpVal);
            Camera.main.fieldOfView = Mathf.Lerp(lastFOV, targetFOV, i / steps);

            //bit of a hack
            if (i == steps - 5)
                PageEventsManager.EnableEvents();
            yield return 0;
        }
        moving = false;
        Camera.main.fieldOfView = targetFOV;

        processingPreviousMove = false;
        PageEventsManager.busy = false;

        yield return 0;

    }

	public void AllPagesFadeIn()
	{
		var pageRoot = GameObject.Find("Pages");
		pageRoot.SetActive (true);
	}	
	public void AllPagesFadeOut()
	{
		var pageRoot = GameObject.Find("Pages");
		CanvasGroup cg = pageRoot.GetComponent<CanvasGroup> ();
		pageRoot.SetActive (false);
	}
}
