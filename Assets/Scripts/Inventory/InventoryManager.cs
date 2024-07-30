using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instant;
    [Header("Controller")]
    [SerializeField] private Transform contestParent;
    [SerializeField] private GameObject cardPrefab;
    [Header("Object For Inventory")]
    [SerializeField] private List<InventoryItem> inventoryItems;
    [SerializeField] private List<InventoryCharacter> inventoryCharacters;
    private List<CharacterCard> cardInstants;
    private Dictionary<int, int> compairItemIDAndCharID;
    [Header("Debug")]
    [SerializeField] private CharacterData[] characterDatas;
    [SerializeField] private ItemData[] itemDatas;
    void Start()
    {
        if (instant == null)
        {
            instant = this;
        }
        else
        {
            Destroy(gameObject);
        }
        InitializeCompaire();
    }

    private void InitializeCompaire()
    {
        compairItemIDAndCharID = new Dictionary<int, int>();
        cardInstants = new List<CharacterCard>();
        foreach (var item in characterDatas)
        {
            var map = item.MapData();
            compairItemIDAndCharID.Add(map.Item1, map.Item2);
        }
        Debug.Log("MapDataSuccess");
    }

    private void Update()
    {
        //In the future change it to load only click on the inventory and first on gameload
        CheckCardOwned();
        CheckSpawnCard();
    }

    [ContextMenu("SpawnCard")]
    private void CheckSpawnCard()
    {
        foreach (var item in inventoryCharacters)
        {
            var card = cardInstants.FirstOrDefault(i => i.CharacterCardID == item.CharacterID);
            if (card == null)
            {
                var instant = Instantiate(cardPrefab);
                card = instant.GetComponent<CharacterCard>();
                card.CharacterCardID = item.CharacterID;
                instant.transform.SetParent(contestParent);
                cardInstants.Add(card);
            }
        }
    }

    private void CheckCardOwned()
    {
        foreach (var item in inventoryItems)
        {
            bool characterOwned = inventoryCharacters.Any(i => i.CharacterID == compairItemIDAndCharID[item.itemData.ItemID]);
            if (characterOwned)
                continue;
            inventoryCharacters.Add(new InventoryCharacter() { CharacterID = compairItemIDAndCharID[item.itemData.ItemID], Level = 0 });
        }
    }
    public void AddItem(int id)
    {
        InventoryItem item = inventoryItems.FirstOrDefault(item => item.itemData.ItemID == id);
        if (item != null)
        {
            item.ItemAmount++;
        }
        else
        {
            inventoryItems.Add(new InventoryItem(itemDatas.First(i => i.ItemID == id)));
        }
    }
    public void AddItem(ItemData itemData, int amount = 1)
    {
        InventoryItem item = inventoryItems.FirstOrDefault(item => item.itemData == itemData);
        if (item != null)
        {
            item.ItemAmount += amount;
        }
        else
        {
            inventoryItems.Add(new InventoryItem(itemData));
        }
    }
}


[Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public int ItemAmount;

    public InventoryItem(ItemData itemData)
    {
        this.itemData = itemData;
        ItemAmount = 1;
    }
}

[Serializable]
public class InventoryCharacter
{
    public int CharacterID;
    public int Level;
}
