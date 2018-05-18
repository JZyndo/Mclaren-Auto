using UnityEngine;
using System.Collections;
using System;

public class DisableAtDistFromCamera : MonoBehaviour {
	public float ActualDist;
	public float DistThreshold = 7f;
	private float FadeTime = .5f;
	private GameObject _pivot;
	private CanvasGroup _pageCG;
	private bool _inTransition = false;
	private float _transToAlpha = 1f;
	void OnEnable()
	{
		PageEventsManager.PageArrival += AssessPosition;
		PageEventsManager.PageDeparture += AssessPosition;
	}

	void OnDisable()
	{
		PageEventsManager.PageArrival -= AssessPosition;
		PageEventsManager.PageDeparture -= AssessPosition;
	}

	// Use this for initialization
	void Start () {
		_pivot = GameObject.Find("Pivot");

		_pageCG = gameObject.GetComponent<CanvasGroup>();
		if (_pageCG == null)
		{		
			_pageCG = gameObject.AddComponent<CanvasGroup>();
		}
		_transToAlpha = _pageCG.alpha;
	}
	
	// Update is called once per frame
	void Update () {
		AssessPosition ();
	}  

	public void AssessPosition(object sender, EventArgs e)
	{
		AssessPosition ();
	}

	public void AssessPosition()
	{
		ActualDist = Vector3.Distance (_pivot.transform.position, transform.position);
		if (ActualDist < DistThreshold && _transToAlpha != 1) {
			if (_inTransition)
			{
				StopAllCoroutines ();
			}
			TuronOffAllMeshRenderers (true);
			StartCoroutine (FadeAlphaTo(1));
		} else if (ActualDist >= DistThreshold && _transToAlpha != 0) {			
			if (_inTransition)
			{
				StopAllCoroutines ();
			}
			TuronOffAllMeshRenderers (false);
			StartCoroutine (FadeAlphaTo(0));
		}
	}

	public IEnumerator FadeAlphaTo(float newAlpha)
	{
		float timeSinceStart = 0;
		float startAlpha = _pageCG.alpha;
		_inTransition = true;
		_transToAlpha = newAlpha;
		while (timeSinceStart < FadeTime)
		{
			timeSinceStart += Time.deltaTime;
			_pageCG.alpha = Mathf.Lerp (startAlpha, newAlpha, timeSinceStart/FadeTime);
			yield return null;
		}
		_inTransition = false;
		_pageCG.alpha = newAlpha;
	}

	public void TuronOffAllMeshRenderers(bool onOffVal)
	{
		MeshRenderer[] rend = GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < rend.Length; i++)
		{
			rend[i].enabled = onOffVal;
		}
	}
}
