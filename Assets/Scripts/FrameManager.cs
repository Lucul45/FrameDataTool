using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

public class FrameManager : Singleton<FrameManager>
{
    // time passed in seconds
    private float _elapsedTime = 0;
    // time passed in frames
    private uint _elapsedFrames = 0;

    private int _p1EndFrame = 0;
    private int _p2EndFrame = 0;

    private List<FrameActionData> _dataList = new List<FrameActionData>();
    private Dictionary<uint, List<FrameActionData>> _playersActionFrames = new Dictionary<uint, List<FrameActionData>>();
    public struct FrameActionData
    {
        public int PlayerID { get; set; }
        public EPlayerState PlayerState { get; set; }
        public int StateFrame { get; set; }
    }

    public uint ElapsedFrames
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

    private void Start()
    {
        
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
            _elapsedTime += Time.fixedDeltaTime;
        }
        //Debug.Log(_playersActionFrames[_elapsedFrames - 1]?.Where(data => data.PlayerID == 1 && data.PlayerState == EPlayerState.IDLE).Any());
        //Debug.Log(_playersActionFrames.Count != 0 && _playersActionFrames[_elapsedFrames].Where(data => data.PlayerID == 1 && data.PlayerState == EPlayerState.IDLE).Any() && _playersActionFrames[_elapsedFrames - 1].Where(data => data.PlayerID == 1 && data.PlayerState == EPlayerState.MELEE).Any());
    }

    public void AddActionFrameData(FrameActionData newData)
    {
        if (_playersActionFrames.ContainsKey(_elapsedFrames))
        { //There's already data for the current frame, YaY!
            _playersActionFrames[_elapsedFrames].Add(newData); //New data has been added into the stack
        }
        else
        { //The current frame has no data! Hell nah! :speaking_head:
            _playersActionFrames.Add(_elapsedFrames, new List<FrameActionData>() { newData });
        }
    }

    public void RemoveActionFrameData()
    {
        if (_playersActionFrames.Count >= 1000)
        {
            uint minFrame = _playersActionFrames.Keys.ToArray()[0];
            foreach (uint frame in _playersActionFrames.Keys.ToArray())
            {
                if (frame < minFrame)
                {
                    minFrame = frame;
                }
            }
            _playersActionFrames.Remove(minFrame);
        }
    }

    public int GetEndFrameAttack()
    {
        // if there is frames that as been recorded and there is a recorded current frame about the player 1 which the player 1 is in idle and was attacking the frame before
        if (_playersActionFrames.Count != 0 && _playersActionFrames[_elapsedFrames].Where(data => data.PlayerID == 1 && data.PlayerState == EPlayerState.IDLE).Any() && _playersActionFrames[_elapsedFrames - 1].Where(data => data.PlayerID == 1 && data.PlayerState == EPlayerState.MELEE).Any())
        {
            // then return the current frame
            _p1EndFrame = (int)_elapsedFrames;
        }
        return _p1EndFrame;
    }

    public int GetEndFrameHurt()
    {
        // if there is frames that as been recorded and there is a recorded current frame about the player 2 which the player 2 is in idle and was hurt the frame before
        if (_playersActionFrames.Count != 0 && _playersActionFrames[_elapsedFrames].Where(data => data.PlayerID == 2 && data.PlayerState == EPlayerState.IDLE).Any() && _playersActionFrames[_elapsedFrames - 1].Where(data => data.PlayerID == 2 && data.PlayerState == EPlayerState.HURT).Any())
        {
            // then return the current frame
            _p2EndFrame = (int)_elapsedFrames;
        }
        return _p2EndFrame;
    }
}
