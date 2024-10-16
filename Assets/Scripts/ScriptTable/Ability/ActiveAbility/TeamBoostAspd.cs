using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamBoostAspd : AbilityData
{
    [SerializeField] private float duration;
    public override void ActiveAbilityToOther(GameObject sender, GameObject gameObject)
    {
        throw new System.NotImplementedException();
    }

    public override void ActiveAbilityToOther(GameObject sender, List<GameObject> targets)
    {

        StatModifier statModifier = modifyType switch
        {
            ModifyType.Add => new StatModifier(abilityName, ModifyStat, duration, (v, r) => v + ModifyStat[r]),
            ModifyType.Multi => new StatModifier(abilityName, ModifyStat, duration, (v, r) => v * ModifyStat[r]),
            _ => throw new ArgumentOutOfRangeException()
        };
        foreach (var item in targets)
        {
            if (item.TryGetComponent(out Character character))
            {
                character.Stats.Mediator.AddModifier(statModifier);
                character.UpdateData();
            }
        }
    }

    public override void ActiveAbilityToSelf(GameObject gameObject)
    {
        throw new System.NotImplementedException();
    }
}
