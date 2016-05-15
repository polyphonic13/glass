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

	private Vector3[] _toyOffsets = {
		new Vector3(0, 0.25f, 0),
		new Vector3(-0.66f, 0.25f, 0)
	};

	private Quaternion[] _toyRotations = {
		new Quaternion(90, 90, 0, 0),
		new Quaternion(90, 90, 0, 0)
	};

	public void AddToy(RabbitHuntToy toy) {

//		Debug.Log("ToyChest/AddToy, toy = " + toy.name);
		for(int i = 0; i < _toyNames.Length; i++) {

			if(toy.name == _toyNames[i]) {
				Transform toyTransform = toy.transform;
				Vector3 newPosition = new Vector3(
					(this.transform.position.x + _toyOffsets[i].x), 
					(this.transform.position.y + _toyOffsets[i].y), 
					(this.transform.position.z + _toyOffsets[i].z)
				);
				toyTransform.parent = this.transform;
				toyTransform.position = newPosition;
				toyTransform.rotation = _toyRotations[i];
				_collected++;

				toy.IsEnabled = false;

				EventCenter.Instance.CloseInventoryUI ();
				EventCenter.Instance.AddNote (toy.itemName + " added to Toy Chest");
				break;
			}
		}
//		Debug.Log("_collected = " + _collected + ", _expected = " + _expected);
		if(_collected == _expected) {
//			Debug.Log ("toy box expected all collected");
			EventCenter.Instance.TriggerEvent(unlockEvent);
			EventCenter.Instance.AddNote ("There's a crash in the room next door.");
		}
	}
}
