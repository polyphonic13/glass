using UnityEngine;
using System.Collections;
 
public class UsableProximity : MonoBehaviour {

	public Transform usabilityTarget;
	public float usableDistance = 2f;

	private CollectableItem _collectableItem;

	void Awake () {
		_collectableItem = gameObject.GetComponent<CollectableItem> ();
	}
	
	void Update () {
		if (_collectableItem.IsCollected) {
			var difference = Vector3.Distance(usabilityTarget.position, transform.position);
			if(difference < usableDistance) {
				if(!_collectableItem.IsUsable) {
					Debug.Log (this.name + " is enabled, proximity difference = " + difference);
					_collectableItem.IsUsable = true;
				}
			} else if(_collectableItem.IsUsable) {
				_collectableItem.IsUsable = false;
			}
		}
	}
}
