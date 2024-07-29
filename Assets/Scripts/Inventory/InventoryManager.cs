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
    [Header("Controller")]
    [SerializeField] private Transform contestParent;
    [SerializeField] private GameObject cardPrefab;
    [Header("Object For Inventory")]
    [SerializeField] private List<InventoryItem> inventoryItems;
    [SerializeField] private List<InventoryCharacter> inventoryCharacters;
    [SerializeField] private CharacterData[] characterDatas;
    private List<CharacterCard> cardInstants;
    private Dictionary<int, int> compairItemIDAndCharID;
    [Header("Debug")]
    [SerializeField] private ItemData[] itemDatas;
    void Start()
    {
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
            inventoryItems.Add(new InventoryItem { itemData = itemDatas.First(i => i.ItemID == id), ItemAmount = 1 });
        }
    }
}


[Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public int ItemAmount;
}

[Serializable]
public class InventoryCharacter
{
    public int CharacterID;
    public int Level;
}
