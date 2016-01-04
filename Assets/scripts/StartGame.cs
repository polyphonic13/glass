using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
	public string FirstLevel = "house_floor02"; 
	public int FirstRoom = 0;

	public void Go() {
		GameControl.Instance.targetRoom = FirstRoom;
		Application.LoadLevel(FirstLevel);
	}

	private void Update() {
		if (Input.GetKeyDown (KeyCode.Return)) {
			Go ();
		}
	}
}
