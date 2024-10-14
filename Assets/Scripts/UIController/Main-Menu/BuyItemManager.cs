using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BuyItemManager : MonoBehaviour
{
    [SerializeField] private GachaData gachaData;
    [SerializeField] private ItemData itemData;

    public void BuyItem()
    {
        int data = 0;
        switch (itemData.Type)
        {
            case ItemData.PriceType.Gold:
                data = PlayerData.Instant.Gold;
                break;
            case ItemData.PriceType.Gem:
                data = PlayerData.Instant.Gem;
                break;
        }
        if (data < itemData.ItemPrice)
        {
            return;
        }
        switch (itemData.Type)
        {
            case ItemData.PriceType.Gold:
                PlayerData.Instant.SetGold(itemData.ItemPrice);
                break;
            case ItemData.PriceType.Gem:
                PlayerData.Instant.SetGem(itemData.ItemPrice);
                break;
        }
        InventoryManager.instant.AddItem(itemData.ItemID);
    }

    public void BuyGaCha(bool buy1card)
    {
        int price = 0;
        if (buy1card)
        {
            price = 10;
        }
        else
        {
            price = 25;
        }
        if (PlayerData.Instant.Gem < price)
        {
            return;
        }
        PlayerData.Instant.SetGem(price);
        FindFirstObjectByType<MainMenuUiController>().UpdateData();
        GachaController.instant.SpawnGacha(gachaData, buy1card ? 1 : 3);
    }
}
