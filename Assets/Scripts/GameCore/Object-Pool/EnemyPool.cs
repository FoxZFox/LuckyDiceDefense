using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : ObjectPool
{
    public override void SetUp()
    {
        container = new GameObject("$Enemy - Pool");
        InitialisePool();
    }
}
