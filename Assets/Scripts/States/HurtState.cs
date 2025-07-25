using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class HurtState : APlayerState
{
    private uint _hitAttackFrame = 0;
    public override void Enter()
    {
        if (_playerController.PlayerID == 1)
        {
            StateFrameP1 = 0;
        }
        else
        {
            StateFrameP2 = 0;
        }
        if (FrameManager.Instance.PlayersActionFrames[FrameManager.Instance.ElapsedFrames][0].PlayerID != _playerController.PlayerID)
        {
            _hitAttackFrame = FrameManager.Instance.PlayersActionFrames[FrameManager.Instance.ElapsedFrames][0].StateFrame;
        }
        else
        {
            _hitAttackFrame = FrameManager.Instance.PlayersActionFrames[FrameManager.Instance.ElapsedFrames][1].StateFrame;
        }
        FrameManager.Instance.FrameDataUI.ResetAdvantageCalculated();
        _playerController.ResetCombo();
        //_stateManager.Knockback(AttackHitten.KnockbackForce, 5);
        _animator.SetBool("IsHurt", true);
    }

    public override void Exit()
    {
        _animator.SetBool("IsHurt", false);
    }

    public override void Init(PlayerStateMachineManager stateManager, Animator animator, SpriteRenderer spriteRenderer, Rigidbody2D rb, PlayerController playerController)
    {
        _stateManager = stateManager;
        _animator = animator;
        _spriteRenderer = spriteRenderer;
        _rb = rb;
        _playerController = playerController;
    }

    public override void Update()
    {
        if (_playerController.PlayerID == 1)
        {
            StateFrameP1++;
            // If the frame on the current is greater or equal than hitstun, then change state to idle
            if (StateFrameP1 >= (int)(AttackHitten.Clip.length * 60) - _hitAttackFrame + AttackHitten.AdvantageFrames)
            {
                _stateManager.ChangeStateP1(EPlayerState.IDLE);
            }
        }
        else
        {
            StateFrameP2++;
            // If the frame on the current is greater or equal than hitstun, then change state to idle
            if (StateFrameP2 >= (int)(AttackHitten.Clip.length * 60) - _hitAttackFrame + AttackHitten.AdvantageFrames)
            {
                _stateManager.ChangeStateP2(EPlayerState.IDLE);
            }
        }
    }
}
