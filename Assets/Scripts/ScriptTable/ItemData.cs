using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemData", menuName = "CreateTableObject/ItemData")]
public class ItemData : ScriptableObject
{
    public int ItemID;
    public string ItemName;
}
