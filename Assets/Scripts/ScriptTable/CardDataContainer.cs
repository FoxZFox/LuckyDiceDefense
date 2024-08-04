using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Container Card Data", menuName = "Stata/CardDataContainer")]
public class CardDataContainer : ScriptableObject
{
    [SerializeField] private List<CardData> cardDatas = new List<CardData>();
    public List<CardData> CardDatas { get => cardDatas; set => cardDatas = value; }
}
