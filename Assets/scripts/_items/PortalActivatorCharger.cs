using UnityEngine;
using System.Collections;
using Polyworks;

public class PortalActivatorCharger : MonoBehaviour
{
	public const string PORTAL_ACTIVATOR_COLLECTED = "isPortalActivatorCollected"; 
	public const string PORTAL_ACTIVATOR_CHARGED = "isPortalActivatorCharged"; 
	public const string USABLE_MESSAGE = "The device vibrated"; 

	public float secondsToCharge = 5.0f; 

	private bool _isCharged = false; 

	private float _isChargedCounter;

	public void OnSceneInitialized(string scene) {
		_isCharged = Game.Instance.GetFlag (PORTAL_ACTIVATOR_CHARGED);
	}

	private void Awake() {
		_isChargedCounter = secondsToCharge;

		EventCenter.Instance.OnSceneInitialized += OnSceneInitialized;
	}

	private void FixedUpdate() {
		//		Debug.Log ("PortalActivator: " + Time.deltaTime + ", _isChargedCounter = " + _isChargedCounter);
		if (!_isCharged) {
			_isChargedCounter -= Time.deltaTime;
			if (_isChargedCounter <= 0) {
				bool isCollected = Game.Instance.GetFlag (PORTAL_ACTIVATOR_COLLECTED);
				// Debug.Log ("PortalActivator now charged, isCollected = " + isCollected);
				if (isCollected) {
					EventCenter.Instance.AddNote (USABLE_MESSAGE);
				}
				_isCharged = true;
				Game.Instance.SetFlag (PORTAL_ACTIVATOR_CHARGED, _isCharged);
				_isChargedCounter = 0;
			}
		}
	}

	private void OnDestroy() {
		EventCenter ec = EventCenter.Instance;
		if (ec != null) {
			ec.OnSceneInitialized -= OnSceneInitialized;
		}
	}
}

