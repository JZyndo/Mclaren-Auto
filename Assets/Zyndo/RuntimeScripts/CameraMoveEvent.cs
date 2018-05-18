using UnityEngine;
using System.Collections;

public class CameraMoveEvent : PageEventBase
{
	private float ZoomVal = 0;
	public Vector3 PositionOffset = new Vector3(0f, 0f, 1.5f);
	public Transform Origin;
	public bool BlockZoomAfter = true;
	public bool BlockDragAfter = true;

	// Use this for initialization
	public override void ForceEvent()
	{
		base.ForceEvent();
		ProcessEvent();
	}

	public override void ProcessEvent()
	{
		if (page != null) 
		{
			Vector3 LocalBasedOn = Vector3.zero;
			if (Origin != null)
			{		
				if (TapZoom.Instance != null) {
					LocalBasedOn = TapZoom.Instance.transform.parent.InverseTransformPoint (Origin.position);	
				} else {
					Debug.LogWarning ("CameraMoveEvent requires TapZoom to be in the scene.");
				}
			}
			if (TapZoom.Instance != null) {
				TapZoom.Instance.ToggleZoomEvent (ZoomVal, LocalBasedOn + PositionOffset, duration, BlockZoomAfter, BlockDragAfter);
			} else {
				Debug.LogWarning ("CameraMoveEvent requires TapZoom to be in the scene.");
			}
		}
	}
}