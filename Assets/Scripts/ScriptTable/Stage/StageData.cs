using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New StageData", menuName = "StageData", order = 0)]
public class StageData : ScriptableObject
{
    public string StageName;
    public int StageHealthPoint;
    [TableList(ShowIndexLabels = true, AlwaysExpanded = true)]
    public List<StageContainer> datas;
    public MapData map;
}

[Serializable]
public class StageContainer
{
    [VerticalGroup("Data")]
    public List<EnemyData> EnemyContainer;
    [VerticalGroup("Reward")]
    public RewardContaner RewardContaner;

    [TableColumnWidth(60)]
    [Button(DisplayParameters = true), VerticalGroup("Actions")]
    public void AddEnemyAmount(EnemyData data, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            EnemyContainer.Add(data);
        }
    }
    [TableColumnWidth(60)]
    [Button(DisplayParameters = true), VerticalGroup("Actions")]
    public void ShufflerData()
    {
        var data = EnemyContainer;
        EnemyContainer = data.OrderBy(value => Guid.NewGuid()).ToList();
    }
}

[Serializable]
public class RewardContaner
{
    public int Gold;
    public int Gem;
}