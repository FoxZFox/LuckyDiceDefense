using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int Level { get; private set; }
    public int Cost { get; private set; }
    [Header("CharacterData")]
    [SerializeField] private CharacterData characterData;
    [SerializeField] private ElementType elementType;
    [SerializeField] private float attackDamage = 0;
    [SerializeField] private float attackRatio = 0;
    [SerializeField] private float attackRange = 0;
    [SerializeField] private float skillChange = 0;

    [SerializeField] private AbilityData abilityDatas;
    private float nextAttack = 0;
    private void Start()
    {
        Debug.Log("Base class");
    }
    private void Attack()
    {
        float skillRng = Random.Range(0f, 100f);
        nextAttack -= Time.deltaTime;
        if (nextAttack <= 0)
        {
            nextAttack = attackRatio;
            if (skillRng <= skillChange)
            {
                UseAbility();
                return;
            }
        }
    }
    private void Update()
    {
        Attack();
    }
    private void UseAbility()
    {
        if (abilityDatas != null)
            abilityDatas.ActiveAbilityToSelf(this);
    }

    public void SetUpData(CharacterData data)
    {
        characterData = data;
        elementType = characterData.elementType;
        attackDamage = characterData.attackDamage;
        attackRatio = characterData.attackRatio;
        skillChange = characterData.skillChange;
        nextAttack = attackRatio;
    }
}
