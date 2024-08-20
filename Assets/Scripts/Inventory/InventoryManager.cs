using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instant;
    [Header("Object For Inventory")]
    [SerializeField] private List<InventoryItem> inventoryItems;
    [SerializeField] private List<InventoryCharacter> inventoryCharacters;
    [SerializeField] private List<InventoryCard> inventoryCards;
    public List<InventoryCharacter> InventoryCharacters => inventoryCharacters;
    private Dictionary<CardData, CharacterData> compairItemIDAndCharID;
    [Header("Debug")]
    [SerializeField] private CharacterData[] characterDatas;
    [SerializeField] private ItemData[] itemDatas;
    [SerializeField] private CardData[] cardDatas;
    void Start()
    {
        if (instant == null)
        {
            instant = this;
            DontDestroyOnLoad(instant);
        }
        else
        {
            Destroy(gameObject);
        }
        InitializeCompaire();
    }

    private void InitializeCompaire()
    {
        compairItemIDAndCharID = new Dictionary<CardData, CharacterData>();
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
        // CheckSpawnCard();
    }
    private void CheckCardOwned()
    {
        foreach (var item in inventoryCards)
        {
            bool characterOwned = inventoryCharacters.Any(i => i.characterData == compairItemIDAndCharID[item.cardData]);
            if (characterOwned)
                continue;
            inventoryCharacters.Add(new InventoryCharacter(compairItemIDAndCharID[item.cardData]));
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

    public void AddCard(CardData cardData, int amount = 1)
    {
        InventoryCard item = inventoryCards.FirstOrDefault(item => item.cardData == cardData);
        if (item != null)
        {
            item.CardAmount += amount;
        }
        else
        {
            inventoryCards.Add(new InventoryCard(cardData));
        }
    }

    public CardData GetCardData(CharacterData characterData)
    {
        return cardDatas.First(i => i == characterData.cardData);
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
public class InventoryCard
{
    public CardData cardData;
    public int CardAmount;
    public InventoryCard(CardData cardData)
    {
        this.cardData = cardData;
        CardAmount = 1;
    }
}

[Serializable]
public class InventoryCharacter
{
    public int CharacterID;
    public CharacterData characterData;
    public int Level;

    public InventoryCharacter(CharacterData data)
    {
        characterData = data;
        CharacterID = characterData.CharacterID;
        Level = 0;
    }
}
