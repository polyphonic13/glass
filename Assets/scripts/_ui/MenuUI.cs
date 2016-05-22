using UnityEngine;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;

public class MenuUI : MonoBehaviour {

	public ArrayList uiButtons; 
	
	public float BaseAlpha = 1;
	
	private int _currentIndex = -1; 
	private CanvasGroup _controlPanel;
	
	public string type;
	
	public bool isEnabled { get; set; }
	
	public void SetEnabled(bool enable) {
		isEnabled = enable;
		if(!enable) {
			DeactivateButtons();
			_controlPanel.alpha = 0;
		} else {
			_controlPanel.alpha = BaseAlpha;
		}
	}
	
	public void DeactivateButtons() {
		ButtonUI button;
		for(int i = 0; i < uiButtons.Count; i++) {
			button = uiButtons[i] as ButtonUI;
			button.SetFocus(false);
		}
	}
	
	private void Awake() {
		_controlPanel = gameObject.GetComponent<CanvasGroup>();
		_controlPanel.alpha = 0;
		isEnabled = false;
		uiButtons = new ArrayList();
	}
	
	private void Update() {
		if(isEnabled) {
			if(CrossPlatformInputManager.GetButtonDown("Fire1")) {
				ButtonUI button = uiButtons[_currentIndex] as ButtonUI;
				button.Activate();
			} else if(CrossPlatformInputManager.GetButtonDown("Cancel")) {
				if(_currentIndex > -1) {
					ButtonUI button = uiButtons[_currentIndex] as ButtonUI;
					button.SetFocus(false);
				}
			} else {
				float horizontal = 0;
				float vertical = 0;
				
				bool axisChanged = DelayedAxisInput.Check(type, out horizontal, out vertical);
				if(axisChanged) {
					ButtonUI button;

					if(_currentIndex > -1) {
						button = uiButtons[_currentIndex] as ButtonUI;
						button.SetFocus(false);
					}

					switch(type) {
						case "both":
						_changeCurrentButton(horizontal);
						_changeCurrentButton(vertical);
						break;
						
						case "horizontal":
						_changeCurrentButton(horizontal);
						break;
						
						case "vertical":
						_changeCurrentButton(vertical);
						break;
						
						default:
						Debug.Log("warning: unknown menu type: " + type);
						break;
					}
					button = uiButtons[_currentIndex] as ButtonUI;
					button.SetFocus(true);
				}
			}
		}
	}
	
	private void _changeCurrentButton(float axis) {
		if(axis > 0) {
			if(_currentIndex < (uiButtons.Count - 1)) {
				_currentIndex++;
			} else {
				_currentIndex = 0;
			}
		} else {
			if(_currentIndex > 0) {
				_currentIndex--;
			} else {
				_currentIndex = (uiButtons.Count - 1);
			}
		}
	}
}
