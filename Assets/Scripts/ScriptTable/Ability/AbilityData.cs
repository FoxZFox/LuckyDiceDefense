using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class AbilityData : SerializedScriptableObject
{
    public enum AbilityType
    {
        Buff,
        Attack,
        Debuff
    }
    public AbilityType abilityType;
    public Dictionary<Stat, float> ModifyStat = new Dictionary<Stat, float>();
    public bool Ispercen;
    [SerializeField] protected string abilityName;
    [SerializeField, TextArea] protected string abilityDetial;
    public abstract void ActiveAbilityToSelf(GameObject gameObject);
    public abstract void ActiveAbilityToOther(GameObject gameObject);
    public abstract (string, string) GetAbilityData();

#if UNITY_EDITOR
    public void Initialise(string s, AbilityType type)
    {
        name = s;
        abilityName = s;
        abilityType = type;
    }
#endif
}
