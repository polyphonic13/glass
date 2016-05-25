using UnityEngine;
using System.Collections;
using Polyworks; 

public class Temp : MonoBehaviour {

	public void Increment() {
		GameController.Instance.Increment ();
	}

	public void Save() {
		GameController.Instance.Save ();
	}

	public void Load() {
		GameController.Instance.Load ();
	}
}
