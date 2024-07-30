using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;

[CreateAssetMenu(fileName = "New GachaData", menuName = "CreateTableObject/GachaData")]
public class GachaData : ScriptableObject
{
    public CharacterData fiveStarCard;
    [SerializeField] private CharacterData[] fourStarCard;
    public CharacterData[] belowFourStar;
    public List<CharacterData> GetDeckList()
    {
        List<CharacterData> decks = new List<CharacterData>();
        RandomCards(decks);
        decks = decks.OrderBy(card => Guid.NewGuid()).ToList();
        return decks;
    }

    private void RandomCards(List<CharacterData> _decks)
    {
        int totalCardBelowFourStar = 8 - 1 - fourStarCard.Count();
        //Add Below FourStar
        for (int i = 0; i < totalCardBelowFourStar; i++)
        {
            _decks.Add(belowFourStar[UnityEngine.Random.Range(0, belowFourStar.Count())]);
        }
        //Add FourStarCard
        for (int i = 0; i < fourStarCard.Count(); i++)
        {
            _decks.Add(fourStarCard[i]);
        }
        //Add FiveStarCard
        _decks.Add(fiveStarCard);
    }
}
