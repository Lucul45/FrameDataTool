using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    [SerializeField] private int _advantageFrames;
    [SerializeField] private float _knockbackForce;
    [SerializeField] private int[] _canComboFrames = new int[2];

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
    public int AdvantageFrames
    {
        get { return _advantageFrames; }
    }
    public float KnockbackForce
    {
        get { return _knockbackForce; }
    }
    public int[] CanComboFrames
    {
        get { return _canComboFrames; }
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
        if (CanComboFrames[0] < 0)
        {
            Debug.LogWarning(name + " Negative Combo Frames start !");
        }
        if (CanComboFrames[1] < 0)
        {
            Debug.LogWarning(name + " Negative Combo Frames end !");
        }
        if (CanComboFrames[0] > CanComboFrames[1])
        {
            Debug.LogWarning(name + " Combo Frames start is superior to end !");
        }

        AddEventsToClip(Clip, (float)(AttackStartup + 1) / 60, "Attack startup", (float)(AttackTotalTime - AttackRecovery) / 60, "Attack recovery");
    }

    public void AddEventsToClip(AnimationClip clip, float timeInSeconds, string functionName, float timeInSeconds2, string functionName2)
    {
        AnimationEvent[] animationEvents = new AnimationEvent[2];

        AnimationEvent animEvent = new AnimationEvent();
        AnimationEvent animEvent2 = new AnimationEvent();
        animEvent.time = timeInSeconds;
        animEvent2.time = timeInSeconds2;

        animationEvents.SetValue(animEvent, 0);
        animationEvents.SetValue(animEvent2, 1);

        // Event name displayed
        animationEvents[0].functionName = functionName;
        animationEvents[1].functionName = functionName2;

        AnimationUtility.SetAnimationEvents(clip, animationEvents);

        Debug.Log($"Event '{functionName}' add to {timeInSeconds}s on the {clip.name} clip");
    }
}
