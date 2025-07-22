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
        //Debug.Log(StateFrame);
        //Debug.Log(((int)((_stateManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().CurrentAttack.Clip.length - (_stateManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().Animator.GetCurrentAnimatorStateInfo(0).normalizedTime) * 60)) + _stateManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().CurrentAttack.AdvantageFrames + 1));
        //Debug.Log(_otherCurrentStateInfo.normalizedTime * _attackHitten.Clip.length);
        // If the character isn't stunned and was hurt more frames than the hitstun of the attack he was hurted with
        //(FrameManager.Instance.ElapsedFrames - _enterFrame) > ((int)(_attackHitten.Clip.length * 60)) + _attackHitten.AdvantageFrames + 1
        if (!_stateManager.IsStunned && StateFrame == (int)(AttackHitten.Clip.length * 60) - _hitAttackFrame + _attackHitten.AdvantageFrames)
        {
            _stateManager.ChangeState(EPlayerState.IDLE);
        }
    }
}
