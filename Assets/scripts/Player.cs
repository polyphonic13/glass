 using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;
using UnitySampleAssets.Utility;
using Random = UnityEngine.Random;

namespace UnitySampleAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class Player : MonoBehaviour
    {
		public Transform temp;

		[SerializeField] private Canvas _menuUI; 
		[SerializeField] private Canvas _inventoryUI; 

		[SerializeField] private bool _damageFromFall;
		[SerializeField] private float _underWaterGravity;
		[SerializeField] private float _CrawlSpeed;
		[SerializeField] private float _DiveSpeed; 
		[SerializeField] private float _SwimSpeed; 

		[SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
		[SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

		enum _movementStates { Normal, Crawl, Climb, Swim, Dive };
		static _movementStates _currentMovementState;
		static _movementStates _previousMovementState; 

		private GravityDamager _gravityDamager;
		private float _gravity; 

		private Transform _collider;
		private float _cameraStartY;
		private const float _CrawlCameraY = 0.1f;
		private bool _justCrouched = false;

		private bool _isMenuOpen = false;
		private bool _isInventoryOpen = false;
		private bool _isInspectorOpen = false;

		private Camera mainCamera;
        private bool m_Jump;
        private float m_YRotation;
        private CameraRefocus mainCameraRefocus;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;

        private InteractiveItem _elementInProximity;

		public bool IsUIOpen() {
			var isOpen = false;
			if (_isMenuOpen || _isInventoryOpen) {
				isOpen = true;
			}
			return isOpen;
		}

		#region delegate handlers
 		public void OnNearInteractiveItem(InteractiveItem item, bool isInProximity) {
			if(isInProximity) {
				_elementInProximity = item;
			} else {
				_elementInProximity = null;
			}
 		}

		public void OnPlayerDamaged(float damage) {
			float health = GameControl.Instance.RemainingHealth - damage;
			GameControl.Instance.UpdateHealth(health);
		}

		public void OnAboveWater(bool water, Transform tgt) {
//			Debug.Log("Player/OnAboveWater, water = " + water);
			if(water) {
				_currentMovementState = _previousMovementState = _movementStates.Swim;
				_gravity = 0;
			} else {
				_currentMovementState = _movementStates.Normal;
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

		public void OnCloseMenuUI() {
			_closeMenuUI();
		}
		#endregion

		#region awake
        private void Awake()
        {
			_menuUI.enabled = false;
			_inventoryUI.enabled = false;
			_isInspectorOpen = _isMenuOpen = _isInventoryOpen = false;
			_collider = GameObject.Find("collider").transform;

            m_CharacterController = GetComponent<CharacterController>();
            mainCamera = Camera.main;
            m_OriginalCameraPosition = mainCamera.transform.localPosition;
            mainCameraRefocus = new CameraRefocus(mainCamera, transform, mainCamera.transform.localPosition);
            m_FovKick.Setup(mainCamera);
            m_HeadBob.Setup(mainCamera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(transform , mainCamera.transform);

			_currentMovementState = _movementStates.Normal;
			_gravity = m_GravityMultiplier;
			_cameraStartY = mainCamera.transform.position.y;

//			_globalFog = mainCamera.GetComponent<GlobalFog>();

			if(_damageFromFall) {
				_gravityDamager = GetComponent<GravityDamager>();
			}

			var ec = EventCenter.Instance;
			ec.OnAboveWater += OnAboveWater;
			ec.OnPlayerDamaged += OnPlayerDamaged;
			ec.OnNearInteractiveItem += OnNearInteractiveItem;
			ec.OnInspectItem += OnInspectItem;
			ec.OnCloseInventoryUI += OnCloseInventoryUI;
			ec.OnCloseMenuUI += OnCloseMenuUI;
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
		private void Update()
        {
			if (!_isInspectorOpen) {
				if(CrossPlatformInputManager.GetButtonDown("Fire3")) {
					_isMenuOpen = !_isMenuOpen;
					_menuUI.enabled = _isMenuOpen;
					_closeInventoryUI();
				} else if(CrossPlatformInputManager.GetButtonDown("Fire2")) {
					_isInventoryOpen = !_isInventoryOpen;
					_inventoryUI.enabled = _isInventoryOpen;
					_closeMenuUI();
				}

				// player updates only happen when menus are closed
				if(!_isMenuOpen && !_isInventoryOpen) {
					RotateView();
					
					if(CrossPlatformInputManager.GetButtonDown("Fire1")) {
//						Debug.Log("Player fire1 pressed, _elementInProximity = " + _elementInProximity);
						if(_elementInProximity != null) {
							_elementInProximity.Actuate();
						}
					}
					// allow to Dive if Swimming 
					if(CrossPlatformInputManager.GetButtonDown("Crouch")) {
						if(_currentMovementState == _movementStates.Swim || _currentMovementState == _movementStates.Dive) {
							_gravity = _underWaterGravity;
							_currentMovementState = _movementStates.Dive;
						}
					}
					// toggle Crawl if walking/Crawling
					if(CrossPlatformInputManager.GetButtonDown("Crouch")) {
						if(_currentMovementState == _movementStates.Normal && m_CharacterController.isGrounded) {
							_currentMovementState = _movementStates.Crawl;
							_switchToCrawling(true);
							_justCrouched = true;
							Debug.Log("Crawl");
						} else if(_currentMovementState == _movementStates.Crawl) {
							_currentMovementState = _movementStates.Normal;
							_switchToCrawling(false);
							_justCrouched = true;
							Debug.Log("walk");
						}
					}
					
					// the jump state needs to read here to make sure it is not missed
					if (!m_Jump)
					{
						m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
					}
					
					if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
					{
						StartCoroutine(m_JumpBob.DoBobCycle());
						//                PlayLandingSound();
						m_MoveDir.y = 0f;
						m_Jumping = false;
						
						if(_damageFromFall && (_currentMovementState == _movementStates.Normal || _currentMovementState == _movementStates.Climb || _currentMovementState == _movementStates.Crawl)) {
							float health = GameControl.Instance.RemainingHealth - _gravityDamager.EndFall();
							GameControl.Instance.UpdateHealth(health);
						}
						
					}
					if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
					{
						m_MoveDir.y = 0f;
					}
					
					if(_damageFromFall) {
						if(!m_CharacterController.isGrounded && m_PreviouslyGrounded) {
							_gravityDamager.BeginFall();
						}
					}
					m_PreviouslyGrounded = m_CharacterController.isGrounded;
				}
				
			}
		}
		#endregion

		private void _switchToCrawling(bool isCrawling) {
			if(isCrawling) {
				transform.localScale -= new Vector3(0, 0.5f, 0);
				transform.localPosition -= new Vector3(0, 0.5f, 0);
			} else {
				transform.localScale += new Vector3(0, 0.5f, 0);
				transform.localPosition += new Vector3(0, 0.5f, 0);
			}
		}

        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            // m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }

        private void FixedUpdate()
        {
			if(!_isMenuOpen && !_isInventoryOpen && !_isInspectorOpen) {
				float speed;
				GetInput(out speed);
				// always move along the camera forward as it is the direction that it being aimed at
				Vector3 desiredMove = mainCamera.transform.forward*m_Input.y + mainCamera.transform.right*m_Input.x;
				
				// get a Normal for the surface that is being touched to move along it
				RaycastHit hitInfo;
				Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
				                   m_CharacterController.height/2f);
				desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
				
				switch(_currentMovementState) {
				case _movementStates.Crawl:
					speed *= _CrawlSpeed;
					if(m_CharacterController.isGrounded) {
						m_MoveDir.y = -m_StickToGroundForce;
					} else {
						m_MoveDir += Physics.gravity*_gravity*Time.fixedDeltaTime;
					}
					break;
					
				case _movementStates.Normal:
					// Normal WALK/FALL
					if (m_CharacterController.isGrounded) {
						m_MoveDir.y = -m_StickToGroundForce;
						
						if (m_Jump) {
							m_MoveDir.y = m_JumpSpeed;
							//                    PlayJumpSound();
							m_Jump = false;
							m_Jumping = true;
						}
					} else {
						// Normal fall to ground
						m_MoveDir += Physics.gravity*_gravity*Time.fixedDeltaTime;
					}
					break;
					
				case _movementStates.Climb:
					break;
					
				case _movementStates.Swim:
					//					Debug.Log("Swimming, _isUnderWater = ");
					// SwimMING
					// do not move y -- stay on surface of water
					m_MoveDir.y = 0f;
					speed *= _SwimSpeed;
					
					break;
					
				case _movementStates.Dive:
					// DIVING
					speed *= _SwimSpeed;
					if(Input.GetKey(KeyCode.C)) {
						//						Debug.Log("diving");
						m_MoveDir += Physics.gravity*(-(_gravity*_DiveSpeed))*Time.fixedDeltaTime;
					} else {
						// floating to surface (default)
						//                        Debug.Log("floating");
						m_MoveDir += Physics.gravity*_gravity*Time.fixedDeltaTime;
					}
					break;
				}
				
				// turn on fog when first diving, remove when not diving
				if(_currentMovementState == _movementStates.Dive) {
					if(_previousMovementState != _movementStates.Dive) {
						//					_globalFog.enabled = true;
					}
				} else {
					//				_globalFog.enabled = false;
				}
				
				_previousMovementState = _currentMovementState;
				
				m_MoveDir.x = desiredMove.x*speed;
				m_MoveDir.z = desiredMove.z*speed;
				
				m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);
				
				ProgressStepCycle(speed);
				UpdateCameraPosition(speed);
			}
        }

        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            // m_AudioSource.Play();
        }

        private void ProgressStepCycle(float speed)
        {
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

        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                mainCamera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = mainCamera.transform.localPosition;
                newCameraPosition.y = mainCamera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = mainCamera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            mainCamera.transform.localPosition = newCameraPosition;

            mainCameraRefocus.SetFocusPoint();
        }

        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
//            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
			m_IsWalking = !StaminaManager.IsBoosted;
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // Normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }

        private void RotateView()
        {
            m_MouseLook.LookRotation (transform, mainCamera.transform);
            mainCameraRefocus.GetFocusPoint();
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
