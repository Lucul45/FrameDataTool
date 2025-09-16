using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

public class FrameManager : Singleton<FrameManager>
{
    [SerializeField] private FrameDataUI _frameDataUI;

    /// <summary>
    /// Time passed in seconds
    /// </summary>
    private float _elapsedTime = 0;
    /// <summary>
    /// Time passed in frames
    /// </summary>
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
    /// <summary>
    /// Time passed in seconds
    /// </summary>
    public uint ElapsedFrames
    {
        get { return _elapsedFrames; }
    }

    public Dictionary<uint, List<FrameActionData>> PlayersActionFrames
    {
        get { return _playersActionFrames; }
    }

    private event Action _frameUpdate = null;
    /// <summary>
    /// Update that is called once every frame
    /// </summary>
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

    /// <summary>
    /// Add the data of the current frame to the dictionary
    /// </summary>
    /// <param name="newData"></param>
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

    /// <summary>
    /// Remove the earliest frame data
    /// </summary>
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
}
