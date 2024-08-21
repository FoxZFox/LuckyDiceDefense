using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Action OnHit;
    GameWaypoints gameWaypoints;
    [Header("Stat")]
    [SerializeField] private int speed = 1;
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float health = 10f;
    [SerializeField] private AbilityData ability;
    [SerializeField] private float skillChange;
    [SerializeField] private Vector3[] paths;
    [SerializeField] private int currentPathIndex = 0;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Header("Debug Only")]
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemyAnimation enemyAnimation;
    private Vector3 currentPath;
    private Vector3 lastPath;
    public float Health { get => health; }
    public float MaxHealth { get => maxHealth; }
    public float InComingDamage = 0;
    public int Speed => speed;
    //Stat Modifier
    private float speedModifier = 0;
    void Start()
    {
        gameWaypoints = FindFirstObjectByType<GameWaypoints>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        paths = gameWaypoints.Waypoints;
        currentPath = paths[currentPathIndex];
        SetupData();
    }

    void Update()
    {
        Move();
        CheckFlipSprtie();
        CheckEndPath();
        if (Input.GetKeyDown(KeyCode.J))
        {
            TakeDamage(null, 1f);
        }
    }
    private void SetupData()
    {
        speed = enemyData.walkSpeed;
        maxHealth = enemyData.health;
        health = maxHealth;
        enemyAnimation.SetAnimationRuntimeController(enemyData.animatorController);
        ability = enemyData.abilityData;
        skillChange = enemyData.skillChange;
    }
    public void TakeDamage(GameObject owner, float damage)
    {
        InComingDamage = damage;
        if (CheckUseSkill())
        {
            UseSkill(owner);
        }
        health -= InComingDamage;
        if (health < 0)
        {
            health = 0;
        }
        else if (health > 0)
        {
            if (enemyAnimation.currentState != enemyAnimation.hurtState)
            {
                StartCoroutine(ChangeSpeed());
            }
        }
        OnHit?.Invoke();
    }

    private void UseSkill(GameObject owner)
    {
        switch (ability.abilityType)
        {
            case AbilityData.AbilityType.Buff:
                ability.ActiveAbilityToSelf(gameObject);
                break;
            case AbilityData.AbilityType.Debuff:
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
        float totalspeed = speed + speedModifier;
        if (totalspeed < 0) totalspeed = 0;
        Vector3 nextMove = Vector3.MoveTowards(transform.position, currentPath, totalspeed * Time.deltaTime);
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
            if (currentPathIndex == paths.Length - 1)
            {
                currentPathIndex = 0;
            }
            else
            {
                currentPathIndex++;
            }
            lastPath = currentPath;
            currentPath = paths[currentPathIndex];
        }
    }

    public void ModifySpeed(float modify)
    {
        speedModifier += modify;
    }
}
