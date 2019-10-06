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
    private Vector3 firstSlotPos;


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
     * distance between slot panel and inventory panel width = 20 (left and right)
     * distance between slot panel and inventory panel height = 16.75 (up and down)
     * inventory width = 370
     */



    public void updateInventorySize()
    {
        /* start position x = 551.5 - 140 = 411.5
         * start position y = 269 - 35*(slotRows-1)
         */

        firstSlotPos = new Vector3(411.5f, 269 - 35*(startSlotRows-1), 0);
        RectTransform rectTransform = inventoryPanel.transform.GetComponent<RectTransform>();

        //* inventory row 1 top + bottom = 454.35, 454.35 + 70 = 524.35 (0 rows)

        float inventoryHeight = 524.35f - 70*startSlotRows;

        // [ left - bottom ]
        rectTransform.offsetMin = new Vector2(366.5f, inventoryHeight/2);
        // [ right - top ]
        rectTransform.offsetMax = new Vector2(-366.5f, -inventoryHeight/2);
    }

    public void addRow()
    {
        bool isHotbar;
        if (slotRow == 0) { isHotbar = true; }
        else { isHotbar = false; }

        for (int i = 0; i < 5; i++)
        {
            GameObject slotObj = Instantiate(itemSlotPrefab, new Vector3(firstSlotPos.x + 70*i, firstSlotPos.y + 70*slotRow, 0), new Quaternion(0, 0, 0, 0), inventoryPanel.transform.GetChild(0));

            ItemSlot itemSlot = new ItemSlot(slotObj, inventoryList.Count, ItemDatabase.fetchItemByID(1), 1, isHotbar);
            inventoryList.Add(itemSlot);
        }

        slotRow += 1;
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

    public static ItemSlot fetchItemSlotByID(int id)
    {
        return inventoryList[id];
    }

}
