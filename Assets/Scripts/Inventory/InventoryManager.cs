using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static bool inventoryEnabled = false;
    public GameObject inventoryPanel;
    public GameObject hotbarPanel;
    public GameObject itemSlotPrefab;
    public int startSlotRows = 4;
    private int slotRow;
    public static List<ItemSlot> inventoryList = new List<ItemSlot>();
    public GameObject canvas;
    public static Vector3 offsetInventory;
    public static Vector3 offsetHotbar;

    public class ItemSlot
    {
        public GameObject slotObj { get; set; }
        public int slotID { get; set; }
        public GameObject itemObj { get; set; }
        public ItemDatabase.Item item { get; set; }
        public int amount { get; set; }
        public GameObject hotbarObj { get; set; }

        /// <summary>
        /// Class for the item slots
        /// </summary>
        /// <param name="slotObj">GameObject of the item slot</param>
        /// <param name="slotID">ID of the item slot</param>
        /// <param name="item">The item in the item slot</param>
        /// <param name="amount">How much of the item is in the slot</param>
        /// <param name="hotbarObj">GameObject of the hotbar slot, is null if the item slot is not a hotbar slot (not the first 5 item slots at the bottom in the inventory)</param>
        public ItemSlot(GameObject slotObj, int slotID, ItemDatabase.Item item, int amount, GameObject hotbarObj)
        {
            this.slotObj = slotObj;
            this.slotObj.transform.GetComponent<Slot>().itemSlot = this;
            this.slotID = slotID;
            this.hotbarObj = hotbarObj;

            this.itemObj = this.slotObj.transform.GetChild(0).gameObject;

            initiateItem(item, amount);
        }


        /// <summary>
        /// Refreshes the item in the item slot with a new item
        /// </summary>
        /// <param name="item">The new item in the item slot</param>
        /// <param name="amount">How much of the item there is</param>
        public void initiateItem(ItemDatabase.Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
            
            this.itemObj.GetComponent<Image>().sprite = this.item.sprite;
            this.itemObj.name = this.item.title;


            if (this.hotbarObj != null) //If it's not a hotbar item slot
            {
                this.hotbarObj.transform.GetChild(0).GetComponent<Image>().sprite = this.item.sprite;
                this.hotbarObj.transform.GetChild(0).name = this.item.title;
            }

            updateAmount();
        }

        /// <summary>
        /// Updates the amount/stack text in the item slot
        /// </summary>
        public void updateAmount()
        {
            GameObject amountObj = this.itemObj.transform.GetChild(0).gameObject;
            string amountText;
            if (this.amount == 0) { amountText = ""; }
            else { amountText = this.amount.ToString(); }
            amountObj.GetComponent<TextMeshProUGUI>().text = amountText;

            if (this.hotbarObj != null)
            {
                amountObj = this.hotbarObj.transform.GetChild(0).GetChild(0).gameObject;
                amountObj.GetComponent<TextMeshProUGUI>().text = amountText;
            }
        }

        /// <summary>
        /// <para>Add the amount of the grabbed item to itemSlot</para>
        /// If the amount is higher than the stackLimit, the amount of the itemSlot will be the stackLimit and the amount of the grabbed item will get the rest
        /// </summary>
        /// <param name="amount">The item amount of the grabbedItem</param>
        public void sumItems(int amount)
        {
            this.amount += amount;

            int rest = 0;
            if (this.amount > Slot.grabbedItem.stackLimit) //If more than stacklimit
            {
                rest = this.amount - Slot.grabbedItem.stackLimit;
                this.amount = Slot.grabbedItem.stackLimit;
            }

            initiateItem(Slot.grabbedItem, this.amount);
            Slot.updateGrabbedItemObj(Slot.grabbedItem, rest);
        }

        /// <summary>
        /// Adds 1 to the amount of itemSlot and remove 1 to the amount of the grabbed item
        /// </summary>
        public void addOne()
        {
            if (this.amount != Slot.grabbedItem.stackLimit)
            {
                this.amount += 1;
                initiateItem(Slot.grabbedItem, this.amount);
                Slot.updateGrabbedItemObj(Slot.grabbedItem, Slot.grabbedAmount - 1);
            }
        }


        /// <summary>
        /// Switch the items and amount in itemSlot and grabbed item
        /// </summary>
        /// <param name="item">The grabbedItem</param>
        /// <param name="amount">The amount of the grabbed item</param>
        public void switchItems(ItemDatabase.Item item, int amount)
        {
            ItemDatabase.Item itemTemp = this.item;
            int amountTemp = this.amount;

            initiateItem(item, amount);

            Slot.updateGrabbedItemObj(itemTemp, amountTemp);
        }

        /// <summary>
        /// Refresh the item slot with the item "Empty"
        /// </summary>
        public void emptyItemSlot()
        {
            initiateItem(ItemDatabase.fetchItemByID(0), 0);
        }

    }

    /// <summary>
    /// Fills the database, updates the UI size, adds the item slots to the inventory and keep them in a list and receive the hotbar slots
    /// </summary>
    private void Start()
    {
        ListenToSettings();
        ItemDatabase.fillDatabase();

        updateUISize();

        while (slotRow < startSlotRows)
        {
            addRow();
        }

        inventoryList[0].initiateItem(ItemDatabase.fetchItemByID(3), 1);
        inventoryList[1].initiateItem(ItemDatabase.fetchItemByID(4), 1);

        Equip.getHotbarItemSlots(inventoryList);
    }


    /* Item image square width and height = 50
     * distance between item slots = 60
     * distance between slot panel and inventory panel width = 20 (left and right)
     * distance between slot panel and inventory panel height = 15 (up and down)
     */


    /// <summary>
    /// Updates the UI size (inventory and hotbar dimensions) if the screen dimensions change
    /// </summary>
    public void updateUISize()
    {
        Vector2 canvasSize = canvas.transform.GetComponent<RectTransform>().sizeDelta;

        RectTransform rectTransformInventory = inventoryPanel.transform.GetComponent<RectTransform>();

        /* width inventory = 330
         * Calculate left and right:
         * (canvas width - width inventory) / 2 = left and right
         * 
         * height 1 row inventory = 80
         * Calculate top and bottom:
         * (canvas height - height inventory) / 2 = top and bottom
         */

        int inventoryWidth = 330;
        int inventoryHeight = 80;

        int extraDistance = 0; //Extra distance for hotbar
        if (startSlotRows > 1)
        {
            extraDistance = 5;
        }

        float inventoryBottomTop = (canvasSize.y - (60*(startSlotRows-1) + inventoryHeight + extraDistance)) / 2;
        float inventoryLeftRight = (canvasSize.x - inventoryWidth) / 2;

        // [ left - bottom ]
        rectTransformInventory.offsetMin = new Vector2(inventoryLeftRight, inventoryBottomTop);
        // [ right - top ]
        rectTransformInventory.offsetMax = new Vector2(-inventoryLeftRight, -inventoryBottomTop);

        offsetInventory = new Vector3(inventoryLeftRight + 20, inventoryBottomTop + 15, 0);


        RectTransform rectTransformHotbar = hotbarPanel.transform.GetComponent<RectTransform>();

        float hotbarHeight = canvasSize.y - inventoryHeight + 10;

        // [ left - bottom ]
        rectTransformHotbar.offsetMin = new Vector2(inventoryLeftRight + 10, hotbarHeight / 20);
        // [ right - top ]
        rectTransformHotbar.offsetMax = new Vector2(-inventoryLeftRight - 10, -hotbarHeight* 19/20);

        offsetHotbar = new Vector3(inventoryLeftRight + 20, hotbarHeight / 20 + 10, 0);
    }

    /// <summary>
    /// Adds a new row in the inventory
    /// </summary>
    public void addRow()
    {

        for (int i = 0; i < 5; i++)
        {
            int extraDistance = 0; //Extra distance for hotbar
            if (slotRow > 0)
            {
                extraDistance = 5;
            }

            GameObject slotObj = Instantiate(itemSlotPrefab, new Vector3(265 + -60*i, 25 + 60*slotRow + extraDistance, 0) + offsetInventory, new Quaternion(0, 0, 0, 0), inventoryPanel.transform.GetChild(0));
            slotObj.name = "Item Slot " + inventoryList.Count;

            GameObject hotbarObj = null;
            if (slotRow == 0)       //If hotbar (first 5 item slots)
            {
                hotbarObj = Instantiate(itemSlotPrefab, new Vector3(265 + -60*i, 25, 0) + offsetHotbar, new Quaternion(0, 0, 0, 0), hotbarPanel.transform.GetChild(0));
                Destroy(hotbarObj.GetComponent<Slot>());
                Destroy(hotbarObj.transform.Find("Hover").gameObject);
                hotbarObj.name = "Hotbar Slot " + inventoryList.Count;
            }

            //ItemSlot itemSlot = new ItemSlot(slotObj, inventoryList.Count, ItemDatabase.fetchItemByID(0), 0, hotbarObj); //Default
            ItemSlot itemSlot;
            if (i % 2 == 0) { itemSlot = new ItemSlot(slotObj, inventoryList.Count, ItemDatabase.fetchItemByID(1), 32, hotbarObj); }
            else { itemSlot = new ItemSlot(slotObj, inventoryList.Count, ItemDatabase.fetchItemByID(2), 50, hotbarObj); }

            inventoryList.Add(itemSlot);
        }



        slotRow += 1;
    }

    /// <summary>
    /// Returns an item Slot from inventoryList with the id
    /// </summary>
    /// <param name="id">The id of the item Slot</param>
    /// <returns>The specified item Slot from the list</returns>
    public static ItemSlot fetchItemSlotByID(int id)
    {
        return inventoryList[id];
    }


//WIP WIP WIP WIP WIP WIP WIP
    /// <summary>
    /// Gets called if the player picks up an item off the ground (WIP)
    /// </summary>
    /// <param name="item">The item the player picked up off the ground</param>
    /// <param name="amount">How much of the item there is</param>
    public static void pickUpItem(ItemDatabase.Item item, int amount)
    {
        //Do stuff
        // if item already in inventory, add
        // if item not in inventory or item in inventory but all full stacks, then put in new item slot (from last to first)
        // if inventory full, don't pick up item

        //This could be a method in a separate class (item entity class)
    }
//WIP WIP WIP WIP WIP WIP WIP


    /// <summary>
    /// Remove the item from the inventory and drop it on the ground (The item gets destroyed/removed for now)
    /// </summary>
    /// <param name="itemSlot">The item slot where the item is contained</param>
    public static void dropItem(ItemSlot itemSlot)
    {
        //Remove item for now
        itemSlot.emptyItemSlot();
    }

    private KeyCode openInvButton;

    /// <summary>
    /// Listen to the settings for the right keycode to open the inventory
    /// </summary>
    private void ListenToSettings()
    {
        Setting oinv = SettingsManager.GetInstance().Settings[0].GetSetting("oinv");

        oinv.changeEvent += (sender, value) =>
        {
            openInvButton = (KeyCode)value;
        };

        openInvButton = (KeyCode)oinv.Value;
    }

    // Update is called once per frame
    /// <summary>
    /// Open and close the inventory by pressing I
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(openInvButton))
        {
            inventoryEnabled = !inventoryEnabled;
            inventoryPanel.SetActive(inventoryEnabled);
            hotbarPanel.SetActive(!inventoryEnabled);
            if (inventoryEnabled) { Cursor.lockState = CursorLockMode.None; }
            else { Cursor.lockState = CursorLockMode.Locked; }
        }
    }

    /// <summary>
    /// Check for any screensize changes
    /// </summary>
    private void OnRectTransformDimensionsChange()
    {
        updateUISize();
    }

}
