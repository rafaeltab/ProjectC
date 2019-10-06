using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static List<Item> database = new List<Item>();


    public class Item
    {
        public int id { get; }
        public string title { get; set; }
        public string description { get; set; }
        public int amount { get; set; }
        public bool stackable { get; set; }

        public Item(int id, string title, string description, bool stackable)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            this.stackable = stackable;
        }
    }

    public static Item getItem(int id)
    {
        return database[id];
    }

    public static void fillDatabase()
    {
        database.Add(new Item(0, "Empty", "", false));
        database.Add(new Item(1, "Dirt", "Some dirt", true));
        Debug.Log("Items added to the database");
    }

}
