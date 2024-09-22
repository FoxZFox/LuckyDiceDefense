using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Enemy enemy;
    public int currentState;
    private float lockedTill;
    public readonly int walkState = Animator.StringToHash("Walk");
    public readonly int hurtState = Animator.StringToHash("Hurt");
    private float hurtDuration = 0.375f;
    private bool hurtTrigger;

    private void Awake()
    {
        Debug.Log("GetData");
        animator = GetComponent<Animator>();
        enemy.OnHit += () => { hurtTrigger = true; };
        enemy.OnDie += (go) => { hurtTrigger = false; };
    }
    public void SetAnimationRuntimeController(RuntimeAnimatorController runtime)
    {
        animator.runtimeAnimatorController = runtime;
    }

    private void Update()
    {
        PlayAnimation();
    }

    private void PlayAnimation()
    {
        var state = GetState();

        hurtTrigger = false;

        if (state == currentState) return;
        animator.CrossFade(state, 0, 0);
        currentState = state;
    }

    private int GetState()
    {
        if (Time.time < lockedTill) return currentState;
        if (hurtTrigger) return LockState(hurtState, hurtDuration);
        return walkState;

        int LockState(int s, float t)
        {
            lockedTill = Time.time + t;
            return s;
        }
    }
}
