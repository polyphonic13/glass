using UnityEngine;

public class ItemContainer : CollidableParent {
	
	public string[] _collectableItems; 
	
	private int _collectedItems;
	
	void Awake() {
		Init();
	}
	
	public override void OnCollision(GameObject target) {
		// Debug.Log("ItemContainer/onChildCollision, target.transform.parent.name = " + target.transform.parent.name);
		string parentName = target.transform.parent.name;
		foreach(string ci in _collectableItems) {
			// Debug.Log(" ci = " + ci);
			if(parentName == ci) {
				string evt = ci + "_Collected";
				// Debug.Log("  triggering: " + evt);
//				EventCenter.Instance.TriggerEvent(evt);
				_collectedItems++;
				InitCollidableChild(target.transform.parent.transform.gameObject);
			}
			HandleColliderItemWeight(target);
			
			if(_collectedItems >= _collectableItems.Length) {
//				EventCenter.Instance.TriggerCollectedEvent(name + "_AllCollected");
			}
		}
	}

	public override void PositionChild(GameObject child) {
		// Debug.Log("ItemContainer/PositionChild, child = " + child.name);
		child.transform.parent = transform;
		child.transform.position = transform.position;
		child.transform.rotation = transform.rotation; 
		
		ItemWeight itemWeight = child.GetComponent<ItemWeight>();
		if(itemWeight != null) {
			itemWeight.KillSelf();
		}
	}
}
