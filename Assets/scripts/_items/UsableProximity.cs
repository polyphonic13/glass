using UnityEngine;
using System.Collections;
 
public class UsableProximity : MonoBehaviour {

	public Transform target1;
	public Transform target2;
	public float usableDistance = 2f;

	private CollectableItem _collectableItem;

	void Awake () {
		_collectableItem = gameObject.GetComponent<CollectableItem> ();
	}
	
	void Update () {
		if (_collectableItem.isCollected) {
			var difference = Vector3.Distance(target1.position, target2.position);
			if(difference < usableDistance) {
				if(!_collectableItem.data.isUsable) {
					Debug.Log (this.name + " is enabled, proximity difference = " + difference);
					_collectableItem.data.isUsable = true;
				}
			} else if(_collectableItem.data.isUsable) {
				_collectableItem.data.isUsable = false;
			}
		}
	}
}
