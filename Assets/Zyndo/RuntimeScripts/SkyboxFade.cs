using UnityEngine;
using System.Collections;
using System;

public class SkyboxFade : PageEventBase
{
	public Material skybox;
	public bool UseAmbientColor = false;
	public Color AmbientColor;
    private Material skyboxProto;
    private Texture front;
    private Texture back;
    private Texture up;
    private Texture down;
    private Texture left;
    private Texture right;

    // Use this for initialization
    void Start()
	{	

        //load the blendable skybox material (i.e. the one that is actually being render)
        skyboxProto = RenderSettings.skybox;

        front = skybox.GetTexture("_FrontTex");
        back = skybox.GetTexture("_BackTex");
        up = skybox.GetTexture("_UpTex");
        down = skybox.GetTexture("_DownTex");
        left = skybox.GetTexture("_LeftTex");
		right = skybox.GetTexture("_RightTex");		

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ForceEvent()
    {
        base.ForceEvent();
        ProcessEvent();
    }

    public override void ProcessEvent()
    {
        Debug.Log("skybox transition...");

		Debug.Log("start time1: " + Time.timeSinceLevelLoad);
        //move last set in to first position
        skyboxProto.SetTexture("_FrontTex", skyboxProto.GetTexture("_FrontTex2"));
        skyboxProto.SetTexture("_BackTex", skyboxProto.GetTexture("_BackTex2"));
        skyboxProto.SetTexture("_LeftTex", skyboxProto.GetTexture("_LeftTex2"));
        skyboxProto.SetTexture("_RightTex", skyboxProto.GetTexture("_RightTex2"));
        skyboxProto.SetTexture("_UpTex", skyboxProto.GetTexture("_UpTex2"));
        skyboxProto.SetTexture("_DownTex", skyboxProto.GetTexture("_DownTex2"));

        //move next set in to second position
        skyboxProto.SetTexture("_FrontTex2", front);
        skyboxProto.SetTexture("_BackTex2", back);
        skyboxProto.SetTexture("_LeftTex2", left);
        skyboxProto.SetTexture("_RightTex2", right);
        skyboxProto.SetTexture("_UpTex2", up);
        skyboxProto.SetTexture("_DownTex2", down);

        skyboxProto.SetFloat("_Blend", 0.0f);

		Debug.Log("start time2: " + Time.timeSinceLevelLoad);
        StartCoroutine(FadeSkybox());
    }

    IEnumerator FadeSkybox()
	{
		ReflectionProbe refP = GameObject.Find ("Pivot").GetComponentInChildren<ReflectionProbe>();
		Debug.Log("fade start: " + Time.timeSinceLevelLoad);
		float steps = Mathf.Ceil(duration / Time.deltaTime);
		steps = Mathf.Max (steps, 1);
		Debug.Log (steps, gameObject);
        var oCol = skyboxProto.GetColor("_Tint");
        var oRot = skyboxProto.GetFloat("_Rotation");
        var oExp = skyboxProto.GetFloat("_Exposure");

        var tCol = skybox.GetColor("_Tint");
        var tRot = skybox.GetFloat("_Rotation");
        var tExp = skybox.GetFloat("_Exposure");
		float startTime = Time.time;

		Color startRenderColor = RenderSettings.ambientLight;
		while (Time.time - startTime < duration)
		{
			if (refP != null) {
				refP.RenderProbe ();
			}
			var lerpVal = curve.Evaluate((Time.time - startTime) / duration);

            var col = Color.Lerp(oCol, tCol, lerpVal);
            var rot = Mathf.Lerp(oRot, tRot, lerpVal);
            var exp = Mathf.Lerp(oExp, tExp, lerpVal);

            skyboxProto.SetColor("_Tint", col);
            skyboxProto.SetFloat("_Rotation", rot);
            skyboxProto.SetFloat("_Exposure", exp);
            skyboxProto.SetFloat("_Blend", lerpVal);
			if (UseAmbientColor)
			{
				RenderSettings.ambientLight = Color.Lerp (startRenderColor, AmbientColor, lerpVal);
			}
			yield return 0;
		}

		var lerpValLast = 1f;

		var colLast = Color.Lerp(oCol, tCol, lerpValLast);
		var rotLast = Mathf.Lerp(oRot, tRot, lerpValLast);
		var expLast = Mathf.Lerp(oExp, tExp, lerpValLast);

		skyboxProto.SetColor("_Tint", colLast);
		skyboxProto.SetFloat("_Rotation", rotLast);
		skyboxProto.SetFloat("_Exposure", expLast);
		skyboxProto.SetFloat("_Blend", lerpValLast);
		Debug.Log("fade end: " + Time.timeSinceLevelLoad);
		Debug.Log ("FadeSkybox End", gameObject);
		//DynamicGI.UpdateEnvironment ();
    }

	public void SetAsStartingSkybox()
	{
		//load the blendable skybox material (i.e. the one that is actually being render)
		GameObject.Find("Main").GetComponent<StartUp>().StartingSkybox = skybox;
		if (UseAmbientColor)
		{
			GameObject.Find("Main").GetComponent<StartUp>().StartingAmbientColor = AmbientColor;
		}
	}

	public void SetToSkybox()
	{
		if (skybox == null)
		{
			return;
		}
		//load the blendable skybox material (i.e. the one that is actually being render)
		skyboxProto = RenderSettings.skybox;

		front = skybox.GetTexture("_FrontTex");
		back = skybox.GetTexture("_BackTex");
		up = skybox.GetTexture("_UpTex");
		down = skybox.GetTexture("_DownTex");
		left = skybox.GetTexture("_LeftTex");
		right = skybox.GetTexture("_RightTex");	

		//move next set in to second position
		skyboxProto.SetTexture("_FrontTex2", front);
		skyboxProto.SetTexture("_BackTex2", back);
		skyboxProto.SetTexture("_LeftTex2", left);
		skyboxProto.SetTexture("_RightTex2", right);
		skyboxProto.SetTexture("_UpTex2", up);
		skyboxProto.SetTexture("_DownTex2", down);

		var tCol = skybox.GetColor("_Tint");
		var tRot = skybox.GetFloat("_Rotation");
		var tExp = skybox.GetFloat("_Exposure");

		skyboxProto.SetColor("_Tint", tCol);
		skyboxProto.SetFloat("_Rotation", tRot);
		skyboxProto.SetFloat("_Exposure", tExp);

		skyboxProto.SetFloat("_Blend", 1);
		SetToAmbientLight ();
	}

	public void SetToAmbientLight()
	{
		if (UseAmbientColor)
		{
			RenderSettings.ambientLight = AmbientColor;
		}
	}
}
