using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityData : ScriptableObject
{
    public abstract void ActiveAbilityToSelf(Character character);
    public abstract void ActiveAbilityToOther(Character character);
    public abstract (string, string) GetAbilityData();
}
