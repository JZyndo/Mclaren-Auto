using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[Serializable]
public class ParamEventData
{
    public GameObject targetObject;
    public MonoBehaviour targetComponent;
    public string targetParam;
    public float targetValue;
    public float orgValue;
    public int componentId;
    public int paramId;
}

public class PropertyFade : PageEventBase
{

    public List<ParamEventData> targetData = new List<ParamEventData>();

    public override void ForceEvent()
    {
        base.ForceEvent();
        SetOrgValues();
        StartCoroutine(FadeWithCurve());
    }


    public override void ProcessEvent()
    {
        base.ProcessEvent();
        SetOrgValues();
        StartCoroutine(FadeWithCurve());
    }

    void SetOrgValues()
    {
        foreach (var data in targetData)
        {
            var param = data.targetComponent.GetType().GetField(data.targetParam, BindingFlags.Instance | BindingFlags.Public);
            if (param != null)
                data.orgValue = (float)param.GetValue(data.targetComponent);
        }
    }

    IEnumerator FadeWithCurve()
    {
        float steps = Mathf.Ceil(duration / Time.deltaTime);
        for (int i = 0; i <= steps; i++)
        {
            var lerpVal = curve.Evaluate(i / steps);

            foreach(var data in targetData)
            {
                var param = data.targetComponent.GetType().GetField(data.targetParam, BindingFlags.Instance | BindingFlags.Public);
                if (param != null)
                {
                    param.SetValue(data.targetComponent, 
                        Mathf.Lerp(data.orgValue, 
                        data.targetValue, lerpVal));
                }
                    
            }

            yield return 0;
        }
    }
}
