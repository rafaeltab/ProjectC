﻿using System.Collections;
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

        /// <summary>
        /// Class for the items in the database
        /// </summary>
        /// <param name="id">ID of the item</param>
        /// <param name="title">Name of the item</param>
        /// <param name="description">Description of the item</param>
        /// <param name="spriteLocation">Name of the image file</param>
        /// <param name="stackLimit">How much the item can stack</param>
        public Item(int id, string title, string description, string spriteLocation, int stackLimit)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            this.sprite = Resources.Load<Sprite>("Sprites/Items/" + spriteLocation);
            this.stackLimit = stackLimit;
        }
    }

    /// <summary>
    /// Returns an Item from the database with the id
    /// </summary>
    /// <param name="id">the ID of the item</param>
    /// <returns>The specified Item from the database</returns>
    public static Item fetchItemByID(int id)
    {
        return database[id];
    }

    /// <summary>
    /// Adds Items to the database
    /// Add a new item to the database by adding this:
    /// database.Add(new Item(id, title, description, imageLocation, stackLimit))
    /// </summary>
    public static void fillDatabase()
    {
        database.Add(new Item(0, "Empty", "", "empty", 0));
        database.Add(new Item(1, "Dirt", "Some dirt", "dirt", 64));
        database.Add(new Item(2, "Stone", "Some stone", "stone", 64));
        database.Add(new Item(3, "Grappling Hook", "A grappling hook.\n use it to get up", "grapplinghook", 1));
        database.Add(new Item(4, "Destruction gun", "A destruction gun.\n use it to destroy stuff", "stone", 1));
        Debug.Log("Items added to the database");
    }
    

}
