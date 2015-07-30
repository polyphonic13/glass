using UnityEngine;
using System.Collections;

public class Notification {

	private GUIStyle _style;
	private string _content;
	
	private EventCenter _eventCenter;

	public bool showNote { get; set; }
	
	private bool _zoomNote = false;
	
	public void init(GUIStyle style) {
		_style = style;
		// Debug.Log("Notification/init, _style = " + _style);
		this.showNote = false;
		_eventCenter = EventCenter.Instance;
		_eventCenter.onAddNote += this.onAddNote;
		_eventCenter.onRemoveNote += this.onRemoveNote;
	}
	
    public void onAddNote(string msg, bool zoom = false) {
        addNote(msg, zoom);
    }
	
	public void onRemoveNote(string msg = "", bool zoom = false) {
		destroy();
	}
	
	public void addNote(string msg, bool zoom = false) {
		// Debug.Log("Notification/draw, msg = " + msg);
		_content = msg;
		_zoomNote = zoom;

		if(_zoomNote) {
			_eventCenter.zoomCamera(true);
		}
		_eventCenter.enablePlayer(false);
		this.showNote = true;
	}
	
	public void destroy() {
		// Debug.Log("Notification/destroy");	
		this.showNote = false;
		_content = "";
		_eventCenter.enablePlayer(true);
	}

	public void drawNote() {
		GUI.Box(new Rect((Screen.width/2 - 250),(Screen.height/2 - 50), 500, 100), _content /*, _style */);
		if(GUI.Button(new Rect((Screen.width/2 + 150),(Screen.height/2 - 70), 100, 20), "Close" /*, _style */)) {
			this.destroy();
			if(_zoomNote) {
				_eventCenter.zoomCamera(false);
				_zoomNote = false;
			}
		}
	}
	
}
