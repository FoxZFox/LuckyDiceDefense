using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class TestAbilityEnemy : AbilityData
{
    [SerializeField] private float duration;
    public override void ActiveAbilityToOther(GameObject sender, GameObject gameObject)
    {
        throw new System.NotImplementedException();
    }

    public override void ActiveAbilityToOther(GameObject sender, List<GameObject> targets)
    {
        throw new NotImplementedException();
    }

    public override void ActiveAbilityToSelf(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Enemy enemy))
        {
            StatModifier statModifier = modifyType switch
            {
                ModifyType.Add => new StatModifier(abilityName, ModifyStat, duration, (v, r) => v + ModifyStat[r]),
                ModifyType.Multi => new StatModifier(abilityName, ModifyStat, duration, (v, r) => v * ModifyStat[r]),
                _ => throw new ArgumentOutOfRangeException()
            };
            enemy.Stats.Mediator.AddModifier(statModifier);
            enemy.UpdateData();
        }
    }

}
