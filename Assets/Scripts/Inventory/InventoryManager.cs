using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Sirenix.OdinInspector;
using UnityEngine;

public class InventoryManager : SerializedMonoBehaviour
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
    void Awake()
    {
        if (instant == null)
        {
            instant = this;
            DontDestroyOnLoad(instant);
            InitializeCompaire();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SaveManager.Instant.OnApplicationExit += UpdateData;
    }

    private void OnDisable()
    {
        SaveManager.Instant.OnApplicationExit -= UpdateData;
    }

    public void SetUpData(SaveData saveData)
    {
        inventoryItems = saveData.inventoryItemsData;
        inventoryCharacters = saveData.inventoryCharactersData;
        inventoryCards = saveData.inventoryCardsData;
        ValiDateData();
    }

    private void ValiDateData()
    {
        foreach (var item in inventoryItems)
        {
            item.itemData = itemDatas.FirstOrDefault(i => i.ItemID == item.ID);
        }
        foreach (var item in inventoryCharacters)
        {
            item.characterData = characterDatas.FirstOrDefault(i => i.CharacterID == item.CharacterID);
        }
        foreach (var item in inventoryCards)
        {
            item.cardData = cardDatas.FirstOrDefault(i => i.CardID == item.ID);
        }

    }

    private void UpdateData(SaveData saveData)
    {
        saveData.inventoryItemsData = inventoryItems;
        saveData.inventoryCharactersData = inventoryCharacters;
        saveData.inventoryCardsData = inventoryCards;
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
        foreach (var item in compairItemIDAndCharID)
        {
            Debug.Log($"Key: {item.Key} Value: {item.Value}");
        }
    }

    public bool CheckCardUpGrade(InventoryCharacter value, bool checkData = true)
    {
        var data = inventoryCharacters.FirstOrDefault(i => i == value);
        var cardData = inventoryCards.FirstOrDefault(i => i.cardData == data.characterData.cardData);
        var cost = (data.Level - 1) * 50;
        int cardneed = data.Level * data.characterData.CardNeed;
        if (checkData)
        {
            if (cardData.CardAmount >= cardneed && PlayerData.Instant.Gold >= cost) return true;
        }
        else
        {
            cardData.CardAmount -= cardneed;
            PlayerData.Instant.SetGold(cost);
            data.Level += 1;
            return true;
        }
        return false;
    }

    public int GetCardAmount(InventoryCharacter value)
    {
        var data = inventoryCharacters.FirstOrDefault(i => i == value);
        var cardData = inventoryCards.FirstOrDefault(i => i.cardData == data.characterData.cardData);
        return cardData.CardAmount;
    }
    private void Update()
    {
        //In the future change it to load only click on the inventory and first on gameload
        // CheckCardOwned();
    }
    public void CheckCardOwned()
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
    public int ID;
    public ItemData itemData;
    public int ItemAmount;

    public InventoryItem(ItemData itemData)
    {
        this.itemData = itemData;
        ID = itemData.ItemID;
        ItemAmount = 1;
    }
}

[Serializable]
public class InventoryCard
{
    public int ID;
    public CardData cardData;
    public int CardAmount;
    public InventoryCard(CardData cardData)
    {
        this.cardData = cardData;
        ID = cardData.CardID;
        CardAmount = 1;
    }
}

[Serializable]
public class InventoryCharacter
{
    public int CharacterID;
    public int Star;
    public CharacterData characterData;
    public int Level;

    public InventoryCharacter(CharacterData data)
    {
        characterData = data;
        CharacterID = characterData.CharacterID;
        Star = data.Star;
        Level = 1;
    }
}
