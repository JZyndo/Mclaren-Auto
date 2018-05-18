using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using VRStandardAssets.Utils;
using UnityEngine.UI;
using UnityEngine.VR;

public class VRButton : VRInteractiveItem {
	public UnityEvent OnOverWrapper;             // Called when the gaze moves over this object
	public UnityEvent OnOutWrapper;              // Called when the gaze leaves this object
	public UnityEvent OnClickWrapper;            // Called when click input is detected whilst the gaze is over this object.
	public UnityEvent OnDoubleClickWrapper;      // Called when double click input is detected whilst the gaze is over this object.
	public UnityEvent OnUpWrapper;               // Called when Fire1 is released whilst the gaze is over this object.
	public UnityEvent OnDownWrapper;             // Called when Fire1 is pressed whilst the gaze is over this object.

	private Button button;

	public void OnEnable()
	{
		if (VRSettings.enabled) {
			AddListeners ();
		} else {
			enabled = false;
		}
	}

	public void AddListeners()
	{
		this.OnOver += OnOverWrapper.Invoke;
		this.OnOut += OnOutWrapper.Invoke ;
		this.OnClick += OnClickWrapper.Invoke;
		this.OnDoubleClick += OnDoubleClickWrapper.Invoke;
		this.OnUp += OnUpWrapper.Invoke;
		this.OnDown += OnDownWrapper.Invoke;
	}	

	public void RemoveListeners()
	{
		this.OnOver -= OnOverWrapper.Invoke;
		this.OnOut -= OnOutWrapper.Invoke ;
		this.OnClick -= OnClickWrapper.Invoke;
		this.OnDoubleClick -= OnDoubleClickWrapper.Invoke;
		this.OnUp -= OnUpWrapper.Invoke;
		this.OnDown -= OnDownWrapper.Invoke;
	}
}
