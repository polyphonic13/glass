using UnityEngine;

public class Inventory : MonoBehaviour {

	[SerializeField] private Canvas _inventoryUI; 

	private static Inventory _instance;
	private Inventory() {}

	public static Inventory Instance {
		get {
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType(typeof(Inventory)) as Inventory;      
			}
			return _instance;
		}
	}
	
	void Start () {
	
	}
	
	void Update () {
	
	}
}
