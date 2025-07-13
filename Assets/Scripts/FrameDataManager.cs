using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FrameDataManager : Singleton<FrameDataManager>
{
    [SerializeField] private TextMeshProUGUI _startupFrameText;
    [SerializeField] private TextMeshProUGUI _activeFrameText;
    [SerializeField] private TextMeshProUGUI _cooldownFrameText;
    [SerializeField] private TextMeshProUGUI _advantageFrameText;
    [SerializeField] private PlayerStateMachineManager _stateMachineManager;

    private int _p1EndFrame = 0;
    private int _p2EndFrame = 0;
    
    // Change the frame data UI
    public void ChangeFrameDataUI()
    {
        if (_stateMachineManager.EnumCurrentState == EPlayerState.MELEE)
        {
            // Simply get the current attack data and show it in UI
            if (_stateMachineManager.CurrentAttack != null)
            {
                _startupFrameText.text = "Start up frames : " + _stateMachineManager.CurrentAttack.AttackStartup;
                _activeFrameText.text = "Active frames : " + (_stateMachineManager.CurrentAttack.AttackStartup + 1) + "-" + (_stateMachineManager.CurrentAttack.AttackCooldown - 1);
                _cooldownFrameText.text = "Cooldown frames : " + _stateMachineManager.CurrentAttack.AttackCooldown;
            }
        }
        // Getting the last frames of the attack and of the hurting state
        if (FrameManager.Instance.GetEndFrameAttack() != 0)
        {
            _p1EndFrame = FrameManager.Instance.GetEndFrameAttack();
        }
        if (FrameManager.Instance.GetEndFrameHurt() != 0)
        {
            _p2EndFrame = FrameManager.Instance.GetEndFrameHurt();
        }
        // security to avoid errors
        if (_p1EndFrame > 0 && _p2EndFrame > 0)
        {
            Debug.Log(_p1EndFrame + "/" + _p2EndFrame);
            _advantageFrameText.text = "Advantage frames : " + (_p2EndFrame - _p1EndFrame);
        }
        else
        {
            _advantageFrameText.text = "Advantage frames : --";
        }
    }

    private void Start()
    {
        // Adding the method to the update
        FrameManager.Instance.FrameUpdate += ChangeFrameDataUI;
    }
}
