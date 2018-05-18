using UnityEngine;
using UnityEngine.VR;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TapZoom : MonoBehaviour
{
	public static TapZoom Instance;
    public float Duration = 0.5f;
	[HideInInspector]
    public float FOVVal = 10.0f;

	public float ZoomVal = 1.4f;
	public float ZoomOutCamDelay = .1f;
	public float BoundsMulti = .75f;
	public bool UseMoveLerp = false;
    public AnimationCurve curve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
	public bool SnapOnDragRelease = false;

	public float MoveMulti = 0.001f;
	public float HasMovedThreshold = 3f;
	public float DragLerp = 10f;
	public float SnapLerp = 5;

	private Button TapToZoomButton = null;
	private Vector3 _orgPos;
	private Vector3 _restPos;
    private Vector3 _lastPos;
	private float org_fov;
	private bool _blockNormalZoom = false;
	private bool _blockDrag = true;
	private bool _inDrag = false;
	private float _timeAtDown = -1;
	private Vector3 _lastTarget;
	private float _timeForZoomOut = .4f;
	private Vector3 _positionAtDown;
	private Vector3 _desiredPostion;
	private bool _zoomedAtLastDown = false;
	private bool _hasMoved = false;
	private Vector3 _minBounds;
	private Vector3 _maxBounds;    
	private bool _zoomedIn = false;
	private bool _camMoving = false;
	private GameObject pivot;
	private PageOverrides _pageBasedOverrides;
	private GameObject navBarRoot;
	private bool _inTransition = false;
	private Vector3 _target;
	private float _usedZoomOutCamDelay;
	private Vector3 _snapToPos;
	private bool _overVRButton = false;

    void OnEnable()
    {
		_target = transform.parent.localPosition;
		PageEventsManager.PageArrival += OnPageArrival; 
		PageEventsManager.PageDeparture += OnPageDeparture; 
    }

    void OnDisable()
    {
		PageEventsManager.PageArrival -= OnPageArrival; 
		PageEventsManager.PageDeparture -= OnPageDeparture; 
    }

    void Reset(object sender, EventArgs args)
	{		
		_zoomedIn = false;
		_restPos = _orgPos;
		_blockDrag = true;
		if (_zoomedIn || _target != _orgPos)
		{
			_inTransition = true;
			_target = _orgPos;

	        //do the move
			StartCoroutine(ProcessTapZoom(1.0f));
		}	
    }

    // Use this for initialization
    void Start()
    {
		Instance = this;
        org_fov = Camera.main.fieldOfView;
		_orgPos = this.transform.parent.localPosition;
		_restPos = _orgPos;
		_usedZoomOutCamDelay = ZoomOutCamDelay;
        navBarRoot = GameObject.Find("NavBar");
		if (TapToZoomButton != null) {
			TapToZoomButton.onClick.AddListener (ToggleZoom);
		}
    }

    void ToggleZoom()
    {
		if (!_inTransition && !_camMoving && !_blockNormalZoom)
		{
		    var w = Screen.width;
		    var h = Screen.height;

            //check that menu is not open
            navBarRoot = GameObject.Find("NavBar");
            if (navBarRoot != null && navBarRoot.activeSelf)
                return;

            //set the target based on mouse position
            _target = new Vector3(0, 0, 0);
			if (!VRSettings.enabled)
			{
	            if (Input.touchCount == 1)
	            {
	                Touch touchZero = Input.GetTouch(0);
	                _target = new Vector3(touchZero.position.x, touchZero.position.y, 0);
	            }
	            else
	            {
	                _target = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
	            }
			}

            //set the target fov
            var fov_target = Camera.main.fieldOfView - FOVVal;

            //adjust the targets based on whether or not we have zoomed in or not
            if(_zoomedIn)
            {
                fov_target = org_fov;
				_target = _restPos;
				_blockDrag = true;
            }
            else
			{	
				Vector3 _topLeft = this.transform.parent.InverseTransformPoint(Camera.main.ScreenToWorldPoint( new Vector3(0, 0, -this.transform.localPosition.z)));
				Vector3 _bottomRight = this.transform.parent.InverseTransformPoint(Camera.main.ScreenToWorldPoint( new Vector3( w,  h, -this.transform.localPosition.z)));
				_minBounds = new Vector3 (Mathf.Min(_topLeft.x, _bottomRight.x), Mathf.Min(_topLeft.y, _bottomRight.y));
				_maxBounds = new Vector3 (Mathf.Max(_topLeft.x, _bottomRight.x), Mathf.Max(_topLeft.y, _bottomRight.y));
				if (!VRSettings.enabled) {
					_target.z = -this.transform.parent.localPosition.z;

					//convert screen space to world
					_target = Camera.main.ScreenToWorldPoint (_target);
					_target = this.transform.parent.InverseTransformPoint (_target);
				}
				float zoomToUse = ZoomVal;
				if (_pageBasedOverrides != null && _pageBasedOverrides.OverrideZoomAmount)
				{
					zoomToUse = _pageBasedOverrides.ZoomVal;
				}
				_target.z = - zoomToUse;
				_snapToPos = _target;
				_blockDrag = false;
            }
				
			StartCoroutine(ProcessTapZoom(Duration));
            _zoomedIn = !_zoomedIn;
        }
	}

	public void ToggleZoomEvent(float eventZoomAmount, Vector3 target, float eventDuration, bool blockNormalZoom = true, bool blockDrag = true)
	{
		Debug.Log ("Start Event Zoom: " + eventZoomAmount);		    
		var w = Screen.width;
		var h = Screen.height;
		Vector3 _topLeft = this.transform.parent.InverseTransformPoint(Camera.main.ScreenToWorldPoint( new Vector3(0, 0, -this.transform.parent.localPosition.z)));
		Vector3 _bottomRight = this.transform.parent.InverseTransformPoint(Camera.main.ScreenToWorldPoint( new Vector3( w,  h, -this.transform.parent.localPosition.z)));
		_minBounds = new Vector3 (Mathf.Min(_topLeft.x, _bottomRight.x), Mathf.Min(_topLeft.y, _bottomRight.y));
		_maxBounds = new Vector3 (Mathf.Max(_topLeft.x, _bottomRight.x), Mathf.Max(_topLeft.y, _bottomRight.y));

		_blockNormalZoom = blockNormalZoom;
		_blockDrag = blockDrag;
		_zoomedIn = false;
		StopAllCoroutines ();

		target.z *= -1;
		this._target = target;	
		if (!blockNormalZoom)
		{
			_restPos = target;
		}
		if (!blockDrag)
		{
			_snapToPos = _target;
		}
		StartCoroutine(ProcessTapZoom(eventDuration));
	}

    IEnumerator ProcessTapZoom(float duration)
    {
		_inTransition = true;
		var startPos = this.transform.parent.localPosition;
        var startFov = Camera.main.fieldOfView;
		float startTime = Time.time;
		if (!UseMoveLerp) {
			while ((Time.time - startTime) <= duration)
	        {
				var lerpVal = curve.Evaluate ((Time.time - startTime) / duration);

				//lerp the camera position in local space
				transform.parent.localPosition = Vector3.Lerp (startPos, _target, lerpVal);

				//transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.Lerp(startPos, target, lerpVal), DragLerp * Time.deltaTime);
				yield return 0;
			} 
		}
		else {

            float timeSoFar = 0;
            Vector3 origPos = transform.parent.localPosition;

            while (timeSoFar < Duration) {
				transform.parent.localPosition = Vector3.Lerp (origPos, _target, timeSoFar/Duration);
                timeSoFar += Time.deltaTime;
				yield return 0;
			}
		}
		transform.parent.localPosition = _target;
		_inTransition = false;
        yield return 0;
    }

	public void OnPageArrival(object sender, EventArgs args)
	{
		_blockNormalZoom = false;
		Reset (sender, args);
		if (PageEventsManager.currentPage != null) {
			_pageBasedOverrides = PageEventsManager.currentPage.GetComponent<PageOverrides> ();
		} else {
			Debug.LogWarning ("There is no currentPage when OnPageArrival is called");
		}
		_camMoving = false;
	}

	public void OnPageDeparture(object sender, EventArgs args)
	{
		_blockNormalZoom = false;
		Reset (sender, args);
		_camMoving = true;
	}

	public void DeactivateAllRaycastsOnPage()
	{
		Image[] images = PageEventsManager.currentPage.GetComponentsInChildren<Image> ();
		for (int i = 0; i < images.Length; i++)
		{
			images [i].raycastTarget = false;
		}
		Button[] buttons = PageEventsManager.currentPage.GetComponentsInChildren<Button> ();
		for (int i = 0; i < buttons.Length; i++)
		{
			if (buttons [i].image != null)
			{
				buttons [i].image.raycastTarget = true;
			}
		}
	}

	public IEnumerator OnCameraMove()
	{
		if (_zoomedIn || _target != _orgPos)
		{
			Reset (null, null);			
			if (_pageBasedOverrides != null && _pageBasedOverrides.OverrideZoomAmount) {
				_usedZoomOutCamDelay = _pageBasedOverrides.ZoomOutCamDelay;
			} else {
				_usedZoomOutCamDelay = ZoomOutCamDelay;
			}
			yield return new WaitForSeconds (_usedZoomOutCamDelay);

		}
		yield return null;
	}

	#region VR
	public void OnVrButtonOver()
	{
		_overVRButton = true;
	}	

	public void OnVrButtonExit()
	{
		_overVRButton = false;
	}
	#endregion

	public void OnPointerDown()
	{	
		_zoomedAtLastDown = _zoomedIn;
		_hasMoved = false;

		//zooms in, if already zoomed out
		if (!_zoomedIn && _timeAtDown == -1) {
			Debug.Log("zooms in, if already zoomed out");
			ToggleZoom ();
		}

		if (!_blockDrag)
		{
			_inDrag = true;
		}

		_timeAtDown = Time.time;

		if (Input.touchCount == 1) {
			Touch touchZero = Input.GetTouch (0);
			_positionAtDown = new Vector3 (touchZero.position.x, touchZero.position.y, 0);
		} else {
			_positionAtDown = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
		}
		_lastTarget = _positionAtDown;
	}

	public void OnPointerUp()
	{		
		_inDrag = false;
		if (!_hasMoved && _zoomedAtLastDown && _zoomedIn && Time.time - _timeAtDown <= _timeForZoomOut)
		{
			Debug.Log("zooms out, if already zoomed in");
			ToggleZoom ();
		}
		_timeAtDown = -1;
	}

	public void Update()
	{			
		if (VRSettings.enabled) { 
			if (!_overVRButton && Input.GetButtonDown ("Fire1")) {
				Debug.Log ("Fire1");
				ToggleZoom ();
			}
			return;
		}
		Vector3 newPos;			
		if (Input.touchCount == 1) {
			Touch touchZero = Input.GetTouch (0);
			newPos = new Vector3 (touchZero.position.x, touchZero.position.y, 0);
		} else {
			newPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
		}

		if ((_positionAtDown -  newPos).magnitude > HasMovedThreshold)
		{
			//flag to determine if we need to zoom out in OnPointerUp
			_hasMoved = true;
		}
		bool usedSnapRelease = SnapOnDragRelease;
		if (_pageBasedOverrides != null && _pageBasedOverrides.OverrideDragSnap)
		{
			usedSnapRelease = _pageBasedOverrides.SnapOnDragRelease;
		}

        if (_inDrag)
            _lastPos = newPos;

		if (!_blockDrag /*&& _timeAtDown != -1*/) {
			if (!_inTransition) {	
				//lerp to drag postion
				_target -= ((_lastPos - _lastTarget) * MoveMulti);
				_target = ClampPosition (_target);
				transform.parent.localPosition = Vector3.Lerp (transform.parent.localPosition, _target, DragLerp * Time.deltaTime);
			}
		} 
		else if (!_inDrag && usedSnapRelease && !_blockDrag && _timeAtDown == -1) {
			if (!_inTransition) {	
				//Snap to start
				_target = ClampPosition(_snapToPos);
				transform.parent.localPosition = Vector3.Lerp (transform.parent.localPosition, _target, SnapLerp * Time.deltaTime);
			}
		}
		_lastTarget = _lastPos;
	}

	//Will this work in reverse?
	public Vector3 ClampPosition(Vector3 toProcess)
	{
		Vector3 finalPos = toProcess;
		float useBoundsMulti = BoundsMulti;

		if (_pageBasedOverrides != null && _pageBasedOverrides.OverrideBoundsMulti)
		{
			useBoundsMulti = _pageBasedOverrides.BoundsMulti;
		}	
		finalPos.x = Mathf.Clamp (finalPos.x, _minBounds.x * useBoundsMulti, _maxBounds.x * useBoundsMulti);
		finalPos.y = Mathf.Clamp (finalPos.y, _minBounds.y * useBoundsMulti, _maxBounds.y * useBoundsMulti);
		return finalPos;
	}
}
