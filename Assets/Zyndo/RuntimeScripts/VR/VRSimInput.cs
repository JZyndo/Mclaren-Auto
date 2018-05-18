using UnityEngine;
using System.Collections;

public class VRSimInput : MonoBehaviour {
	public float SpeedX = 50; 
	public float SpeedY = 50; 

	public void Update(){
		if (Input.GetKey(KeyCode.LeftControl))
		{
			transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * SpeedY, Input.GetAxis("Mouse X") * SpeedX, 0) * Time.deltaTime);
		}
	}
}
