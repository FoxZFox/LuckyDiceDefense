using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instant;
    [SerializeField] private int gold = 0;
    [SerializeField] private int gem = 0;
    [SerializeField] private List<InventoryCharacter> loadOut;
    public int Gold => gold;
    public int Gem => gem;

    private void Awake()
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
        gold = saveData.GoldData;
        gem = saveData.GemData;
        loadOut = saveData.LoadOutData;
    }

    private void UpdateData(SaveData saveData)
    {
        saveData.GoldData = gold;
        saveData.GemData = gem;
        saveData.LoadOutData = loadOut;
    }

    public void SetGold(int data)
    {
        gold = (gold - data) <= 0 ? 0 : gold - data;
    }

    public void SetGem(int data)
    {
        gem = (gem - data) <= 0 ? 0 : gem - data;
    }

    public CharacterData GetLoadOutData(int index)
    {
        if (index + 1 > loadOut.Count)
        {
            return null;
        }
        return loadOut[index].characterData;
    }
}
