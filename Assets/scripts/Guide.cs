using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Guide : InteractiveElement {

	public Material lenseOn;
	public Material lenseOff;
	public Material innardsOn;
	public Material innardsOff;

	public ItemWeight weight; 

	public float animationSpeed = 1.0f;
	public float movementSpeed = 4.0f;
	public float followDistance = 2.0f;
	public float minDistance = 1.0f;

	public bool isIntact { get; set; }
	public bool isActive { get; set; }

	private const string DEFAULT_CLIP = "guide01_default";
	private const string ARMS_OPEN_CLIP = "guide01_arms_open";
	private const string ARMS_CLOSE_CLIP = "guide01_arms_close";

	private List<Vector3> _activeBreadcrumbs;
	private Vector3 _lastPlayerPosition;
	private Vector3 _startY;
	private Vector3 _endY;
	private float _startTime = 0;
	private float _levitationAmount = 0;
	private bool _isLevitated = false;

	private Transform _lens;
	private Transform _innards;

	private Animation _animation;
	private Camera _mainCamera; 
	private Player _player; 

	public void initGuide() {
		Debug.Log("Guide/initGuide, layer = " + this.gameObject.layer);
		_player = GameObject.Find("player").GetComponent<Player>();
		_mainCamera = Camera.main;
		isActive = false;
		_lens = this.transform.Search("lens");
		_innards = this.transform.Search("innards");
		_animation = GetComponent<Animation>();
		_playClip(DEFAULT_CLIP);
		_applyOffMaterials();
//		Debug.Log("Guide/initGuide, lens = " + _lens + ", _innards = " + _innards);
		EventCenter.Instance.onRoomEntered += this.onRoomEntered;
		EventCenter.Instance.onPlayerBreadcrumb += this.onPlayerBreadcrumb;
		_activeBreadcrumbs = new List<Vector3>();
		init();
	}
	 
	public void OnMouseDown() {
		this.mouseClick ();
	}

	public override void mouseClick() {
		toggleActivated();

	}

	public override void onRoomEntered(string room) {
		this.containingRoom = room;
		this.isRoomActive = true;
//		this.gameObject.layer = LayerMask.NameToLayer(room);
//		Debug.Log("Guide/onRoomEntered, room = " + room + ", layer = " + this.gameObject.layer + ", room layer = " + LayerMask.NameToLayer(room));
	}

	public void onPlayerBreadcrumb(Vector3 position) {
		_lastPlayerPosition = position;
		_activeBreadcrumbs.Add(position);
	}

	public void toggleActivated() {
		if(isActive) {
			deactivate();
		} else {
			activate();
		}
		isActive = !isActive;
	}

	public void activate() {
		Debug.Log("Guide/activate");
		_applyOnMaterials();
		_playClip(ARMS_OPEN_CLIP);
		_levitationOn();
}

	public void deactivate() {
		Debug.Log("Guide/deactivate");
		_applyOffMaterials();
		_playClip(ARMS_CLOSE_CLIP);
		_levitationOff();
	}

	private void _applyOffMaterials() {
		_lens.renderer.material = lenseOff;
		_innards.renderer.material = innardsOff;
	}

	private void _applyOnMaterials() {
		_lens.renderer.material = lenseOn;
		_innards.renderer.material = innardsOn;
	}

	private void _playClip(string clip) {
		_animation[clip].wrapMode = WrapMode.Once;
		_animation.Play(clip);
	}
	
	private void _levitationOff() {
		float right = this.transform.position.x;
		float up = this.transform.position.y - 1f;
		float forward = this.transform.position.z;
//		ItemWeight _weightClone = (ItemWeight) Instantiate(weight, this.transform.position, this.transform.rotation);
		ItemWeight _weightClone = (ItemWeight) Instantiate(weight, new Vector3(right, up, forward), this.transform.rotation);
		_weightClone.parentObject = this.gameObject;
		_weightClone.transform.parent = this.transform;
	}

	private void _levitationOn() {
		_isLevitated = false;
		_startY = this.transform.position;
		_endY = new Vector3(this.transform.position.x, (_mainCamera.transform.position.y - 0.1f), this.transform.position.z);
		_startTime = Time.time;
		_levitationAmount = Vector3.Distance(_startY, _endY);
	}

	private void _updateLevitation() {
		float distCovered = (Time.time - _startTime) * animationSpeed;
		float fracJourney = distCovered / _levitationAmount;
		this.transform.position = Vector3.Lerp(_startY, _endY, fracJourney);
		if(this.transform.position.y >= _endY.y) {
			this._isLevitated = true;
		}
	}
	
	private void _facePlayer() {
		var targetPos = _mainCamera.transform.position - transform.position;
//		targetPos.y = 0;
		Quaternion newRotation = Quaternion.LookRotation(targetPos);
		this.transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * animationSpeed);
	}

	private void _follow() {
		Vector3 newDestination;
		RaycastHit hit;
		bool isCollided = false;
		
		if(_activeBreadcrumbs.Count > 0) {
			newDestination = _activeBreadcrumbs[0]; // get the first (oldest) position in the list
			var breadcrumbDistance = Vector3.Distance(this.transform.position, _activeBreadcrumbs[0]);
			//				Debug.Log("newDestination = " +newDestination + ", breadcrumbDistance = " + breadcrumbDistance);
			if(breadcrumbDistance <= 2f) {
				_activeBreadcrumbs.RemoveAt(0); // remove that position
			}
		} else {
			newDestination = _mainCamera.transform.position;
		}
		
		var direction = (newDestination - this.transform.position).normalized;
		
		//			if(Physics.Raycast(this.transform.position, this.transform.forward, out hit, 0.1f)) {
		//				if(hit.transform != this.transform && hit.transform.tag != "Player") {
		//					Debug.DrawRay(this.transform.position, this.transform.forward, Color.red);
		//					direction += hit.normal * 5;
		//					isCollided = true;
		//				}
		//			}
		
		//			var leftR = transform.position;
		//			var rightR = transform.position;
		//			leftR.x -= 1;
		//			rightR.x += 1;
		//
		//			if(Physics.Raycast(leftR, this.transform.forward, out hit, 0.1f)) {
		//				if(hit.transform != this.transform && hit.transform.tag != "Player") {
		//					Debug.DrawRay(leftR, this.transform.forward, Color.green);
		//					direction += hit.normal * 5;
		//					isCollided = true;
		//				}
		//			}
		//
		//			if(Physics.Raycast(rightR, this.transform.forward, out hit, 0.1f)) {
		//				if(hit.transform != this.transform && hit.transform.tag != "Player") {
		//					Debug.DrawRay(rightR, this.transform.forward, Color.yellow);
		//					direction += hit.normal * 5;
		//					isCollided = true;
		//				}
		//			}
		//			Debug.Log("direction: " + direction);
		var rot = Quaternion.LookRotation(direction);
		//			this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rot, Time.deltaTime);
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rot, 0.5f);
		//			this.transform.rotation = rot;
		if(!isCollided) {
			this.transform.position += this.transform.forward * movementSpeed * Time.deltaTime;
			//			} else {
			//				// back up a little
			//				this.transform.position -= this.transform.forward * 2 * Time.deltaTime;
		}
		
		//			this.transform.Translate(animationSpeed * Vector3.forward * Time.deltaTime); 

	}

	private void _backAway() {
		RaycastHit hit;
		bool backAreaHit = Physics.Raycast(this.transform.position, -(this.transform.forward), out hit, 0.5f);
		bool backRightHit = Physics.Raycast(transform.position, (transform.forward+transform.right*.25f), out hit, 0.5f);
		bool backLeftHit = Physics.Raycast(transform.position, (transform.forward+transform.right*-.25f), out hit, 0.5f);

		if(backAreaHit || backRightHit || backLeftHit) {
			Debug.Log("Something behind Guide, can't back up");
		} else {
			this.transform.position -= this.transform.forward * movementSpeed * Time.deltaTime;
		}
	}

	private void _updatePosition() {
		var distance = Vector3.Distance(this.transform.position, _mainCamera.transform.position);

//		Debug.Log("distance = " + distance + ", followDistance = " + followDistance);
		if(distance > followDistance) {
			_follow();
		} else {
			_activeBreadcrumbs.Clear();
			if(distance < minDistance) {
				_backAway();
			} else {
				_facePlayer();
			}
		}

	}
	
	void Awake() {
		initGuide();
	}

	void Update() {
		if(this.isActive) {
			if(!this._isLevitated) {
				_updateLevitation();
			} else {
//				_facePlayer();
				_updatePosition();
			} 
		}
	}
}
