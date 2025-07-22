using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class APlayerState
{
    protected PlayerStateMachineManager _stateManager;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    protected Rigidbody2D _rb;

    protected uint _stateFrame = 0;
    protected AttackData _attackHitten;

    protected uint StateFrame
    {
        get {  return _stateFrame; }
        set 
        {  
            _stateFrame = value;
            _stateManager.StateFrame = _stateFrame;
        }
    }
    public AttackData AttackHitten
    {
        get { return _attackHitten; }
        set {  _attackHitten = value; }
    }

    public abstract void Init(PlayerStateMachineManager stateManager, Animator animator, SpriteRenderer spriteRenderer, Rigidbody2D rb);

    public abstract void Enter();

    public abstract void Update();

    public abstract void Exit();
}
