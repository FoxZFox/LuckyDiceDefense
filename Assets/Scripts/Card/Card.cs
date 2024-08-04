using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private Button button;
    private int cardID;
    private int cardStar;
    public int CardID => cardID;
    public int CardStar => cardStar;
    [SerializeField] private GameObject cardOutline;
    private bool selected = false;
    public bool Selected => selected;
    public CharacterData characterData { get; private set; }

    private int amount = 0;
    public int Amount => amount;

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
        bool selecseucces = GachaController.instant.PickupCard(gameObject, cardStar, selected);
        if (selecseucces)
        {
            cardOutline.SetActive(true);
            selected = true;
        }
        else
        {
            cardOutline.SetActive(false);
            selected = false;
        }
    }

    public void SetCardID(CharacterData data, int a)
    {
        cardID = data.CharacterID;
        cardStar = data.Star;
        characterData = data;
        amount = a;
        cardOutline.SetActive(false);
    }

    private void OnEnable()
    {
        selected = false;
    }
}
