using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Polyworks {
	public class InventoryItemUI : MonoBehaviour {

		public Text itemName;
		public Text itemCount;
		public Image itemThumbnail;
		public bool isDroppable; 

		public CanvasGroup _controlPanel;

		public Color32 activeColor = new Color32(150, 150, 150, 100);
		public Color32 inactiveColor = new Color32(0, 0, 0, 100);
		public Color32 controlInactivateColor = new Color32(75, 75, 75, 100); 

		private Image _itemBg;

		private ArrayList _panels;
		private int _focusedControlButton; 
		private int _previousControlButton; 
		private int _availableControlButtons; 

		private string _initName = ""; 

		public void Select() {
			// Debug.Log ("InventoryItemUI/Select");
			_controlPanel.alpha = 1;
			SetControlButtonFocus (0);
		}

		public void Deselect() {
			// Debug.Log ("InventoryItemUI/Deselect");
			_controlPanel.alpha = 0;
			_focusedControlButton = 0;
		}

		public void IncrementControlButtonFocus(bool increment) {
			int btn = _focusedControlButton;
			if(increment) {
				if(_focusedControlButton < (_availableControlButtons - 1)) {
					btn++;
				} else {
					btn = 0;
				}
			} else {
				if(_focusedControlButton > 0) {
					btn--;
				} else {
					btn = (_availableControlButtons -1);
				}
			}
			SetControlButtonFocus(btn);
		}

		public void SetControlButtonFocus(int btn) {
			_previousControlButton = _focusedControlButton;
			_focusedControlButton = btn;

			Image panel = _panels[_previousControlButton] as Image;
			panel.color = controlInactivateColor;
			panel = _panels[_focusedControlButton] as Image;
			panel.color = activeColor;
		}

		public void SelectControlButton() {
			Inventory playerInventory = Game.Instance.GetPlayerInventory();

			switch(_focusedControlButton) {
			case 0:
				playerInventory.Use(this.name);
				break;

			case 1:
				EventCenter.Instance.InspectItem(true, this.name);
				break;

			case 2:
				playerInventory.Drop(this.name);
				break;

			default:
				// Debug.LogWarning("Unknown control button");
				break;
			}
		}

		public void SetFocus(bool active) {
			if(active) {
				_itemBg.color = activeColor;
			} else {
				_itemBg.color = inactiveColor;
			}
		} 

		public void SetName(string name) {
			itemName.text = name;
			if(_initName == "") {
				_initName = name;
			}
		}

		public void SetCount(int count) {
			if(count > 0) {
				itemCount.text = "x" + count;
			} else {
				itemCount.text = "";
			}
		}

		public void SetThumbnail(Sprite thumbnail) {
			if(thumbnail != null) {
				itemThumbnail.sprite = thumbnail;
				itemThumbnail.color = Color.white;
			} else {
				itemThumbnail.sprite = null;
				itemThumbnail.color = new Color32(0, 0, 0, 0);
			}
		}

		public void SetDroppable(bool droppable) {
			isDroppable = droppable;
			if (!isDroppable) {
				_availableControlButtons--;
				GameObject dropPanel = _controlPanel.transform.Find("panel_drop").gameObject;
				dropPanel.SetActive (false);
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

		void Awake() {
			
			_controlPanel.alpha = 0;

			_itemBg = GetComponent<Image>();
			SetFocus(false);
			SetThumbnail(null);

			GameObject usePanel = _controlPanel.transform.Find("panel_use").gameObject;
			GameObject inspectPanel = _controlPanel.transform.Find("panel_inspect").gameObject;
			GameObject dropPanel = _controlPanel.transform.Find("panel_drop").gameObject;

			Image useImage = usePanel.GetComponent<Image>();
			Image inspectImage = inspectPanel.GetComponent<Image>();
			Image dropImage = dropPanel.GetComponent<Image>();

			useImage.color = activeColor;
			inspectImage.color = controlInactivateColor;
			dropImage.color = controlInactivateColor;

			_panels = new ArrayList(3);
			_panels.Add(useImage);
			_panels.Add(inspectImage);
			_panels.Add(dropImage);

			_availableControlButtons = _panels.Count;
		}
	}
}

