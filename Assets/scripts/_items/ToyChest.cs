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
		new Vector3(0, 0, 0),
		new Vector3(0, 0, 0)
	};

	public void AddToy(RabbitHuntToy toy) {
		Debug.Log("ToyChest/AddToy, toy = " + toy.name);
		for(int i = 0; i < _toyNames.Length; i++) {

			if(toy.name == _toyNames[i]) {
				Transform toyTransform = toy.transform;
				toyTransform.parent = this.transform.parent;
				toyTransform.position = _toyLocations[i];
				_collected++;

				EventCenter.Instance.CloseInventoryUI ();
				EventCenter.Instance.AddNote (toy.ItemName + " added to Toy Chest");
				break;
			}
			if(_collected == _expected) {
				EventCenter.Instance.TriggerEvent(unlockEvent);
			}
		}
	}
}
