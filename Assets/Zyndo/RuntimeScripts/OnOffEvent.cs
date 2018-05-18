using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class OnOffData
{
    public GameObject targetObject;
    public MonoBehaviour targetComponent;
    public int id;

    public OnOffData(GameObject target)
    {
        targetObject = target;
        targetComponent = null;
    }
}

public class OnOffEvent : PageEventBase
{
    public List<OnOffData> targetData = new List<OnOffData>();

    public override void ForceEvent()
    {
        base.ForceEvent();
        foreach (var d in targetData)
        {
            if (d.targetComponent != null)
            {
                d.targetComponent.enabled = !d.targetComponent.enabled;
            }
        }
    }
    public override void ProcessEvent()
    {

        base.ProcessEvent();
        foreach(var d in targetData)
        {
            if(d.targetComponent != null)
            {
                d.targetComponent.enabled = !d.targetComponent.enabled;
            }
        }
    }
}
