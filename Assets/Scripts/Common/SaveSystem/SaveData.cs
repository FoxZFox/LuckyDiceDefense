using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public List<InventoryItem> inventoryItemsData;
    public List<InventoryCharacter> inventoryCharactersData;
    public List<InventoryCard> inventoryCardsData;
    public int GoldData;
    public int GemData;
    public List<InventoryCharacter> LoadOutData;
    public StageSelected StageSelected;

    public SaveData()
    {
        inventoryItemsData = new List<InventoryItem>();
        inventoryCharactersData = new List<InventoryCharacter>();
        inventoryCardsData = new List<InventoryCard>();
        GoldData = 0;
        GemData = 0;
        LoadOutData = new List<InventoryCharacter>();
        StageSelected = new StageSelected();
    }
}

[Serializable]
public class StageSelected
{
    public int ID;
}