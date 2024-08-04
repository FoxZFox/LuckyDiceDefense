using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class ItemData : ScriptableObject
{
    [SerializeField] private ItemDataContainer container;
    [SerializeField] private int itemID;
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemImage;
    public int ItemID { get => itemID; }
    public string ItemName { get => itemName; }
    public Sprite ItemImage { get => itemImage; }
    public ItemDataContainer Container { get => container; }
#if UNITY_EDITOR
    public void Initialise(string n, int i, ItemDataContainer con)
    {
        itemName = n;
        name = n;
        itemID = i;
        container = con;
    }

    public void EditData(string n = "", int i = 0)
    {
        if (!string.IsNullOrEmpty(n) && n != itemName)
        {
            name = n;
            itemName = n;
        }
        if (i > 0 && i != itemID)
        {
            itemID = i;
        }
    }

#endif
}
