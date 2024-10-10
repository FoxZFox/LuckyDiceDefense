using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instant;

    [BoxGroup("Utility")]
    [SerializeField] private DrawMap drawMap;
    [BoxGroup("Utility")]
    [SerializeField] private EnemyPool enemyPool;
    [BoxGroup("Utility")]
    [SerializeField] private CharacterPool characterPool;
    [BoxGroup("Utility")]
    [SerializeField] private GameSpawn gameSpawn;
    [BoxGroup("Utility")]
    [SerializeField] private GameWaypoints gameWaypoints;
    [BoxGroup("Utility")]
    [SerializeField] private BuildManager buildManager;
    [BoxGroup("Data")]
    [ShowInInspector] public StageType StageType { get; private set; }
    [BoxGroup("Data")]
    [SerializeField] private MapData map;
    public BuildManager BuildManager => buildManager;
    public DrawMap Drawmap => drawMap;
    private void Awake()
    {
        if (instant == null)
        {
            //Delete This After Finish GamePlay Setup
            LoadGamePlayData();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [Button()]
    public void LoadGamePlayData()
    {
        instant = this;
        drawMap = GetComponent<DrawMap>();
        enemyPool = GetComponent<EnemyPool>();
        gameSpawn = GetComponent<GameSpawn>();
        gameWaypoints = GetComponent<GameWaypoints>();
        buildManager = GetComponent<BuildManager>();
        characterPool = GetComponent<CharacterPool>();
        SetUp();

    }

    private void SetUp()
    {
        gameSpawn.SetUp(enemyPool, gameWaypoints);
        enemyPool.SetUp();
        characterPool.SetUp();
        drawMap.SetUp(map);
        StageType = StageType.BuildMap;
        // drawMap.CreateTileInstant();
    }

    public static GameManager GetInstant()
    {
        return instant;
    }

    public void SetStage(StageType type)
    {
        StageType = type;
    }
}

public enum StageType
{
    Prepare,
    Start,
    SpeedUp,
    End,
    BuildMap
}
