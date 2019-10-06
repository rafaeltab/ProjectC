using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private bool inventoryEnabled = false;
    public GameObject inventoryPanel;
    public GameObject itemSlotPrefab;
    public int startSlotRows = 4;
    private int slotRow;
    public static List<ItemSlot> inventoryList = new List<ItemSlot>();
    public GameObject canvas;
    public Vector3 offset;

    public class ItemSlot
    {
        public GameObject slotObj { get; set; }
        public int slotID { get; set; }
        public GameObject itemObj { get; set; }
        public ItemDatabase.Item item { get; set; }
        public int amount { get; set; }
        public bool isHotbar;

        public ItemSlot(GameObject slotObj, int slotID, ItemDatabase.Item item, int amount, bool isHotbar)
        {
            this.slotObj = slotObj;
            this.slotObj.transform.GetComponent<Slot>().itemSlot = this;
            this.slotID = slotID;
            this.isHotbar = isHotbar;

            this.itemObj = this.slotObj.transform.GetChild(0).gameObject;
            this.item = item;
            this.amount = amount;

            initiateItem();
            updateAmount();

        }

        public void initiateItem()
        {
            this.itemObj.GetComponent<ItemDrag>().itemSlot = this;
            
            this.itemObj.GetComponent<Image>().sprite = this.item.sprite;
            this.itemObj.name = this.item.title;
        }

        public void updateAmount()
        {
            GameObject amountObj = this.itemObj.transform.GetChild(0).gameObject;

            string amountText;
            if (this.amount == 0) { amountText = ""; }
            else { amountText = this.amount.ToString(); }
            amountObj.GetComponent<Text>().text = amountText;
        }

    }


    private void Start()
    {

        ItemDatabase.fillDatabase();

        updateInventorySize();

        while (slotRow < startSlotRows)
        {
            addRow();
        }

    }


    /* Item image square width and height = 50
     * distance between item slots = 70
     * distance between slot panel and inventory panel width = 10 (left and right)
     * distance between slot panel and inventory panel height = 10 (up and down)
     */



    public void updateInventorySize()
    {
        Vector2 canvasSize = canvas.transform.GetComponent<RectTransform>().sizeDelta;

        RectTransform rectTransform = inventoryPanel.transform.GetComponent<RectTransform>();

        /* width inventory = 350
         * Calculate left and right:
         * (canvas width - width inventory) / 2 = left and right
         * 
         * height 1 row inventory = 70
         * Calculate top and bottom:
         * (canvas height - height inventory) / 2 = top and bottom
         */

        float inventoryHeight = (canvasSize.y - 70*startSlotRows) / 2;
        float inventoryWidth = (canvasSize.x - 350) / 2;

        // [ left - bottom ]
        rectTransform.offsetMin = new Vector2(inventoryWidth, inventoryHeight);
        // [ right - top ]
        rectTransform.offsetMax = new Vector2(-inventoryWidth, -inventoryHeight);

        offset = new Vector3(inventoryWidth + 10, inventoryHeight + 10, 0);
    }

    public void addRow()
    {
        bool isHotbar;
        if (slotRow == 0) { isHotbar = true; }
        else { isHotbar = false; }

        for (int i = 0; i < 5; i++)
        {
            GameObject slotObj = Instantiate(itemSlotPrefab, new Vector3(25 + 70*i, 25 + 70*slotRow, 0) + offset, new Quaternion(0, 0, 0, 0), inventoryPanel.transform.GetChild(0));

            ItemSlot itemSlot = new ItemSlot(slotObj, inventoryList.Count, ItemDatabase.fetchItemByID(1), 1, isHotbar);
            inventoryList.Add(itemSlot);
        }

        slotRow += 1;
    }


    public static ItemSlot fetchItemSlotByID(int id)
    {
        return inventoryList[id];
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            inventoryEnabled = !inventoryEnabled;
            inventoryPanel.SetActive(inventoryEnabled);
        }


    }


    private void OnRectTransformDimensionsChange()
    {
        updateInventorySize();
    }

}
