using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Timer
{
    public Action OnTimeStart = delegate { };
    public Action OnTimeStop = delegate { };
    protected float initialTime;
    public bool IsRunning { get; private set; }
    public float Time { get; set; }
    public Timer(float value)
    {
        initialTime = value;
        IsRunning = false;
    }

    public void Start()
    {
        Time = initialTime;
        if (!IsRunning)
        {
            IsRunning = true;
            OnTimeStart.Invoke();
        }
    }

    public void Stop()
    {
        if (IsRunning)
        {
            IsRunning = false;
            OnTimeStop.Invoke();
        }
    }
    public abstract void Tick(float deltaTime);

}
