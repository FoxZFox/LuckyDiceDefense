using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlowAbility : AbilityData
{
    [SerializeField] private float slowDuration;
    [SerializeField] private float slowPercen;
    [SerializeField] private bool isForst;
    public override void ActiveAbilityToSelf(GameObject gameObject)
    {

    }

    public override void ActiveAbilityToOther(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Enemy enemy))
        {
            float modify = enemy.Speed * (slowPercen / 100);
            IEnumerator Modify()
            {
                enemy.ModifySpeed(modify * -1);
                yield return new WaitForSeconds(slowDuration);
                enemy.ModifySpeed(modify);
            }
            UnityMainThread.Instant.EnqueCorutine(Modify());
        }
    }
    public override (string, string) GetAbilityData()
    {
        return (abilityName, abilityDetial);
    }
}
