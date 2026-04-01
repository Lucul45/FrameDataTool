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
    [SerializeField] private int _attackTotalTime;
    [SerializeField] private int _attackStartup;
    [SerializeField] private int _attackRecovery;
    [SerializeField] private float _advantageFrames;
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
    public int AttackTotalTime
    {
        get { return _attackTotalTime; }
    }
    public int AttackStartup
    {
        get { return _attackStartup; }
    }
    public int AttackRecovery
    {
        get { return _attackRecovery; }
    }
    public float AdvantageFrames
    {
        get { return _advantageFrames; }
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

    private void OnValidate()
    {
        if (AttackID < 0)
        {
            Debug.LogWarning(name + " Negative Attack ID !");
        }
        if (Clip == null)
        {
            Debug.LogWarning(name + " No Animation clip !");
        }
        if (AnimationName == string.Empty)
        {
            Debug.LogWarning(name + " No Animation name !");
        }
        if (AnimatorCondition == string.Empty)
        {
            Debug.LogWarning(name + " No Animator condition !");
        }
        if (AttackDamage < 0)
        {
            Debug.LogWarning(name + " Negative Attack Damage !");
        }
        if (AttackTotalTime != (int)(Clip.length * 60))
        {
            Debug.LogWarning(name + " Attack total time is not equal to clip length !");
        }
        if (AttackTotalTime < 0)
        {
            Debug.LogWarning(name + " Negative Attack total time !");
        }
        if (AttackStartup < 0)
        {
            Debug.LogWarning(name + " Negative Attack startup !");
        }
        if (AttackRecovery < 0)
        {
            Debug.LogWarning(name + " Negative Attack recovery !");
        }
    }
}
