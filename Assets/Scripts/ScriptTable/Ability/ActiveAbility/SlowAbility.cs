using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlowAbility : AbilityData
{
    [SerializeField] private float slowDuration;
    [SerializeField] private bool isForst;
    public override void ActiveAbilityToSelf(GameObject gameObject)
    {

    }

    public override void ActiveAbilityToOther(GameObject sender, GameObject gameObject)
    {
        if (sender.GetComponent<Character>() == null)
        {
            return;
        }
        StatModifier statModifier = new StatModifier(abilityName, ModifyStat, slowDuration, (v, r) => v + ModifyStat[r]);
        if (gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.Stats.Mediator.AddModifier(statModifier);
            enemy.UpdateData();
        }
    }

    public override void ActiveAbilityToOther(GameObject sender, List<GameObject> targets)
    {
        throw new System.NotImplementedException();
    }
}
