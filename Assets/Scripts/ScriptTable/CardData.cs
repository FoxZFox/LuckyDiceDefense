using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : ScriptableObject
{
    [SerializeField] private CardDataContainer container;
    [SerializeField] private string cardName;
    [SerializeField] private int cardID;
    [SerializeField] private Sprite cardImage;

    public string CardName { get => cardName; }
    public int CardID { get => cardID; }
    public Sprite CardImage { get => cardImage; }

#if UNITY_EDITOR
    public void Initialise(string n, int i, CardDataContainer cardDataContainer)
    {
        cardName = n;
        cardID = i;
        container = cardDataContainer;
        name = n;
    }

    public void EditNameAndID(string n = "", int i = 0)
    {
        if (!string.IsNullOrEmpty(n) && n != cardName)
        {
            cardName = n;
            name = n;
        }
        if (i > 0 && i != cardID)
        {
            cardID = i;
        }

    }

#endif
}
