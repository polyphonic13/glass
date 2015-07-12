using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {

	public int DEFAULT_CURSOR = 0;
	public int INTERACT_CURSOR = 1;
	public int COLLECT_CURSOR = 2;
	public int PUSH_CURSOR = 3;
	public int MAGNIFY_CURSOR = 4;

	public Texture2D[] cursors;
	
	private string[] _cursorDescriptions = new string[5] { 
		"Default look / move",
		"Can be interacted with",
		"Can be collected",
		"Push or pull object",
		"Inspect for more information"
	};
	
	public float cursorWidth = 50;
	public float cursorHeight = 50;

	public int cursorType = 0;
	
	private static MouseManager _instance;
	private MouseManager() {}
	
	public static MouseManager Instance {
		get {
			if(_instance == null) {
	                _instance = GameObject.FindObjectOfType(typeof(MouseManager)) as MouseManager;      
			}
			return _instance;
		}
	}
	
	public string[] getCursorDescriptions() {
		return _cursorDescriptions;
	}
	
	public void Awake() {
//		Debug.Log("MouseManager/Awake, _cursorDescriptions.Length = " + _cursorDescriptions.Length);
//		Screen.showCursor = false;
//		Screen.lockCursor = true;
//		Screen.lockCursor = false;
//		Screen.lockCursor = true;
	}

	public void Init() {
	}

	public void drawCursor() {
//		Debug.Log("MouseManager/drawCursor, cursorType = " + cursorType);
		GUI.DrawTexture(new Rect(Input.mousePosition.x - cursorWidth / 2,(Screen.height - Input.mousePosition.y) - cursorHeight / 2, cursorWidth, cursorHeight), cursors[cursorType]);
	}

	public void setCursorType(int type) {
		cursorType = type;
	}
}
