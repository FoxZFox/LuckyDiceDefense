using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject prefabs;
    [SerializeField] private GameObject container;
    [SerializeField] private int poolSize = 0;
    [SerializeField] Queue<GameObject> pool;
    private void Start()
    {
        pool = new Queue<GameObject>();
        container = new GameObject("$Enemy - Pool");
        InitialisePool();
    }

    private void InitialisePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateObjectToPool();
        }
    }

    private void CreateObjectToPool()
    {
        var instant = Instantiate(prefabs);
        instant.transform.SetParent(container.transform);
        pool.Enqueue(instant);
        instant.SetActive(false);
    }

    private GameObject CreateObjectToReturn()
    {
        var instant = Instantiate(prefabs);
        instant.transform.SetParent(container.transform);
        return instant;
    }

    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            var init = pool.Dequeue();
            init.SetActive(true);
            return init;
        }
        else
        {
            return CreateObjectToReturn();
        }
    }

    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        pool.Enqueue(gameObject);
    }
}
