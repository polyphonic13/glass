using UnityEngine;
using System.Collections;

public class CollidableChild : MonoBehaviour {
	
	public delegate void CollisionHander(GameObject collisionTarget);

	public event CollisionHander onChildCollision;
	
	public void childCollided(GameObject child) {
		if(onChildCollision != null) {
			onChildCollision(child);
		}
	}
	
	void OnCollisionEnter(Collision target) {
		childCollided(target.transform.gameObject);
	}
}
