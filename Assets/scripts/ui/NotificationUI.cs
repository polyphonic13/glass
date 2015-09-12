using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotificationUI : MonoBehaviour {

	[SerializeField] private Text _message;

	private CanvasGroup _canvasGroup;
	private float _startAlpha; 

	private ArrayList _queue;
	private bool _isClearing; 

	void Awake() {
		_queue = new ArrayList ();
		_canvasGroup = gameObject.GetComponent<CanvasGroup>();
		_startAlpha = _canvasGroup.alpha;
		_canvasGroup.alpha = 0;
		EventCenter ec = EventCenter.Instance;
		ec.OnAddNote += OnAddNote;
		ec.OnRemoveNote += OnRemoveNote;
	}
	
	public void OnAddNote(string message, bool fadeOut = true) {
		if (_queue.Count == 0 || (message != _queue [_queue.Count - 1])) {
			_queue.Add (message);
			if (!_isClearing) {
				_isClearing = true;
				_clearQueue ();
			}
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

	private void _clearQueue() {
		if (_queue.Count > 0) {
			_canvasGroup.alpha = _startAlpha;
			int index = _queue.Count - 1;
			string msg = _queue [index] as string;
			_message.text = msg;
			_queue.RemoveAt (index);
			StartCoroutine ("_fade");
		} else {
			_isClearing = false;
		}
	}

	private IEnumerator _fade() {
		for (float f = 2f; f >= 0; f -= 0.015f) {
			if(f <= 1f) {
				if(f < 0.1f) {
					f = 0;
				}
				_canvasGroup.alpha = f;
				if(f == 0) {
					_clearQueue();
				}
			}
			yield return null;
		}
	}
}
