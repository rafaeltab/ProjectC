using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private bool inventoryEnabled = false;
    public GameObject inventoryPanel;
    public GameObject itemSlotPrefab;
    public int startSlotRows = 4;
    private int slotRows;
    public List<ItemSlot> inventoryList = new List<ItemSlot>();
    private Vector3 firstSlotPos;


    public class ItemSlot
    {
        public GameObject gameObject { get; }
        public ItemDatabase.Item item { get; set; }

        public ItemSlot(GameObject gameObject, ItemDatabase.Item item)
        {
            this.gameObject = gameObject;
            this.item = item;
        }

    }


    private void Start()
    {

        ItemDatabase.fillDatabase();

        updateInventorySize();

        while (slotRows < startSlotRows)
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
        for (int i = 0; i < 5; i++)
        {
            GameObject obj = Instantiate(itemSlotPrefab, new Vector3(firstSlotPos.x + 70*i, firstSlotPos.y + 70*slotRows, 0), new Quaternion(0, 0, 0, 0), inventoryPanel.transform.GetChild(0));
            ItemSlot itemSlot = new ItemSlot(obj, ItemDatabase.getItem(0));
            inventoryList.Add(itemSlot);
        }

        slotRows += 1;
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
}
