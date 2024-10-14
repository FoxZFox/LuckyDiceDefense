using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ReduceDamageAbility : AbilityData
{
    [SerializeField, Range(0f, 100f)] private float reducePercen;

    public override void ActiveAbilityToOther(GameObject sender, GameObject gameObject)
    {
        throw new System.NotImplementedException();
    }

    public override void ActiveAbilityToOther(GameObject sender, List<GameObject> targets)
    {
        throw new System.NotImplementedException();
    }

    public override void ActiveAbilityToSelf(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Enemy enemy))
        {
            float incomdamage = enemy.InComingDamage;
            incomdamage -= Mathf.Round(incomdamage * reducePercen / 100f);
            enemy.InComingDamage = incomdamage;
            Debug.Log("ReduceDamage");
        }
    }
}
