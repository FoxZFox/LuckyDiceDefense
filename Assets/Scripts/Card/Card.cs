using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
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
    [SerializeField] private Image cardPlaceHolder;
    [SerializeField] private TMP_Text cardText;
    private bool selected = false;
    public bool Selected => selected;
    public CharacterData characterData { get; private set; }

    private int amount = 0;
    public int Amount => amount;

    public GameObject FrontCard;
    public GameObject BackCard;

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

    public void DisableCardOutline()
    {
        cardOutline.SetActive(false);
    }

    private void PickCard()
    {
        bool selecseucces = GachaController.instant.PickupCard(this, cardStar, selected);
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
        cardPlaceHolder.sprite = data.placeHolderSpitre;
        cardText.text = $"x {amount}";
    }

    private void OnEnable()
    {
        selected = false;
    }
}
