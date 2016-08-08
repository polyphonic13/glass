using UnityEngine;
using System.Collections;
using Polyworks;

public class UsableProximity : MonoBehaviour {

	public Transform target1;
	public Transform target2;
	public float usableDistance = 2f;

	private Item _item;

	void Awake () {
		_item = gameObject.GetComponent<Item> ();
	}
	
	void Update () {
		if (_item.isEnabled && _item.data.isCollected) {
			var difference = Vector3.Distance(target1.position, target2.position);
			if(difference < usableDistance) {
				if(!_item.data.isUsable) {
					// Debug.Log (this.name + " is enabled, proximity difference = " + difference);
					_item.data.isUsable = true;
				}
			} else if(_item.data.isUsable) {
				_item.data.isUsable = false;
			}
		}
	}
}
