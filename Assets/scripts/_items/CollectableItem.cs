using UnityEngine;
using Polyworks;

public class CollectableItem : Item {

	public string description = "";
	public Sprite Thumbnail;

	private const string ITEM_WEIGHT = "item_weight";
	
	void Awake() {
		InitCollectableItem();
	}
	
	public void InitCollectableItem() {
		data.isCollected = false;
	}

	public override void Actuate () {
		if(!data.isCollected) {
			AddToInventory();
		}
	}

	public void AddToInventory() {
		var isAdded = Inventory.Instance.AddItem(this);
		if(isAdded) {
			data.isCollected = true;
		}
	}

	public virtual void Collect() {
		data.isCollected = true;
		EventCenter.Instance.NearInteractiveItem (this, false);
	}
	
	public virtual void Use() {
		Debug.Log("CollectableItem[ " + name + " ]/Use");
	}
	
	public virtual void Drop(bool useGravity = true) {
		if (useGravity) {
			BoxCollider collider = gameObject.GetComponent<BoxCollider> ();

			var scale = collider.transform.localScale;
			var _weightClone = (ItemWeight) Instantiate (Resources.Load(ITEM_WEIGHT), collider.transform.position, collider.transform.rotation);
			_weightClone.transform.localScale = scale;
			_weightClone.collectableItem = this;
			_weightClone.transform.parent = collider.transform;
		}
		data.isUsable = false;
	}

}
