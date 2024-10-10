using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPool : ObjectPool
{
    public override void SetUp()
    {
        container = new GameObject("$Character - Pool");
        InitialisePool();
    }
}
