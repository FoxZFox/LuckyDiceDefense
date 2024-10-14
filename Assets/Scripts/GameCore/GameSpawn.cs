using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

public class GameSpawn : MonoBehaviour
{
    [SerializeField] private EnemyPool enemyPool;
    [SerializeField] private int enemyRemain = 0;
    [SerializeField] private int spawnMax = 0;
    [SerializeField] private float cooldown = 0;
    [SerializeField] private int spawnRemain = 0;
    [SerializeField] private List<GameObject> enemyList;
#if UNITY_EDITOR
    [Header("Debug Only")]
    [SerializeField] private EnemyData testData;
#endif
    private Coroutine onSpawn = null;
    private GameWaypoints gameWaypoints;
    private Vector3 spawnLocation;
    private GameManager gameManager;
    private int currentWave = 0;
    private int maxWave = 0;

    public Action OnVictory;
    public Action<int, int> OnWaveEnd;

    private void Start()
    {
        gameManager = GameManager.GetInstant();
        currentWave = 0;
        maxWave = PlayerData.Instant.selectStage.datas.Count;
    }
    public void SetUp(EnemyPool enemyPool, GameWaypoints gameWaypoints)
    {
        this.enemyPool = enemyPool;
        this.gameWaypoints = gameWaypoints;
        spawnRemain = spawnMax;
        spawnLocation = gameWaypoints.Waypoints[0];
    }

    [Button]
    public void Spawn()
    {
        if (onSpawn == null && gameManager.StageType == StageType.Start)
        {
            SetUpSpawnData();
            spawnRemain = spawnMax;
            onSpawn = StartCoroutine(SpawnEnemy());
        }
    }

    private void SetUpSpawnData()
    {
        enemyRemain = 0;
        spawnMax = PlayerData.Instant.selectStage.datas[currentWave].EnemyContainer.Count;
        spawnRemain = 0;
    }

    private void Update()
    {
        //Debug Zone
#if UNITY_EDITOR
#endif
    }

    private IEnumerator SpawnEnemy()
    {
        while (spawnRemain > 0)
        {
            if (gameManager.StageType != StageType.Start)
            {
                continue;
            }
            GetEnemyFormPool();
            spawnRemain--;
            enemyRemain++;
            gameManager.UiSystem.UpdateEnemyRemain(enemyRemain);
            yield return new WaitForSeconds(cooldown);
        }
        onSpawn = null;
    }

    private void GetEnemyFormPool()
    {
        var init = enemyPool.GetObject();
        var enemy = init.GetComponent<Enemy>();
        enemy.OnDie += ReturnObjectToPool;
        enemy.OnEndPath += ReturnObjectToPool;
        enemy.OnEndPath += OnEndPath;
#if UNITY_EDITOR
        enemy.SetupData(testData, gameWaypoints.Waypoints);
#endif
        init.transform.position = spawnLocation;
        enemyList.Add(init);
        init.SetActive(true);
    }

    private void ReturnObjectToPool(GameObject gameObject)
    {
        enemyList.Remove(gameObject);
        enemyPool.ReturnPool(gameObject);
        enemyRemain--;
        gameObject.GetComponent<Enemy>().OnDie -= ReturnObjectToPool;
        gameObject.GetComponent<Enemy>().OnEndPath -= ReturnObjectToPool;
        gameManager.UiSystem.UpdateEnemyRemain(enemyRemain);
        CheckEndWave();
    }

    private void CheckEndWave()
    {
        if (currentWave + 1 == maxWave)
        {
            OnVictory?.Invoke();
            return;
        }
        if (enemyRemain <= 0)
        {
            var reward = PlayerData.Instant.selectStage.datas[currentWave].RewardContaner;
            OnWaveEnd?.Invoke(reward.Gold, reward.Gem);
            currentWave++;
        }
    }

    public int GetCurrentWave()
    {
        if (currentWave + 1 != maxWave)
        {
            return currentWave + 1;
        }
        return -1;
    }

    private void OnEndPath(GameObject gameObject)
    {
        var enemy = gameObject.GetComponent<Enemy>();
        int damage = (int)Mathf.Floor(enemy.Health);
        gameManager.StagePointTakeDamage(damage);
        enemy.OnEndPath -= OnEndPath;
    }
}
