using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class HurtState : APlayerState
{
    private int _hitAttackFrame = 0;
    public override void Enter()
    {
        base.Enter();

        if (_opponent.PlayerID == 1)
        {
            _hitAttackFrame = (int)PlayerStateMachineManager.Instance.CurrentStateP1.StateFrame;
        }
        else
        {
            _hitAttackFrame = (int)PlayerStateMachineManager.Instance.CurrentStateP2.StateFrame;
        }

        FrameManager.Instance.FrameDataUI.ResetAdvantageCalculated();
        _playerController.ResetCombo();
        _animator.SetBool("IsHurt", true);
    }

    public override void Exit()
    {
        _animator.SetBool("IsHurt", false);
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

        int hitstunDuration = _opponent.CurrentAttack.AttackTotalTime - _hitAttackFrame + _opponent.CurrentAttack.AdvantageFrames;

        if (StateFrame >= hitstunDuration)
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.IDLE);
        }
    }
}
