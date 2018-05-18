using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public enum ZEventType
{
	AudioEventBase,
	AnimationEventBase,
	InOutFade,
	PanelTransition
}

[Serializable]
public class ZEventCreationData
{
    public ZEventType eventType;
	public int rank;
    public float duration;
    public float delay;
    public bool ignoreEmptyPage;
    public AnimationCurve curve;

    //public ObjectFadeData(Component obj, SupportedFadeTypes type, float alpha, float delay)
    //{
    //    this.type = type; 
    //    this.objectToFade = obj;
    //    this.targetAlpha = alpha;
    //    this.delay = delay;
    //}

    public ZEventCreationData(ZEventType type, float alpha, float delay)
    {
	    this.eventType = type;
		this.rank = 0;
	    this.duration = 2.0f;
	    this.delay = 0.0f;
	    this.ignoreEmptyPage = false;
	    this.curve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
    }
}

public class ControlPanel : MonoBehaviour {

	public List<ZEventCreationData> allArrivalData;
	public List<ZEventCreationData> allNextData;
	public List<ZEventCreationData> allDepartureData;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
