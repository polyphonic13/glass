using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotificationUI : MonoBehaviour {

	[SerializeField] private Text _message;

	private CanvasGroup _canvasGroup;
	private float _startAlpha; 

	void Awake() {
		_canvasGroup = gameObject.GetComponent<CanvasGroup>();
		_startAlpha = _canvasGroup.alpha;
		_canvasGroup.alpha = 0;

		EventCenter ec = EventCenter.Instance;
		ec.OnAddNote += OnAddNote;
		ec.OnRemoveNote += OnRemoveNote;
	}
	
	public void OnAddNote(string message, bool fadeOut = true) {
		_canvasGroup.alpha = _startAlpha;
		_message.text = message;

		if(fadeOut) {
			StartCoroutine("_fade");
		}
	}

	public void OnRemoveNote(string message) {
		Hide();
	}

	public void Hide() {
		_removeNote();
		_canvasGroup.alpha = 0;
	}
	
	private void _removeNote() {
		_message.text = "";
	}

	private IEnumerator _fade() {
		for (float f = 2f; f >= 0; f -= 0.03f) {
			if(f < 0.1f) {
				f = 0;
			}
			if(f <= 1f) {
				_canvasGroup.alpha = f;
			}
			yield return null;
		}
	}
}
