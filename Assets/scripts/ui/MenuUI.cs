using UnityEngine;
using System.Collections;

public enum MenuType {both,horizontal,vertical}

public class MenuUI : MonoBehaviour {

	public ButtonUI[] UIButtons; 
	
	public float BaseAlpha = 1;
	
	private int _currentIndex = -1; 
	private CanvasGroup _controlPanel;
	
	public MenuType type;
	
	public bool IsEnabled { get; set; }
	
	public void SetEnabled(bool enable) {
		IsEnabled = enable;
		if(!enable) {
			DeactivateButtons();
			_controlPanel.alpha = 0;
		} else {
			_controlPanel.alpha = BaseAlpha;
		}
	}
	
	public void DeactivateButtons() {
		for(int i = 0; i < UIButtons.length; i++) {
			UIButtons[i].SetFocus(false);
		}
	}
	
	private void Awake() {
		_controlPanel = gameObject.GetComponent<CanvasGroup>();
		_controlPanel.alpha = 0;
		IsEnabled = false;
	}
	
	private void Update() {
		if(IsEnabled) {
			if(CrossPlatformInputManager.GetButtonDown("Fire1")) {
				UIButtons[_currentIndex].Activate():
			} else if(CrossPlatformInputManager.GetButtonDown("Cancel")) {
				if(_currentIndex > -1) {
					UIButtons[_currentIndex].SetFocus(false);
				}
			} else {
				float horizontal;
				float vertical;
				
				bool axisChanged = DelayedAxisInput.Check(type, horizontal, vertical);
				if(axisChanged) {
					if(_currentIndex > -1) {
						UIButtons[_currentIndex].SetFocus(false);
					}

					switch(type) {
						case MenuType.both:
						_changeCurrentButton(horizontal);
						_changeCurrentButton(vertical);
						break;
						
						case MenuType.horizontal:
						_changeCurrentButton(horizontal);
						break;
						
						case MenuType.vertical:
						_changeCurrentButton(vertical);
						break;
						
						default:
						Debug.Log("warning: unknown menu type: " + type);
						break;
					}
					
					UIButtons[_currentIndex].SetFocus(true);
				}
			}
		}
	}
	
	private _changeCurrentButton(float axis) {
		if(axis > 0) {
			if(_currentIndex < (UIButtons.length - 1)) {
				_currentIndex++;
			} else {
				_currentIndex = 0;
			}
		} else {
			if(_currentIndex > 0) {
				_currentIndex--;
			} else {
				_currentIndex = (UIButtons.length - 1);
			}
		}
	}
}
