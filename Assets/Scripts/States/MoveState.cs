using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : APlayerState
{
    public override void Enter()
    {
        StateFrame = 0;
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
        StateFrame++;
        // if we don't move, change to idle
        if (_stateManager.MovementInput.x == 0)
        {
            _stateManager.ChangeState(EPlayerState.IDLE);
        }

        _stateManager.Move(_stateManager.MovementInput);
    }

    private void Attack()
    {
        if (_stateManager.CanAttack)
        {
            _stateManager.ChangeState(EPlayerState.MELEE);
        }
    }
}
