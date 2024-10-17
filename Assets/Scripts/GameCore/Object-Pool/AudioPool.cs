using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPool : ObjectPool
{
    public override void SetUp()
    {
        container = new GameObject("$Audio - Pool");
        DontDestroyOnLoad(container);
        DontDestroyOnLoad(gameObject);
        InitialisePool();
    }
}
