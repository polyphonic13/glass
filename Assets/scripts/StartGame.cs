using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
	public string FirstLevel = "house_floor02"; 

	public void Go() {
		Application.LoadLevel(FirstLevel);
	}
}
