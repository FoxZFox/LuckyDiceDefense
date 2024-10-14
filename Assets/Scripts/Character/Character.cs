using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum TargetPriority
    {
        First,
        Last,
        Close
    }
    public Action<Character> OnSell;
    public Action OnAttack;
    [BoxGroup("CharacterData")]
    [SerializeField] private CharacterData characterData;
    [BoxGroup("CharacterData")]
    [SerializeField] private ElementType elementType;
    [BoxGroup("CharacterData")]
    [SerializeField] private float attackDamage = 0;
    [BoxGroup("CharacterData")]
    [SerializeField] private float attackRatio = 0;
    [BoxGroup("CharacterData")]
    [SerializeField] private float attackRange = 0;
    [BoxGroup("CharacterData")]
    [SerializeField] private float skillChange = 0;
    [BoxGroup("CharacterData")]
    [SerializeField] private AbilityData ability;
    [BoxGroup("CharacterData")]
    [SerializeField] private TargetPriority priority;
    [BoxGroup("TarGet Debug")]
    [SerializeField] private List<Enemy> enemys;
    [BoxGroup("TarGet Debug")]
    [SerializeField] private Enemy target;
    [BoxGroup("Component")]
    [SerializeField] private CircleCollider2D circleCollider;
    [BoxGroup("Component")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool longRange;
    private float nextAttack = 1f;
    private CharacterAnimation characterAnimation;
    private void Start()
    {
        Debug.Log("Base class");

        SetUpData();
    }
    public void SetUpData()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        characterAnimation = GetComponentInChildren<CharacterAnimation>();
        elementType = characterData.elementType;
        attackDamage = characterData.attackDamage;
        attackRatio = characterData.attackRatio;
        attackRange = characterData.attackRange;
        skillChange = characterData.skillChange;
        ability = characterData.ability;
        circleCollider.radius = attackRange;
        characterAnimation.SetUpAnimator(characterData.animatorController, characterData.AttackDuretionAnimation);
    }
    public void SetUpData(CharacterData characterData)
    {
        circleCollider = GetComponent<CircleCollider2D>();
        characterAnimation = GetComponentInChildren<CharacterAnimation>();
        elementType = characterData.elementType;
        attackDamage = characterData.attackDamage;
        attackRatio = characterData.attackRatio;
        attackRange = characterData.attackRange;
        skillChange = characterData.skillChange;
        ability = characterData.ability;
        circleCollider.radius = attackRange;
        characterAnimation.SetUpAnimator(characterData.animatorController, characterData.AttackDuretionAnimation);
        this.characterData = characterData;
    }
    private void Update()
    {
        FindEnemy();
        Attack();
        LookAtTarget();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemys.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemys.Remove(enemy);
            target = null;
        }
    }
    private void Attack()
    {
        nextAttack -= Time.deltaTime * attackRatio;
        if (nextAttack <= 0 && target != null)
        {
            nextAttack = 1f;
            OnAttack?.Invoke();
            if (CheckUseAbility())
            {
                UseAbility();
                return;
            }
            target.TakeDamage(gameObject, attackDamage);
        }
    }

    private void LookAtTarget()
    {
        if (target != null)
        {
            if (target.transform.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
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

    private Enemy FindCloseEnemy()
    {
        Enemy closeEnemy = null;
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
        if (skillRng <= characterData.skillChange)
        {
            return true;
        }
        return false;
    }
    private void UseAbility()
    {

        if (ability != null)
            switch (ability.targetType)
            {
                case AbilityTargetType.Self:
                    ability.ActiveAbilityToSelf(gameObject);
                    break;
                case AbilityTargetType.Target:
                    ability.ActiveAbilityToOther(target.gameObject);
                    break;
            }
    }
    [Button()]
    public void Sell()
    {
        OnSell?.Invoke(this);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, characterData.attackRange);
    }
#endif
}
