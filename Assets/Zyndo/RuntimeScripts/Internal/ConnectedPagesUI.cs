using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class ConnectedPagesUI : MonoBehaviour
{
    public GameObject panelPrefab;
    public Font font;
    void OnEnable()
    {
        PageEventsManager.PageArrival += OnPageArrival;
        PageEventsManager.PageDeparture += OnPageDeparture;
    }
    void OnDisable()
    {
        PageEventsManager.PageArrival -= OnPageArrival;
        PageEventsManager.PageDeparture -= OnPageDeparture;
    }
    // Use this for initialization
    void Start()
    {
		EventTrigger eventTrigger = GameObject.Find ("LeftBar").GetComponent<EventTrigger>();
		if (eventTrigger != null)
		{
			eventTrigger.triggers [0].callback.AddListener (GoToPreviousWrapper);
			eventTrigger.triggers [1].callback.AddListener (GoToPreviousWrapper);
		}
        SetUpSelectionView();
    }

	public void GoToPreviousWrapper(BaseEventData baseEventData)
	{
		SessionManager.instance.GoToPrevious ();
	}
    
	// Update is called once per frame
    void Update()
    {

    }

    void OnPageArrival(object sender, EventArgs e)
    {
        SetUpSelectionView();
    }

    void OnPageDeparture(object sender, EventArgs e)
    {
        foreach (Transform t in this.transform)
            GameObject.Destroy(t.gameObject);
    }

    void SetUpSelectionView()
    {
        foreach (Transform t in this.transform)
            GameObject.Destroy(t.gameObject);

        if(PageEventsManager.currentPage)
        {
            var connPages = PageEventsManager.currentPage.connectedPages;

            for (int i = 0; i < Mathf.Max(1, connPages.Count); i++)
            {
                GameObject newOption = Instantiate(panelPrefab) as GameObject;
                newOption.name = "ConnectedPageID_" + i;
                newOption.transform.SetParent(this.transform, false);
                newOption.GetComponentInChildren<Text>().font = font;
                newOption.GetComponentInChildren<Text>().text = connPages.Count < i ? connPages[i].text : "";
                EventTrigger eventTrigger = newOption.GetComponent<EventTrigger>();

                //hook up next event
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.EndDrag;
                entry.callback = new EventTrigger.TriggerEvent();
                entry.callback.AddListener((data) => PageEventsManager.instance.NextEventMonitor());
                entry.callback.AddListener((data) => PageEventsManager.instance.TrackCallerID(data));
                eventTrigger.triggers.Add(entry);

                //hook up next event
                EventTrigger.Entry click = new EventTrigger.Entry();
                click.eventID = EventTriggerType.PointerClick;
                click.callback = new EventTrigger.TriggerEvent();
                click.callback.AddListener((data) => PageEventsManager.instance.NextEventMonitor());
                click.callback.AddListener((data) => PageEventsManager.instance.TrackCallerID(data));
                eventTrigger.triggers.Add(click);
            }
        }
       
    }
}
