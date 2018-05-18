using UnityEngine;
using System.Collections;

public class LookAtMainCamera : MonoBehaviour {
	public Vector3 Up;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (Camera.main.transform.position, Up);
	}
}
