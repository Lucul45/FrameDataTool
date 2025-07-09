using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "AttackData")]
public class AttackData : ScriptableObject
{
    [SerializeField] private int _attackID;
    [SerializeField] private AnimationClip _clip;
    [SerializeField] private string _animationName;
    [SerializeField] private string _animatorCondition;
    [SerializeField] private int _attackDamage;
    [SerializeField] private int _attackStartup;
    [SerializeField] private int _attackCooldown;
    [SerializeField] private float _hitStun;
    [SerializeField] private float _knockbackForce;
    [SerializeField] private Sprite[] _canComboFrames;
    [SerializeField] private Sprite _endFrame;

    public int AttackID
    {
        get { return _attackID; }
    }
    public AnimationClip Clip
    {
        get { return _clip; }
    }
    public string AnimationName
    {
        get { return _animationName; }
    }
    public string AnimatorCondition
    {
        get { return _animatorCondition; }
    }
    public int AttackDamage
    {
        get { return _attackDamage; }
    }
    public int AttackStartup
    {
        get { return _attackStartup; }
    }
    public int AttackCooldown
    {
        get { return _attackCooldown; }
    }
    public float HitStun
    {
        get { return _hitStun; }
    }
    public float KnockbackForce
    {
        get { return _knockbackForce; }
    }
    public Sprite[] CanComboFrames
    {
        get { return _canComboFrames; }
    }
    public Sprite EndFrame
    {
        get { return _endFrame; }
    }
}
