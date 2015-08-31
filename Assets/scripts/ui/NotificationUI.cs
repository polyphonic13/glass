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
//		_canvasGroup.alpha = _startAlpha;
//		_message.text = message;
//		if (fadeOut) {
//			StartCoroutine("_fade");
//		}
		Debug.Log ("NotificationUI/OnAddNote, _isClearing = " + _isClearing + ", message = " + message);
		if (_queue.Count == 0 || (message != _queue [_queue.Count - 1])) {
			_queue.Add (message);
			if (!_isClearing) {
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
			_isClearing = true;
			_canvasGroup.alpha = _startAlpha;
			int index = _queue.Count - 1;
			string msg = _queue [index] as string;
			_message.text = msg;
			_queue.RemoveAt (index);
			Debug.Log("NotificationUI/_clearQueue, msg = " + msg + ", count = " + _queue.Count);
			StartCoroutine ("_fade");
		} else {
			_isClearing = false;
		}
	}

	private IEnumerator _fade() {
		for (float f = 2f; f >= 0; f -= 0.015f) {
			if(f < 0.1f) {
				f = 0;
			}
			if(f <= 1f) {
				_canvasGroup.alpha = f;
				_clearQueue();
				Debug.Log("...end coroutine, count = " + _queue.Count);
//				break;
			}
			yield return null;
		}
	}
}
