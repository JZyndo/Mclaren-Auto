using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class InOutFade : MonoBehaviour {

	public float delay = 0.0f;
	public float randomize = 0.0f;
	public float startAlpha = 0.0f;
	public float endAlpha = 1.0f;
	public float duration = 1.0f;
	public int rank;
	public LinkedEvent linkedEvent = LinkedEvent.None;
    public AnimationCurve curve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);

    private Page page;

	void OnEnable()
	{
		PageEventsManager.PageArrival += FadeIn;
		PageEventsManager.PageDeparture += FadeOut;
	}

	void OnDisable()
	{
		PageEventsManager.PageArrival -= FadeIn;
		PageEventsManager.PageDeparture -= FadeOut;
	}
	// Use this for initialization
	void Start () {
		randomize = Mathf.Clamp(randomize, 0.0f, 1.0f);
		page = PageEventBase.FindPage(this.gameObject);
		SetAlpha(startAlpha);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	List<Component> GetAllFadeableComponents(GameObject gameObject)
	{
		var componentsOut = new List<Component>();

		var allComps = gameObject.GetComponents<Component>();
		var allChildComps = gameObject.GetComponentsInChildren<Component>();

		var fullList = new List<Component>(allComps);
		fullList.AddRange(new List<Component>(allChildComps));

		foreach(var c in fullList)
		{
			var type = c.GetType();
			if(c != null && type == typeof(Image) ||
				type == typeof(MeshRenderer) ||
				type == typeof(Text) ||
				type == typeof(TextMeshProUGUI))
			{
				componentsOut.Add(c);
			}
		}

		return componentsOut;
	}

	void SetAlpha(float alpha)
	{
		//fade this object
		var fadeComps = GetAllFadeableComponents(this.gameObject);
		foreach(var c in fadeComps)
		{
			c.SetAlphaGeneric(alpha);
		}
	}

	void FadeOut(object sender, EventArgs e)
	{
		if (page != PageEventsManager.currentPage)
			return;
		
		//fade this object
		var fadeComps = GetAllFadeableComponents(this.gameObject);
		foreach(var c in fadeComps)
		{
			StartCoroutine(LerpAlpha(c, startAlpha));
		}
	}

	void FadeIn(object sender, EventArgs e)
	{
		if (page != PageEventsManager.currentPage)
			return;
		
		//fade this object
		var fadeComps = GetAllFadeableComponents(this.gameObject);
		foreach(var c in fadeComps)
		{
			StartCoroutine(LerpAlpha(c, endAlpha));
		}
	}

	IEnumerator LerpAlpha(Component componentToFade, float tAlpha)
	{
		float randomWait = UnityEngine.Random.Range(-randomize * duration, randomize * duration);
		yield return new WaitForSeconds(delay + randomWait);

		if (componentToFade != null)
        {
            //continue with fade
            var steps = Mathf.Ceil(duration / Time.deltaTime);
            steps = Mathf.Max(steps, 1);

            var orgVal = componentToFade.GetAlphaGeneric();

            //do the yielding loop
            for (int i = 0; i <= steps; i++)
            {
                var lerpVal = curve.Evaluate(i / steps);
                var alpha = Mathf.Lerp(orgVal, tAlpha, lerpVal);
                componentToFade.SetAlphaGeneric(alpha);
				//Debug.Log ("ASDFASDFASDF");
                yield return 0;
            }
        }
	}
}
