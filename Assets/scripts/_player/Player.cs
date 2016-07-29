using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;
using UnitySampleAssets.Utility;
using Random = UnityEngine.Random;
using Rewired;
using Polyworks;

namespace UnitySampleAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class Player : MonoBehaviour
    {
		#region serialized members
		[SerializeField] private Canvas _menuUI; 
		[SerializeField] private Canvas _inventoryUI; 
		[SerializeField] private Canvas _interactiveItemUI; 
		[SerializeField] private Flashlight _flashLight; 

		[SerializeField] private bool _damageFromFall;
		[SerializeField] private float _underWaterGravity;
		[SerializeField] private float _crawlSpeedMultiplier;
		[SerializeField] private float _diveSpeedMultiplier; 
		[SerializeField] private float _swimSpeedMultiplier; 
		[SerializeField] private float _climbSpeedMultiplier; 

		[SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float _isJumpingSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
		[SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob _isJumpingBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip _isJumpingSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
		#endregion

		#region private members
		private enum MovementStates { Normal, Crawl, Climb, Swim, Dive };
		static MovementStates _currentMovementState;
		static MovementStates _previousMovementState; 
		private VerticalMovement _verticalMovement; 

		private GravityDamager _gravityDamager;
		private float _gravity; 

		private Transform _collider;
		private float _cameraStartY;
		private const float _CrawlCameraY = 0.1f;
		private bool _justCrouched = false;

		private bool _isMenuOpen = false;
		private bool _isInventoryOpen = false;
		private bool _isInspectorOpen = false;
		private bool _hasFlashlight = false;

		private Camera _mainCamera;
		
        private bool _isJumping;
        private bool _isClimbing;
        private bool _isDiving;
		private bool _isCrawling; 

        private float m_YRotation;
        private CameraRefocus _mainCameraRefocus;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool _isJumpinging;
        private AudioSource m_AudioSource;

		private float _horizontal;
		private float _vertical;

        private Item _elementInProximity;

		private Rewired.Player _controls;

		private Transform[] _childTransforms; 
		#endregion

		#region public methods
		public void SetHorizontal(float horizontal) {
			_horizontal = horizontal;
		}
		
		public void SetVertical(float vertical) {
			_vertical = vertical;
		}
		
		public void SetJumping() {
			if (!_isJumping) {
				_isJumping = true;
			}
		}

		public void SetClimbState(bool isClimbing) {
			_currentMovementState = (isClimbing) ? MovementStates.Climb : MovementStates.Normal;
		}

		public void SetClimbing(bool isClimbing) {
			_isClimbing = isClimbing;
		}

		public void SetDiving(bool isDiving) {
			_isDiving = isDiving;
		}
		
		public void SetCrawling(bool isCrawling) {
			_isCrawling = isCrawling;
		}
		#endregion

		#region delegate handlers
		public void OnNearItem(Item item, bool isFocused) {
			if(isFocused) {
				_elementInProximity = item;
			} else {
				_elementInProximity = null;
			}
			//			_interactiveItemUI.enabled = isFocused;
		}

		public void OnPlayerDamaged(float damage) {
//			float health = Game.Instance.RemainingHealth - damage;
//			Game.Instance.UpdateHealth(health);
		}

		public void OnAboveWater(bool water, Transform tgt) {
//			// Debug.Log("Player/OnAboveWater, water = " + water);
			if(water) {
				_currentMovementState = _previousMovementState = MovementStates.Swim;
				_gravity = 0;
			} else {
				_currentMovementState = MovementStates.Normal;
				_gravity = m_GravityMultiplier;
			}
			_gravityDamager.CancelFall();
		}

		public void OnInspectItem(bool isInspecting, string item) {
			_isInspectorOpen = isInspecting;
		}

		public void OnCloseInventoryUI() {
			_closeInventoryUI();
		}

		public void OnInventoryChanged(string name, int count) {
//			// Debug.Log ("OnInventoryChanged, name = " + name);
			if (name == "flashlight") {
				_hasFlashlight = true;
			}
		}

		public void OnCloseMenuUI() {
			_closeMenuUI();
		}
		#endregion

		#region awake
        private void Awake() {
			_controls = ReInput.players.GetPlayer(0);
			_verticalMovement = GetComponent<VerticalMovement> ();

			_menuUI.enabled = false;
			_inventoryUI.enabled = false;
			_interactiveItemUI.enabled = false;
			_isInspectorOpen = _isMenuOpen = _isInventoryOpen = false;

            m_CharacterController = GetComponent<CharacterController>();
            _mainCamera = Camera.main;
            m_OriginalCameraPosition = _mainCamera.transform.localPosition;
            _mainCameraRefocus = new CameraRefocus(_mainCamera, transform, _mainCamera.transform.localPosition);
            m_FovKick.Setup(_mainCamera);
            m_HeadBob.Setup(_mainCamera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            _isJumpinging = false;
            m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(transform , _mainCamera.transform);

			_currentMovementState = MovementStates.Normal;
			_gravity = m_GravityMultiplier;
			_cameraStartY = _mainCamera.transform.position.y;

//			_globalFog = _mainCamera.GetComponent<GlobalFog>();

			if(_damageFromFall) {
				_gravityDamager = GetComponent<GravityDamager>();
			}

			int i = 0;
			_childTransforms = new Transform[transform.childCount];
			foreach (Transform t in transform) {
				_childTransforms [i++] = t;
			}

//			_hasFlashlight = Game.Instance.hasFlashlight;
				
			var ec = EventCenter.Instance;
			ec.OnAboveWater += OnAboveWater;
			ec.OnPlayerDamaged += OnPlayerDamaged;
			ec.OnNearItem += OnNearItem;
			ec.OnInspectItem += OnInspectItem;
			ec.OnCloseInventoryUI += OnCloseInventoryUI;
			ec.OnCloseMenuUI += OnCloseMenuUI;
			ec.OnInventoryAdded += OnInventoryChanged;
			ec.OnInventoryRemoved += OnInventoryChanged;
		}
		#endregion

		#region ui
		private void _closeInventoryUI() {
			_inventoryUI.enabled = _isInventoryOpen = false;
		}
		
		private void _closeMenuUI() {
			_menuUI.enabled = _isMenuOpen = false;
		}
		#endregion

		#region update		
		private void Update() {
			if (!_isInspectorOpen) {
				if(_controls.GetButtonDown ("cancel")) {
					if(_isMenuOpen) {
						_isMenuOpen = !_isMenuOpen;
						_menuUI.enabled = _isMenuOpen;
						_closeMenuUI();
					}
				} else if(_controls.GetButtonDown("open_menu")) {
					_isMenuOpen = !_isMenuOpen;
					_menuUI.enabled = _isMenuOpen;
					_closeInventoryUI();
				} else if(_controls.GetButtonDown("open_inventory")) {
					_isInventoryOpen = !_isInventoryOpen;
					_inventoryUI.enabled = _isInventoryOpen;
					_closeMenuUI();
				}

				// player updates only happen when menus are closed
				if(!_isMenuOpen && !_isInventoryOpen) {
					_rotateView();
					
					// allow to Dive if Swimming 
					// if(_controls.GetButtonDown("dive")) {
					// }
					if(_isDiving) {
						_currentMovementState = MovementStates.Dive;
					}
					if(_currentMovementState == MovementStates.Swim || _currentMovementState == MovementStates.Dive) {
						_gravity = _underWaterGravity;
					}

					// toggle Crawl if walking/Crawling
					// if(_controls.GetButtonDown("crouch")) {
					// }
					if(_isCrawling) {
						if(_currentMovementState == MovementStates.Normal && m_CharacterController.isGrounded) {
							_currentMovementState = MovementStates.Crawl;
							_switchToCrawling(true);
						} else if(_currentMovementState == MovementStates.Crawl) {
							_currentMovementState = MovementStates.Normal;
							_switchToCrawling(false);
						}
						_justCrouched = true;
					}

					// the jump state needs to read here to make sure it is not missed
					// if (!_isJumping) {
					// 	_isJumping = _controls.GetButtonDown("jump");
					// }
					
					if (!m_PreviouslyGrounded && m_CharacterController.isGrounded) {
						StartCoroutine(_isJumpingBob.DoBobCycle());
						//                PlayLandingSound();
						m_MoveDir.y = 0f;
						_isJumpinging = false;
						
						if(_damageFromFall && (_currentMovementState == MovementStates.Normal || _currentMovementState == MovementStates.Crawl)) {
//							float health = Game.Instance.RemainingHealth - _gravityDamager.EndFall();
//							Game.Instance.UpdateHealth(health);
						}
						
					}
					if (!m_CharacterController.isGrounded && !_isJumpinging && m_PreviouslyGrounded)
					{
						m_MoveDir.y = 0f;
					}
					
					if(_damageFromFall) {
						if (_currentMovementState != MovementStates.Climb) {
							if(!m_CharacterController.isGrounded && m_PreviouslyGrounded) {
								_gravityDamager.BeginFall();
							}
						} else {
							_gravityDamager.CancelFall ();
						}
					}
					m_PreviouslyGrounded = m_CharacterController.isGrounded;
				}
				
			}

			if (_hasFlashlight && _controls.GetButtonDown ("flashlight")) {
				_flashLight.Actuate ();
			}
		}

        private void FixedUpdate() {
			if(!_isMenuOpen && !_isInventoryOpen && !_isInspectorOpen) {
				float speed;
				// Read input
				// float _horizontal = _controls.GetAxis("move__horizontal");
				// float _vertical = _controls.GetAxis("move__vertical");

				_getMovement(out speed, _horizontal, _vertical);
				// always move along the camera forward as it is the direction that it being aimed at
				Vector3 desiredMove = _mainCamera.transform.forward*m_Input.y + _mainCamera.transform.right*m_Input.x;
				
				// get a Normal for the surface that is being touched to move along it
				RaycastHit hitInfo;
				Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,  m_CharacterController.height/2f);
				desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
				
				switch(_currentMovementState) {
					case MovementStates.Crawl:
						speed *= _crawlSpeedMultiplier;
						if(m_CharacterController.isGrounded) {
							m_MoveDir.y = -m_StickToGroundForce;
						} else {
							m_MoveDir += Physics.gravity*_gravity*Time.fixedDeltaTime;
						}
					break;
					
					case MovementStates.Normal:
						// Normal WALK/FALL
						if (m_CharacterController.isGrounded) {
							m_MoveDir.y = -m_StickToGroundForce;
						
							if (_isJumping) {
								m_MoveDir.y = _isJumpingSpeed;
								//                    PlayJumpSound();
								_isJumping = false;
								_isJumpinging = true;
							}
						} else {
							// Normal fall to ground
							m_MoveDir += Physics.gravity*_gravity*Time.fixedDeltaTime;
						}
					break;
					
					case MovementStates.Climb:
						speed *= _climbSpeedMultiplier;

						Vector3 move = _verticalMovement.GetMovement (_horizontal, _vertical, _isJumping, _isClimbing);
						m_MoveDir.x = move.x * speed;
						m_MoveDir.y = move.y * speed;
						m_MoveDir.z = move.z * speed;

	//					// Debug.Log ("move = " + move);
					break;
					
					case MovementStates.Swim:
						//					// Debug.Log("Swimming, _isUnderWater = ");
						// SwimMING
						// do not move y -- stay on surface of water
						m_MoveDir.y = 0f;
						speed *= _swimSpeedMultiplier;
					
					break;
					
					case MovementStates.Dive:
						// DIVING
						speed *= _swimSpeedMultiplier;
						if(_controls.GetButton("dive")) {
							// dive down
							m_MoveDir += Physics.gravity*(-(_gravity*_diveSpeedMultiplier))*Time.fixedDeltaTime;
						} else {
							// rise towards surface
							m_MoveDir += Physics.gravity*_gravity*Time.fixedDeltaTime;
						}
					break;
				}
				
				// turn on fog when first diving, remove when not diving
//				if(_currentMovementState == MovementStates.Dive) {
//					if(_previousMovementState != MovementStates.Dive) {
//						//					_globalFog.enabled = true;
//					}
//				} else {
//					//				_globalFog.enabled = false;
//				}
				
				_previousMovementState = _currentMovementState;

				if (_currentMovementState != MovementStates.Climb) {
					m_MoveDir.x = desiredMove.x * speed;
					m_MoveDir.z = desiredMove.z * speed;
				}
				m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);
				
				_progressStepCycle(speed);
				_updateCameraPosition(speed);
			}
        }
		#endregion

        private void _progressStepCycle(float speed) {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

//            PlayFootStepAudio();
        }

		private void _updateCameraPosition(float speed) {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                _mainCamera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = _mainCamera.transform.localPosition;
                newCameraPosition.y = _mainCamera.transform.localPosition.y - _isJumpingBob.Offset();
            }
            else
            {
                newCameraPosition = _mainCamera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - _isJumpingBob.Offset();
            }
            _mainCamera.transform.localPosition = newCameraPosition;

            _mainCameraRefocus.SetFocusPoint();
        }

		private void _switchToCrawling(bool isCrawling) {

			transform.DetachChildren();

			if(isCrawling) {
				transform.localScale -= new Vector3(0, 0.75f, 0);
				transform.localPosition -= new Vector3(0, 0.75f, 0);
				_mainCamera.transform.localPosition -= new Vector3(0, 0.75f, 0);
			} else {
				transform.localScale += new Vector3(0, 0.75f, 0);
				transform.localPosition += new Vector3(0, 0.75f, 0);
				_mainCamera.transform.localPosition += new Vector3(0, 0.75f, 0);
			}

			foreach (Transform t in _childTransforms) {
				t.parent = transform;
			}
		}

		private void _getMovement(out float speed, float horizontal, float vertical) {
            bool wasWalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
//            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
			m_IsWalking = !StaminaManager.IsBoosted;
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(_horizontal, _vertical);

            // Normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != wasWalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }

        private void _rotateView() {
            m_MouseLook.LookRotation (transform, _mainCamera.transform);
            _mainCameraRefocus.GetFocusPoint();
        }

		#region audio
		private void PlayFootStepAudio()
		{
			if (!m_CharacterController.isGrounded)
			{
				return;
			}
			// pick & play a random footstep sound from the array,
			// excluding sound at index 0
			int n = Random.Range(1, m_FootstepSounds.Length);
			m_AudioSource.clip = m_FootstepSounds[n];
			// m_AudioSource.PlayOneShot(m_AudioSource.clip);
			// move picked sound to index 0 so it's not picked next time
			m_FootstepSounds[n] = m_FootstepSounds[0];
			m_FootstepSounds[0] = m_AudioSource.clip;
		}

		private void PlayJumpSound()
		{
			m_AudioSource.clip = _isJumpingSound;
			// m_AudioSource.Play();
		}

		private void PlayLandingSound() {
			m_AudioSource.clip = m_LandSound;
			// m_AudioSource.Play();
			m_NextStep = m_StepCycle + .5f;
		}
		#endregion

	}
}
