using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUiController : MonoBehaviour
{
    [SerializeField] private GameObject panelDetialObjcet;
    [SerializeField] private Image placeHolder;
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private TMP_Text elemnetType;
    [SerializeField] private TMP_Text atkText;
    [SerializeField] private TMP_Text atkRatrioText;
    [SerializeField] private TMP_Text atkRangeText;
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private TMP_Text skillChangeText;
    [SerializeField] private TMP_Text skillDetail;
    [SerializeField] private TMP_Text buildCost;
    [SerializeField] private Button upGradeButton;
    [Header("Controller")]
    [SerializeField] private Transform contestParent;
    [SerializeField] private GameObject cardPrefab;

    [SerializeField] private DataSortType dataSortType;

    private List<InventoryCharacter> cardInstants;
    private List<GameObject> cardpool;
    private bool isDataSort = false;
    private void Awake()
    {
        cardInstants = new List<InventoryCharacter>();
        cardpool = new List<GameObject>();
    }
    [ContextMenu("SpawnCard")]
    public void LoadCharacterCard()
    {
#if UNITY_EDITOR
        if (InventoryManager.instant.InventoryCharacters.Count < 1)
        {
            foreach (var item in cardpool)
            {
                Destroy(item);
            }
            cardInstants.Clear();
            cardpool.Clear();
        }
#endif
        foreach (var item in InventoryManager.instant.InventoryCharacters)
        {
            var cardData = cardInstants.FirstOrDefault(i => i.CharacterID == item.CharacterID);
            if (cardData == null)
            {
                var instant = Instantiate(cardPrefab);
                instant.SetActive(false);
                var card = instant.GetComponent<CharacterCard>();
                card.SetUpCard(item);
                instant.transform.SetParent(contestParent);
                cardInstants.Add(item);
                cardpool.Add(instant);
                isDataSort = false;
            }
        }

        if (!isDataSort)
        {
            cardInstants = SortData(cardInstants);
            for (int i = 0; i < cardInstants.Count; i++)
            {
                var card = cardpool[i].GetComponent<CharacterCard>();
                card.SetUpCard(cardInstants[i]);
                cardpool[i].SetActive(true);
            }
        }
    }
    private List<InventoryCharacter> SortData(List<InventoryCharacter> data)
    {
        switch (dataSortType)
        {
            case DataSortType.Level:
                data.Sort((x, y) => x.Level.CompareTo(y.Level));
                break;
            case DataSortType.Star:
                data.Sort((x, y) => x.Star.CompareTo(y.Star));
                break;
            case DataSortType.ID:
                data.Sort((x, y) => x.CharacterID.CompareTo(y.CharacterID));
                break;
        }
        isDataSort = true;
        return data;
    }
    public void SetActivePanel(bool active)
    {
        if (active && panelDetialObjcet.activeInHierarchy)
            return;
        panelDetialObjcet.SetActive(active);
    }
    public void SetCharacterDetailText(CharacterCard owner, InventoryCharacter data)
    {
        var map = data.characterData.ability.GetAbilityData();
        var characterData = data.characterData;
        placeHolder.sprite = characterData.placeHolderSpitre;
        characterName.text = $"Name: {characterData.name}";
        elemnetType.text = $"Element: {characterData.elementType.name}";
        atkText.text = $"Atk: {characterData.attackDamage + characterData.GetAttackDamageWithGrowth(data.Level)}";
        atkRatrioText.text = $"AtkRatio: {characterData.attackRatio + characterData.GetAttackRaioWithGrowth(data.Level)}";
        atkRangeText.text = $"AtkRge: {characterData.attackRange}";
        skillName.text = $"Skill: {map.Item1}";
        skillChangeText.text = $"SkillChg: {characterData.skillChange}%";
        skillDetail.text = $"Detail: {map.Item2}";
        buildCost.text = $"Cost: {characterData.costToBuild}";
        upGradeButton.interactable = InventoryManager.instant.CheckCardUpGrade(data);
        upGradeButton.onClick.RemoveAllListeners();
        upGradeButton.onClick.AddListener(() => UpGradeCharacter(owner, data));
        var cardAmount = InventoryManager.instant.GetCardAmount(data);
        upGradeButton.GetComponentInChildren<TMP_Text>().text = $"{cardAmount}/{characterData.CardNeed * data.Level}";
    }
    private void UpGradeCharacter(CharacterCard owner, InventoryCharacter data)
    {
        InventoryManager.instant.CheckCardUpGrade(data, false);
        SetCharacterDetailText(owner, data);
        owner.UpdateLevelTxt();
    }

    private enum DataSortType
    {
        Level,
        Star,
        ID
    }
}
