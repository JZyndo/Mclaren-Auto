using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum PanMode
{
    left_up,
    left_down,
    right_up,
    right_down,
    reset
}

public class CameraPan : PageEventBase
{

    public Vector2 target;
    public Vector2 extents;
    public PanMode panMode;

    private Vector3 from;
    private Vector3 to;

    public override void ForceEvent()
    {
        base.ForceEvent();
        ProcessEvent();
    }

    public override void ProcessEvent()
    {
        StartCoroutine(CamPan());
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1.0f, 0.7f, 1.0f, 0.5f);
        var center = new Vector3(target.x, target.y, 0.0f);
        var scale = new Vector3(extents.x, extents.y, 0.01f);
        Gizmos.DrawCube(center, scale);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.7f, 1.0f, 0.7f, 0.5f);
        var center = new Vector3(target.x, target.y, 0.0f);
        var scale = new Vector3(extents.x, extents.y, 0.01f);
        Gizmos.DrawCube(center, scale);
    }
#endif

    public void SetTargets()
    {
        if (panMode == PanMode.left_up)
        {
            from = new Vector3(target.x, target.y, Camera.main.transform.position.z)
                + new Vector3(-extents.x / 2, -extents.y / 2, 0f);
            to = new Vector3(target.x, target.y, Camera.main.transform.position.z)
                + new Vector3(extents.x / 2, extents.y / 2, 0f);
        }

        if (panMode == PanMode.reset)
        {
            from = Camera.main.transform.position;
            to = new Vector3(0f, 0f, -2f);
        }
    }

    IEnumerator CamPan()
    {
        float steps = Mathf.Ceil(0.2f * duration / Time.deltaTime);
		steps = Mathf.Max (steps, 1);
        var camObj = Camera.main.gameObject;
        var orgPos = camObj.transform.position;
        SetTargets();

        //first pass: lerp to start point
        if (panMode != PanMode.reset)
        {
            for (int i = 0; i <= steps; i++)
            {
                var lerpVal = curve.Evaluate(i / steps);
                camObj.transform.position = Vector3.Lerp(orgPos, from, lerpVal);
                yield return 0;
            }
        }

        steps = Mathf.Ceil(duration / Time.deltaTime);
		steps = Mathf.Max (steps, 1);
        //second pass do the pan
        for (int i = 0; i <= steps; i++)
        {
            var lerpVal = curve.Evaluate(i / steps);
            camObj.transform.position = Vector3.Lerp(from, to, lerpVal);
            yield return 0;
        }
    }
}
