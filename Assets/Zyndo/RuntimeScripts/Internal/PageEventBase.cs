using UnityEngine;
using System;
using System.Collections;

[Serializable]
public enum LinkedEvent
{
    OnNext,
    OnPageArrival,
    OnPageDeparture,
    OnTopRank,
    None
}

public class PageEventBase : MonoBehaviour {

    public int rank;
    public float duration = 2.0f;
    public float delay = 0.0f;
    public AnimationCurve curve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
    public LinkedEvent linkedEvent;
    public bool ignoreEmptyPage = false;

    protected bool processed = false;
    protected Page page;
    protected int callerID;
    public virtual void OnEnable()
    {
        if(linkedEvent == LinkedEvent.OnPageArrival)
            PageEventsManager.PageArrival += PreEvent;
        if (linkedEvent == LinkedEvent.OnPageDeparture)
            PageEventsManager.PageDeparture += PreEvent;
        if (linkedEvent == LinkedEvent.OnNext ||
            linkedEvent == LinkedEvent.OnTopRank)
            PageEventsManager.NextEvent += PreEvent;

        //Search the hiearchy for the closest page
        page = FindPage(this.gameObject);

        //sign up to track which ui element calls the event
        PageEventsManager.CallerID += OnCallerID;

        //modify the rank of this event if necessary
        if (linkedEvent == LinkedEvent.OnTopRank)
            SetTopRank();

    }

    void SetTopRank()
    {
        rank = 0;
        if (page != null)
        {
            var allEvents = page.gameObject.GetComponentsInChildren<PageEventBase>();
            foreach(var e in allEvents)
            {
                if (e.rank >= rank && e != this && 
                    e.enabled && 
                    e.linkedEvent != LinkedEvent.OnTopRank)
                    rank = e.rank + 1;
            }
        }
		rank = Mathf.Max (rank, 1);
    }

    void OnDisable()
    {
        if (linkedEvent == LinkedEvent.OnPageArrival)
            PageEventsManager.PageArrival -= PreEvent;
        if (linkedEvent == LinkedEvent.OnPageDeparture)
            PageEventsManager.PageDeparture -= PreEvent;
        if (linkedEvent == LinkedEvent.OnNext ||
            linkedEvent == LinkedEvent.OnTopRank)
            PageEventsManager.NextEvent -= PreEvent;
        PageEventsManager.CallerID -= OnCallerID;
    }

    public virtual void PreEvent(object sender, EventArgs e)
    {
        if (!ignoreEmptyPage)
        {
            if (page != PageEventsManager.currentPage || (linkedEvent != LinkedEvent.OnPageDeparture && rank != PageEventsManager.currentEventRank))
                return;
        }

        processed = true;
        StartCoroutine(DelayEvent(sender, e));
    }

    /// <summary>
    /// Allows you to trigger the event outside of the main pipeline
    /// </summary>
    public virtual void ForceEvent()
    {
        
    }

    public virtual void ProcessEvent()
    {
        //main stuff happens in derived classes
    }

    public virtual void OnCallerID(object sender, CallerIDArgs e)
    {
        callerID = e.callerID;
    }

    IEnumerator DelayEvent(object sender, EventArgs e)
    {
        yield return new WaitForSeconds(delay);
        ProcessEvent();
    }

    public static Page FindPage(GameObject obj)
    {
        Page page = null;

        page = obj.GetComponent<Page>();
        if (page != null)
            return page;

        var currentObj = obj.transform.parent;
        while(currentObj != null)
        {
            page = currentObj.GetComponent<Page>();
            if (page != null)
                return page;
            currentObj = currentObj.transform.parent;
        }

        return page;
    }

}
