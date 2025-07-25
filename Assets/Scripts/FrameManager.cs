using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

public class FrameManager : Singleton<FrameManager>
{
    [SerializeField] private FrameDataUI _frameDataUI;

    // time passed in seconds
    private float _elapsedTime = 0;
    // time passed in frames
    private uint _elapsedFrames = 0;

    private uint _p1EndFrame = 0;
    private uint _p2EndFrame = 0;
    private FrameActionData _p1EndFrameData;

    private List<FrameActionData> _dataList = new List<FrameActionData>();
    private Dictionary<uint, List<FrameActionData>> _playersActionFrames = new Dictionary<uint, List<FrameActionData>>();
    public struct FrameActionData
    {
        public int PlayerID { get; set; }
        public EPlayerState PlayerState { get; set; }
        public uint StateFrame { get; set; }
        public bool IsHitting { get; set; }
    }

    public FrameDataUI FrameDataUI
    {
        get { return _frameDataUI; }
    }
    public uint ElapsedFrames
    {
        get { return _elapsedFrames; }
    }
    public Dictionary<uint, List<FrameActionData>> PlayersActionFrames
    {
        get { return _playersActionFrames; }
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
        // every 1/60 of a second, a frame passed
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
    }

    // Add the data of the current frame to the dictionary
    public void AddActionFrameData(FrameActionData newData)
    {
        // There's already data for the current frame
        if (_playersActionFrames.ContainsKey(_elapsedFrames))
        {
            _playersActionFrames[_elapsedFrames].Add(newData); //New data has been added into the list
        }
        else
        { // The current frame has no data
            _playersActionFrames.Add(_elapsedFrames, new List<FrameActionData>() { newData });
        }
    }

    // Remove the earliest frame data
    public void RemoveActionFrameData()
    {
        // There 1000 frames registered
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

    public uint GetEndFrameAttack()
    {
        // if there is frames that as been recorded and there is a recorded current frame about the player 1 which the player 1 is in idle and was attacking the frame before
        if (_playersActionFrames.Count > 0 && _playersActionFrames[_elapsedFrames].Where(data => data.PlayerID == 1 && data.PlayerState == EPlayerState.IDLE).Any() && _playersActionFrames[_elapsedFrames - 1].Where(data => data.PlayerID == 1 && data.PlayerState == EPlayerState.MELEE && data.IsHitting).Any())
        {
            // Get the state frame of the player 1 from the frame before
            if (_playersActionFrames[_elapsedFrames - 1][0].PlayerID == 1)
            {
                _p1EndFrame = _playersActionFrames[_elapsedFrames - 1][0].StateFrame;
            }
            else
            {
                _p1EndFrame = _playersActionFrames[_elapsedFrames - 1][1].StateFrame;
            }
        }
        return _p1EndFrame;
    }

    public uint GetEndFrameHurt()
    {
        // if there is frames that as been recorded and there is a recorded current frame about the player 2 which the player 2 is in idle and was hurt the frame before
        if (_playersActionFrames.Count > 0 && _playersActionFrames[_elapsedFrames].Where(data => data.PlayerID == 2 && data.PlayerState == EPlayerState.IDLE).Any() && _playersActionFrames[_elapsedFrames - 1].Where(data => data.PlayerID == 2 && data.PlayerState == EPlayerState.HURT).Any())
        {
            // Get the state frame of the player 2 from the frame before
            if (_playersActionFrames[_elapsedFrames - 1][0].PlayerID == 2)
            {
                _p2EndFrame = _playersActionFrames[_elapsedFrames - 1][0].StateFrame;
            }
            else
            {
                _p2EndFrame = _playersActionFrames[_elapsedFrames - 1][1].StateFrame;
            }
        }
        return _p2EndFrame;
    }
}
