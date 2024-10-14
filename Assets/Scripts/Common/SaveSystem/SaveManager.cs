using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

    public static SaveManager Instant;
    [SerializeField] private SaveData saveData;
    private SaveSystem saveSystem;
    public Action<SaveData> OnApplicationExit;
    private void Awake()
    {
        if (Instant == null)
        {
            Instant = this;
            saveSystem = new();
            LoadSave();
            InventoryManager.instant.SetUpData(saveData);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayerData.Instant.SetUpData(saveData);
    }
#if UNITY_EDITOR
    [SerializeField] private CharacterData testchadata;
    [SerializeField, Range(1, 100)] private int testlevel;
    [SerializeField] private bool debugChadata;
    private void Update()
    {
        if (debugChadata)
        {
            Debug.Log($"Attack : {testchadata.attackDamage * testchadata.GetAttackDamageWithGrowth(testlevel)}");
            Debug.Log($"Ratio : {testchadata.attackDamage * testchadata.GetAttackRaioWithGrowth(testlevel)}");
        }
    }

#endif
    [ButtonGroup()]
    public void Save()
    {
        saveSystem.Save(saveData);
    }
    [ButtonGroup()]
    public void LoadSave()
    {
        var data = saveSystem.Load();
        saveData = data;
    }

    private void OnApplicationQuit()
    {
        OnApplicationExit?.Invoke(saveData);
        saveSystem.Save(saveData);
    }

#if UNITY_EDITOR
    [Button()]
    private void DeleteSave()
    {
        saveSystem = new();
        saveSystem.DeleteSave();
    }
#endif

}
