using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instant;
    [SerializeField] private int gold = 0;
    [SerializeField] private int gem = 0;
    [SerializeField] private List<InventoryCharacter> loadOut;
    public StageData selectStage;
    public int Gold => gold;
    public int Gem => gem;

    private void Awake()
    {
        if (Instant == null)
        {
            Instant = this;
            Instantiate();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Instantiate()
    {
        loadOut = new List<InventoryCharacter>();
        for (int i = 0; i < 5; i++)
        {
            loadOut.Add(null);
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
        CompareLoadOutData(saveData.LoadOutData);
    }

    private void UpdateData(SaveData saveData)
    {
        saveData.GoldData = gold;
        saveData.GemData = gem;
        saveData.LoadOutData = loadOut;
    }

    private void CompareLoadOutData(List<InventoryCharacter> data)
    {
        var datas = InventoryManager.instant.InventoryCharacters;
        int index = 0;
        foreach (var item in data)
        {
            var value = datas.FirstOrDefault(i => i.CharacterID == item.CharacterID);
            loadOut[index++] = value;
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

    public void AddGold(int data)
    {
        gold += data;
    }

    public void AddGem(int data)
    {
        gem += data;
    }
    public List<InventoryCharacter> GetLoadOut()
    {
        return loadOut;
    }

    public void UpdateLoadOut(List<InventoryCharacter> datas)
    {
        loadOut = datas;
    }

    public InventoryCharacter GetLoadOutData(int index)
    {
        if (index + 1 > loadOut.Count)
        {
            return null;
        }
        else
        {
            if (loadOut[index].CharacterID == 0)
            {
                return null;
            }
        }
        return loadOut[index];
    }
}
