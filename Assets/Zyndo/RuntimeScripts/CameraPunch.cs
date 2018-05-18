using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum FillMode
{
    width,
    height,
    reset
}

public class CameraPunch : PageEventBase
{

    public Vector2 target;
    public Vector2 extents;
    public FillMode fillMode;

    private float targetFOV;
    private Vector3 orgPos;
    private float orgFOV;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ForceEvent()
    {
        base.ForceEvent();
        ProcessEvent();
    }

    public override void ProcessEvent()
    {
        StartCoroutine(PunchIn());
    }

#if UNITY_EDITOR
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(1.0f, 1.0f, 0.7f, 0.5f);
    //    var center = SetTargetPoint();
    //    var scale = new Vector3(extents.x, extents.y, 0.01f);
    //    Gizmos.DrawCube(center, scale);
    //}

    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = new Color(0.7f, 0.7f, 1.0f, 0.5f);
    //    var center = new Vector3(target.x, target.y, 0.0f);
    //    var scale = new Vector3(extents.x, extents.y, 0.01f);
    //    Gizmos.DrawCube(center, scale);
    //}
#endif

    public void SetTargetFOV()
    {
        var z = Mathf.Abs(Camera.main.transform.localPosition.z);
        var center = new Vector3(Screen.width / 2, Screen.height / 2, z);
        var screenExtents = center + new Vector3(extents.x / 2, extents.y / 2, 0);
        var worldExtents = Camera.main.ScreenToWorldPoint(screenExtents);
        worldExtents = Camera.main.transform.InverseTransformPoint(worldExtents);

        var worldX = Mathf.Abs(worldExtents.x);
        var worldY = Mathf.Abs(worldExtents.y);
        if (fillMode == FillMode.height)
        {
            targetFOV = Mathf.Atan2(worldY, z) * 2.0f * Mathf.Rad2Deg;
            targetFOV *= (float)Screen.width / Screen.height;
        }
        else if (fillMode == FillMode.width)
        {
            targetFOV = Mathf.Atan2(worldX, z) * 2.0f * Mathf.Rad2Deg;
            targetFOV *= (float)Screen.height / Screen.width;
        }
        else if (fillMode == FillMode.reset)
        {
            targetFOV = 60f;
        }
    }

    public Vector3 SetTargetPoint()
    {
        var z = Mathf.Abs(Camera.main.transform.localPosition.z);
        var center = new Vector3(Screen.width / 2, Screen.height / 2, z);
        var screenTarget = center + new Vector3(target.x, target.y, 0);
        var target3d = Camera.main.ScreenToWorldPoint(screenTarget);
        target3d = Camera.main.transform.InverseTransformPoint(target3d);
        target3d.z = Camera.main.transform.localPosition.z;
        return target3d;
    }

    IEnumerator PunchIn()
    {
        float steps = Mathf.Ceil(duration / Time.deltaTime);
		steps = Mathf.Max (steps, 1);
        var camObj = Camera.main.gameObject;
        orgPos = camObj.transform.localPosition;
        orgFOV = Camera.main.fieldOfView;
        SetTargetFOV();
        var target3d = SetTargetPoint();
        if (fillMode == FillMode.reset)
        {
            target3d = new Vector3(0f, 0f, -2f);
            targetFOV = 60.0f;
        }
        for (int i = 0; i <= steps; i++)
        {
            var lerpVal = curve.Evaluate(i / steps);
            camObj.transform.localPosition = Vector3.Lerp(orgPos, target3d, lerpVal);
            Camera.main.fieldOfView = Mathf.Lerp(orgFOV, targetFOV, lerpVal);
            yield return 0;
        }
    }
}
