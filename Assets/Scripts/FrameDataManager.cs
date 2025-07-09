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
    [SerializeField] private PlayerStateMachineManager _player1;
    [SerializeField] private PlayerStateMachineManager _player2;

    private int _p1EndFrame = 0;
    private int _p2EndFrame = 0;
    
    // Change the frame data UI
    public void ChangeFrameDataUI()
    {
        // Simply get the current attack data and show it in UI
        if (_player1.CurrentAttack != null)
        {
            _startupFrameText.text = "Start up frames : " + _player1.CurrentAttack.AttackStartup;
            _activeFrameText.text = "Active frames : " + (_player1.CurrentAttack.AttackStartup + 1) + "-" + (_player1.CurrentAttack.AttackCooldown - 1);
            _cooldownFrameText.text = "Cooldown frames : " + _player1.CurrentAttack.AttackCooldown;
        }
        // if player 1 transition from an attack to idle and player 2 transition from being hurt to idle
        if (_player1.EnumCurrentState == EPlayerState.IDLE && _player1.LastState == EPlayerState.MELEE && _player2.EnumCurrentState == EPlayerState.IDLE && _player2.LastState == EPlayerState.HURT)
        {
            if ( _player1.StateOnFrame.ContainsKey(EPlayerState.IDLE))
            {
                // getting the frame the transition happened
                _p1EndFrame = _player1.StateOnFrame[EPlayerState.IDLE];
            }
            if (_player2.StateOnFrame.ContainsKey(EPlayerState.IDLE))
            {
                // getting the frame the transition happened
                _p2EndFrame = _player2.StateOnFrame[EPlayerState.IDLE];
            }
        }
        // security to avoid errors
        if (_p1EndFrame >= 0 && _p2EndFrame >= 0)
        {
            _advantageFrameText.text = "Advantage frames : " + (_p2EndFrame - _p1EndFrame);
        }
    }

    private void Start()
    {
        // Adding the method to the update
        FrameManager.Instance.FrameUpdate += ChangeFrameDataUI;
    }
}
