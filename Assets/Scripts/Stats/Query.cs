using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Query
{
    public readonly StatType StatType;
    public float Value;

    public Query(StatType statType, float value)
    {
        StatType = statType;
        Value = value;
    }
}
