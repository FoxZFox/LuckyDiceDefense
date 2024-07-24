using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int Level { get; protected set; }
    public int Cost { get; protected set; }
    protected Skill skill;
    protected abstract void Attack();
    protected virtual void Start()
    {
        Debug.Log("Base class");
    }
    protected virtual void Update()
    {

    }
    protected abstract void UseSkill();
}
