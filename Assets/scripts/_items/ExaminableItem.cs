using UnityEngine;
using System.Collections;
using Polyworks; 

public class ExaminableItem : Item {

	public string description = "";
	public bool isSingleUse = true;

	private bool _isUsedOnce = false;

	private ProximityController _proximityController; 
	
	void Awake() {
		_proximityController = GetComponent<ProximityController>();
	}

	public override void Actuate() {
		if(isEnabled) {
			if(_proximityController.Check()) {
				if(!isSingleUse || !_isUsedOnce) {
					_isUsedOnce = true;
					EventCenter.Instance.AddNote(description);
				}
			}
		}
	}
}
