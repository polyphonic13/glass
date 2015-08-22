using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotificationUI : MonoBehaviour {

	[SerializeField] private Text _message;

	void Awake() {
		EventCenter ec = EventCenter.Instance;
		ec.OnAddNote += OnAddNote;
		ec.OnRemoveNote += OnRemoveNote;
	}
	
	public void OnAddNote(string message) {
		_message.text = message;
	}

	public void OnRemoveNote(string message) {
		_message.text = "";
	}

	private IEnumerator _fade(Material mat) {
		for (float f = 1f; f >= 0; f -= 0.1f) {
			Color c = mat.color;
			c.a = f;
			mat.color = c;
			yield return null;
		}
	}
}
