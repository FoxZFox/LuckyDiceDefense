using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    private readonly StatMediator mediator;
    private float attack;
    private float attackRatio;
    private float attackRange;
    private float walkSpeed;
    private float health;
    public StatMediator Mediator => mediator;

    public float Attack
    {
        get
        {
            var q = new Query(StatType.Attack, attack);
            mediator.PerformQuery(this, q);
            return q.Value;
        }
    }

    public float AttackRatio
    {
        get
        {
            var q = new Query(StatType.AttackRatio, attackRatio);
            mediator.PerformQuery(this, q);
            return q.Value;
        }
    }
    public float AttackRange
    {
        get
        {
            var q = new Query(StatType.AttackRange, attackRange);
            mediator.PerformQuery(this, q);
            return q.Value;
        }
    }
    public float WalkSpeed
    {
        get
        {
            var q = new Query(StatType.WalkSpeed, walkSpeed);
            mediator.PerformQuery(this, q);
            return q.Value;
        }
    }
    public float Health
    {
        get
        {
            var q = new Query(StatType.Health, health);
            mediator.PerformQuery(this, q);
            return q.Value;
        }
    }

    public Stats(StatMediator statMediator, float attack = 0f, float attackRatio = 0f, float attackRange = 0f, float walkSpeed = 0f, float health = 0f)
    {
        mediator = statMediator;
        this.attack = attack;
        this.attackRatio = attackRatio;
        this.attackRange = attackRange;
        this.walkSpeed = walkSpeed;
        this.health = health;
    }

    public void UpdateData(float attack = 0f, float attackRatio = 0f, float attackRange = 0f, float walkSpeed = 0f, float health = 0f)
    {
        this.attack = attack;
        this.attackRatio = attackRatio;
        this.attackRange = attackRange;
        this.walkSpeed = walkSpeed;
        this.health = health;
    }
}
