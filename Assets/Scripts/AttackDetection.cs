using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetection : MonoBehaviour
{
    [SerializeField] private PlayerStateMachineManager _playerStateMachineManager;
    [SerializeField] private BoxCollider2D _attackCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player2")
        {
            _playerStateMachineManager.IsHitting = true;
        }
    }
}
