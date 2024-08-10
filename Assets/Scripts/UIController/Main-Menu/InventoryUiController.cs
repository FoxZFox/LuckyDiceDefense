using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
    public void SetActivePanel(bool active)
    {
        if (active && panelDetialObjcet.activeInHierarchy)
            return;
        panelDetialObjcet.SetActive(active);
    }
    public void SetCharacterDetailText(CharacterData characterData)
    {
        characterName.text = $"Name: {characterData.name}";
        elemnetType.text = $"Element: {characterData.elementType.name}";
        atkText.text = $"Atk: {characterData.attackDamage}";
        atkRatrioText.text = $"AtkRatio: {characterData.attackRatio}";
        atkRangeText.text = $"AtkRge: {characterData.attackRange}";
        skillName.text = $"Skill: ";
        skillChangeText.text = $"SkillChg: {characterData.skillChange}%";
        skillDetail.text = $"Detail: ";
        buildCost.text = $"Cost: {characterData.costToBuild}";
    }
}
