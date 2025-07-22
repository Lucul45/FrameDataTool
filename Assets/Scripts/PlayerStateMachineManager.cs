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
    [Header("Player ID")]
    [SerializeField] private int _playerID;

    [Header("States")]
    private Dictionary<EPlayerState, APlayerState> _states = null;
    private EPlayerState _currentState;
    private EPlayerState _lastState;

    [Header("Refs")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AttackData[] _attacksData;
    [SerializeField] private GameObject _hitbox;
    [SerializeField] private GameObject _otherPlayer;

    private uint _stateFrame = 0;
    private float _fixedTime = 0f;

    private uint _lastAttackToIdleFrame = 0;
    private uint _lastHurtToIdleFrame = 0;

    [SerializeField] private float _playerSpeed = 10f;
    private Vector2 _movementInput = Vector2.zero;
    private bool _canMove = true;

    private AttackData _currentAttack = null;
    private bool _canAttack = true;
    private int _attackIndex = 1;
    private bool _shouldCombo = false;

    private bool _isHitting = false;

    private bool _isStunned = false;

    private PlayerControls _controls;

    private Dictionary<EPlayerState, uint> _stateOnFrame = new Dictionary<EPlayerState, uint>();
    #endregion Attributs

    #region Events
    private event Action _attackPressed = null;
    public event Action AttackPressed
    {
        add 
        {
            _attackPressed -= value;
            _attackPressed += value; 
        }
        remove { _attackPressed -= value; }
    }
    #endregion Events

    #region Properties
    public int PlayerID
    {
        get { return _playerID; }
    }
    public APlayerState CurrentState
    {
        get
        {
            return _states[_currentState];
        }
    }
    public EPlayerState EnumCurrentState
    {
        get
        {
            return _currentState;
        }
    }
    public EPlayerState LastState
    {
        get { return _lastState; }
    }
    public Animator Animator
    {
        get { return _animator; }
    }
    public AttackData[] AttacksData
    {
        get
        {
            return _attacksData;
        }
    }
    public GameObject Hitbox
    {
        get { return _hitbox; }
    }
    public GameObject OtherPlayer
    {
        get { return _otherPlayer; }
    }
    public uint StateFrame
    {
        get { return _stateFrame; }
        set {  _stateFrame = value; }
    }
    public uint LastAttackToIdleFrame
    {
        get { return _lastAttackToIdleFrame; }
    }
    public uint LastHurtToIdleFrame
    {
        get { return _lastHurtToIdleFrame; }
    }
    public Vector2 MovementInput
    {
        get { return _movementInput; }
    }
    public bool CanMove
    {
        get { return _canMove; }
    }
    public AttackData CurrentAttack
    {
        get { return _currentAttack; }
        set 
        { 
            _currentAttack = value;
        }
    }
    public bool CanAttack
    {
        get { return _canAttack; }
        set { _canAttack = value; }
    }
    public int AttackIndex
    {
        get { return _attackIndex; }
        set { _attackIndex = value; }
    }
    public bool ShouldCombo
    {
        get { return _shouldCombo; }
        set { _shouldCombo = value; }
    }
    public bool IsHitting
    {
        get { return _isHitting; }
        set { _isHitting = value; }
    }
    public bool IsStunned
    {
        get { return _isStunned; }
    }
    public float FixedTime
    {
        get { return _fixedTime; }
        set { _fixedTime = value; }
    }
    public Dictionary<EPlayerState, uint> StateOnFrame
    {
        get { return _stateOnFrame; }
    }
    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {
        FrameManager.Instance.FrameUpdate += UpdateOnFrame;
        _states = new Dictionary<EPlayerState, APlayerState>();
        _states.Add(EPlayerState.IDLE, new IdleState());
        _states.Add(EPlayerState.MOVE, new MoveState());
        _states.Add(EPlayerState.MELEE, new MeleeBaseState());
        _states.Add(EPlayerState.HURT, new HurtState());
        foreach (KeyValuePair<EPlayerState, APlayerState> state in _states)
        {
            state.Value.Init(this, _animator, _spriteRenderer, _rb);
        }
        _currentState = EPlayerState.IDLE;
        CurrentState.Enter();
    }

    // UpdateOnFrame is called once per frame
    public void UpdateOnFrame()
    {
        FrameActionData data = new FrameActionData()
        {
            PlayerID = _playerID,
            PlayerState = _currentState,
            StateFrame = _stateFrame,
            IsHitting = _isHitting
        };
        FixedTime += Time.deltaTime;
        CurrentState.Update();
        _animator.SetFloat("Speed", _rb.velocity.x);
        FrameManager.Instance.AddActionFrameData(data);
        FrameManager.Instance.RemoveActionFrameData();
    }

    // Change the state of the state machine and store on which frame it does
    public void ChangeState(EPlayerState nextState)
    {
        UnityEngine.Debug.Log("Transition from " + CurrentState + " To " + nextState);
        CurrentState.Exit();
        _stateOnFrame.Clear();
        _lastState = _currentState;
        if (_currentState == EPlayerState.MELEE && nextState == EPlayerState.IDLE)
        {
            _lastAttackToIdleFrame = FrameManager.Instance.ElapsedFrames;
            UnityEngine.Debug.Log($"Joueur {gameObject.name} (Attack) -> IDLE à la frame : {_lastAttackToIdleFrame}");
        }
        if (_currentState == EPlayerState.HURT && nextState == EPlayerState.IDLE)
        {
            _lastHurtToIdleFrame = FrameManager.Instance.ElapsedFrames;
            UnityEngine.Debug.Log($"Joueur {gameObject.name} (Hurt) -> IDLE à la frame : {_lastHurtToIdleFrame}");
        }
        _currentState = nextState;
        _stateOnFrame.Add(_currentState, FrameManager.Instance.ElapsedFrames);
        CurrentState.Enter();
    }

    public void GetMovementInput(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void GetAttackInput(InputAction.CallbackContext context)
    {
        if (_attackPressed != null && context.started)
        {
            _attackPressed();
        }
        
    }

    public void Move(Vector2 dir)
    {
        _rb.velocity = new Vector2(dir.x * _playerSpeed, 0);
    }

    // Push the player backward
    public void Knockback(float force, int duration)
    {
        uint startFrame = FrameManager.Instance.ElapsedFrames;
        uint time = 0;
        while (time < duration)
        {
            time = FrameManager.Instance.ElapsedFrames - startFrame;
            _rb.velocity = -transform.right * force;
        }
    }

    // Reset the combo counter
    public void ResetCombo()
    {
        _attackIndex = 1;
        _shouldCombo = false;
        _animator.SetBool("IsAttacking1", false);
        _animator.SetBool("IsAttacking2", false);
        _animator.SetBool("IsAttacking3", false);
    }

    public void ResetLastAttackToIdleFrame()
    {
        _lastAttackToIdleFrame = 0;
    }

    public void ResetLastHurtToIdleFrame()
    {
        _lastHurtToIdleFrame = 0;
    }

    // if we collide with an attack, change to hurt state
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<PlayerStateMachineManager>() != null)
        {
            if (collision.GetComponentInParent<PlayerStateMachineManager>().CurrentAttack != null)
            {
                ChangeState(EPlayerState.HURT);
                CurrentState.AttackHitten = collision.GetComponentInParent<PlayerStateMachineManager>().CurrentAttack;
            }
        }
    }
}
