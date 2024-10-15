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
    public Stats Stats { get; set; }

    [BoxGroup("CharacterData"), TabGroup("Data")]
    [SerializeField] private int level;
    [BoxGroup("CharacterData"), TabGroup("Data")]
    [SerializeField] private CharacterData characterData;
    [BoxGroup("CharacterData"), TabGroup("Data")]
    [SerializeField] private ElementType elementType;
    [BoxGroup("CharacterData"), TabGroup("Data")]
    [SerializeField] private float attackDamage = 0;
    [BoxGroup("CharacterData"), TabGroup("Data")]
    [SerializeField] private float attackRatio = 0;
    [BoxGroup("CharacterData"), TabGroup("Data")]
    [SerializeField] private float attackRange = 0;
    [BoxGroup("CharacterData"), TabGroup("Data")]
    [SerializeField] private float skillChange = 0;
    [BoxGroup("CharacterData"), TabGroup("Data")]
    [SerializeField] private AbilityData ability;
    [BoxGroup("CharacterData"), TabGroup("Data")]
    [SerializeField] private TargetPriority priority;
    [BoxGroup("TarGet Debug"), TabGroup("Data")]
    [SerializeField] private List<Enemy> enemys;
    [BoxGroup("TarGet Debug"), TabGroup("Data")]
    [SerializeField] private Enemy target;
    [BoxGroup("Component"), TabGroup("Data")]
    [SerializeField] private CircleCollider2D circleCollider;
    [BoxGroup("Component"), TabGroup("Data")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [TabGroup("Setting")]
    [SerializeField] private float minDamage;
    [TabGroup("Setting")]
    [SerializeField] private float maxDamage;
    [TabGroup("Setting")]
    [SerializeField] private float minAttackRatio;
    [TabGroup("Setting")]
    [SerializeField] private float maxAttackRatio;
    [TabGroup("Setting")]
    [SerializeField] private float minAttackRange;
    [TabGroup("Setting")]
    [SerializeField] private float maxAttackRange;
    private bool longRange;
    private float nextAttack = 1f;
    private CharacterAnimation characterAnimation;
    private void Awake()
    {
        Stats = new Stats(new StatMediator());

    }
    private void Start()
    {
        Debug.Log("Base class");
    }
    private void OnEnable()
    {
        Stats.Mediator.OnUpdateData += UpdateData;
    }

    private void OnDisable()
    {
        Stats.Mediator.OnUpdateData -= UpdateData;
    }

    public void UpdateData()
    {
        attackDamage = Mathf.Clamp(Stats.Attack, minDamage, maxDamage);
        attackRatio = Mathf.Clamp(Stats.AttackRatio, minAttackRatio, maxAttackRatio);
        attackRange = Mathf.Clamp(Stats.AttackRange, minAttackRange, maxAttackRange);
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
    public void SetUpData(InventoryCharacter data)
    {
        circleCollider = GetComponent<CircleCollider2D>();
        characterAnimation = GetComponentInChildren<CharacterAnimation>();
        level = data.Level;
        characterData = data.characterData;
        elementType = characterData.elementType;
        attackDamage = characterData.attackDamage + characterData.GetAttackDamageWithGrowth(level);
        attackRatio = characterData.attackRatio + characterData.GetAttackRaioWithGrowth(level);
        attackRange = characterData.attackRange;
        skillChange = characterData.skillChange;
        ability = characterData.ability;
        circleCollider.radius = attackRange;
        characterAnimation.SetUpAnimator(characterData.animatorController, characterData.AttackDuretionAnimation);
        Stats.UpdateData(attackDamage, attackRatio, attackRange);
    }
    private void Update()
    {
        if (GameManager.GetInstant().StageType == StageType.Start)
        {
            Stats.Mediator.Update(Time.deltaTime);
            FindEnemy();
            Attack();
            LookAtTarget();
        }

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
            target.TakeDamage(gameObject, elementType, attackDamage);
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
                    ability.ActiveAbilityToOther(gameObject, target.gameObject);
                    break;
                case AbilityTargetType.TeamAoe:
                    ability.ActiveAbilityToOther(gameObject, GetTeamTargetInRange());
                    break;
            }
    }

    private List<GameObject> GetTeamTargetInRange()
    {
        List<GameObject> targets = new List<GameObject>();
        var colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var item in colliders)
        {
            if (item.gameObject.GetComponent<Character>() != null)
            {
                if (Vector2.Distance(transform.position, item.transform.position) <= attackRange)
                {
                    targets.Add(item.gameObject);
                }
            }
        }
        return targets;
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
