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
    [BoxGroup("Utility")]
    [SerializeField] private UIGamePlaySystem uiSystem;
    [BoxGroup("Data")]
    [ShowInInspector] public StageType StageType { get; private set; }
    [BoxGroup("Data")]
    [SerializeField] private MapData map;
#if UNITY_EDITOR
    [SerializeField] private GameObject draftile;
    [SerializeField] private GameObject maptile;
#endif
    public BuildManager BuildManager => buildManager;
    public DrawMap Drawmap => drawMap;
    public int DiceRollCount { get; private set; }
    public int DicePoint { get; private set; }
    private void Awake()
    {
        if (instant == null)
        {
            LoadGamePlayData();
#if UNITY_EDITOR
            draftile.SetActive(false);
            maptile.SetActive(true);
            uiSystem.SetUIPosition();
#endif
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.Instant.OnSceneLoaded += OnSceneLoaded;
        drawMap.OnDrawMap += OnDrawMapComplete;
    }

    private void OnDisable()
    {
        SceneManager.Instant.OnSceneLoaded -= OnSceneLoaded;
        drawMap.OnDrawMap -= OnDrawMapComplete;
    }

    private void OnSceneLoaded(SceneManager.sceneName name)
    {
        if (name == SceneManager.sceneName.gameplay)
        {
            SetUp();
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
        uiSystem = GetComponent<UIGamePlaySystem>();
    }

    private void SetUp()
    {
        gameSpawn.SetUp(enemyPool, gameWaypoints);
        enemyPool.SetUp();
        characterPool.SetUp();
        drawMap.SetUp(map);
        DicePoint = 0;
        DiceRollCount = 3;
        StageType = StageType.BuildMap;
        drawMap.CreateTileInstant();
    }

    public static GameManager GetInstant()
    {
        return instant;
    }

    public void SetStage(StageType type)
    {
        StageType = type;
    }

    private void OnDrawMapComplete()
    {
        uiSystem.FadeInUI();
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
