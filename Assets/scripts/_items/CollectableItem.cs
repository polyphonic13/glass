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
		Init();
		data.isCollected = false;
	}

	public override void Actuate () {
		if(!isCollected) {
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
		Store ();
		EventCenter.Instance.NearInteractiveItem (this, false);
	}
	
	public virtual void Use() {
		Debug.Log("CollectableItem[ " + name + " ]/Use");
	}
	
	public virtual void Drop(bool useGravity = true) {
		if (useGravity) {
			BoxCollider collider = gameObject.GetComponent<BoxCollider> ();

			var scale = collider.transform.localScale;
			//		Debug.Log (this.name + " local scale = " + scale);
			var _weightClone = (ItemWeight) Instantiate (Resources.load(ITEM_WEIGHT), collider.transform.position, collider.transform.rotation);
			__weightClone.transform.localScale = scale;
			__weightClone.collectableItem = this;
			__weightClone.transform.parent = collider.transform;
		}
		data.isUsable = false;
	}

}
