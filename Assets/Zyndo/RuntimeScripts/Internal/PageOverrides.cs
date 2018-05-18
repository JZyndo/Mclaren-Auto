using UnityEngine;
using System.Collections;

public class PageOverrides : MonoBehaviour {
	public bool OverrideZoomAmount = false;
	public float ZoomVal = 1.5f;

	public bool OverrideBoundsMulti = false; 	
	[Range(0, 2)]
	public float BoundsMulti = 1f;

	public bool OverrideCamDelay = false; 	
	public float ZoomOutCamDelay = 0;

	public bool OverrideDragSnap = false; 	
	public bool SnapOnDragRelease = false;

    public bool OverridePageTilt = false;
    public bool PageTiltOn = true;

    public bool OverrideCameraFOV = false;
    public float CameraFOV = 60;
}
