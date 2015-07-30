using UnityEngine;
using System.Collections;

public class ItemContainer : CollidableParent {
	
	public string[] collectableItems; 
	
	private GameObject _containerSpot;
	private int _collectedItems = 0;
	
	void Awake() {
		init ();
		_containerSpot = this.transform.Search("containerSpot").gameObject;
		Debug.Log("ItemContainer/Awake, _containerSpot = " + _containerSpot);
	}
	
	public override void onCollision(GameObject collisionTarget) {
		Debug.Log("ItemContainer/onChildCollision, collisionTarget.transform.parent.name = " + collisionTarget.transform.parent.name);
		string parentName = collisionTarget.transform.parent.name;
		foreach(string ci in collectableItems) {
			Debug.Log(" ci = " + ci);
			if(parentName == ci) {
				string evt = ci + "_Collected";
				Debug.Log("  triggering: " + evt);
				EventCenter.Instance.triggerEvent(evt);
				_collectedItems++;
				initCollidableChild(collisionTarget.transform.parent.transform.gameObject);
			}
			handleColliderItemWeight(collisionTarget);
			
			if(_collectedItems >= collectableItems.Length) {
				EventCenter.Instance.collectedEvent(this.name + "_AllCollected");
			}
		}
	}

	public override void positionChild(GameObject child) {
		Debug.Log("ItemContainer/positionChild, child = " + child.name + ", _containerSpot = " + _containerSpot.name);
		child.transform.parent = _containerSpot.transform;
		child.transform.position = _containerSpot.transform.position;
		child.transform.rotation = _containerSpot.transform.rotation; 
		
		ItemWeight itemWeight = child.GetComponent<ItemWeight>();
		if(itemWeight != null) {
			itemWeight.killSelf();
		}
	}
}
