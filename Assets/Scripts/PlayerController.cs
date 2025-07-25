using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player ID")]
    [SerializeField] private int _playerID;

    [Header("Refs")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AttackData[] _attacksData;

    [SerializeField] private float _playerSpeed = 10f;
    private Vector2 _movementInput = Vector2.zero;

    private AttackData _currentAttack = null;
    private bool _canAttack = true;
    private int _attackIndex = 1;
    private bool _shouldCombo = false;

    private bool _isHitting = false;

    public int PlayerID
    {
        get { return _playerID; }
    }
    public Animator Animator
    {
        get { return _animator; }
    }
    public Rigidbody2D Rb
    {
        get { return _rb; }
    }
    public SpriteRenderer SpriteRenderer
    {
        get { return _spriteRenderer; }
    }
    public AttackData[] AttacksData
    {
        get
        {
            return _attacksData;
        }
    }
    public Vector2 MovementInput
    {
        get { return _movementInput; }
    }
    public AttackData CurrentAttack
    {
        get { return _currentAttack; }
        set
        {
            _currentAttack = value;
        }
    }
    public bool CanAttack
    {
        get { return _canAttack; }
        set { _canAttack = value; }
    }
    public int AttackIndex
    {
        get { return _attackIndex; }
        set { _attackIndex = value; }
    }
    public bool ShouldCombo
    {
        get { return _shouldCombo; }
        set { _shouldCombo = value; }
    }
    public bool IsHitting
    {
        get { return _isHitting; }
        set { _isHitting = value; }
    }

    #region Events
    private event Action _attackPressed = null;
    public event Action AttackPressed
    {
        add
        {
            _attackPressed -= value;
            _attackPressed += value;
        }
        remove { _attackPressed -= value; }
    }
    #endregion Events

    public void GetMovementInput(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void GetAttackInput(InputAction.CallbackContext context)
    {
        if (_attackPressed != null && context.started)
        {
            _attackPressed();
        }
    }

    public void Move(Vector2 dir)
    {
        _rb.velocity = new Vector2(dir.x * _playerSpeed, 0);
    }

    // Push the player backward
    public void Knockback(float force, int duration)
    {
        uint startFrame = FrameManager.Instance.ElapsedFrames;
        uint time = 0;
        while (time < duration)
        {
            time = FrameManager.Instance.ElapsedFrames - startFrame;
            _rb.velocity = -transform.right * force;
        }
    }

    // Reset the combo counter
    public void ResetCombo()
    {
        _attackIndex = 1;
        _shouldCombo = false;
        _animator.SetBool("IsAttacking1", false);
        _animator.SetBool("IsAttacking2", false);
        _animator.SetBool("IsAttacking3", false);
    }

    // if we collide with an attack, change to hurt state
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<PlayerController>() != null)
        {
            Debug.Log("HIT");
            if (collision.GetComponentInParent<PlayerController>().PlayerID == 1)
            {
                PlayerStateMachineManager.Instance.ChangeStateP2(EPlayerState.HURT);
                PlayerStateMachineManager.Instance.CurrentStateP2.AttackHitten = collision.GetComponentInParent<PlayerController>().CurrentAttack;
            }
            else
            {
                PlayerStateMachineManager.Instance.ChangeStateP1(EPlayerState.HURT);
                PlayerStateMachineManager.Instance.CurrentStateP1.AttackHitten = collision.GetComponentInParent<PlayerController>().CurrentAttack;
            }
        }
    }*/
}
