﻿namespace Polyworks
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;

    public class InventoryItemUI : MonoBehaviour
    {
        public Text itemName;
        public Text itemCount;
        public Image itemThumbnail;
        public bool isDroppable;

        public CanvasGroup _controlPanel;

        public Color32 activeColor = new Color32(150, 150, 150, 100);
        public Color32 inactiveColor = new Color32(0, 0, 0, 100);
        public Color32 controlInactivateColor = new Color32(75, 75, 75, 100);

        private Image itemBg;
        private Image useImage;
        private Image inspectImage;
        private Image dropImage;

        private ArrayList panels;
        private int focusedControlButton;
        private int previousControlButton;
        private int availableControlButtons;

        private string initName = "";

        public void Select()
        {
            // Debug.Log ("InventoryItemUI/Select");
            _controlPanel.alpha = 1;
            SetControlButtonFocus(0);
        }

        public void Deselect()
        {
            // Debug.Log ("InventoryItemUI/Deselect");
            _controlPanel.alpha = 0;
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

            Image panel = panels[previousControlButton] as Image;
            panel.color = controlInactivateColor;
            panel = panels[focusedControlButton] as Image;
            panel.color = activeColor;
        }

        public void SelectControlButton()
        {
            Inventory playerInventory = Game.Instance.GetPlayerInventory();

            switch (focusedControlButton)
            {
                case 0:
                    Debug.Log("InventoryItemUI[ " + this.name + " ]/SelectControlButton");
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
                itemBg.color = activeColor;
                return;
            }

            itemName.gameObject.SetActive(false);
            itemBg.color = inactiveColor;
            initFirstButtonImage();
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
                GameObject dropPanel = _controlPanel.transform.Find("panel_drop").gameObject;
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

        private void initFirstButtonImage()
        {
            useImage.color = activeColor;
            inspectImage.color = controlInactivateColor;
            dropImage.color = controlInactivateColor;
        }

        private void Awake()
        {

            _controlPanel.alpha = 0;

            itemBg = GetComponent<Image>();
            GameObject usePanel = _controlPanel.transform.Find("panel_use").gameObject;
            GameObject inspectPanel = _controlPanel.transform.Find("panel_inspect").gameObject;
            GameObject dropPanel = _controlPanel.transform.Find("panel_drop").gameObject;

            useImage = usePanel.GetComponent<Image>();
            inspectImage = inspectPanel.GetComponent<Image>();
            dropImage = dropPanel.GetComponent<Image>();

            SetFocus(false);
            SetThumbnail(null);

            panels = new ArrayList(3);
            panels.Add(useImage);
            panels.Add(inspectImage);
            panels.Add(dropImage);

            availableControlButtons = panels.Count;
        }
    }
}

