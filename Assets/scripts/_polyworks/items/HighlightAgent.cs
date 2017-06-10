using UnityEngine;
using System.Collections;

public class HighlightAgent : MonoBehaviour
{
	public GameObject target;

	public void SetInProximity(bool isInProximity) {
//		Debug.Log ("HighlightAgent[" + this.name + "]/SetInProximity, isInProximity = " + isInProximity);
		_setHighlight(isInProximity);
	}

	private void Awake() {
		_setHighlight (false);
	}

	private void _setHighlight(bool isHighlighted) {
		target.SetActive (isHighlighted);
	}
}

