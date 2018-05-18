using UnityEngine;
using System.Collections;

public class CameraTilt : MonoBehaviour
{

    Quaternion orgRot;
    Vector3 orgAccel;

    // Use this for initialization
    void Start()
    {
        orgRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 diff = Vector3.zero;

        //make sure there is a page, if it has overrides then if it has tiltcam turned off
        if (PageEventsManager.currentPage != null && PageEventsManager.currentPage.GetComponent<PageOverrides>() && PageEventsManager.currentPage.GetComponent<PageOverrides>().OverridePageTilt && !PageEventsManager.currentPage.GetComponent<PageOverrides>().PageTiltOn)
        {
            return;
        }

        if (PageEventsManager.currentPage != null)
        {
            orgRot = PageEventsManager.currentPage.transform.localRotation;
        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            diff = Camera.main.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f, 0.5f);
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            var targetRot = Quaternion.Euler(orgRot.eulerAngles - new Vector3(diff.y * 10, -diff.x * 10, 0));
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime);
            orgAccel = Vector3.Lerp(orgAccel, Input.acceleration, 0.5f * Time.deltaTime);
        }
        
        if (Input.acceleration.sqrMagnitude > 0.001)
        {
            diff = orgAccel - Input.acceleration;
        }

        if (!PageEventsManager.busy && !PageEventsManager.disableEvents)
        {
            var targetRot = Quaternion.Euler(orgRot.eulerAngles - new Vector3(diff.y * 10, -diff.x * 10, 0));
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime);
            orgAccel = Vector3.Lerp(orgAccel, Input.acceleration, 0.5f * Time.deltaTime);
        }

    }
}
