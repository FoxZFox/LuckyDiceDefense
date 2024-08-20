using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instant;
    [SerializeField] private int gold = 0;
    [SerializeField] private int gem = 0;
    public int Gold => gold;
    public int Gem => gem;

    private void Start()
    {
        if (Instant == null)
        {
            Instant = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetGold(int data)
    {
        gold = (gold - data) <= 0 ? 0 : gold - data;
    }

    public void SetGem(int data)
    {
        gem = (gem - data) <= 0 ? 0 : gem - data;
    }
}
