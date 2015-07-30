using UnityEngine;
using System.Collections;

public class CollidableParent : MonoBehaviour {
	
	// Use this for initialization
	void Awake () {
		init ();	
	}
	
	public void init() {
		initCollidableChildren();
	}
	
	public void initCollidableChildren() {
		var childTransforms = gameObject.GetComponentsInChildren<Transform>();
		
		foreach(Transform childTransform in childTransforms) {
			initCollidableChild(childTransform.gameObject);
		}
	}
	
	public void initCollidableChild(GameObject child) {
//		Debug.Log("Adding CollidableChild to " + child.name);
		CollidableChild touchableChild = child.AddComponent<CollidableChild>();
		touchableChild.onChildCollision += this.onChildCollision;
	}
	
	public void onChildCollision(GameObject collisionTarget) {
//		Debug.Log("CollidableParent/onChildCollision, collisionTarget = " + collisionTarget.name);
		onCollision(collisionTarget);
	}
	
	public virtual void onCollision(GameObject target) {
		handleColliderItemWeight(target);
	}
	
	public void handleColliderItemWeight(GameObject target) {
//		Debug.Log("CollidableParent[ " + this.name + " ]/_onCollision, collisionTarget = " + target.name);
		if(target.name == "weight(Clone)") {
//			Debug.Log(" it is a weight");
			ItemWeight itemWeight = target.GetComponent<ItemWeight>();
//			Debug.Log("  itemWeight = " + itemWeight + ", targetContainerName = " + itemWeight.targetContainerName);
			if(itemWeight.targetContainerName != null && itemWeight.targetContainerName == this.name) {
//				Debug.Log("  itemWeight.parent = " + itemWeight.parentObject);
				positionChild(itemWeight.parentObject);
			}
		}
	}
	
	void OnCollisionEnter(Collision target) {
		onCollision(target.transform.gameObject);
	}
	
	public virtual void positionChild(GameObject child) {
		// implement
	}
}
