using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public enum SupportedFadeTypes
{
    Image,
    MeshRenderer,
    CanvasGroup,
	TextMeshProUGUI
}

[Serializable]
public struct ObjectFadeData
{
    public Component objectToFade;
    public SupportedFadeTypes type;
    public float targetAlpha;
    public float delay;

    //public ObjectFadeData(Component obj, SupportedFadeTypes type, float alpha, float delay)
    //{
    //    this.type = type; 
    //    this.objectToFade = obj;
    //    this.targetAlpha = alpha;
    //    this.delay = delay;
    //}

    public ObjectFadeData(GameObject obj, SupportedFadeTypes type, float alpha, float delay)
    {
        this.type = type;
        this.objectToFade = obj.GetComponent(type.ToString());
        this.targetAlpha = alpha;
        this.delay = delay;
    }
}

public class PanelFade : PageEventBase
{

    public List<ObjectFadeData> objectsToFade;
    // Use this for initialization

    public override void ForceEvent()
    {
        base.ForceEvent();
        ProcessEvent();
    }

    public override void ProcessEvent()
    {
        base.ProcessEvent();
        foreach (var go in objectsToFade)
            StartCoroutine(LerpAlpha(go));
    }


    IEnumerator LerpAlpha(ObjectFadeData fadeData)
    {
        //wait if delay is set
        yield return new WaitForSeconds(fadeData.delay);

        if (fadeData.objectToFade != null)
        {
            var componentToFade = fadeData.objectToFade.GetComponent(fadeData.type.ToString());
            if (componentToFade != null)
            {
				float timeAtstart = Time.time;

                var orgVal = componentToFade.GetAlphaGeneric();

                //do the yielding loop
				while (duration > Time.time - timeAtstart)
                {
					var lerpVal = curve.Evaluate(Mathf.Clamp01((Time.time - timeAtstart)/ duration));
                    var alpha = Mathf.Lerp(orgVal, fadeData.targetAlpha, lerpVal);
                    componentToFade.SetAlphaGeneric(alpha);

                    yield return 0;
                }
            }
        }
    }
}
