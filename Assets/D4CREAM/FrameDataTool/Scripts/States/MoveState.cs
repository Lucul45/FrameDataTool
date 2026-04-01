using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MoveState : APlayerState
{
    public override void Enter()
    {
        base.Enter();
        _playerController.AttackPressed += Attack;
    }

    public override void Exit()
    {
        _playerController.AttackPressed -= Attack;
    }

    public override void Init(PlayerController opponent, PlayerStateMachineManager stateManager, Animator animator, SpriteRenderer spriteRenderer, Rigidbody2D rb, PlayerController playerController)
    {
        _opponent = opponent;
        _stateManager = stateManager;
        _animator = animator;
        _spriteRenderer = spriteRenderer;
        _rb = rb;
        _playerController = playerController;
    }

    public override void Update()
    {
        base.Update();
        // if we don't move, change to idle
        if (_playerController.MovementInput.x == 0)
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.IDLE);
        }

        _playerController.Move(_playerController.MovementInput);
    }

    private void Attack()
    {
        if (_playerController.CanAttack)
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.MELEE);
        }
    }
}
