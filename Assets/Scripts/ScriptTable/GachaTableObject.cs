using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New GachaConfig", menuName = "CreateTableObject/GachaObject")]
public class GachaTableObject : ScriptableObject
{
    [SerializeField] private int fiveStarCardID = 0;
    [SerializeField] private int fourStarCardID = 0;

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
        for (int i = 0; i < 3; i++)
        {
            _decks.Add(UnityEngine.Random.Range(11, 20));
        }
        //Add 2 Star
        for (int i = 0; i < 2; i++)
        {
            _decks.Add(UnityEngine.Random.Range(21, 30));
        }
        _decks.Add(UnityEngine.Random.Range(30, 40));
        if (fourStarCardID == 0)
        {
            _decks.Add(UnityEngine.Random.Range(40, 50));
        }
        _decks.Add(fiveStarCardID);
    }
}
