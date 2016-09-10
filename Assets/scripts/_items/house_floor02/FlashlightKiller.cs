using UnityEngine;
using System.Collections;
using Polyworks; 

public class FlashlightKiller : MonoBehaviour
{
	public void OnCollectFlashlight() {
		_removeListeners ();
		Destroy (this.gameObject);
	}

	private void Awake() {
		EventCenter ec = EventCenter.Instance;
		ec.OnCollectFlashlight += OnCollectFlashlight;
	}

	private void OnDestroy() {
		_removeListeners ();
	}

	private void _removeListeners() {
		EventCenter ec = EventCenter.Instance; 
		if (ec != null) {
			ec.OnCollectFlashlight -= OnCollectFlashlight;
		}
	}
}

