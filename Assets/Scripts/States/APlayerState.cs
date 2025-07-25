using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class APlayerState
{
    protected PlayerStateMachineManager _stateManager;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    protected Rigidbody2D _rb;
    protected PlayerController _playerController;

    protected uint _stateFrameP1 = 0;
    protected uint _stateFrameP2 = 0;
    protected AttackData _attackHitten;

    protected uint StateFrameP1
    {
        get {  return _stateFrameP1; }
        set 
        {  
            _stateFrameP1 = value;
            _stateManager.StateFrameP1 = _stateFrameP1;
        }
    }
    protected uint StateFrameP2
    {
        get { return _stateFrameP2; }
        set
        {
            _stateFrameP2 = value;
            _stateManager.StateFrameP2 = _stateFrameP2;
        }
    }
    public AttackData AttackHitten
    {
        get { return _attackHitten; }
        set {  _attackHitten = value; }
    }

    public abstract void Init(PlayerStateMachineManager stateManager, Animator animator, SpriteRenderer spriteRenderer, Rigidbody2D rb, PlayerController _playerController);

    public abstract void Enter();

    public abstract void Update();

    public abstract void Exit();
}
