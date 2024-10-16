using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private static GameManager instant;
    [ShowInInspector, BoxGroup("Utility")] public DrawMap DrawMap { get; private set; }
    [ShowInInspector, BoxGroup("Utility")] public EnemyPool EnemyPool { get; private set; }
    [ShowInInspector, BoxGroup("Utility")] public CharacterPool CharacterPool { get; private set; }
    [ShowInInspector, BoxGroup("Utility")] public GameSpawn GameSpawn { get; private set; }
    [ShowInInspector, BoxGroup("Utility")] public GameWaypoints GameWaypoints { get; private set; }
    [ShowInInspector, BoxGroup("Utility")] public BuildManager BuildManager { get; private set; }
    [ShowInInspector, BoxGroup("Utility")] public UIGamePlaySystem UiSystem { get; private set; }
    [ShowInInspector, BoxGroup("Utility")] public DiceManager DiceManager { get; private set; }
    [ShowInInspector, BoxGroup("Utility")] public InputManager InputManager { get; private set; }
    [ShowInInspector, BoxGroup("Data")] public StageType StageType { get; private set; }
    [BoxGroup("Data")]
    [SerializeField] private MapData map;
    [BoxGroup("Data")]
    [SerializeField] private GameObject draftile;
    [BoxGroup("Data")]
    [SerializeField] private GameObject maptile;
    [ShowInInspector, ReadOnly] public int DiceRollCount { get; private set; }
    [ShowInInspector, ReadOnly] public int DicePoint { get; private set; }
    public int StageHealthPoint { get; private set; }
    public int GoldReward { get; private set; }
    public int GemReward { get; private set; }
    private void Awake()
    {
        if (instant == null)
        {
            LoadGamePlayData();
            draftile.SetActive(false);
            maptile.SetActive(true);
            UiSystem.SetUIPosition();
            StageHealthPoint = PlayerData.Instant.selectStage.StageHealthPoint;
            map = PlayerData.Instant.selectStage.map;
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
        UiSystem.OnWaveStart += OnWaveStart;
        UiSystem.OnResume += OnResume;
        UiSystem.OnExit += OnExit;
        GameSpawn.OnWaveEnd += OnWaveEnd;
        InputManager.OnPause += OnPause;
        GameSpawn.OnVictory += OnVictory;
    }

    private void OnDisable()
    {
        SceneManager.Instant.OnSceneLoaded -= OnSceneLoaded;
        DrawMap.OnDrawMap -= OnDrawMapComplete;
        DiceManager.OnRiceRollStart -= OnDiceRollStart;
        DiceManager.OnDiceRollEnd -= OnDiceRollComplete;
        DiceManager.OnDiceCountEnd -= OnDiceCountComplete;
        BuildManager.OnBuildCharacter -= OnBuildCharacterComplete;
        UiSystem.OnWaveStart -= OnWaveStart;
        UiSystem.OnResume -= OnResume;
        UiSystem.OnExit -= OnExit;
        GameSpawn.OnWaveEnd -= OnWaveEnd;
        InputManager.OnPause -= OnPause;
        GameSpawn.OnVictory -= OnVictory;
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
        InputManager = GetComponent<InputManager>();
    }

    private void SetUp()
    {
        GameWaypoints.SetUp(map.GetPaths());
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
        DicePoint = Mathf.Clamp(DicePoint + value, 0, 999);
    }

    private void OnDiceCountComplete(int value)
    {
        DiceRollCount = value;
        UiSystem.FadeOutDiceRollCount();
        StageType = StageType.Prepare;
    }

    private void OnBuildCharacterComplete(Vector3 _, InventoryCharacter data)
    {
        DicePoint -= data.characterData.costToBuild;
    }

    private void OnWaveStart()
    {
        StageType = StageType.Start;
        GameSpawn.Spawn();
    }

    private void OnWaveEnd(int goldWard, int gemWard)
    {
        GoldReward += goldWard;
        GemReward += gemWard;
    }

    private void OnPause()
    {
        StageType = StageType.Pasue;
    }

    private void OnResume()
    {
        StageType = StageType.Start;
    }

    private void OnVictory(int data1, int data2)
    {
        StageType = StageType.End;
        GoldReward += data1;
        GemReward += data2;
    }

    private void OnExit()
    {
        PlayerData.Instant.AddGem(GemReward);
        PlayerData.Instant.AddGold(GoldReward);
        SaveManager.Instant.UpdateSave();
        SceneManager.Instant.LoadSceneWithTransition(SceneManager.sceneName.mainmenu, TransitionType.Circle);
    }
    public void StagePointTakeDamage(int value)
    {
        StageHealthPoint -= value;
        if (StageHealthPoint <= 0)
        {
            StageType = StageType.End;
            StageHealthPoint = 0;
            UiSystem.OnLose();
        }
        UiSystem.UpDateHearthInfo();
    }

    private void OnDestroy()
    {
        instant = null;
    }
}

public enum StageType
{
    Prepare,
    Start,
    SpeedUp,
    End,
    BuildMap,
    Pasue,
    RollDiceRemain
}
