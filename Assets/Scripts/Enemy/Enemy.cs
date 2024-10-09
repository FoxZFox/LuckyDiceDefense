using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : SerializedMonoBehaviour
{
    [HideInInspector] public Action OnHit;
    public Action<GameObject> OnDie;
    public Action<GameObject> OnEndPath;
    public Stats Stats { get; set; }
    [Header("Stat")]
    [SerializeField] private float speed = 1;
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float health = 10f;
    [SerializeField] private AbilityData ability;
    [SerializeField] private float skillChange;
    [SerializeField] private Vector3[] paths;
    [SerializeField] private int currentPathIndex = 0;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemyAnimation enemyAnimation;
    private Vector3 currentPath;
    private Vector3 lastPath;
    public float Health { get => health; }
    public float MaxHealth { get => maxHealth; }
    public float InComingDamage = 0;
    public float Speed => speed;
    [Header("Debug")]
    [SerializeField] bool loopLocation;
    [SerializeField] AbilityData testAbility;
    //Stat Modifier
    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        Stats = new Stats(new StatMediator(), walkSpeed: speed, health: health);
    }

    void Update()
    {
        Move();
        CheckFlipSprtie();
        CheckEndPath();
        Stats.Mediator.Update(Time.deltaTime);
        //Update Debug Zone
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.J))
        {
            TakeDamage(null, 1f);
        }
#endif
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
        speed = Stats.WalkSpeed;
    }

    private void Handle(StatModifier statModifier)
    {

    }

#if UNITY_EDITOR
    //Method Debug Zone
    [Button]
    private void TestAbility()
    {
        testAbility.ActiveAbilityToSelf(gameObject);
    }
#endif
    public void SetupData(EnemyData data, Vector3[] path)
    {
        paths = path;
        enemyData = data;
        currentPathIndex = 0;
        enemyAnimation.SetAnimationRuntimeController(enemyData.animatorController);
        currentPath = paths[currentPathIndex];
        speed = enemyData.walkSpeed;
        maxHealth = enemyData.health;
        health = maxHealth;
        ability = enemyData.abilityData;
        GetComponent<EnemyHealth>().UpdateHealthBar();
        skillChange = enemyData.skillChange;
        Stats.UpdateData(walkSpeed: speed, health: health);
    }
    public void TakeDamage(GameObject owner, float damage)
    {
        InComingDamage = damage;
        if (CheckUseSkill())
        {
            UseSkill(owner);
        }
        health -= InComingDamage;
        if (health <= 0)
        {
            health = 0;
            OnDie?.Invoke(gameObject);
        }
        else if (health > 0)
        {
            if (enemyAnimation.currentState != enemyAnimation.hurtState)
            {
                StartCoroutine(ChangeSpeed());
            }
            OnHit?.Invoke();
        }
    }

    private void UseSkill(GameObject owner)
    {
        switch (ability.abilityType)
        {
            case AbilityType.Buff:
                ability.ActiveAbilityToSelf(gameObject);
                break;
            case AbilityType.Debuff:
                ability.ActiveAbilityToOther(owner);
                break;
        }
    }

    private bool CheckUseSkill()
    {
        float skillRng = UnityEngine.Random.Range(0f, 100f);
        if (skillRng <= skillChange)
        {
            return true;
        }
        return false;
    }

    private IEnumerator ChangeSpeed()
    {
        speed = 0;
        yield return new WaitForSeconds(0.375f);
        speed = enemyData.walkSpeed;
    }

    private void Move()
    {
        float minspeed = speed * 0.1f;
        float total = Mathf.Clamp(speed, minspeed, speed);
        // float totalspeed = speed + modifyStats[Stat.WalkSpeed];
        Vector3 nextMove = Vector3.MoveTowards(transform.position, currentPath, total * Time.deltaTime);
        transform.position = nextMove;

    }

    private void CheckFlipSprtie()
    {
        if (lastPath.x > currentPath.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    private void CheckEndPath()
    {
        float magnitude = (transform.position - currentPath).magnitude;
        if (magnitude < 0.01f)
        {
            transform.position = currentPath;
            if (currentPathIndex >= paths.Length - 1)
            {
                if (!loopLocation)
                {
                    OnEndPath?.Invoke(gameObject);
                }
                else
                {
                    currentPathIndex = 0;
                }
            }
            else
            {
                currentPathIndex++;
            }
            lastPath = currentPath;
            currentPath = paths[currentPathIndex];
        }
    }
}
