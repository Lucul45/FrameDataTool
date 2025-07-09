using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : APlayerState
{
    private float _fixedTime = 0;
    public override void Enter()
    {
        _stateManager.ResetCombo();
        _stateManager.Knockback(_stateManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().CurrentAttack.KnockbackForce, 0.5f);
        _animator.SetBool("IsHurt", true);
        _fixedTime = 0;
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
        _fixedTime += Time.deltaTime;
        if (!_stateManager.IsStunned && _fixedTime > _stateManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().CurrentAttack.HitStun)
        {
            _stateManager.ChangeState(EPlayerState.IDLE);
        }
    }
}
