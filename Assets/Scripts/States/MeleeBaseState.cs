using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeBaseState : APlayerState
{
    private int _currentAttackFrame = 0;

    public override void Enter()
    {
        // Getting the current attack based on which index we are on
        foreach (AttackData a in _stateManager.AttacksData)
        {
            if (a.AttackID == _stateManager.AttackIndex)
            {
                _stateManager.CurrentAttack = a;
            }
        }
        _animator.SetBool(_stateManager.CurrentAttack.AnimatorCondition, true);
        _stateManager.AttackPressed += Attack;
    }

    public override void Exit()
    {
        _stateManager.IsHitting = false;
        _stateManager.AttackPressed -= Attack;
    }

    public override void Init(PlayerStateMachineManager stateManager, Animator animator, SpriteRenderer spriteRenderer, Rigidbody2D rb)
    {
        _stateManager = stateManager;
        _animator = animator;
        _spriteRenderer = spriteRenderer;
        _rb = rb;
        _stateManager.ShouldCombo = false;
        _stateManager.AttackIndex = 1;
    }

    public override void Update()
    {
        // Calculate which frame the current attack is on
        _currentAttackFrame = Convert.ToInt32(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime * (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * _animator.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate));
        // Making sure the character can't move while attacking
        _stateManager.Move(Vector2.zero);
        // If the character attack is on a frame where he can combo
        if (_stateManager.CurrentAttack.CanComboFrames.Contains<Sprite>(_spriteRenderer.sprite))
        {
            _stateManager.ShouldCombo = true;
        }
        else
        {
            _stateManager.ShouldCombo = false;
        }
        // If the character finished his attack animation
        if (_animator.GetBool(_stateManager.CurrentAttack.AnimatorCondition) && _animator.GetCurrentAnimatorStateInfo(0).IsName(_stateManager.CurrentAttack.AnimationName))
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !_animator.IsInTransition(0))
            {
                _stateManager.ResetCombo();
                _stateManager.ChangeState(EPlayerState.IDLE);
            }
        }
    }


    private void Attack()
    {
        if (_stateManager.ShouldCombo && _stateManager.AttackIndex < 3)
        {
            _stateManager.AttackIndex++;
            _stateManager.ChangeState(EPlayerState.MELEE);
        }
    }
}
