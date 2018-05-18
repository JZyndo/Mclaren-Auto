using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Animation))]
public class CameraGlobalMovement : MonoBehaviour
{
    Page[] pages;
    public int playHead = 0;
    Animation anim;
    // Use this for initialization
    void Start()
    {

        var container = GameObject.Find("Container");
        pages = container.transform.GetComponentsInChildren<Page>();
        anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (PageManager.currentPage == null)
        //    return;
        if (!anim.isPlaying)
        {
            //if (Input.GetKey(KeyCode.LeftShift))
            // {
            //Vector3 diff = Camera.main.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f, 0.5f);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(PageManager.currentPage.transform.rotation.eulerAngles - new Vector3(diff.y * 10, -diff.x * 10, 0)), Time.deltaTime);
            //}
            // else
            //{
            //transform.rotation = Quaternion.Lerp(transform.rotation, pages[playHead].transform.rotation, Time.deltaTime);
            //Debug.Log(pages[playHead].transform.rotation.eulerAngles);
            // }
        }

        //transform.position = new Vector3(0, 0, 0);

    }

    void OnNextPage(Page currPage)
    {
        if (currPage == null)
			return;

        if (anim.GetClip(currPage.gameObject.name) != null)
        {
            anim[currPage.gameObject.name].speed = 1;
            anim.Play(currPage.gameObject.name);
        }
    }

    void OnPreviousPage(Page currPage)
    {
		if (currPage == null)
			return;  

    }

    public static void AddTransformKeyframe(Transform start, Transform end, float startTime, float endTime, ref AnimationClip clip)
    {
        AnimationCurve curve = AnimationCurve.EaseInOut(startTime, start.localPosition.x, endTime, end.localPosition.x);
        clip.SetCurve("", typeof(Transform), "localPosition.x", curve);
        curve = AnimationCurve.EaseInOut(startTime, start.localPosition.y, endTime, end.localPosition.y);
        clip.SetCurve("", typeof(Transform), "localPosition.y", curve);
        curve = AnimationCurve.EaseInOut(startTime, start.localPosition.z, endTime, end.localPosition.z);
        clip.SetCurve("", typeof(Transform), "localPosition.z", curve);
        curve = AnimationCurve.EaseInOut(startTime, start.localRotation.x, endTime, end.localRotation.x);
        clip.SetCurve("", typeof(Transform), "localRotation.x", curve);
        curve = AnimationCurve.EaseInOut(startTime, start.localRotation.y, endTime, end.localRotation.y);
        clip.SetCurve("", typeof(Transform), "localRotation.y", curve);
        curve = AnimationCurve.EaseInOut(startTime, start.localRotation.z, endTime, end.localRotation.z);
        clip.SetCurve("", typeof(Transform), "localRotation.z", curve);
        curve = AnimationCurve.EaseInOut(startTime, start.localRotation.w, endTime, end.localRotation.w);
        clip.SetCurve("", typeof(Transform), "localRotation.w", curve);	
    }
}
