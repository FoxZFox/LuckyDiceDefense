using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuUiController : MonoBehaviour
{
    [Header("UIObject")]
    [SerializeField] private GameObject playerInfo;
    [SerializeField] private GameObject MainUiObject;
    [SerializeField] private GameObject shopObject;
    [SerializeField] private GameObject inventoryObject;

    [Header("Text")]
    [SerializeField] private TMP_Text gemText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text playerIdText;

    void Start()
    {
        SetPlayerStatus();
        SceneManager.Instant.LoaadSceneAsync(SceneManager.sceneName.gacha);
    }

    private void SetPlayerStatus()
    {
        if (!Networkmanager.Instant.StanAlone)
        {
            gemText.text = $"Gem: {Client.instant.account.Gem}";
            goldText.text = $"Gold: {Client.instant.account.Gold}";
            playerIdText.text = $"ID: {Client.instant.account.ID}";
        }
        else
        {
            playerInfo.SetActive(false);
        }
        shopObject.SetActive(false);
        inventoryObject.SetActive(false);
    }

    public void OnClickShop()
    {
        MainUiObject.SetActive(false);
        shopObject.SetActive(true);
        inventoryObject.SetActive(false);
    }
    public void OnClickInventory()
    {
        MainUiObject.SetActive(false);
        shopObject.SetActive(false);
        inventoryObject.SetActive(true);
    }
}