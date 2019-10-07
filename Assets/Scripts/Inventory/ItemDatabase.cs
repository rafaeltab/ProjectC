using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static List<Item> database = new List<Item>();


    public class Item
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Sprite sprite { get; set; }
        public bool stackable { get; set; }

        public Item(int id, string title, string description, string spriteLocation, bool stackable)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            this.sprite = Resources.Load<Sprite>("Sprites/Items/" + spriteLocation);
            this.stackable = stackable;
        }
    }

    public static Item fetchItemByID(int id)
    {
        return database[id];
    }

    public static void fillDatabase()
    {
        database.Add(new Item(0, "Empty", "", "empty", false));
        database.Add(new Item(1, "Dirt", "Some dirt", "dirt", true));
        database.Add(new Item(2, "Stone", "Some stone", "stone", true));
        Debug.Log("Items added to the database");
    }

}
