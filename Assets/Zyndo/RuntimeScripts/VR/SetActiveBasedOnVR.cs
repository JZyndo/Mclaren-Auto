using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class SetActiveBasedOnVR : MonoBehaviour {
	public bool Reverse = false;
	// Use this for initialization
	void Start () {
		if (!Reverse) {
			gameObject.SetActive (VRSettings.enabled);
		} else {
			gameObject.SetActive (!VRSettings.enabled);
		}
	}
}
