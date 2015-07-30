using UnityEngine;
using System.Collections;

public class CollidableParent : MonoBehaviour {
	
	// Use this for Initialization
	void Awake() {
		Init();	
	}
	
	public void Init() {
		InitCollidableChildren();
	}
	
	public void InitCollidableChildren() {
		var childTransforms = gameObject.GetComponentsInChildren<Transform>();
		
		foreach(Transform childTransform in childTransforms) {
			InitCollidableChild(childTransform.gameObject);
		}
	}
	
	public void InitCollidableChild(GameObject child) {
//		Debug.Log("Adding CollidableChild to " + child.name);
		CollidableChild touchableChild = child.AddComponent<CollidableChild>();
		touchableChild.onChildCollision += onChildCollision;
	}
	
	public void onChildCollision(GameObject collisionTarget) {
//		Debug.Log("CollidableParent/onChildCollision, collisionTarget = " + collisionTarget.name);
		onCollision(collisionTarget);
	}
	
	public virtual void onCollision(GameObject target) {
		handleColliderItemWeight(target);
	}
	
	public void handleColliderItemWeight(GameObject target) {
//		Debug.Log("CollidableParent[ " + name + " ]/_onCollision, collisionTarget = " + target.name);
		if(target.name == "weight(Clone)") {
//			Debug.Log(" it is a weight");
			ItemWeight itemWeight = target.GetComponent<ItemWeight>();
//			Debug.Log("  itemWeight = " + itemWeight + ", targetContainerName = " + itemWeight.targetContainerName);
			if(itemWeight.targetContainerName != null && itemWeight.targetContainerName == name) {
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
