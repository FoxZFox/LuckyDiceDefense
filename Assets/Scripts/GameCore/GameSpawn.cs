using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefabs;
    [SerializeField] private int spawnCount = 0;
    [SerializeField] private int spawnMax = 0;
    private int spawnRemain = 0;
    [SerializeField] private float cooldown = 0;

    private void Start()
    {
        spawnRemain = spawnMax;
        currentCooldown = cooldown;
    }

    private float currentCooldown;
    private void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0)
        {
            currentCooldown = cooldown;
            if (spawnCount < spawnMax)
            {
                Instantiate(enemyPrefabs);
                spawnRemain--;
                spawnCount++;
            }
        }

    }
}
