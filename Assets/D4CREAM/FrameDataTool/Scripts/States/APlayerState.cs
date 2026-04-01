using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class APlayerState
{
    protected PlayerController _opponent;
    protected PlayerStateMachineManager _stateManager;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    protected Rigidbody2D _rb;
    protected PlayerController _playerController;

    protected uint _stateFrame = 0;
    protected AttackData _attackHitten;

    /// <summary>
    /// The number of frames during which the player remained in the current state.
    /// </summary>
    public uint StateFrame
    {
        get {  return _stateFrame; }
        set 
        {  
            _stateFrame = value;
        }
    }
    /// <summary>
    /// The data of the attack the 
    /// </summary>
    public AttackData AttackHitten
    {
        get { return _attackHitten; }
        set {  _attackHitten = value; }
    }

    public abstract void Init(PlayerController opponent, PlayerStateMachineManager stateManager, Animator animator, SpriteRenderer spriteRenderer, Rigidbody2D rb, PlayerController _playerController);

    public virtual void Enter()
    {
        StateFrame = 0;
    }

    public virtual void Update()
    {
        StateFrame++;
    }

    public abstract void Exit();
}
