using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Container Enemy Data", menuName = "Stata/EnemyDataContainer")]
public class EnemyDataContainer : ScriptableObject
{
    [SerializeField] private List<EnemyData> enemyDatas = new List<EnemyData>();
    public List<EnemyData> EnemyDatas { get => enemyDatas; set => enemyDatas = value; }
}
