using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownTimer : Timer
{
    public bool IsFinish => Time <= 0;
    public CountDownTimer(float value) : base(value)
    {
    }

    public override void Tick(float deltaTime)
    {
        if (IsRunning && Time > 0)
        {
            Time -= deltaTime;
        }

        if (IsRunning && Time <= 0)
        {
            Stop();
        }
    }

    public void Reset()
    {
        Time = initialTime;
    }

    public void Reset(float duration)
    {
        Time = duration;
    }
}
