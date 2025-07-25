using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FrameDataUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _startupFrameText;
    [SerializeField] private TextMeshProUGUI _activeFrameText;
    [SerializeField] private TextMeshProUGUI _cooldownFrameText;
    [SerializeField] private TextMeshProUGUI _advantageFrameText;
    [SerializeField] private PlayerStateMachineManager _playerStateMachineManager;
    [SerializeField] private PlayerController _p1;
    [SerializeField] private PlayerController _p2;

    private int _p1EndFrame = 0;
    private int _p2EndFrame = 0;
    private bool _advantageCalculated = false;
    
    // Change the frame data UI
    public void ChangeFrameDataUI()
    {
        if (_playerStateMachineManager.EnumCurrentStateP1 == EPlayerState.MELEE)
        {
            // Simply get the current attack data and show it in UI
            if (_p1.CurrentAttack != null)
            {
                _startupFrameText.text = "Start up frames : " + _p1.CurrentAttack.AttackStartup;
                _activeFrameText.text = "Active frames : " + (_p1.CurrentAttack.AttackStartup + 1) + "-" + (_p1.CurrentAttack.AttackCooldown - 1);
                _cooldownFrameText.text = "Cooldown frames : " + _p1.CurrentAttack.AttackCooldown;
            }
        }
        // Getting the last frames of the attack and of the hurting state
        _p1EndFrame = (int)_playerStateMachineManager.LastAttackToIdleFrameP1;
        _p2EndFrame = (int)_playerStateMachineManager.LastHurtToIdleFrameP2;
        // security to avoid errors and making sure both end frames are updated
        if (_p1EndFrame > 0 && _p2EndFrame > 0 && !_advantageCalculated)
        {
            ShowFrameAdvantage();
        }
    }

    private void Start()
    {
        // Adding the method to the update
        FrameManager.Instance.FrameUpdate += ChangeFrameDataUI;
    }

    // Show the frame advantage in the UI and reset the last frames of the attack and hurting state
    private void ShowFrameAdvantage()
    {
        _advantageFrameText.text = "Advantage frames : " + (_p2EndFrame - _p1EndFrame);
        _advantageCalculated = true;
        _playerStateMachineManager.ResetLastAttackToIdleFrameP1();
        _playerStateMachineManager.ResetLastHurtToIdleFrameP2();
    }

    public void ResetAdvantageCalculated()
    {
        _advantageCalculated = false;
    }
}
