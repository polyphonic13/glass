using UnityEngine;
using System.Collections;
using Polyworks; 

public class Temp : MonoBehaviour {

	public void Increment() {
		Debug.Log ("Temp/Increment");
		Game.Instance.Increment ();
	}

	public void Save() {
		Debug.Log ("Temp/Save");
		Game.Instance.Save ();
	}

	public void Load() {
		Debug.Log ("Temp/Load");
		Game.Instance.Load ();
	}
	
	public void ChangeScene(string scene) {
		Debug.Log ("Temp/ChangeScene");
		Game.Instance.ChangeScene(scene);
	}
}
