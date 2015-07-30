using UnityEngine;

public class Notification {

	private GUIStyle _style;
	private string _content;
	
	private EventCenter _eventCenter;

	public bool IsShowable { get; set; }
	
	private bool _zoomNote;
	
	public void Init(GUIStyle style) {
		_style = style;
		// Debug.Log("Notification/Init, _style = " + _style);
		IsShowable = false;
		_eventCenter = EventCenter.Instance;
		_eventCenter.OnAddNote += OnAddNote;
		_eventCenter.OnRemoveNote += OnRemoveNote;
	}
	
    public void OnAddNote(string msg, bool zoom = false) {
        AddNote(msg, zoom);
    }
	
	public void OnRemoveNote(string msg = "", bool zoom = false) {
		Destroy();
	}
	
	public void AddNote(string msg, bool zoom = false) {
		// Debug.Log("Notification/draw, msg = " + msg);
		_content = msg;
		_zoomNote = zoom;

		if(_zoomNote) {
			_eventCenter.ZoomCamera(true);
		}
		_eventCenter.EnablePlayer(false);
		IsShowable = true;
	}
	
	public void Destroy() {
		// Debug.Log("Notification/Destroy");	
		IsShowable = false;
		_content = "";
		_eventCenter.EnablePlayer(true);
	}

	public void DrawNote() {
		GUI.Box(new Rect((Screen.width/2 - 250),(Screen.height/2 - 50), 500, 100), _content /*, _style */);
		if(GUI.Button(new Rect((Screen.width/2 + 150),(Screen.height/2 - 70), 100, 20), "Close" /*, _style */)) {
			Destroy();
			if(_zoomNote) {
				_eventCenter.ZoomCamera(false);
				_zoomNote = false;
			}
		}
	}
	
}
