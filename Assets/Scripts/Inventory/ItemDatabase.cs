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
        public int stackLimit { get; set; }

        public Item(int id, string title, string description, string spriteLocation, int stackLimit)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            this.sprite = Resources.Load<Sprite>("Sprites/Items/" + spriteLocation);
            this.stackLimit = stackLimit;
        }
    }

    public static Item fetchItemByID(int id)
    {
        return database[id];
    }

    public static void fillDatabase()
    {
        database.Add(new Item(0, "Empty", "", "empty", 0));
        database.Add(new Item(1, "Dirt", "Some dirt", "dirt", 64));
        database.Add(new Item(2, "Stone", "Some stone", "stone", 64));
        Debug.Log("Items added to the database");
    }

}
