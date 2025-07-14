using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class HurtState : APlayerState
{
    private float _enterFrame = 0;
    private AttackData _attackHitten;
    private AnimatorStateInfo _otherCurrentStateInfo;
    public override void Enter()
    {
        _stateManager.ResetCombo();
        //_stateManager.Knockback(_stateManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().CurrentAttack.KnockbackForce, 5);
        _animator.SetBool("IsHurt", true);
        _enterFrame = FrameManager.Instance.ElapsedFrames;
        _attackHitten = _stateManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().CurrentAttack;
        _otherCurrentStateInfo = _stateManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().Animator.GetCurrentAnimatorStateInfo(0);
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
        //Debug.Log(((int)((_stateManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().CurrentAttack.Clip.length - (_stateManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().Animator.GetCurrentAnimatorStateInfo(0).normalizedTime) * 60)) + _stateManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().CurrentAttack.AdvantageFrames + 1));
        Debug.Log(_otherCurrentStateInfo.normalizedTime * _attackHitten.Clip.length);
        // If the character isn't stunned and was hurt more frames than the hitstun of the attack he was hurted with
        if (!_stateManager.IsStunned && (FrameManager.Instance.ElapsedFrames - _enterFrame) > ((int)(_attackHitten.Clip.length * 60)) + _attackHitten.AdvantageFrames + 1)
        {
            _stateManager.ChangeState(EPlayerState.IDLE);
        }
    }
}
