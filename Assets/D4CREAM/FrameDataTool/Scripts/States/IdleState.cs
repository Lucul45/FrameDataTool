using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class IdleState : APlayerState
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
        // If the input isn't neutral
        if (_playerController.MovementInput.x != 0f)
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.MOVE);
        }
    }

    private void Attack()
    {
        if (_playerController.CanAttack)
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.MELEE);
        }
    }
}
