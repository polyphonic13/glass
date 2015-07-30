using UnityEngine;

public class CollectableItem : InteractiveElement {

	public string description = "";
	// TBD: extend class with ContainableItem:
	public string _targetContainerName = "";
	
	public Texture iconTexture;
	public Texture detailTexture;

	private Vector3 _originalSize;

	public bool IsCollected { get; set; }
	public bool IsEquipped { get; set; }
	public string ItemName { get; set; }
	public bool IsDroppable { get; set; }
	public bool IsEquipable { get; set; }
	// private Player _player;
	
	public ItemWeight _weight; 
	private Vector3 _previousRigidBodyPos;
	
	void Awake() {
		InitCollectableItem();
		
	}
	
	public void InitCollectableItem() {
		Init(MouseManager.Instance.COLLECT_CURSOR);
		IsCollected = false;
		IsEquipped = false;
		// _player = GameObject.Find("player").GetComponent<Player>();
		_originalSize = transform.localScale;

		EventCenter.Instance.OnEquipItem += OnEquipItem;
	}

	public void OnEquipItem(string itemName) {
		if(IsCollected) {
			Debug.Log("CollectableItem[ " + name + " ]/OnEquipItem, itemName = " + itemName + ", IsEquipped = " + IsEquipped);
			if(name == itemName) {
				if(IsEquipped) { 					// item is already in Use, Store it
					UnEquip();
				} else { 								// item is not being Used, Equip it
					Equip();
				}
			} else { 									// a different item is being Equipped; Store this one
				UnEquip();
			}
		}
	}

	public void OnInputTaken() {
		if(IsRoomActive) {
			InputTaken();
		}
	}

	public override void InputTaken() {
		Debug.Log("CollectableItem/InputTaken, name = " + name);
		var difference = Vector3.Distance(Camera.main.gameObject.transform.position, transform.position);
		if(difference < _interactDistance) {
			if(!IsCollected) {
				AddToInventory();
			}
		}
	}
	
	public void AddToInventory() {
		OnHighlightEnd();
		IsCollected = true;
		// _player.inventory.addItem(this);
		UnEquip();
		Attach();
	}

	public virtual void Attach() {
//		Debug.Log("CollectableItem/Attach");
//		AttachToRightHand();
		AttachToBackpack();
	}
	 
	public void AttachToBackpack() {
		AttachToObject("backpack");
	}
	
	public void AttachToRightHand() {
		AttachToObject("right_hand");
	}
	
	public void AttachToLeftHand() {
		AttachToObject ("left_hand");
	}
	
	public void AttachToObject(string target) {
		Debug.Log ("CollectableItem[" + name + "]/AttachToObject, target = " + target);
//		var tgt = Camera.main.transform.Search(target);
		// var tgt = _player.transform.Search (target);
		// Debug.Log ("tgt = " + tgt);
		// transform.position = tgt.transform.position;
		// transform.rotation = tgt.transform.rotation;
		// transform.parent = tgt.transform;	
	}
	
	public void RemoveFromInventory() {
		IsCollected = false;
	}
	
	public virtual void Equip() {
		Use();
	}

	public void Use(string tgt = "right_hand") {
//		Debug.Log("CollectableItem[ " + name + " ]/Use");
		IsEquipped = true;
		transform.localScale = _originalSize;
		AttachToObject(tgt);
	}
	
	public virtual void UnEquip() {
		Store();
	}
	
	public void Store() {
//		Debug.Log("CollableItem[ " + name + " ]/putAway");
		IsEquipped = false;
		transform.localScale = new Vector3(0, 0, 0);
		AttachToBackpack();
	}

	public virtual void Drop() {
		IsEquipped = false;
		IsCollected = false;
		transform.localScale = _originalSize;
		transform.parent = null;

		var __weightClone = (ItemWeight) Instantiate(_weight, transform.position, transform.rotation);
		__weightClone.TargetContainerName = _targetContainerName;
		__weightClone.ParentObject = gameObject;
		__weightClone.transform.parent = transform;
	}
	
}
