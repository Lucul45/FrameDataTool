using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using static FrameManager;

public enum EPlayerState
{
    IDLE,
    MOVE,
    MELEE,
    HURT,
    NONE
}

public class PlayerStateMachineManager : Singleton<PlayerStateMachineManager>
{
    #region Attributs
    [Header("Refs")]
    [SerializeField] private PlayerController _player1;
    [SerializeField] private PlayerController _player2;

    [Header("States P1")]
    private Dictionary<EPlayerState, APlayerState> _statesP1 = null;
    private EPlayerState _currentStateP1;
    private EPlayerState _lastStateP1;

    [Header("States P2")]
    private Dictionary<EPlayerState, APlayerState> _statesP2 = null;
    private EPlayerState _currentStateP2;
    private EPlayerState _lastStateP2;

    private uint _stateFrameP1 = 0;
    private uint _stateFrameP2 = 0;

    private uint _lastAttackToIdleFrameP1 = 0;
    private uint _lastHurtToIdleFrameP1 = 0;
    private uint _lastAttackToIdleFrameP2 = 0;
    private uint _lastHurtToIdleFrameP2 = 0;
    #endregion Attributs

    #region Properties
    public APlayerState CurrentStateP1
    {
        get
        {
            return _statesP1[_currentStateP1];
        }
    }
    public EPlayerState EnumCurrentStateP1
    {
        get
        {
            return _currentStateP1;
        }
    }
    public APlayerState CurrentStateP2
    {
        get
        {
            return _statesP2[_currentStateP2];
        }
    }
    public EPlayerState EnumCurrentStateP2
    {
        get
        {
            return _currentStateP2;
        }
    }
    public EPlayerState LastStateP1
    {
        get { return _lastStateP1; }
    }
    public EPlayerState LastStateP2
    {
        get { return _lastStateP2; }
    }
    public uint StateFrameP1
    {
        get { return _stateFrameP1; }
        set {  _stateFrameP1 = value; }
    }
    public uint StateFrameP2
    {
        get { return _stateFrameP2; }
        set { _stateFrameP2 = value; }
    }
    public uint LastAttackToIdleFrameP1
    {
        get { return _lastAttackToIdleFrameP1; }
    }
    public uint LastHurtToIdleFrameP1
    {
        get { return _lastHurtToIdleFrameP1; }
    }
    public uint LastAttackToIdleFrameP2
    {
        get { return _lastAttackToIdleFrameP2; }
    }
    public uint LastHurtToIdleFrameP2
    {
        get { return _lastHurtToIdleFrameP2; }
    }
    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {
        // Plug the update on the frames
        FrameManager.Instance.FrameUpdate += UpdateOnFrame;
        // Initializing the state machine
        _statesP1 = new Dictionary<EPlayerState, APlayerState>();
        _statesP1.Add(EPlayerState.IDLE, new IdleState());
        _statesP1.Add(EPlayerState.MOVE, new MoveState());
        _statesP1.Add(EPlayerState.MELEE, new MeleeBaseState());
        _statesP1.Add(EPlayerState.HURT, new HurtState());

        _statesP2 = new Dictionary<EPlayerState, APlayerState>();
        _statesP2.Add(EPlayerState.IDLE, new IdleState());
        _statesP2.Add(EPlayerState.MOVE, new MoveState());
        _statesP2.Add(EPlayerState.MELEE, new MeleeBaseState());
        _statesP2.Add(EPlayerState.HURT, new HurtState());

        foreach (KeyValuePair<EPlayerState, APlayerState> state in _statesP1)
        {
            state.Value.Init(this, _player1.Animator, _player1.SpriteRenderer, _player1.Rb, _player1);
        }
        foreach (KeyValuePair<EPlayerState, APlayerState> state in _statesP2)
        {
            state.Value.Init(this, _player2.Animator, _player2.SpriteRenderer, _player2.Rb, _player2);
        }
        _currentStateP1 = EPlayerState.IDLE;
        _currentStateP2 = EPlayerState.IDLE;
        CurrentStateP1.Enter();
        CurrentStateP2.Enter();
    }

    // UpdateOnFrame is called once per frame
    public void UpdateOnFrame()
    {
        // Registering the data on each frame
        FrameActionData dataP1 = new FrameActionData()
        {
            PlayerID = _player1.PlayerID,
            PlayerState = _currentStateP1,
            StateFrame = _stateFrameP1,
            IsHitting = _player1.IsHitting
        };
        FrameActionData dataP2 = new FrameActionData()
        {
            PlayerID = _player2.PlayerID,
            PlayerState = _currentStateP2,
            StateFrame = _stateFrameP2,
            IsHitting = _player2.IsHitting
        };
        CurrentStateP1.Update();
        CurrentStateP2.Update();
        _player1.Animator.SetFloat("Speed", _player1.Rb.velocity.x);
        _player2.Animator.SetFloat("Speed", _player2.Rb.velocity.x);
        // Add a new frame data to the dictionary
        FrameManager.Instance.AddActionFrameData(dataP1);
        FrameManager.Instance.AddActionFrameData(dataP2);
        // Remove the earliest frame data of the dictionary
        FrameManager.Instance.RemoveActionFrameData();
    }

    // Change the state of the state machine and store on which frame it does
    public void ChangeStateP1(EPlayerState nextState)
    {
        UnityEngine.Debug.Log("Transition from " + CurrentStateP1 + " To " + nextState);
        CurrentStateP1.Exit();
        _lastStateP1 = _currentStateP1;
        if (_currentStateP1 == EPlayerState.MELEE && nextState == EPlayerState.IDLE)
        {
            _lastAttackToIdleFrameP1 = FrameManager.Instance.ElapsedFrames;
            UnityEngine.Debug.Log($"Joueur {gameObject.name} (Attack) -> IDLE à la frame : {_lastAttackToIdleFrameP1}");
        }
        if (_currentStateP1 == EPlayerState.HURT && nextState == EPlayerState.IDLE)
        {
            _lastHurtToIdleFrameP1 = FrameManager.Instance.ElapsedFrames;
            UnityEngine.Debug.Log($"Joueur {gameObject.name} (Hurt) -> IDLE à la frame : {_lastHurtToIdleFrameP1}");
        }
        _currentStateP1 = nextState;
        CurrentStateP1.Enter();
    }

    public void ChangeStateP2(EPlayerState nextState)
    {
        UnityEngine.Debug.Log("Transition from " + CurrentStateP2 + " To " + nextState);
        CurrentStateP2.Exit();
        _lastStateP2 = _currentStateP2;
        if (_currentStateP2 == EPlayerState.MELEE && nextState == EPlayerState.IDLE)
        {
            _lastAttackToIdleFrameP2 = FrameManager.Instance.ElapsedFrames;
            UnityEngine.Debug.Log($"Joueur {gameObject.name} (Attack) -> IDLE à la frame : {_lastAttackToIdleFrameP2}");
        }
        if (_currentStateP2 == EPlayerState.HURT && nextState == EPlayerState.IDLE)
        {
            _lastHurtToIdleFrameP2 = FrameManager.Instance.ElapsedFrames;
            UnityEngine.Debug.Log($"Joueur {gameObject.name} (Hurt) -> IDLE à la frame : {_lastHurtToIdleFrameP2}");
        }
        _currentStateP2 = nextState;
        CurrentStateP2.Enter();
    }

    public void ResetLastAttackToIdleFrameP1()
    {
        _lastAttackToIdleFrameP1 = 0;
    }

    public void ResetLastHurtToIdleFrameP1()
    {
        _lastHurtToIdleFrameP1 = 0;
    }
    public void ResetLastAttackToIdleFrameP2()
    {
        _lastAttackToIdleFrameP2 = 0;
    }

    public void ResetLastHurtToIdleFrameP2()
    {
        _lastHurtToIdleFrameP2 = 0;
    }
}
