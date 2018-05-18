using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPanelOperations : MonoBehaviour
{

    public static float duration = 0.2f;
    public static AnimationCurve curve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleActive()
    {
        if(this.gameObject.activeSelf)
        {
            StartCoroutine(FadeOff());
        }
        else
        {
            this.gameObject.SetActive(true);
            StartCoroutine(FadeOn());
        }
    }

    IEnumerator FadeOff()
    {
        float steps = Mathf.Ceil(duration / Time.deltaTime);
        steps = Mathf.Max(steps, 1);
        var image = GetComponent<CanvasGroup>();
        if (image != null)
        {
            var orgCol = image.GetAlpha();
            for (int i = 0; i <= steps; i++)
            {
                var lerpVal = curve.Evaluate(i / steps);
                image.SetAlpha(Mathf.Lerp(orgCol, 0.0f, lerpVal));
                yield return 0;
            }
        }

        this.gameObject.SetActive(false);
    }

    IEnumerator FadeOn()
    {
        float steps = Mathf.Ceil(duration / Time.deltaTime);
        steps = Mathf.Max(steps, 1);
        var image = GetComponent<CanvasGroup>();
        if (image != null)
        {
            image.SetAlpha(0.0f);
            for (int i = 0; i <= steps; i++)
            {
                var lerpVal = curve.Evaluate(i / steps);
                image.SetAlpha(Mathf.Lerp(0.0f, 1.0f, lerpVal));
                yield return 0;
            }
        }
    }
}
