using UnityEngine;

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
		touchableChild.OnChildCollision += OnChildCollision;
	}
	
	public void OnChildCollision(GameObject collisionTarget) {
//		Debug.Log("CollidableParent/onChildCollision, collisionTarget = " + collisionTarget.name);
		OnCollision(collisionTarget);
	}
	
	public virtual void OnCollision(GameObject target) {
		HandleColliderItemWeight(target);
	}
	
	public void HandleColliderItemWeight(GameObject target) {
//		Debug.Log("CollidableParent[ " + name + " ]/_onCollision, collisionTarget = " + target.name);
		if(target.name == "weight(Clone)") {
//			Debug.Log(" it is a weight");
			ItemWeight itemWeight = target.GetComponent<ItemWeight>();
//			Debug.Log("  itemWeight = " + itemWeight + ", TargetContainerName = " + itemWeight.TargetContainerName);
			if(itemWeight.TargetContainerName != null && itemWeight.TargetContainerName == name) {
//				Debug.Log("  itemWeight.parent = " + itemWeight.ParentObject);
//				PositionChild(itemWeight.ParentObject);
			}
		}
	}
	
	void OnCollisionEnter(Collision target) {
		OnCollision(target.transform.gameObject);
	}
	
	public virtual void PositionChild(GameObject child) {
		// implement
	}
}
