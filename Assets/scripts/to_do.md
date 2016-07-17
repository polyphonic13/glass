* rename InteractiveItem
* CollectableItem
	- extend MonoBehaviour
	- get Item reference > story Item.data reference
	- utilize Item.data properties, adding isUsable to ItemData
	- ItemWeight moved to Resources and instantiated via string const name
	- Remove GameObject storage code
* Icons
	- icon property Sprite > int
	- CrosshairUI: store all Sprites in inspector array, change icon by item icon int
* ItemInspector
	- Clone prefab from Item.data.prefabName on inspect
	- Destroy prefab instance on close
	