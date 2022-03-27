namespace Polyworks
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;

    public class InventoryUI : UIController
    {
        #region public members
        public InventoryItemUI[] Items;
        public ScrollRect ItemScrollRect;

        public int NumColumns = 5;
        public int NumRows = 12;

        public string[] IgnoredItems;
        #endregion

        #region private members
        private Inventory playerInventory;

        private InventoryItemUI selectedInventoryItemUI = null;

        private float itemHolderHeight;
        private float scrollSpeed = 10f;
        private int scroll = 0;

        private int itemIndex;

        private int xMove;
        private int yMove;

        private int currentColumn;
        private int currentRow;
        private int currentItemIndex;
        private int previousItemIndex;

        private bool isInspectingItem = false;

        #endregion

        #region public event handlers
        public void OnOpenInventoryUI()
        {
            SetActive(true);
        }

        public void OnCloseInventoryUI()
        {
            reset();
        }

        public void OnInventoryAdded(string name, int count, bool isPlayerInventory)
        {
            // Debug.Log ("InventoryUI/OnInventoryAdded, name = " + name);
            if (!isPlayerInventory)
            {
                return;
            }
            for (int i = 0; i < IgnoredItems.Length; i++)
            {
                if (name == IgnoredItems[i])
                {
                    return;
                }
            }
            resetItems();
        }

        public void OnInventoryRemoved(string name, int count)
        {
            // Debug.Log ("InventoryUI/OnInventoryRemoved, name = " + name);
            resetItems();
        }

        public void OnInspectItem(bool isInspecting, string item)
        {
            isInspectingItem = isInspecting;
            base.SetActive(!isInspecting);
        }
        #endregion

        #region public methods
        protected override void Init()
        {
            base.Init();

            eventCenter.OnOpenInventoryUI += OnOpenInventoryUI;
            eventCenter.OnCloseInventoryUI += OnCloseInventoryUI;
            eventCenter.OnInventoryAdded += OnInventoryAdded;
            eventCenter.OnInventoryRemoved += OnInventoryRemoved;
            eventCenter.OnInspectItem += OnInspectItem;

            itemIndex = -1;
            itemHolderHeight = ItemScrollRect.content.sizeDelta.y;
        }

        public void InitInventory(Inventory inventory)
        {
            playerInventory = inventory;
            resetItems();
        }

        protected override void SetActive(bool isActive)
        {
            // Debug.Log("InventoryUI/SetActive, isActive = " + isActive);
            base.SetActive(isActive);

            if (itemIndex == -1 || !isActive)
            {
                return;
            }
            InventoryItemUI item = Items[currentItemIndex];
            item.SetFocus(true);
        }
        #endregion

        #region build UI
        private void setItem(string name)
        {
            if (itemIndex == (NumColumns * NumRows) - 1)
            {
                // used all available slots
                return;
            }
            itemIndex++;

            CollectableItemData itemData = playerInventory.Get(name);
            InventoryItemUI itemUI = Items[itemIndex];
            // Debug.Log ("InventoryUI/setItem, name = " + name + ", itemData = " + itemData);
            // Debug.Log ("itemData.thumbnail = " + itemData.thumbnail);
            itemUI.name = name;
            itemUI.SetName(itemData.displayName);
            itemUI.SetCount(itemData.count);

            if (itemData.thumbnail != "")
            {
                // Debug.Log("Inventory/setItem, itemData.thumbnail = " + itemData.thumbnail);
                GameObject itemObj = (GameObject)Instantiate(Resources.Load(itemData.thumbnail, typeof(GameObject)), transform.position, transform.rotation);
                itemObj.transform.SetParent(this.transform);
                Image thumbnail = itemObj.GetComponent<Image>();
                itemUI.SetThumbnail(thumbnail.sprite);
            }
            if (itemIndex == 0)
            {
                itemUI.SetFocus(true);
            }
        }

        private void resetItems()
        {
            InventoryItemUI itemUI;

            itemIndex = -1;
            previousItemIndex = currentItemIndex = 0;

            for (int i = 0; i < Items.Length; i++)
            {
                itemUI = Items[i];
                itemUI.Reset();
            }

            int total = NumColumns * NumRows;
            int count = 0;

            Hashtable items = playerInventory.GetAll();
            foreach (CollectableItemData itemData in items.Values)
            {
                if (count < total)
                {
                    setItem(itemData.name);
                }
                count++;
            }
        }

        #endregion

        #region update
        private void handleCancel()
        {
            cancel = false;

            if (isInspectingItem)
            {
                return;
            }

            if (selectedInventoryItemUI == null)
            {
                eventCenter.CloseInventoryUI();
                return;
            }
            selectedInventoryItemUI.Deselect();
            selectedInventoryItemUI = null;
        }

        private void handleConfirm()
        {
            confirm = false;

            if (selectedInventoryItemUI != null)
            {
                selectedInventoryItemUI.SelectControlButton();
                return;
            }

            if (currentItemIndex > itemIndex)
            {
                return;
            }

            selectedInventoryItemUI = Items[currentItemIndex];

            if (selectedInventoryItemUI == null)
            {
                return;
            }
            selectedInventoryItemUI.Select();
        }

        private void handleControlButtonFocus()
        {
            if (selectedInventoryItemUI == null)
            {
                return;
            }

            if (up)
            {
                selectedInventoryItemUI.UpdateControlButtonFocus(false);
                return;
            }

            if (!down)
            {
                return;
            }
            selectedInventoryItemUI.UpdateControlButtonFocus(true);
        }

        private void setXandYDirection()
        {
            xMove = 0;
            yMove = 0;
            if (up)
            {
                yMove = 1;
                return;
            }

            if (down)
            {
                yMove = -1;
                return;
            }

            if (left)
            {
                xMove = -1;
                return;
            }

            if (!right)
            {
                return;
            }
            xMove = 1;
        }

        private void updateRowAndColumn()
        {
            setXandYDirection();

            // Debug.Log("xMove = " + xMove + ", yMove = " + yMove);
            if (xMove == 0 && yMove == 0)
            {
                return;
            }

            bool isColumnChanged = calculateCol();

            if (isColumnChanged)
            {
                return;
            }
            calculateRow();
        }

        private void handleItemNavigationAndFocus()
        {
            updateRowAndColumn();

            currentItemIndex = (currentRow * NumColumns) + currentColumn;

            if (currentItemIndex == previousItemIndex)
            {
                return;
            }
            InventoryItemUI item = Items[currentItemIndex];
            // Debug.Log("  currentItemIndex = " + currentItemIndex + ", item = " + item);
            if (item == null)
            {
                return;
            }
            item.SetFocus(true);

            InventoryItemUI prevItem = Items[previousItemIndex];
            if (prevItem == null)
            {
                return;
            }
            prevItem.SetFocus(false);
            previousItemIndex = currentItemIndex;
        }

        private void checkInput()
        {
            // Debug.Log("InventoryUI/checkInput, u/d, l/f = " + up + " / " + down + ", " + left + " / " + right);
            if (cancel)
            {
                handleCancel();
                return;
            }

            if (itemIndex == -1 || isInspectingItem)
            {
                return;
            }

            if (confirm)
            {
                handleConfirm();
                return;
            }

            if (selectedInventoryItemUI != null)
            {
                handleControlButtonFocus();
                return;
            }

            handleItemNavigationAndFocus();
        }
        #endregion

        #region ui nav
        private bool calculateCol()
        {
            if (xMove == 0)
            {
                return false;
            }
            if (xMove < 0)
            {
                decrementCol();
                return true;
            }
            incrementCol();
            return true;
        }

        private void incrementCol()
        {
            if (currentColumn < (NumColumns - 1))
            {
                currentColumn++;
                return;
            }

            if (currentRow == (NumRows - 1))
            {
                // at the end, do nothing
                return;
            }
            currentColumn = 0;
            incrementRow();
        }

        private void decrementCol()
        {
            if (currentColumn > 0)
            {
                currentColumn--;
                return;
            }

            if (currentRow == 0)
            {
                // at beginning, do nothing
                return;
            }
            currentColumn = NumColumns - 1;
            decrementRow();
        }

        private void calculateRow()
        {
            if (yMove == 0)
            {
                return;
            }
            if (yMove > 0)
            {
                decrementRow();
                return;
            }
            incrementRow();
        }

        private void incrementRow()
        {
            if (currentRow >= (NumRows - 1))
            {
                return;
            }
            currentRow++;
            scroll = 1;
            moveHolder();
        }

        private void decrementRow()
        {
            if (currentRow < 1)
            {
                return;
            }
            currentRow--;
            scroll = -1;
            moveHolder();
        }

        private void moveHolder()
        {
            float position = 1f - ((float)currentRow / (float)NumRows);
            // Debug.Log("currentRow = " + currentRow + ", NumRows = " + NumRows + ", position = " + position);
            ItemScrollRect.verticalNormalizedPosition = position;
        }
        #endregion

        #region private methods
        private void reset()
        {
            // Debug.Log("InventoryUI/reset");
            if (selectedInventoryItemUI != null)
            {
                selectedInventoryItemUI.Deselect();
                selectedInventoryItemUI = null;
            }
            InventoryItemUI item = Items[currentItemIndex];
            item.SetFocus(false);

            currentItemIndex = 0;
            currentColumn = 0;
            currentRow = 0;
            currentItemIndex = 0;
            previousItemIndex = 0;

            base.SetActive(false);
        }
        #endregion

        #region unity methods
        private void Awake()
        {
            Init();
        }

        private void FixedUpdate()
        {
            if (!isActive)
            {
                return;
            }
            checkInput();
            xMove = yMove = scroll = 0;
            up = down = left = right = false;
        }

        private void OnDestroy()
        {
            if (eventCenter == null)
            {
                return;
            }
            eventCenter.OnOpenInventoryUI += OnOpenInventoryUI;
            eventCenter.OnCloseInventoryUI -= OnCloseInventoryUI;
            eventCenter.OnInventoryAdded -= OnInventoryAdded;
            eventCenter.OnInventoryRemoved -= OnInventoryRemoved;
            eventCenter.OnInspectItem -= OnInspectItem;
        }
        #endregion
    }
}
