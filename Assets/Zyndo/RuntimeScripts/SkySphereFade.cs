using UnityEngine;
using System.Collections;

public class SkySphereFade : PageEventBase
{
    public Material skySphere;

    private Material skyboxProto;
    private Cubemap skyMap;
    // Use this for initialization
    void Start()
    {

        skyboxProto = RenderSettings.skybox;
        skyMap = (Cubemap)skySphere.GetTexture("_Tex");

    }

    public override void ForceEvent()
    {
        base.ForceEvent();
        ProcessEvent();
    }

    public override void ProcessEvent()
    {
        Debug.Log("skybox transition...");

        //move last set in to first position
        skyboxProto.SetTexture("_SkyTex", skyboxProto.GetTexture("_SkyTex2"));
        //move next set in to second position
        skyboxProto.SetTexture("_SkyTex2", skyMap);
        //do the blend
        skyboxProto.SetFloat("_Blend", 0.0f);
        StartCoroutine(FadeSkySphere());

    }

    IEnumerator FadeSkySphere()
    {
        float steps = Mathf.Ceil(duration / Time.deltaTime);
        steps = Mathf.Max(steps, 1);
        for (int i = 0; i <= steps; i++)
        {
            var lerpVal = curve.Evaluate(i / steps);
            skyboxProto.SetFloat("_Blend", lerpVal);
            yield return 0;
        }
    }
}
