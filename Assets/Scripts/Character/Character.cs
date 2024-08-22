using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum TargetPriority
    {
        First,
        Last,
        Close
    }
    public Action OnAttack;
    [Header("CharacterData")]
    [SerializeField] private CharacterData characterData;
    [SerializeField] private ElementType elementType;
    [SerializeField] private float attackDamage = 0;
    [SerializeField] private float attackRatio = 0;
    [SerializeField] private float attackRange = 0;
    [SerializeField] private float skillChange = 0;
    [SerializeField] private AbilityData ability;
    [SerializeField] private TargetPriority priority;
    [Header("TarGet Debug")]
    [SerializeField] private List<GameObject> enemys;
    [SerializeField] private GameObject target;
    private CircleCollider2D circleCollider;
    private bool longRange;
    private float nextAttack = 0;
    private CharacterAnimation characterAnimation;
    private void Start()
    {
        Debug.Log("Base class");
        circleCollider = GetComponent<CircleCollider2D>();
        characterAnimation = GetComponentInChildren<CharacterAnimation>();
        SetUpData();
    }
    public void SetUpData()
    {
        elementType = characterData.elementType;
        attackDamage = characterData.attackDamage;
        attackRatio = characterData.attackRatio;
        attackRange = characterData.attackRange;
        skillChange = characterData.skillChange;
        ability = characterData.ability;
        nextAttack = attackRatio;
        circleCollider.radius = attackRange;
        characterAnimation.SetUpAnimator(characterData.animatorController, characterData.AttackDuretionAnimation);

    }
    private void Update()
    {
        FindEnemy();
        Attack();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemys.Add(enemy.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        enemys.Remove(other.gameObject);
        target = null;
    }
    private void Attack()
    {
        nextAttack -= Time.deltaTime;
        if (nextAttack <= 0 && target != null)
        {
            nextAttack = attackRatio;
            OnAttack?.Invoke();
            if (CheckUseAbility())
            {
                UseAbility();
                return;
            }
            target.GetComponent<Enemy>().TakeDamage(gameObject, attackDamage);
        }
    }
    private void FindEnemy()
    {
        if (enemys.Count > 0)
            switch (priority)
            {
                case TargetPriority.First:
                    target = enemys.First();
                    break;
                case TargetPriority.Last:
                    target = enemys.Last();
                    break;
                case TargetPriority.Close:
                    target = FindCloseEnemy();
                    break;
            }
    }

    private GameObject FindCloseEnemy()
    {
        GameObject closeEnemy = null;
        float closeDistant = Mathf.Infinity;
        foreach (var item in enemys)
        {
            float distant = Vector3.Distance(transform.position, item.transform.position);
            if (distant < closeDistant)
            {
                closeDistant = distant;
                closeEnemy = item;
            }
        }
        return closeEnemy;
    }
    private bool CheckUseAbility()
    {
        float skillRng = UnityEngine.Random.Range(0f, 100f);
        if (skillRng <= skillChange)
        {
            return true;
        }
        return false;
    }
    private void UseAbility()
    {

        if (ability != null)
            switch (ability.abilityType)
            {
                case AbilityData.AbilityType.Buff:
                    ability.ActiveAbilityToSelf(gameObject);
                    break;
                case AbilityData.AbilityType.Debuff:
                    ability.ActiveAbilityToOther(target);
                    break;
            }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, characterData.attackRange);
    }
#endif
}
