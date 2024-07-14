using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private Button button;
    private int cardID;
    public int CardID => cardID;
    void Start()
    {
        button = GetComponent<Button>();
        SetButtonListenner();
    }

    private void SetButtonListenner()
    {
        button.onClick.AddListener(() => PickCard());
        button.enabled = false;
    }

    private void PickCard()
    {
        GachaController.instant.PickupCard(cardID);
    }

    public void SetCardID(int id)
    {
        cardID = id;
    }
}
