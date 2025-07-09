using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : APlayerState
{
    public override void Enter()
    {
        _stateManager.AttackPressed += Attack;
    }

    public override void Exit()
    {
        _stateManager.AttackPressed -= Attack;
    }

    public override void Init(PlayerStateMachineManager stateManager, Animator animator, SpriteRenderer spriteRenderer, Rigidbody2D rb)
    {
        _stateManager = stateManager;
        _animator = animator;
        _spriteRenderer = spriteRenderer;
        _rb = rb;
    }

    public override void Update()
    {
        if (_stateManager.MovementInput.x != 0f && _stateManager.CanMove)
        {
            _stateManager.ChangeState(EPlayerState.MOVE);
        }
    }

    private void Attack()
    {
        if(_stateManager.CanAttack)
        {
            _stateManager.ChangeState(EPlayerState.MELEE);
        }
    }
}
