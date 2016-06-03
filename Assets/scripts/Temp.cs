using UnityEngine;
using System.Collections;
using Polyworks; 

public class Temp : MonoBehaviour {

	public void Increment() {
		Game.Instance.Increment ();
	}

	public void Save() {
		Game.Instance.Save ();
	}

	public void Load() {
		Game.Instance.Load ();
	}
}
