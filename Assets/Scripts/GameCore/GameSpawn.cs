using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

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
        if (onSpawn == null)
        {
            spawnRemain = spawnMax;
            onSpawn = StartCoroutine(SpawnEnemy());
        }
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
            GetEnemyFormPool();
            spawnRemain--;
            enemyRemain++;
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
    }
}
