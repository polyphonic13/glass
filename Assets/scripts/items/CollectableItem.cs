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

	public virtual void ItemUpdate() {
		if(IsEnabled && !IsCollected) {
			CheckProximity();
		}
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
//		transform.localScale = new Vector3(0, 0, 0);
		AttachToObject(_backpack);
	}
	
	public void AttachToRightHand() {
		transform.localScale = _originalSize;
		AttachToObject (_rightHand);
	}
	
	public void AttachToObject(Transform target) {
		// Debug.Log("CollectableItem[" + name + "]/AttachToObject, target = " + target);
		if (target != null) {

			transform.position = target.transform.position;
//			transform.rotation = target.transform.rotation;
			transform.parent = target;
		}
	}

	public virtual void Collect(Transform backpack, Transform rightHand) {
		IsCollected = true;
		_backpack = backpack;
		_rightHand = rightHand;
		Store ();
		EventCenter.Instance.NearInteractiveItem(this, false);
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
		AttachToBackpack();
	}

	public virtual void Drop() {
		AttachToRightHand ();
		transform.parent = null;

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
