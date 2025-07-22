using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class HurtState : APlayerState
{
    private uint _hitAttackFrame = 0;
    public override void Enter()
    {
        StateFrame = 0;
        if (FrameManager.Instance.PlayersActionFrames[FrameManager.Instance.ElapsedFrames][0].PlayerID != _stateManager.PlayerID)
        {
            _hitAttackFrame = FrameManager.Instance.PlayersActionFrames[FrameManager.Instance.ElapsedFrames][0].StateFrame;
        }
        else
        {
            _hitAttackFrame = FrameManager.Instance.PlayersActionFrames[FrameManager.Instance.ElapsedFrames][1].StateFrame;
        }
        FrameDataManager.Instance.ResetAdvantageCalculated();
        _stateManager.ResetCombo();
        //_stateManager.Knockback(_stateManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().CurrentAttack.KnockbackForce, 5);
        _animator.SetBool("IsHurt", true);
    }

    public override void Exit()
    {
        _animator.SetBool("IsHurt", false);
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
        // If the frame on the current is greater or equal than hitstun, then change state to idle
        if (StateFrame >= (int)(AttackHitten.Clip.length * 60) - _hitAttackFrame + _attackHitten.AdvantageFrames)
        {
            _stateManager.ChangeState(EPlayerState.IDLE);
        }
    }
}
