using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Container-Item-Data", menuName = "Stata/itemcontainer")]
public class ItemDataContainer : ScriptableObject
{
    [SerializeField] List<ItemData> itemDatas = new List<ItemData>();
    public List<ItemData> ItemDatas { get => itemDatas; set => itemDatas = value; }
}
