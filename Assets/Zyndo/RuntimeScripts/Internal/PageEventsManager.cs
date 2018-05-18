using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;


public class CallerIDArgs : EventArgs
{
    public int callerID;
}
//set up some event handlers
public delegate void PageArrivalEventHandler(object sender, EventArgs e);
public delegate void PageDepartureEventHandler(object sender, EventArgs e);
public delegate void NextEventHandler(object sender, EventArgs e);
public delegate void CallerIDHandler(object sender, CallerIDArgs e);

public class PageEventsManager : MonoBehaviour
{
    public static PageEventsManager instance;
    //the event handlers
    public static event PageArrivalEventHandler PageArrival;
    public static event PageDepartureEventHandler PageDeparture;
    public static event NextEventHandler NextEvent;
    public static event CallerIDHandler CallerID;

    //various options and public properties
    public static Page[] pages;
    public static Page currentPage;
    public static int currentEventRank = 0;
    public float pageArrivalDistance = 0.5f;
    public bool rememberLastPage = false;
    //public float pageDepartureDistance = 0.5f;
    public bool autoStartNextEvents = true;
    public static GameObject pivot;
    public static bool disableEvents = false;
    public static bool busy = false;
    //private
    private static bool hasArrivedAtPage = false;
    private static Page closestPage;
    private bool skipInput = false;

    /// Called when the camera pivot enters pageArrivalDistance of closest page
    protected virtual void OnPageArrival(EventArgs e)
    {
        if (PageArrival != null && !disableEvents)
            PageArrival(this, e);
    }

    /// Called when the camera pivot departs pageDepartureDistance of closest page
    protected virtual void OnPageDeparture(EventArgs e)
    {
        if (PageDeparture != null && !disableEvents)
            PageDeparture(this, e);
    }

    public static void ForcePageDeparture()
    {
        if (PageDeparture != null)
            PageDeparture(null, EventArgs.Empty);
    }

    /// Called whenever a UI element trigger that has been hooked up is tiggered
    protected virtual void OnNextEvent(EventArgs e)
    {
        if (NextEvent != null && !disableEvents)
            NextEvent(this, e);
    }

    protected virtual void OnCallerID(CallerIDArgs e)
    {
        if (CallerID != null && !disableEvents)
            CallerID(this, e);
    }

    public static void DisableEvents()
    {
        disableEvents = true;
        hasArrivedAtPage = false;
        currentEventRank = 0;
    }

    public static void EnableEvents()
    {
        disableEvents = false;
    }

    // Use this for initialization
    void Start()
    {
        instance = this;

        var pageRoot = GameObject.Find("Pages");
        if (pageRoot != null)
        {
            pages = pageRoot.GetComponentsInChildren<Page>();
        }
        pivot = GameObject.Find("Pivot");
    }

    // Update is called once per frame
    void Update()
    {
        //check for arrival/departure
        PageArrivalMonitor();
        if (!disableEvents && hasArrivedAtPage && !busy && !skipInput)
        {
            //monitor left/right keys for next event stuff
            if (Input.GetAxis("Horizontal") < 0)
            {
                StartCoroutine(InputSkipTimer());
                GameObject.Find("Main").GetComponent<SessionManager>().GoToPrevious();
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                StartCoroutine(InputSkipTimer());
                NextEventMonitor();
            }
        }
    }

    //wait a half second before allowing more input via axis, this helps with instant transition pages
    IEnumerator InputSkipTimer()
    {
        skipInput = true;
        yield return new WaitForSeconds(.75f);
        skipInput = false;
    }

    public void PageArrivalMonitor()
    {
        if (disableEvents)
            return;

        //analyze the scene to determine what the current page is
        var cost = double.MaxValue;
        var id = -1;
        for (int i = 0; pages != null && i < pages.Length; i++)
        {
            var d = (pages[i].transform.position - pivot.transform.position).magnitude;
            if (d < cost)
            {
                cost = d;
                id = i;
            }
        }

        //track the current page
        if (id >= 0)
            closestPage = pages[id];

        //change the current page
        if (currentPage != closestPage)
        {
            //if this is true, we need to fix some things
            //happens on a really fast or instant page transition
            if (hasArrivedAtPage)
            {
                hasArrivedAtPage = false;
                currentEventRank = 0;
                SessionManager.instance.visitedPages.Push(currentPage);
            }

            currentPage = closestPage;
        }

        //use the shortest distance to send the arrival/departure events
        if (cost <= pageArrivalDistance && !hasArrivedAtPage)
        {
            hasArrivedAtPage = true;
            OnPageArrival(EventArgs.Empty);
            //call next event if auto start is on
            if (autoStartNextEvents)
                NextEventMonitor();
        }
        else if (cost > pageArrivalDistance && hasArrivedAtPage)
        {
            hasArrivedAtPage = false;
            OnPageDeparture(EventArgs.Empty);
            //reset the event rank
            currentEventRank = 0;
        }
    }

    public void NextEventMonitor()
    {
        Debug.Log("Next Event triggered...");
        OnNextEvent(EventArgs.Empty);
        currentEventRank++;
    }

    public void TrackCallerID(BaseEventData data)
    {
        var idArgs = new CallerIDArgs();
        var typedData = (PointerEventData)data;
        if (typedData != null)
        {
            var nameData = typedData.pointerDrag.name.Split('_');
            if (nameData.Length == 2)
            {
                var id = int.Parse(nameData[1]);
                idArgs.callerID = id;
                Debug.Log("callerID: " + id);
                OnCallerID(idArgs);
            }
        }
    }

    private void OnApplicationQuit()
    {
        //save this into playerprefs
        if (rememberLastPage)
        {
            ExitDataSave();
        }

        WipeOtherData();
    }

    private void OnApplicationPause(bool pause)
    {
        //save this into playerprefs
        if (rememberLastPage && pause)
        {
            ExitDataSave();
        }

        WipeOtherData();
    }

    private void OnApplicationFocus(bool focus)
    {
        //save this into playerprefs
        if (rememberLastPage && !focus)
        {
            ExitDataSave();
        }

        WipeOtherData();
    }

    void ExitDataSave()
    {
        PlayerPrefs.SetInt("LastVisitedPage", currentPage.gameObject.transform.GetSiblingIndex());
        PlayerPrefs.SetString("LastSceneLoaded", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        Debug.Log("LastVisitedPage: " + currentPage.gameObject.transform.GetSiblingIndex());
        PlayerPrefs.Save();
    }

    void WipeOtherData()
    {
        PlayerPrefs.DeleteKey("TransitionPage");
        PlayerPrefs.DeleteKey("LoadedBack");
    }


}
