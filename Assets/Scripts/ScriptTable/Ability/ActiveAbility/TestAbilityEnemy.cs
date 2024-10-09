using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class TestAbilityEnemy : AbilityData
{
    [SerializeField] private float duration;
    public override void ActiveAbilityToOther(GameObject gameObject)
    {
        throw new System.NotImplementedException();
    }

    public override void ActiveAbilityToSelf(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Enemy enemy))
        {
            StatModifier statModifier = new StatModifier(abilityName, ModifyStat, duration, (v, r) => v + ModifyStat[r]);
            enemy.Stats.Mediator.AddModifier(statModifier);
            enemy.UpdateData();
        }
    }

}
