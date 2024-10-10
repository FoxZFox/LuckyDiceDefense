using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool : MonoBehaviour
{
    [SerializeField] protected GameObject prefabs;
    [SerializeField] protected GameObject container;
    [SerializeField] protected int poolSize = 0;
    [SerializeField] protected Queue<GameObject> pool;

    public abstract void SetUp();

    protected void InitialisePool()
    {
        pool = new Queue<GameObject>();
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
