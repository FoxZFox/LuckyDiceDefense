using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifier : IDisposable
{
    public readonly string AbilityName;
    public bool MarkForRemove { get; set; }
    private readonly Dictionary<StatType, float> modifyStats;
    private readonly CountDownTimer timer;

    public Action<StatModifier> OnDispose = delegate { };

    readonly Func<float, StatType, float> operation;
    public StatModifier(string abilityName, Dictionary<StatType, float> modifys, float duration, Func<float, StatType, float> operation)
    {
        if (duration <= 0) return;
        timer = new CountDownTimer(duration);
        timer.OnTimeStop += () => MarkForRemove = true;
        timer.Start();

        AbilityName = abilityName;
        modifyStats = modifys;
        this.operation = operation;
    }

    public void ResetTime()
    {
        timer.Reset();
    }


    public void Update(float deltaTime) => timer?.Tick(deltaTime);
    public void Handle(object sender, Query query)
    {
        foreach (var item in modifyStats)
        {
            if (query.StatType == item.Key)
            {
                query.Value = operation(query.Value, query.StatType);
            }
        }

    }

    public void Dispose()
    {
        OnDispose?.Invoke(this);
    }
}
