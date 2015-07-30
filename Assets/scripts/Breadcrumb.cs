using UnityEngine;
using System.Collections;

public class Breadcrumb : MonoBehaviour {

	public int lifeTime = 3;

	// Use this for Initialization
	IEnumerator Start() {
		yield return new WaitForSeconds(lifeTime);
		_killSelf();
	}

	// Update is called once per frame
	void _killSelf() {
		//Debug.Log("SlingshotBullet/_killSelf");
		Destroy(gameObject);	
	}
}
