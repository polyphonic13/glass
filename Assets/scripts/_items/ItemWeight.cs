using UnityEngine;

public class ItemWeight : MonoBehaviour {
	
	
	private Rigidbody _rigidbody;
	
	public CollectableItem collectableItem { get; set; }
	public string TargetContainerName { get; set; }

	void Awake() {
		_rigidbody = GetComponent<Rigidbody>();
//		Debug.Log("ItemWeight/Start, _rigidBody = " + _rigidbody);

	}
	
	void OnCollisionEnter(Collision target) {
		Debug.Log("ItemWeight/OnCollisionEnter, target = " + target.gameObject.name + ", parent = " + transform.parent.name);	
		if(target.gameObject.name != transform.parent.name && target.gameObject.name != "player") {
			Debug.Log(" position = " + _rigidbody.position);
			Vector3 parentPos = transform.parent.transform.position;
			var newPos = new Vector3(parentPos.x, _rigidbody.position.y, parentPos.z);
			transform.parent.transform.position = newPos;
			KillSelf();
		}
	}

	public void KillSelf() {
		Debug.Log("ItemWeight/KillSelf");
		collectableItem.isCollected = false;
		Destroy(gameObject);
	}
}
