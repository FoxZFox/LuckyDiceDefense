using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterData", menuName = "CreateTableObject/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("Common")]
    public GameObject CharacterPrefab;
    public int CharacterID = 1;
    [Range(1, 5)] public int Star = 1;
    public int ItemNeded = 1;
    public Sprite placeHolderSpitre;
    [Header("Units and Build")]
    public float attackRatio = 1f;
    public float attackRange = 3f;
    [Range(1f, 100f)] public float skillChange = 100f;
    [Min(1)] public int costToBuild = 100;
    public (int, int) MapData()
    {
        return (ItemNeded, CharacterID);
    }
}
