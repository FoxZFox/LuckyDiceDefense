using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New GachaData", menuName = "CreateTableObject/GachaData")]
public class GachaData : ScriptableObject
{
    public CharacterData fiveStarCard;
    public CharacterData[] fourStarCard;
    public CharacterData[] belowFourStar;
    public List<int> GetDeckList()
    {
        List<int> decks = new List<int>();
        RandomCards(decks);
        decks = decks.OrderBy(card => Guid.NewGuid()).ToList();
        return decks;
    }

    private void RandomCards(List<int> _decks)
    {
        //Add 1 Star

    }
}
