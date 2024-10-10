using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum AbilityType
{
    Buff,
    Attack,
    Debuff,
    BuffAndDebuff
}

public enum AbilityTargetType
{
    TargetAoe,
    Target,
    TeamAoe,
    Self
}

public enum ModifyType
{
    Add,
    Multi
}
public abstract class AbilityData : SerializedScriptableObject
{

    public AbilityType abilityType;
    public AbilityTargetType targetType;
    public ModifyType modifyType;
    public Dictionary<StatType, float> ModifyStat = new Dictionary<StatType, float>();
    public bool Ispercen;
    [SerializeField] protected string abilityName;
    [SerializeField, TextArea] protected string abilityDetial;
    public abstract void ActiveAbilityToSelf(GameObject gameObject);
    public abstract void ActiveAbilityToOther(GameObject gameObject);
    public (string, string) GetAbilityData()
    {
        return (abilityName, abilityDetial);
    }

#if UNITY_EDITOR
    public void Initialise(string s, AbilityType type)
    {
        name = s;
        abilityName = s;
        abilityType = type;
    }
#endif
}
