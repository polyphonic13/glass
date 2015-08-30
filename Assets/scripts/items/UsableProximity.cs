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
		if (_collectableItem.IsEnabled) {
			var difference = Vector3.Distance(usabilityTarget.position, transform.position);
			if(difference < usableDistance) {
				_collectableItem.IsUsable = true;
			}
		}
	}
}
