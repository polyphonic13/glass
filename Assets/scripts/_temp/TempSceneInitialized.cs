using UnityEngine;
using System.Collections;
using Polyworks; 

public class TempSceneInitialized : MonoBehaviour {

	public void OnSceneInitialized(string scene) {
		Debug.Log ("TempSceneInitialized/OnSceneInitialized");
		Polyworks.EventCenter ec = Polyworks.EventCenter.Instance;
		ec.OnSceneInitialized -= this.OnSceneInitialized;
		Destroy (this.gameObject);
	}

	private void Awake () {
		Polyworks.EventCenter ec = Polyworks.EventCenter.Instance;
		ec.OnSceneInitialized += this.OnSceneInitialized;
	}
}
