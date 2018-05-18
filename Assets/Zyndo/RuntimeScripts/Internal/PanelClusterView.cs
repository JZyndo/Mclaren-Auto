using UnityEngine;
using System.Collections;


public class PanelClusterView : MonoBehaviour
{

    GameObject[] allPanels;
    GameObject[] orgTransforms;
    GameObject[] targetTransforms;

    Vector3 orgPos;

    bool init = false;

    public float panelSpacing = 100.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            if (!init)
            {
                var pagesObj = GameObject.Find("Pages");
                int childCount = pagesObj.transform.childCount;
                allPanels = new GameObject[childCount];
                orgTransforms = new GameObject[childCount];
                targetTransforms = new GameObject[childCount];
                for (int i = 0; i < childCount; i++)
                {
                    GameObject currObj = pagesObj.transform.GetChild(i).gameObject;
                    allPanels[i] = currObj;
                    orgTransforms[i] = new GameObject("org_g_" + i);
                    orgTransforms[i].transform.position = currObj.transform.position;
                    orgTransforms[i].transform.rotation = currObj.transform.rotation;

                }

                SetTargetTransforms();

                orgPos = this.transform.position;

                init = true;
            }

            LerpTransforms(targetTransforms);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            SetOrgTransforms();
            init = false;
        }


    }

    void SetOrgTransforms()
    {
        for (int i = 0; i < allPanels.Length; i++)
        {
            allPanels[i].transform.position = orgTransforms[i].transform.position;
        }

        for (int i = 0; i < allPanels.Length; i++)
        {
            Destroy(orgTransforms[i]);
            Destroy(targetTransforms[i]);
        }

        this.transform.position = orgPos;
    }

    void SetTargetTransforms()
    {
        int cols = 6;
        int rows = Mathf.CeilToInt(allPanels.Length / (float)cols);

        targetTransforms = new GameObject[allPanels.Length];

        for (int i = 0; i < allPanels.Length; i++)
        {
            var x = (i % cols) * panelSpacing;
            var y = Mathf.CeilToInt(i / (float)cols) * panelSpacing;
            var pos = new Vector3(x, y, 0);
            var rot = Quaternion.identity;

            targetTransforms[i] = new GameObject("target_" + i);
            targetTransforms[i].transform.position = pos;

        }
    }


    void LerpTransforms(GameObject[] targets)
    {
        Vector3 avgPos = Vector3.zero;
        for (int i = 0; i < allPanels.Length; i++)
        {
            allPanels[i].transform.position =
                Vector3.Lerp(allPanels[i].transform.position,
                targets[i].transform.position,
                1.0f * Time.deltaTime);

            avgPos += allPanels[i].transform.position;

           //allPanels[i].transform.LookAt(this.transform.position);
        }

        avgPos /= Mathf.Max(1.0f, allPanels.Length);
        avgPos.z = -10.0f;

        this.transform.position = Vector3.Lerp(this.transform.position, avgPos, 1.0f * Time.deltaTime);
    }
}
