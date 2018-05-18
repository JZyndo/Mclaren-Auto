using UnityEngine;
using System;
using System.Collections;

public class Zoom : MonoBehaviour
{

    public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.
    public AnimationCurve curve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
    void OnEnable()
    {
        PageEventsManager.PageDeparture += Reset;
    }

    void OnDisable()
    {
        PageEventsManager.PageDeparture -= Reset;
    }

    void Reset(object sender, EventArgs args)
    {
        StartCoroutine(LerpFOV(60.0f)); 
    }

    void Update()
    {
        ProcessTouchZoom();
        ProcessScrollZoom();
    }

    void ProcessScrollZoom()
    {
        if (Camera.main.orthographic)
        {
            // ... change the orthographic size based on the change in distance between the touches.
            Camera.main.orthographicSize += Input.mouseScrollDelta.y;

            // Make sure the orthographic size never drops below zero.
            Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, 0.1f);
        }
        else
        {
            // Otherwise change the field of view based on the change in distance between the touches.
            Camera.main.fieldOfView += Input.mouseScrollDelta.y * perspectiveZoomSpeed;

            // Clamp the field of view to make sure it's between 0 and 180.
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 40.0f, 80.0f);
        }
    }

    void ProcessTouchZoom()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // If the camera is orthographic...
            if (Camera.main.orthographic)
            {
                // ... change the orthographic size based on the change in distance between the touches.
                Camera.main.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                // Make sure the orthographic size never drops below zero.
                Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, 0.1f);
            }
            else
            {
                // Otherwise change the field of view based on the change in distance between the touches.
                Camera.main.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                // Clamp the field of view to make sure it's between 0 and 180.
                Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 40.0f, 80.0f);
            }
        }
    }

    IEnumerator LerpFOV(float target)
    {
        //continue with fade
        var steps = Mathf.Ceil(1.0f / Time.deltaTime);
        steps = Mathf.Max(steps, 1);

        var orgVal = Camera.main.fieldOfView;

        //do the yielding loop
        for (int i = 0; i <= steps; i++)
        {
            var lerpVal = curve.Evaluate(i / steps);
            Camera.main.fieldOfView = Mathf.Lerp(orgVal, target, lerpVal);
            yield return 0;
        }
    }
}

