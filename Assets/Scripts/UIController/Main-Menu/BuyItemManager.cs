using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyItemManager : MonoBehaviour
{
    [SerializeField] private GachaData gachaData;
    [SerializeField] private ItemData itemData;

    public void BuyItem()
    {
        InventoryManager.instant.AddItem(itemData.ItemID);
    }

    public void BuyGaCha(bool buy1card)
    {
        GachaController.instant.SpawnGacha(gachaData, buy1card ? 1 : 3);
    }
}
