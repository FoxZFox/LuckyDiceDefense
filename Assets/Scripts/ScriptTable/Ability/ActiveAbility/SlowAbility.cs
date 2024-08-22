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

    public override void ActiveAbilityToOther(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Enemy enemy))
        {
            UnityMainThread.Instant.EnqueCorutine(enemy.ModifyDuration(ModifyStat, Ispercen, slowDuration));
        }
    }
    public override (string, string) GetAbilityData()
    {
        return (abilityName, abilityDetial);
    }
}
