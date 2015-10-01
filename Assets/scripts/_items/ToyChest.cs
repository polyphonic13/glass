using UnityEngine;
using System.Collections;

public class ToyChest : MonoBehaviour {

	public string unlockEvent; 

	private int _collected = 0;
	private int _expected = 2;

	private string[] _toyNames = {
		"rabbit_doll01",
		"toy_dog01"
	};

	private Vector3[] _toyLocations = {
		new Vector3(0.21f, -0.46f, -0.347f),
		new Vector3(-9.819f, -0.495f, 2.031f)
	};

	private Quaternion[] _toyRotations = {
		new Quaternion(0, 0, 270, 0),
		new Quaternion(0, 90, 90, 0)
	};

	public void AddToy(RabbitHuntToy toy) {

		Debug.Log("ToyChest/AddToy, toy = " + toy.name);
		for(int i = 0; i < _toyNames.Length; i++) {

			if(toy.name == _toyNames[i]) {
				Transform toyTransform = toy.transform;
				toyTransform.parent = this.transform.parent;
//				toyTransform.position = _toyLocations[i];
				toyTransform.position = this.transform.position;
				toyTransform.rotation = _toyRotations[i];
				_collected++;

				EventCenter.Instance.CloseInventoryUI ();
				EventCenter.Instance.AddNote (toy.ItemName + " added to Toy Chest");
				break;
			}
		}
		Debug.Log("_collected = " + _collected + ", _expected = " + _expected);
		if(_collected == _expected) {
			Debug.Log ("toy box expected all collected");
			EventCenter.Instance.TriggerEvent(unlockEvent);
			EventCenter.Instance.AddNote ("There's a crash in the room next door.");
		}
	}
}
