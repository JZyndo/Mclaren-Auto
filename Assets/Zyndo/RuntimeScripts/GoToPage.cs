using UnityEngine;
using System.Collections;

public class GoToPage : PageEventBase
{
    public Page targetPage;
    // Use this for initialization
    public override void ForceEvent()
    {
        base.ForceEvent();
        ProcessEvent();
    }

    public override void ProcessEvent()
    {
        if (page != null)
            StartCoroutine(MoveToPage());
    }

    IEnumerator MoveToPage()
    {
        float steps = Mathf.Ceil(duration / Time.deltaTime);
		steps = Mathf.Max (steps, 1);
        var pivot = GameObject.Find("Pivot");
        var orgPos = pivot.transform.position;
        var orgRot = pivot.transform.localRotation;
        for (int i = 0; i <= steps; i++)
        {
            var lerpVal = curve.Evaluate(i / steps);
            pivot.transform.position = Vector3.Lerp(orgPos, targetPage.transform.position, lerpVal);
            pivot.transform.localRotation = Quaternion.Lerp(orgRot, targetPage.transform.localRotation, lerpVal);
            yield return 0;
        }
    }
}