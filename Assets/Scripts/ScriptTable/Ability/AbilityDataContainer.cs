using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Container-Ability-Data", menuName = "Stata/AbilityContainer")]
public class AbilityDataContainer : ScriptableObject
{
    [SerializeField] private List<AbilityData> abilityDatas = new List<AbilityData>();
    public List<AbilityData> AbilityDatas { get => abilityDatas; set => abilityDatas = value; }

#if UNITY_EDITOR
    [HideInInspector] public AbilityData deleteAbility;
#endif
}
