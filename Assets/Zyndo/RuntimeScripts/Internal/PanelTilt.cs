using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Animation))]
public class PanelTilt : MonoBehaviour
{

    Quaternion orgRot;
	Vector3 orgAccel;
    Page page;

    // Use this for initialization
    void Start()
    {
        orgRot = transform.localRotation;
        page = GetComponent<Page>();
    }

    // Update is called once per frame
    void Update()
    {
        if (page != PageEventsManager.currentPage)
            return;

        Vector3 diff = Vector3.zero;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            diff = Camera.main.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f, 0.5f);
        }
        if (Input.acceleration.sqrMagnitude > 0.001) {
			diff = orgAccel - Input.acceleration;
		}
		var targetRot = Quaternion.Euler (orgRot.eulerAngles - new Vector3 (diff.y * 10, -diff.x * 10, 0));
		transform.localRotation = Quaternion.Lerp (transform.localRotation, targetRot, Time.deltaTime);
		orgAccel = Vector3.Lerp (orgAccel, Input.acceleration, 0.5f * Time.deltaTime);

    }
}
