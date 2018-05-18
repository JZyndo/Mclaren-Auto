using UnityEngine;
using UnityEngine.VR;
using System.Collections;
using System;

[Serializable]
public class Parallax : MonoBehaviour
{
    public float parallaxFactor;
    public Vector2 parallaxAxisMultiplier = Vector2.one;
    public Vector2 parallaxRotation = Vector2.zero;
    Vector3 orig, origRot;
    GameObject obj;
    GameObject targetObject;
    string animationName;
    private Vector3 org_accel;
    private Vector3 last_rot;
    private Vector3 last_pos;

    private StartUp startUpScript;
    private bool _parallaxActive = true;
    Page pageParent;

    void Start()
    {
        Transform tmpParent = this.transform;

        //get the page above this parallax just in case it is nested
        while (tmpParent != null && pageParent == null)
        {
            tmpParent = tmpParent.parent;
            pageParent = tmpParent.GetComponent<Page>();
        }

        if (pageParent == null)
        {
            Debug.LogError("Parallax script must be under a page.");
        }

        //put the orig position in terms of the page parent
        orig = pageParent.transform.InverseTransformPoint(this.transform.position);
        origRot = pageParent.transform.InverseTransformDirection(this.transform.eulerAngles);
        org_accel = Input.acceleration;
        last_rot = Vector3.zero;
        last_pos = transform.position;

        startUpScript = GameObject.Find("Main").GetComponent<StartUp>();

        if (Application.isMobilePlatform)
        {
            parallaxFactor *= GameObject.Find("Main").GetComponent<StartUp>().parallaxMobileMultiplier;
        }
    }

    // Update is called once per frame
    public void CalculatePosition()
    {
        if (pageParent != PageEventsManager.currentPage)
            return;

        var currentParallaxFactor = 1.0f;
        _parallaxActive = true;

        if (Input.GetKeyDown(KeyCode.LeftShift) || (ParallaxSettings.instance.ParallaxPauseInput != "" && Input.GetAxis(ParallaxSettings.instance.ParallaxPauseInput) == 1))
        {
            _parallaxActive = false;
        }
        if (!VRSettings.enabled)
        {
            if (Input.GetKey(KeyCode.RightShift) || (ParallaxSettings.instance.ParallaxToZeroInput != "" && Input.GetAxis(ParallaxSettings.instance.ParallaxToZeroInput) == 1))
            {
                Vector3 diff = org_accel - Input.acceleration;

                diff.x = Mathf.Clamp(diff.x, -.5f, .5f);
                diff.y = Mathf.Clamp(diff.y, -.5f, .5f);
                
                //diff.y = -1.0f - diff.y;//fix y to be perp by default
                diff.z = 0.0f;
                Vector3 newPos;
                newPos.x = orig.x - (diff.x * parallaxFactor * parallaxAxisMultiplier.x * -ParallaxSettings.instance.ParallaxMulti * currentParallaxFactor);
                newPos.y = orig.y - (diff.y * parallaxFactor * parallaxAxisMultiplier.y * -ParallaxSettings.instance.ParallaxMulti * currentParallaxFactor);
                newPos.z = 0;

                Vector3 newRot;
                newRot.x = origRot.x - (diff.x * parallaxRotation.x);
                newRot.y = origRot.y - (diff.y * parallaxRotation.y);
                newRot.z = 0;

                //convert the page parent local space to world space
                newPos = pageParent.transform.TransformPoint(newPos);
                newRot = pageParent.transform.TransformDirection(newRot);

                transform.position = Vector3.Lerp(last_pos, newPos, Time.deltaTime);

                if (parallaxRotation.magnitude != 0)
                    transform.eulerAngles = newRot;

                //Debug.Log(diff);
                org_accel = Vector3.Lerp(org_accel, Input.acceleration, 0.5f * Time.deltaTime);
            }
            else if (_parallaxActive)
            {
                Vector3 diff = Camera.main.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f, 0.5f);


                if (Input.acceleration.magnitude > 0.0001)
                {
                    diff = org_accel - Input.acceleration;
                    //diff.y = -1.0f - diff.y;//fix y to be perp by default
                    diff.z = 0.0f;

                    org_accel = Vector3.Lerp(org_accel, Input.acceleration, 0.5f * Time.deltaTime);
                }

                diff.x = Mathf.Clamp(diff.x, -.5f, .5f);
                diff.y = Mathf.Clamp(diff.y, -.5f, .5f);

                //calculate new pos in parent local space
                Vector3 newPos = Vector3.Lerp(pageParent.transform.InverseTransformPoint(last_pos), orig - (new Vector3(diff.x * parallaxFactor * parallaxAxisMultiplier.x, diff.y * parallaxFactor * parallaxAxisMultiplier.y, diff.z) * -ParallaxSettings.instance.ParallaxMulti * currentParallaxFactor), Time.deltaTime);

                Debug.Log(parallaxFactor + ": " + (-ParallaxSettings.instance.ParallaxMulti * currentParallaxFactor));

                //to world space
                newPos = pageParent.transform.TransformPoint(newPos);

                Vector3 newRot;
                newRot.x = origRot.x - (diff.y * parallaxRotation.x);
                newRot.y = origRot.y - (diff.x * parallaxRotation.y);
                newRot.z = 0;

                newRot = pageParent.transform.TransformDirection(newRot);

                transform.position = newPos;

                if (parallaxRotation.magnitude != 0)
                    transform.eulerAngles = newRot;

            }
        }
        else
        {
            if (PageEventsManager.currentPage != null)
            {
                if (Input.GetKey(KeyCode.RightShift) || (ParallaxSettings.instance.ParallaxToZeroInput != "" && Input.GetAxis(ParallaxSettings.instance.ParallaxToZeroInput) == 1))
                {
                    Vector3 newPos = orig;
                    newPos.z = 0;

                    //convert the page parent local space to world space
                    newPos = pageParent.transform.TransformPoint(newPos);
                    Vector3 newRot = pageParent.transform.TransformDirection(origRot);

                    transform.position = Vector3.Lerp(last_pos, newPos, Time.deltaTime);

                    if (parallaxRotation.magnitude != 0)
                        transform.eulerAngles = newRot;
                }
                else if (_parallaxActive)
                {
                    var pageXVec = PageEventsManager.currentPage.transform.right;
                    var pageYVec = PageEventsManager.currentPage.transform.up;
                    var diff = Vector3.zero;
                    diff.z = -0.5f;
                    diff.x = Vector3.Dot(Camera.main.transform.forward, pageXVec);
                    diff.y = Vector3.Dot(Camera.main.transform.forward, pageYVec);

                    diff.x = Mathf.Clamp(diff.x, -.5f, .5f);
                    diff.y = Mathf.Clamp(diff.y, -.5f, .5f);

                    Vector3 newPos = orig - (new Vector3(diff.x * parallaxFactor * parallaxAxisMultiplier.x, diff.y * parallaxFactor * parallaxAxisMultiplier.y, diff.z) * -ParallaxSettings.instance.ParallaxMulti * currentParallaxFactor);

                    //convert the page parent local space to world space
                    newPos = pageParent.transform.TransformPoint(newPos);

                    Vector3 newRot;
                    newRot.x = origRot.x - (diff.y * parallaxRotation.x);
                    newRot.y = origRot.y - (diff.x * parallaxRotation.y);
                    newRot.z = 0;

                    newRot = pageParent.transform.TransformDirection(newRot);

                    transform.position = Vector3.Lerp(last_pos, newPos, Time.deltaTime);

                    if (parallaxRotation.magnitude != 0)
                        transform.eulerAngles = newRot;
                }
            }
        }

        last_pos = transform.position;

    }
}

