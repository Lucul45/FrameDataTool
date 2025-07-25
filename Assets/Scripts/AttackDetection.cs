using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetection : MonoBehaviour
{
    [SerializeField] private PlayerController _p1;
    [SerializeField] private PlayerController _p2;
    [SerializeField] private BoxCollider2D _attackCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player2")
        {
            _p1.IsHitting = true;
            PlayerStateMachineManager.Instance.ChangeStateP2(EPlayerState.HURT);
            PlayerStateMachineManager.Instance.CurrentStateP2.AttackHitten = collision.GetComponentInParent<PlayerController>().CurrentAttack;
        }
        else if (collision.tag == "Player1")
        {
            _p2.IsHitting = true;
            PlayerStateMachineManager.Instance.ChangeStateP1(EPlayerState.HURT);
            PlayerStateMachineManager.Instance.CurrentStateP1.AttackHitten = collision.GetComponentInParent<PlayerController>().CurrentAttack;
        }
    }
}
