using UnityEngine;
using System.Collections;

public class Menu {

	public bool showMenu { get; set; }
	
	private string[] _controlKeys =  new string [9] {
		"mouse",
		"w",
		"s",
		"a",
		"d",
		"q",
		"m",
		"f",
		"x"
	};
	
	private string[] _controlDescriptions = new string[9] {
		"Look / move direction",
		"Move forward",
		"Move backwards",
		"Move left",
		"Move right",
		"Open / close inventory",
		"Open / close menu",
		"Activate / Deactivate flashlight",
		"Drop equipped item"
	};
	
	private const string MENU_TITLE = "Menu";
	private const string CONTROLS = "Controls";
	private const string ICONS = "Icons";
	
	private const float DESCRIPTION_WIDTH = 400;

	private const float CONTROL_DESCRIPTION_HEIGHT = 20;
	
	private const float ICON_WIDTH = 50;
	private const float ICON_DESCRIPTION_HEIGHT = 50;

	private const float MIN_MOUSE_SENSITIVITY = 1f;
	private const float MAX_MOUSE_SENSITIVITY = 10f;
	
	private float _mouseSensitivity = 2f;
	
	private GUIStyle _style;
	
	public void Init(GUIStyle style) {
		_style = style;
	}
		
	public void draw() {
		EventCenter.Instance.enablePlayer(false);
		drawBackground("Menu");
		Rect columnBg = new Rect(0, 30, Screen.width/2 - 60, Screen.height - 60);
		GUI.Box(new Rect(10, columnBg.y, columnBg.width, columnBg.height), CONTROLS);
		drawControls();
		GUI.Box(new Rect(Screen.width/2 + 30, columnBg.y, columnBg.width, columnBg.height), ICONS);
		drawCursors();
	}
	
	public void drawControls() {
//		Debug.Log("Menu/drawControls");
		for(int i = 0; i < _controlKeys.Length; i++) {
//			Debug.Log("  c = " + c.Key + ", value = " + c.Value);
			GUI.skin.label.alignment = TextAnchor.LowerLeft;
			GUI.Label(new Rect(50, CONTROL_DESCRIPTION_HEIGHT + (i * (CONTROL_DESCRIPTION_HEIGHT + 20)), DESCRIPTION_WIDTH, CONTROL_DESCRIPTION_HEIGHT), _controlKeys[i] + ": " + _controlDescriptions[i]);
		}
		GUI.Label(new Rect(50, ((_controlKeys.Length + 1) * (CONTROL_DESCRIPTION_HEIGHT + 20)), DESCRIPTION_WIDTH, CONTROL_DESCRIPTION_HEIGHT), "Mouse X sensitivity: " + Mathf.Floor(_mouseSensitivity));
		
		_mouseSensitivity =  Mathf.Floor(GUI.HorizontalSlider(new Rect(100, ((_controlKeys.Length + 1) * (CONTROL_DESCRIPTION_HEIGHT + 20)), 100, 30), _mouseSensitivity, MIN_MOUSE_SENSITIVITY, MAX_MOUSE_SENSITIVITY));
		EventCenter.Instance.changeMouseSensitivity(_mouseSensitivity);
	}
	
	public void drawCursors() {
		MouseManager mouseManager = MouseManager.Instance;
		string[] descriptions = mouseManager.getCursorDescriptions();
		Texture2D[] icons = mouseManager.cursors;
//		Debug.Log("Menu/drawCursors, descriptions.length = " + descriptions.Length);

		for(int i = 0; i < descriptions.Length; i++) {
		    GUI.skin.label.alignment = TextAnchor.LowerLeft;
			GUI.DrawTexture (new Rect (Screen.width/2 + 100, ICON_DESCRIPTION_HEIGHT + (i * (ICON_DESCRIPTION_HEIGHT + 20)), ICON_WIDTH, ICON_DESCRIPTION_HEIGHT), icons[i]);
			GUI.Label(new Rect(Screen.width/2 + 200, ICON_DESCRIPTION_HEIGHT + (i * (ICON_DESCRIPTION_HEIGHT + 20)), DESCRIPTION_WIDTH, ICON_DESCRIPTION_HEIGHT), descriptions[i]);
			
		}
	}
	
	public void drawBackground(string title) {
		GUI.Box(new Rect(5, 5, Screen.width - 10, Screen.height - 10), MENU_TITLE /*, _style */);
	}
	
	public void destroy() {
		// Debug.Log("Notification/destroy");	
		showMenu = false;
		EventCenter eventCenter = EventCenter.Instance;
		eventCenter.enablePlayer(true);
		eventCenter.changeMouseSensitivity(_mouseSensitivity);
	}

}
