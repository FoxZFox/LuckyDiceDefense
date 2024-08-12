using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityData : ScriptableObject
{
    public abstract void ActiveAbility();
    public abstract (string, string) GetAbilityData();
}
