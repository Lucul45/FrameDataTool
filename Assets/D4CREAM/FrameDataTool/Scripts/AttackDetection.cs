using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponentInParent<PlayerController>().IsHitting = true;
        if (collision.tag == "Player2")
        {
            PlayerStateMachineManager.Instance.ChangeState(2, EPlayerState.HURT);
            PlayerStateMachineManager.Instance.CurrentStateP2.AttackHitten = GetComponentInParent<PlayerController>().CurrentAttack;
            Debug.Log((int)(GetComponentInParent<PlayerController>().CurrentAttack.Clip.length * 60));
        }
        else if (collision.tag == "Player1")
        {
            PlayerStateMachineManager.Instance.ChangeState(1, EPlayerState.HURT);
            PlayerStateMachineManager.Instance.CurrentStateP1.AttackHitten = GetComponentInParent<PlayerController>().CurrentAttack;
        }
    }
}
