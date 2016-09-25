using UnityEngine;
using System.Collections;
using Polyworks;

public class PortalActivatorCharger : MonoBehaviour
{
	public const string GAME_DATA_ITEM_KEY = "isPortalActivatorCharged"; 
	public const string USABLE_MESSAGE = "The device vibrated"; 

	public float secondsToCharge = 5.0f; 

	private bool _isCharged = false; 

	private float _isChargedCounter;

	public void OnSceneInitialized(string scene) {
		_isCharged = Game.Instance.GetFlag (GAME_DATA_ITEM_KEY);
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
				Debug.Log ("PortalActivator now charged");
				EventCenter.Instance.AddNote (USABLE_MESSAGE);
				_isCharged = true;
				Game.Instance.SetFlag (GAME_DATA_ITEM_KEY, _isCharged);
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

