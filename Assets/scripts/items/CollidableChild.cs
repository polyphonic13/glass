using UnityEngine;

public class CollidableChild : MonoBehaviour {
	
	public delegate void CollisionHander(GameObject collisionTarget);

	public event CollisionHander OnChildCollision;
	
	public void ChildCollided(GameObject child) {
		if(OnChildCollision != null) {
			OnChildCollision(child);
		}
	}
	
	void OnCollisionEnter(Collision target) {
		ChildCollided(target.transform.gameObject);
	}
}
