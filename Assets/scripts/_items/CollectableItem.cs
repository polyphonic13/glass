using UnityEngine;
using Polyworks;

public class CollectableItem : Item {

	public string description = "";
	public Sprite Thumbnail;

	private const string ITEM_WEIGHT = "item_weight";
	
	public override void Actuate () {
		base.Collect (Game.Instance.GetPlayerInventory ());
	}

}
