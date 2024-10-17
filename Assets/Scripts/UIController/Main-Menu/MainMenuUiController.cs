using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class MainMenuUiController : MonoBehaviour
{
    [Header("UIObject")]
    [SerializeField] private GameObject playerInfo;
    [SerializeField] private GameObject MainUiObject;
    [SerializeField] private GameObject shopObject;
    [SerializeField] private GameObject inventoryObject;
    [SerializeField] private GameObject loadOutObject;

    [Header("Text")]
    [SerializeField] private TMP_Text gemText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text playerIdText;
    private InventoryUiController inventoryUiController;

    void Start()
    {
        SceneManager.Instant.LoadSceneAsync(SceneManager.sceneName.gacha);
        MainUiObject.SetActive(true);
        shopObject.SetActive(false);
        inventoryObject.SetActive(false);
        loadOutObject.SetActive(false);
        inventoryUiController = GetComponent<InventoryUiController>();
        SetPlayerStatus();
        SoundManager.Instant.PlayBackGroundMusic(SoundType.BackGroundMusic, transform);
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
            gemText.text = PlayerData.Instant.Gem.ToString();
            goldText.text = PlayerData.Instant.Gold.ToString();
        }
        shopObject.SetActive(false);
        inventoryObject.SetActive(false);
    }
    public void UpdateData()
    {
        gemText.text = PlayerData.Instant.Gem.ToString();
        goldText.text = PlayerData.Instant.Gold.ToString();
    }

    public void OnClickMainMenu()
    {
        MainUiObject.SetActive(true);
        shopObject.SetActive(false);
        inventoryObject.SetActive(false);
        loadOutObject.SetActive(false);
        UpdateData();
        SoundManager.Instant.PlayAudioOneShot(SoundType.ButtonClick, transform);
    }
    public void OnClickShop()
    {
        MainUiObject.SetActive(false);
        shopObject.SetActive(true);
        inventoryObject.SetActive(false);
        loadOutObject.SetActive(false);
        SoundManager.Instant.PlayAudioOneShot(SoundType.ButtonClick, transform);
    }
    public void OnClickInventory()
    {
        inventoryUiController.LoadCharacterCard();
        MainUiObject.SetActive(false);
        shopObject.SetActive(false);
        inventoryObject.SetActive(true);
        loadOutObject.SetActive(false);
        SoundManager.Instant.PlayAudioOneShot(SoundType.ButtonClick, transform);
    }

    public void OnClickLoadOutButton()
    {
        MainUiObject.SetActive(false);
        shopObject.SetActive(false);
        inventoryObject.SetActive(false);
        loadOutObject.SetActive(true);
        SoundManager.Instant.PlayAudioOneShot(SoundType.ButtonClick, transform);
    }

    public void OnClickPlay()
    {
        SoundManager.Instant.PlayAudioOneShot(SoundType.ButtonClick, transform);
        if (PlayerData.Instant.selectStage != null)
        {
            SceneManager.Instant.LoadSceneWithTransition(SceneManager.sceneName.gameplay, TransitionType.Circle);
        }
    }
}