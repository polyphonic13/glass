using UnityEngine;

public class ButtonUI : MonoBehaviour {
	
	public Color32 standardColor = new Color32(50, 50, 50, 50);
	public Color32 focusColor = new Color32(150, 150, 150, 100); 
	
	private Image _background;
	
	private void Awake() {
		_background = gameObject.GetComponent<Image>();
		_background.color = standardColor;
	}
	
	public void SetFocus(bool focus) {
		if(focus) {
			_background.color = focusColor;
		} else {
			_background.color = standardColor;
		}
	}
	
	public virtual void Activate() {
		Debug.Log("ButtonUI["+this.name+"]/Activate");
	}
}