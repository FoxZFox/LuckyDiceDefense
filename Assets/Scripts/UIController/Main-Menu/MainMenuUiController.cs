using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuUiController : MonoBehaviour
{
    [SerializeField] TMP_Text gemText;
    [SerializeField] TMP_Text goldText;
    [SerializeField] TMP_Text playerIdText;

    void Start()
    {
        gemText.text = $"Gem: {Client.instant.account.Gem}";
        goldText.text = $"Gold: {Client.instant.account.Gold}";
        playerIdText.text = $"ID: {Client.instant.account.ID}";
    }
}
