using UnityEngine;

public class CollectableItem : InteractiveItem {

	public string description = "";
	// TBD: extend class with ContainableItem:
	public string targetContainerName = "";
	public bool preserveCollisions; 

	public bool IsCollected { get; set; }
	public bool IsEquipped { get; set; }
	public bool IsDroppable { get; set; }
	public bool IsEquipable { get; set; }
	public bool IsInspected { get; set; }
	public bool IsUsable { get; set; }

	public ItemWeight _weight; 

	private Transform _backpack;
	private Transform _leftHand;
	private Transform _rightHand;

	private Vector3 _originalSize;

	void Awake() {
		InitCollectableItem();
	}
	
	public void InitCollectableItem() {
		Init();
		IsCollected = IsEquipped = IsInspected = false;
		// _player = GameObject.Find("player").GetComponent<Player>();
		_originalSize = transform.localScale;
	}

	public override void Actuate () {
		if(!IsCollected) {
//			base.Actuate();
			AddToInventory();
		}
	}

	public void AddToInventory() {
		var isAdded = Inventory.Instance.AddItem(this);
		if(isAdded) {
			IsCollected = true;
		}
	}

	public virtual void Attach() {
		AttachToBackpack();
	}
	 
	public void AttachToBackpack() {
		AttachToObject(_backpack);
	}
	
	public void AttachToRightHand() {
		AttachToObject (_rightHand);
	}
	
	public void AttachToLeftHand() {
		AttachToObject (_leftHand);
	}
	
	public void AttachToObject(Transform target) {
		// Debug.Log("CollectableItem[" + name + "]/AttachToObject, target = " + target);
		if (target != null) {

			transform.position = target.transform.position;
//			transform.rotation = target.transform.rotation;
			transform.parent = target;
		}
	}

	public virtual void Collect(Transform backpack) {
		IsCollected = true;
		_backpack = backpack;
		Store ();
	}
	
	public virtual void Equip(Transform rightHand) {
		IsEquipped = true;
		transform.localScale = _originalSize;
		_rightHand = rightHand;
		AttachToRightHand();
	}

	public virtual void Use() {
		Debug.Log("CollectableItem[ " + name + " ]/Use");

	}
	
	public virtual void UnEquip() {
		Store();
	}

	public void Store() {
		IsEquipped = false;
		transform.localScale = new Vector3(0, 0, 0);
		AttachToBackpack();
	}

	public virtual void Drop() {
		transform.localScale = _originalSize;
		AttachToRightHand ();

		BoxCollider collider = gameObject.GetComponent<BoxCollider> ();

		var scale = collider.transform.localScale;
//		Debug.Log (this.name + " local scale = " + scale);
		var __weightClone =(ItemWeight) Instantiate(_weight, collider.transform.position, collider.	transform.rotation);
		__weightClone.transform.localScale = scale;
		__weightClone.collectableItem = this;
		__weightClone.transform.parent = collider.transform;

		IsEquipped = IsUsable = false;
	}

}
