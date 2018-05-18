using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ReticleFeedback : Reticle {
	public UnityEvent OnOver;
	public UnityEvent OnExit;

	private bool _somethingSelected = false;
	// Use this for initialization
	// This overload of SetPosition is used when the the VREyeRaycaster hasn't hit anything.
	public override void SetPosition ()
	{
		base.SetPosition ();
		if (_somethingSelected) {
			Debug.Log ("Reticle: Nothing Selected");
			OnExit.Invoke ();
			_somethingSelected = false;
            EventSystem.current.SetSelectedGameObject(null);
        }
	}


	// This overload of SetPosition is used when the VREyeRaycaster has hit something.
	public override void SetPosition (RaycastHit hit)
	{
		base.SetPosition (hit);
		if (!_somethingSelected) {
			Debug.Log ("reticle: Something Selected");	
			OnOver.Invoke ();
			_somethingSelected = true;

            EventSystem.current.SetSelectedGameObject(hit.transform.gameObject);
		}
	}
}
