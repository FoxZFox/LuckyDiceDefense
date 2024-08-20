using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class InventoryUiController : MonoBehaviour
{
    [SerializeField] private GameObject panelDetialObjcet;
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private TMP_Text elemnetType;
    [SerializeField] private TMP_Text atkText;
    [SerializeField] private TMP_Text atkRatrioText;
    [SerializeField] private TMP_Text atkRangeText;
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private TMP_Text skillChangeText;
    [SerializeField] private TMP_Text skillDetail;
    [SerializeField] private TMP_Text buildCost;
    private List<CharacterCard> cardInstants;
    [Header("Controller")]
    [SerializeField] private Transform contestParent;
    [SerializeField] private GameObject cardPrefab;

    private void Start()
    {
        cardInstants = new List<CharacterCard>();
    }

    [ContextMenu("SpawnCard")]
    public void LoadCharacterCard()
    {
        if (InventoryManager.instant.InventoryCharacters.Count < 1)
        {
            foreach (var item in cardInstants)
            {
                Destroy(item.gameObject);
            }
            cardInstants.Clear();
        }
        foreach (var item in InventoryManager.instant.InventoryCharacters)
        {
            var card = cardInstants.FirstOrDefault(i => i.CharacterCardID == item.CharacterID);
            if (card == null)
            {
                var instant = Instantiate(cardPrefab);
                card = instant.GetComponent<CharacterCard>();
                card.SetUpCard(item.characterData);
                instant.transform.SetParent(contestParent);
                cardInstants.Add(card);
            }
        }
    }

    public void SetActivePanel(bool active)
    {
        if (active && panelDetialObjcet.activeInHierarchy)
            return;
        panelDetialObjcet.SetActive(active);
    }
    public void SetCharacterDetailText(CharacterData characterData)
    {
        var map = characterData.ability.GetAbilityData();
        characterName.text = $"Name: {characterData.name}";
        elemnetType.text = $"Element: {characterData.elementType.name}";
        atkText.text = $"Atk: {characterData.attackDamage}";
        atkRatrioText.text = $"AtkRatio: {characterData.attackRatio}";
        atkRangeText.text = $"AtkRge: {characterData.attackRange}";
        skillName.text = $"Skill: {map.Item1}";
        skillChangeText.text = $"SkillChg: {characterData.skillChange}%";
        skillDetail.text = $"Detail: {map.Item2}";
        buildCost.text = $"Cost: {characterData.costToBuild}";
    }
}
