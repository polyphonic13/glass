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
        public Color32 inactiveColor = new Color32(25, 25, 25, 100);

        private Image itemBg;
        private Image useImage;
        private Image inspectImage;
        private Image dropImage;

        private int focusedControlButton = 0;
        private int previousControlButton = 0;
        private int availableControlButtons;

        private string initName = "";
        private bool isInitialized = false;

        public void Select()
        {
            ButtonGroup.alpha = 1;
            SetControlButtonFocus(0);
        }

        public void Deselect()
        {
            ButtonGroup.alpha = 0;
            focusedControlButton = 0;
        }

        public void UpdateControlButtonFocus(bool increment)
        {
            int index = getNextFocusedControlIndex(increment);
            SetControlButtonFocus(index);
        }

        public void SetControlButtonFocus(int index)
        {
            previousControlButton = focusedControlButton;
            focusedControlButton = index;

            setButtonBgAndTextColor(focusedControlButton, activeColor, inactiveColor);

            if (previousControlButton == focusedControlButton)
            {
                return;
            }
            setButtonBgAndTextColor(previousControlButton, inactiveColor, activeColor);
        }

        public void SelectControlButton()
        {
            Inventory playerInventory = Game.Instance.GetPlayerInventory();

            switch (focusedControlButton)
            {
                case 0:
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
                    Debug.LogWarning("Unknown control button");
                    break;
            }
        }

        public void SetFocus(bool isActive)
        {
            itemName.gameObject.SetActive(isActive);
            nameBg.SetActive(isActive);
            itemBg.color = (isActive) ? activeColor : inactiveColor;

            initButtonColors();
        }

        public void SetName(string name)
        {
            itemName.text = name;
            if (initName != "")
            {
                return;
            }
            initName = name;
        }

        public void SetCount(int count)
        {
            itemCount.text = (count > 1) ? ("x" + count) : "";
        }

        public void SetThumbnail(Sprite thumbnail)
        {
            if (thumbnail != null)
            {
                itemThumbnail.sprite = thumbnail;
                itemThumbnail.color = Color.white;
                return;
            }
            itemThumbnail.sprite = null;
            itemThumbnail.color = new Color32(0, 0, 0, 0);
        }

        public void SetDroppable(bool droppable)
        {
            isDroppable = droppable;
            if (isDroppable)
            {
                return;
            }
            availableControlButtons--;
            GameObject dropPanel = ButtonGroup.transform.Find("panel_drop").gameObject;
            dropPanel.SetActive(false);
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
                    textColor = inactiveColor;
                    bgColor = activeColor;
                }
                else
                {
                    textColor = activeColor;
                    bgColor = inactiveColor;
                }
                setButtonBgAndTextColor(i, bgColor, textColor);
            }
        }

        private void setButtonBgAndTextColor(int index, Color bgColor, Color textColor)
        {
            Text text = ButtonLabels[index];
            text.color = textColor;
            Image bg = ButtonBackgrounds[index];
            bg.color = bgColor;
        }

        private int getNextFocusedControlIndex(bool increment)
        {
            if (increment)
            {
                if (focusedControlButton < (availableControlButtons - 1))
                {
                    return focusedControlButton + 1;
                }
                return 0;
            }

            if (focusedControlButton > 0)
            {
                return focusedControlButton - 1;
            }
            return availableControlButtons - 1;
        }

        private void init()
        {
            if (isInitialized)
            {
                return;
            }

            isInitialized = true;
            itemBg = GetComponent<Image>();
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

