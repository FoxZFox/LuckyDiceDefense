using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowAbility : AbilityData
{
    [SerializeField] private string abilityName;
    [SerializeField, TextArea] private string abilityDetial;
    public override void ActiveAbility()
    {

    }

    public override (string, string) GetAbilityData()
    {
        return (abilityName, abilityDetial);
    }

#if UNITY_EDITOR
    public void Initialise(string s)
    {
        abilityName = s;
        name = s;
    }
#endif
}
