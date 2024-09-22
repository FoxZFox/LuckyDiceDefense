using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Character character;
    private Animator animator;
    private float lockedTill;
    public readonly int idleState = Animator.StringToHash("Idle");
    public readonly int attckState = Animator.StringToHash("Attack");
    private bool attackTrigger;
    public int currentState;
    private float attackDuration = 0.2f;
    private void Start()
    {
        animator = GetComponent<Animator>();
        character = GetComponentInParent<Character>();
        character.OnAttack += () => { attackTrigger = true; };
    }

    private void Update()
    {
        PlayAnimation();
    }
    public void SetUpAnimator(RuntimeAnimatorController runtimeAnimatorController, float duration)
    {
        animator.runtimeAnimatorController = runtimeAnimatorController;
        attackDuration = duration;
    }

    private void PlayAnimation()
    {
        var state = GetState();

        attackTrigger = false;

        if (state == currentState) return;
        animator.CrossFade(state, 0, 0);
        currentState = state;
    }

    private int GetState()
    {
        if (Time.time < lockedTill) return currentState;
        if (attackTrigger) return LockState(attckState, attackDuration);
        return idleState;

        int LockState(int s, float t)
        {
            lockedTill = Time.time + t;
            return s;
        }
    }

}
