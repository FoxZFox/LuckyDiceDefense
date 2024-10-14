using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private static GameManager instant;

    [BoxGroup("Utility")]
    [SerializeField] public DrawMap DrawMap { get; private set; }
    [BoxGroup("Utility")]
    [SerializeField] public EnemyPool EnemyPool { get; private set; }
    [BoxGroup("Utility")]
    [SerializeField] public CharacterPool CharacterPool { get; private set; }
    [BoxGroup("Utility")]
    [SerializeField] public GameSpawn GameSpawn { get; private set; }
    [BoxGroup("Utility")]
    [SerializeField] public GameWaypoints GameWaypoints { get; private set; }
    [BoxGroup("Utility")]
    [SerializeField] public BuildManager BuildManager { get; private set; }
    [BoxGroup("Utility")]
    [SerializeField] public UIGamePlaySystem UiSystem { get; private set; }
    [BoxGroup("Utility")]
    [SerializeField] public DiceManager DiceManager { get; private set; }
    [BoxGroup("Data")]
    [ShowInInspector] public StageType StageType { get; private set; }
    [BoxGroup("Data")]
    [SerializeField] private MapData map;
#if UNITY_EDITOR
    [SerializeField] private GameObject draftile;
    [SerializeField] private GameObject maptile;
#endif
    [ShowInInspector, ReadOnly] public int DiceRollCount { get; private set; }
    [ShowInInspector, ReadOnly] public int DicePoint { get; private set; }
    private void Awake()
    {
        if (instant == null)
        {
            LoadGamePlayData();
#if UNITY_EDITOR
            draftile.SetActive(false);
            maptile.SetActive(true);
            UiSystem.SetUIPosition();
            DiceRollCount += 3;
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
        DrawMap.OnDrawMap += OnDrawMapComplete;
        DiceManager.OnRiceRollStart += OnDiceRollStart;
        DiceManager.OnDiceRollEnd += OnDiceRollComplete;
        DiceManager.OnDiceCountEnd += OnDiceCountComplete;
        BuildManager.OnBuildCharacter += OnBuildCharacterComplete;
    }

    private void OnDisable()
    {
        SceneManager.Instant.OnSceneLoaded -= OnSceneLoaded;
        DrawMap.OnDrawMap -= OnDrawMapComplete;
        DiceManager.OnRiceRollStart -= OnDiceRollStart;
        DiceManager.OnDiceRollEnd -= OnDiceRollComplete;
        DiceManager.OnDiceCountEnd -= OnDiceCountComplete;
        BuildManager.OnBuildCharacter -= OnBuildCharacterComplete;
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
        DrawMap = GetComponent<DrawMap>();
        EnemyPool = GetComponent<EnemyPool>();
        GameSpawn = GetComponent<GameSpawn>();
        GameWaypoints = GetComponent<GameWaypoints>();
        BuildManager = GetComponent<BuildManager>();
        CharacterPool = GetComponent<CharacterPool>();
        UiSystem = GetComponent<UIGamePlaySystem>();
        DiceManager = GetComponent<DiceManager>();
    }

    private void SetUp()
    {
        GameSpawn.SetUp(EnemyPool, GameWaypoints);
        EnemyPool.SetUp();
        CharacterPool.SetUp();
        DrawMap.SetUp(map);
        DicePoint = 0;
        DiceRollCount = 3;
        StageType = StageType.BuildMap;
        DrawMap.CreateTileInstant();
    }

    public static GameManager GetInstant()
    {
        return instant;
    }

    public void SetStage(StageType type)
    {
        StageType = type;
    }

    private void OnDrawMapComplete(Tilemap _)
    {
        UiSystem.FadeInDiceRollCount();
        StageType = StageType.RollDiceRemain;
    }
    private void OnDiceRollStart()
    {
        DiceRollCount -= 1;
    }
    private void OnDiceRollComplete(int value)
    {
        DicePoint += value;
    }

    private void OnDiceCountComplete(int value)
    {
        DiceRollCount = value;
        UiSystem.FadeOutDiceRollCount();
        StageType = StageType.Prepare;
    }

    private void OnBuildCharacterComplete(Vector3 _, CharacterData data)
    {
        DicePoint -= data.costToBuild;
    }
}

public enum StageType
{
    Prepare,
    Start,
    SpeedUp,
    End,
    BuildMap,
    RollDiceRemain
}
