using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FrameManager : Singleton<FrameManager>
{
    // time passed in seconds
    private float _elapsedTime = 0;
    // time passed in frames
    private int _elapsedFrames = 0;

    public int ElapsedFrames
    {
        get { return _elapsedFrames; }
    }

    private event Action _frameUpdate = null;
    public event Action FrameUpdate
    {
        add
        {
            _frameUpdate -= value;
            _frameUpdate += value;
        }
        remove { _frameUpdate -= value; }
    }

    private void FixedUpdate()
    {
        if (_elapsedTime >= 1/60)
        {
            _elapsedFrames++;
            _frameUpdate();
            _elapsedTime = 0;
        }
        else
        {
            _elapsedTime += Time.deltaTime;
        }
    }
}
