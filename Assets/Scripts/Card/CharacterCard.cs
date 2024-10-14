using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CharacterCard : MonoBehaviour
{
    [SerializeField] private GameObject[] starShow;
    [SerializeField] private InventoryCharacter characterData;
    [SerializeField] private TMP_Text levelTxt;
    private int characterCardID = 0;
    public int CharacterCardID => characterCardID;

    private void Start()
    {

    }
    private void MatchStar()
    {
        foreach (var item in starShow)
        {
            item.SetActive(false);
        }
        for (int i = 0; i < characterData.Star; i++)
        {
            starShow[i].SetActive(true);
        }
    }
    public void OnCardClick()
    {
        InventoryUiController ic = FindFirstObjectByType<InventoryUiController>();
        ic.SetActivePanel(true);
        ic.SetCharacterDetailText(this, characterData);
        Debug.Log($"ID: {characterCardID}");
    }

    public void UpdateLevelTxt()
    {
        levelTxt.text = $"Lv.{characterData.Level}";
    }

    public void SetUpCard(InventoryCharacter _data)
    {
        characterData = _data;
        characterCardID = characterData.CharacterID;
        MatchStar();
        UpdateLevelTxt();
    }
}
