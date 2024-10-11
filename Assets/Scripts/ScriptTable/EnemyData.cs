using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : ScriptableObject
{
    public EnemyDataContainer container;
    public string enemyName;
    public float walkSpeed;
    [Min(1f)] public float health;
    public ElementType elementType;
    public Sprite sprite;
    public RuntimeAnimatorController animatorController;
    public AbilityData abilityData;
    [Range(1f, 100f)] public float skillChange = 100f;
    [TextArea] public string enemyDetail;

#if UNITY_EDITOR
    public void Initialise(string n, EnemyDataContainer container)
    {
        enemyName = n;
        this.container = container;
        name = n;
    }

    public void EditName(string n = "")
    {
        if (!string.IsNullOrEmpty(n) && n != enemyName)
        {
            enemyName = n;
            name = n;
        }
    }

#endif
}
