using UnityEngine;
using System.Collections;
using System;
using Jacovone;

[ExecuteInEditMode]
[RequireComponent(typeof(PathMagic))]
public class PanelTransition : PageEventBase
{
    public PathMagic path;
    public int connectedID;
    public bool useDuration = true;
    bool moving = false;
    void Start()
    {
        //path = GetComponent<BezierSpline>();
        if (path == null)
        {
            Debug.LogError("Panel Transition with no Path component! Page: " + page.name);
            this.enabled = false;
        }
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            UpdateSpline(false);
        }
    }

    public override void ForceEvent()
    {
        if (page.connectedPages.Count > connectedID)
        {
            if (useDuration)
                StartCoroutine(MoveAlongPath());
            else
                StartCoroutine(PlayPath());
        }
        else
            Debug.LogError("Panel Transition is reference a non existent Page Connection! Page name: " + page.name);
    }

    public override void ProcessEvent()
    {
        if (callerID == connectedID)
        {
            if (page.connectedPages.Count > connectedID)
            {
                if (useDuration)
                    StartCoroutine(MoveAlongPath());
                else
                    StartCoroutine(PlayPath());
            }

            else
                Debug.LogError("Panel Transition is reference a non existent Page Connection! Page name: " + page.name);
        }
    }

    public override void OnCallerID(object sender, CallerIDArgs e)
    {
        base.OnCallerID(sender, e);
    }

    public void UpdateSpline(bool resetTangents, bool rounded = false)
    {
        if (path == null || page.connectedPages.Count <= connectedID)
            return;

        //check if this path is new
		if (path.waypoints.Length == 0)
        {
            var startPoint = new Waypoint();
            startPoint.position = Vector3.zero;
            startPoint.rotation = this.page.transform.rotation.eulerAngles;
            var endPoint = new Waypoint();
            Quaternion newLocalRot = page.connectedPages[connectedID].page.transform.rotation;
            endPoint.rotation = newLocalRot.eulerAngles;
            endPoint.position = this.transform.InverseTransformPoint(page.connectedPages[connectedID].page.transform.position);
			Waypoint[] wps = new Waypoint[2];
			wps[0] = startPoint;
			wps[1] = endPoint;
			path.waypoints = wps;
			Debug.Log ("Hit Critical code");
        }
        else
        {
            path.Waypoints[0].position = Vector3.zero;
            path.Waypoints[0].rotation = this.page.transform.rotation.eulerAngles;
			path.Waypoints[path.Waypoints.Length - 1].position = this.transform.InverseTransformPoint(page.connectedPages[connectedID].page.transform.position);
            Quaternion newLocalRot = page.connectedPages[connectedID].page.transform.rotation;
			path.Waypoints[path.Waypoints.Length - 1].rotation = newLocalRot.eulerAngles;
        }

        if (resetTangents)
        {
            var nextPos = this.transform.InverseTransformPoint(page.connectedPages[connectedID].page.transform.position);
            if (rounded)
            {
                //path.SetControlPoint(1, 0.1f * next_pos - 2f * this.transform.forward);
                //path.SetControlPoint(2, 0.9f * next_pos - 2f * this.transform.forward);
            }
            else
            {
                path.Waypoints[0].inTangent = -0.1f * nextPos;
                path.Waypoints[0].outTangent = 0.1f * nextPos;
				path.Waypoints[path.Waypoints.Length - 1].inTangent = -0.1f * nextPos;
				path.Waypoints[path.Waypoints.Length - 1].outTangent = 0.1f * nextPos;
            }
        }
    }

    IEnumerator MoveAlongPath()
	{			
		if (TapZoom.Instance != null) {
			yield return TapZoom.Instance.OnCameraMove ();
		}
        float steps = Mathf.Ceil(duration / Time.deltaTime);
        steps = Mathf.Max(steps, 1);

        var pivot = PageEventsManager.pivot;
        var orgPos = pivot.transform.position;
        var orgRot = pivot.transform.localRotation;
        var targetForward = page.connectedPages[connectedID].page.transform.forward;
        var targetRot = page.connectedPages[connectedID].page.transform.localRotation;
        float targetFOV = Camera.main.fieldOfView;
        float lastFOV = targetFOV;

        var overrides = page.connectedPages[connectedID].page.gameObject.GetComponent<PageOverrides>();
        if (overrides != null && overrides.OverrideCameraFOV)
            targetFOV = overrides.CameraFOV;

        moving = true;
        PageEventsManager.busy = true;

        //set the path start frame to the pivot's frame
        path.Waypoints[0].position = this.transform.InverseTransformPoint(orgPos);
        path.Waypoints[0].rotation = orgRot.eulerAngles;

        for (int i = 0; i <= steps; i++)
        {
            var lerpVal = curve.Evaluate(i / steps);
            var velocity = path.computeVelocityAtPos(lerpVal);
            var new_pos = this.transform.TransformPoint(path.computePositionAtPos(lerpVal));
            Camera.main.fieldOfView = Mathf.Lerp(lastFOV, targetFOV, i / steps);
            pivot.transform.position = new_pos; // Vector3.Lerp(pivot.transform.position, new_pos, 0.5f);
            pivot.transform.localRotation = path.computeRotationAtPos(lerpVal); //Quaternion.Lerp(pivot.transform.localRotation, path.computeRotationAtPos(lerpVal), 0.5f);
            yield return 0;
        }

        Camera.main.fieldOfView = targetFOV;

        PageEventsManager.busy = false;
        moving = false;
    }

    IEnumerator PlayPath()
	{
		if (TapZoom.Instance != null)
		{
			yield return TapZoom.Instance.OnCameraMove ();
		}
        float steps = Mathf.Ceil(duration / Time.deltaTime);
        steps = Mathf.Max(steps, 1);
        var pivot = PageEventsManager.pivot;
        PageEventsManager.busy = true;
        float lastVelocity = 0.0f; ;
        float currentPos = 0.0f;

        //set the path start frame to the pivot's frame
        path.Waypoints[0].position = this.transform.InverseTransformPoint(pivot.transform.position);
        path.Waypoints[0].rotation = pivot.transform.localRotation.eulerAngles;

        float targetFOV = Camera.main.fieldOfView;
        float lastFOV = targetFOV;

        var overrides = page.connectedPages[connectedID].page.gameObject.GetComponent<PageOverrides>();
        if (overrides != null && overrides.OverrideCameraFOV)
            targetFOV = overrides.CameraFOV;

        while (currentPos <= 1.0f)
        {
            float advance = (0.5f * path.velocityBias * lastVelocity * Time.deltaTime);
            currentPos += advance;
            //var lerpVal = curve.Evaluate(i / steps);
            var velocity = path.computeVelocityAtPos(currentPos);
            var new_pos = this.transform.TransformPoint(path.computePositionAtPos(currentPos));
            var rot = path.computeRotationAtPos(currentPos);
            Camera.main.fieldOfView = Mathf.Lerp(lastFOV, targetFOV, 1f / currentPos);

            pivot.transform.position = new_pos;
            pivot.transform.localRotation = rot;

            lastVelocity = velocity;

            yield return 0;
        }

        pivot.transform.position = this.transform.TransformPoint(path.computePositionAtPos(1.0f));
        pivot.transform.localRotation = path.computeRotationAtPos(1.0f); ;
        Camera.main.fieldOfView = targetFOV;

        PageEventsManager.busy = false;
        Debug.Log("end time: " + Time.timeSinceLevelLoad);
    }
}