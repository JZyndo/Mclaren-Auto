using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class ParallaxTrigger : MonoBehaviour {

    public float upThreshold = 0.5f, downThreshold = -0.5f, leftThreshold = -0.5f, rightThreshold = 0.5f;
    public PageEventBase upEvent, downEvent, leftEvent, rightEvent;

    private bool upFired, downFired, leftFired, rightFired;

    private Vector3 org_accel;
    Vector3 orig, origRot;

    // Update is called once per frame
    void Update()
    {
        bool _parallaxActive = true;
        Vector3 diff = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.LeftShift) || (ParallaxSettings.instance.ParallaxPauseInput != "" && Input.GetAxis(ParallaxSettings.instance.ParallaxPauseInput) == 1))
        {
            _parallaxActive = false;
        }
        if (!VRSettings.enabled)
        {
            if (Input.GetKey(KeyCode.RightShift) || (ParallaxSettings.instance.ParallaxToZeroInput != "" && Input.GetAxis(ParallaxSettings.instance.ParallaxToZeroInput) == 1))
            {
                diff = org_accel - Input.acceleration;

                org_accel = Vector3.Lerp(org_accel, Input.acceleration, 0.5f * Time.deltaTime);
            }
            else if (_parallaxActive)
            {
                diff = Camera.main.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f, 0.5f);                
            }
        }
        else
        {
            if (PageEventsManager.currentPage != null)
            {
                if (Input.GetKey(KeyCode.RightShift) || (ParallaxSettings.instance.ParallaxToZeroInput != "" && Input.GetAxis(ParallaxSettings.instance.ParallaxToZeroInput) == 1))
                {
                    diff = Vector2.zero;
                }
                else if (_parallaxActive)
                {
                    var pageXVec = PageEventsManager.currentPage.transform.right;
                    var pageYVec = PageEventsManager.currentPage.transform.up;
                    diff = Vector3.zero;
                    diff.z = -0.5f;
                    diff.x = Vector3.Dot(Camera.main.transform.forward, pageXVec);
                    diff.y = Vector3.Dot(Camera.main.transform.forward, pageYVec);                                    
                }
            }
        }

        if (diff == Vector3.zero)
            return;

        //now check diff to see if we need to trigger any events
        if (!upFired && diff.y > upThreshold)
        {
            if (upEvent)
                upEvent.ProcessEvent();
            upFired = true;
            Debug.Log("UpFired");
        }
        if (!downFired && diff.y < downThreshold)
        {
            if (downEvent)
                downEvent.ProcessEvent();
            downFired = true;
            Debug.Log("downFired");
        }
        if (!leftFired && diff.x < leftThreshold)
        {
            if (leftEvent)
                leftEvent.ProcessEvent();
            leftFired = true;
            Debug.Log("leftFired");
        }
        if (!rightFired && diff.x > rightThreshold)
        {
            if (rightEvent)
                rightEvent.ProcessEvent();
            rightFired = true;
            Debug.Log("rightFired");
        }



    }
}
