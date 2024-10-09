using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatMediator
{
    readonly LinkedList<StatModifier> modifiers = new();
    public event EventHandler<Query> Queries;
    public Action OnUpdateData;
    public void PerformQuery(object sender, Query query) => Queries?.Invoke(sender, query);

    public void AddModifier(StatModifier modifier)
    {
        if (CheckModifierWithSkill(modifier))
        {
            return;
        }
        modifiers.AddLast(modifier);
        modifier.MarkForRemove = false;
        Queries += modifier.Handle;

        modifier.OnDispose += _ =>
        {
            modifiers.Remove(modifier);
            Queries -= modifier.Handle;
        };
    }

    private bool CheckModifierWithSkill(StatModifier statModifier)
    {
        var node = modifiers.First;
        while (node != null)
        {
            if (node.Value.AbilityName == statModifier.AbilityName)
            {
                node.Value.ResetTime();
                return true;
            }
            node = node.Next;
        }
        return false;
    }

    public void Update(float deltaTime)
    {
        var node = modifiers.First;
        while (node != null)
        {
            var modifier = node.Value;
            modifier.Update(deltaTime);
            node = node.Next;
        }

        node = modifiers.First;
        while (node != null)
        {
            var modifier = node.Value;
            if (modifier.MarkForRemove)
            {
                node.Value.Dispose();
                OnUpdateData?.Invoke();
            }

            node = node.Next;
        }
    }
}

