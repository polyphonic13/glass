namespace Polyworks
{
    using UnityEngine;
    using UnityEngine.UI;

    public class InventoryItemUI : MonoBehaviour
    {
        public Text itemName;
        public GameObject nameBg;
        public Text itemCount;

        public Image itemThumbnail;
        public bool isDroppable;

        public CanvasGroup ButtonGroup;
        public Text[] ButtonLabels;
        public Image[] ButtonBackgrounds;

        public Color32 activeColor = new Color32(150, 150, 150, 100);
        public Color32 inactiveColor = new Color32(0, 0, 0, 100);
        public Color32 controlInactivateColor = new Color32(25, 25, 25, 100);

        private Image itemBg;
        private Image useImage;
        private Image inspectImage;
        private Image dropImage;

        private int focusedControlButton;
        private int previousControlButton;
        private int availableControlButtons;

        private string initName = "";
        private bool isInitialized = false;

        public void Select()
        {
            // Debug.Log ("InventoryItemUI/Select");
            ButtonGroup.alpha = 1;
            SetControlButtonFocus(0);
        }

        public void Deselect()
        {
            // Debug.Log ("InventoryItemUI/Deselect");
            ButtonGroup.alpha = 0;
            focusedControlButton = 0;
        }

        public void UpdateControlButtonFocus(bool increment)
        {
            int btn = focusedControlButton;
            if (increment)
            {
                if (focusedControlButton < (availableControlButtons - 1))
                {
                    btn++;
                }
                else
                {
                    btn = 0;
                }
            }
            else
            {
                if (focusedControlButton > 0)
                {
                    btn--;
                }
                else
                {
                    btn = (availableControlButtons - 1);
                }
            }
            SetControlButtonFocus(btn);
        }

        public void SetControlButtonFocus(int btn)
        {
            previousControlButton = focusedControlButton;
            focusedControlButton = btn;

            setButtonTextAndBgColor(previousControlButton, controlInactivateColor, activeColor);
            setButtonTextAndBgColor(focusedControlButton, activeColor, controlInactivateColor);
        }

        public void SelectControlButton()
        {
            Inventory playerInventory = Game.Instance.GetPlayerInventory();

            switch (focusedControlButton)
            {
                case 0:
                    // Debug.Log("InventoryItemUI[ " + this.name + " ]/SelectControlButton");
                    EventCenter.Instance.CloseInventoryUI();
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

        public void SetFocus(bool isActive)
        {
            // Debug.Log("InventoryItemUI[ " + this.name + " ]/SetFocus, isActive = " + isActive);
            if (isActive)
            {
                itemName.gameObject.SetActive(true);
                nameBg.SetActive(true);
                itemBg.color = activeColor;
                return;
            }

            itemName.gameObject.SetActive(false);
            nameBg.SetActive(false);
            itemBg.color = inactiveColor;

            initButtonColors();
        }

        public void SetName(string name)
        {
            itemName.text = name;
            if (initName == "")
            {
                initName = name;
            }
        }

        public void SetCount(int count)
        {
            if (count > 1)
            {
                itemCount.text = "x" + count;
            }
            else
            {
                itemCount.text = "";
            }
        }

        public void SetThumbnail(Sprite thumbnail)
        {
            if (thumbnail != null)
            {
                itemThumbnail.sprite = thumbnail;
                itemThumbnail.color = Color.white;
            }
            else
            {
                itemThumbnail.sprite = null;
                itemThumbnail.color = new Color32(0, 0, 0, 0);
            }
        }

        public void SetDroppable(bool droppable)
        {
            isDroppable = droppable;
            if (!isDroppable)
            {
                availableControlButtons--;
                GameObject dropPanel = ButtonGroup.transform.Find("panel_drop").gameObject;
                dropPanel.SetActive(false);
            }
        }

        public void Reset()
        {
            gameObject.name = initName;
            SetName("");
            SetCount(0);
            SetThumbnail(null);
            SetControlButtonFocus(0);
            SetFocus(false);
            Deselect();
        }

        private void initButtonColors()
        {
            Color textColor;
            Color bgColor;

            for (int i = 0; i < ButtonLabels.Length; i++)
            {
                if (i == 0)
                {
                    textColor = controlInactivateColor;
                    bgColor = activeColor;
                }
                else
                {
                    textColor = activeColor;
                    bgColor = controlInactivateColor;
                }
                setButtonTextAndBgColor(i, textColor, bgColor);
            }
        }

        private void setButtonTextAndBgColor(int index, Color textColor, Color bgColor)
        {
            Text text = ButtonLabels[index];
            text.color = textColor;
            Image bg = ButtonBackgrounds[index];
            bg.color = bgColor;
        }

        private void init()
        {
            if (isInitialized)
            {
                return;
            }

            isInitialized = true;
            itemBg = GetComponent<Image>();
            // Debug.Log("InventoryItemUI[ " + this.nameBg + "/Awake, itemBg = " + itemBg);
            ButtonGroup.alpha = 0;

            SetFocus(false);
            SetThumbnail(null);

            availableControlButtons = ButtonLabels.Length;
        }

        private void Awake()
        {
            init();
        }
    }
}

