using UnityEngine;
using UnityEngine.UI;

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

}
