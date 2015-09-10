using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour {

	[SerializeField] private Text _itemName;
    [SerializeField] private Text _itemCount;
	[SerializeField] private Image _itemThumbnail;

	[SerializeField] private CanvasGroup _controlPanel;

	private Image _itemBg;
    private Color32 _active;
	private Color32 _inactive; 
	private Color32	_controlInactive; 

	private ArrayList _panels;
	private int _focusedControlButton; 
	private int _previousControlButton; 

	private string _initName = ""; 
	
	void Awake() {
		_controlPanel.alpha = 0;
		_active = new Color32(150, 150, 150, 100);
		_inactive = new Color32(0, 0, 0, 100);
		_controlInactive = new Color32(75, 75, 75, 100);

		_itemBg = GetComponent<Image>();
		SetFocus(false);
		SetThumbnail(null);

		var usePanel = _controlPanel.transform.Find("panel_use").gameObject;
		var inspectPanel = _controlPanel.transform.Find("panel_inspect").gameObject;
		var dropPanel = _controlPanel.transform.Find("panel_drop").gameObject;

		var useImage = usePanel.GetComponent<Image>();
		var inspectImage = inspectPanel.GetComponent<Image>();
		var dropImage = dropPanel.GetComponent<Image>();

		useImage.color = _active;
		inspectImage.color = _controlInactive;
		dropImage.color = _controlInactive;

		_panels = new ArrayList(3);
		_panels.Add(useImage);
		_panels.Add(inspectImage);
		_panels.Add(dropImage);
	}

	public void Select() {
		_controlPanel.alpha = 1;
		SetControlButtonFocus (0);
	}

	public void Deselect() {
		_controlPanel.alpha = 0;
	}

	public void IncrementControlButtonFocus(bool increment) {
		int btn = _focusedControlButton;
		if(increment) {
			if(_focusedControlButton < (_panels.Count - 1)) {
				btn++;
			} else {
				btn = 0;
			}
		} else {
			if(_focusedControlButton > 0) {
				btn--;
			} else {
				btn = (_panels.Count -1);
			}
		}
		SetControlButtonFocus(btn);
	}

	public void SetControlButtonFocus(int btn) {
		_previousControlButton = _focusedControlButton;
		_focusedControlButton = btn;
		
		Image panel = _panels[_focusedControlButton] as Image;
		panel.color = _active;
		panel = _panels[_previousControlButton] as Image;
		panel.color = _controlInactive;
		Debug.Log ("SetControlButtonFocus[" + this.name + "], btn = " + btn);
	}

	public void SelectControlButton() {
		switch(_focusedControlButton) {
			case 0:
				Inventory.Instance.UseItem(this.name);
			break;

			case 1:
				EventCenter.Instance.InspectItem(true, this.name);
			break;

			case 2:
				Inventory.Instance.RemoveItem(this.name);
			break;

			default:
				Debug.LogWarning("Unknown control button");
			break;
		}
	}

	public void SetFocus(bool active) {
		if(active) {
			_itemBg.color = _active;
		} else {
			_itemBg.color = _inactive;
		}
	} 

	public void SetName(string itemName) {
		// Debug.Log("InventoryItemUI/setName: " + itemName);
		_itemName.text = itemName;
		if(_initName == "") {
			_initName = itemName;
		}
	}

	public void SetCount(int count) {
		if(count > 0) {
			_itemCount.text = "x" + count;
		} else {
			_itemCount.text = "";
		}
	}

	public void SetThumbnail(Sprite thumbnail) {
		if(thumbnail != null) {
			_itemThumbnail.sprite = thumbnail;
			_itemThumbnail.color = Color.white;
		} else {
			_itemThumbnail.sprite = null;
			_itemThumbnail.color = new Color32(0, 0, 0, 0);
		}
	}
	
	public void Reset() {
		gameObject.name = _initName;
		SetName("");
		SetCount(0);
		SetThumbnail(null);
		SetControlButtonFocus(0);
		SetFocus(false);
		Deselect();
	}

}
