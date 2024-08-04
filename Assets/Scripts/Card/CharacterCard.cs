using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterCard : MonoBehaviour
{
    [SerializeField] private GameObject[] starShow;
    private int characterCardID = 0;
    public int CharacterCardID => characterCardID;
    private CharacterData characterData;

    private void Start()
    {
        MatchStar();
    }

    private void MatchStar()
    {
        for (int i = starShow.Count(); i > characterData.Star; i--)
        {
            starShow[i - 1].SetActive(false);
        }
    }
    public void OnCardClick()
    {
        Debug.Log($"ID: {characterCardID}");
    }

    public void SetUpCard(CharacterData _data)
    {
        characterData = _data;
        characterCardID = characterData.CharacterID;
    }
}
