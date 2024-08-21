using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThread : MonoBehaviour
{
    public static UnityMainThread Instant;
    private Queue<Action> excutionQueue = new Queue<Action>();
    // Start is called before the first frame update
    void Start()
    {
        if (Instant == null)
        {
            Instant = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        lock (excutionQueue)
        {
            while (excutionQueue.Count > 0)
            {
                excutionQueue.Dequeue().Invoke();
            }
        }
    }

    public void Enqueue(Action action)
    {
        excutionQueue.Enqueue(action);
    }

    public void EnqueCorutine(IEnumerator enumerator)
    {
        excutionQueue.Enqueue(() => StartCoroutine(enumerator));
    }
}
