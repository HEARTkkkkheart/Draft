using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTrigger : MonoBehaviour
{
    [SerializeField] private MoveController player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.NameToLayer("Floor") == collision.gameObject.layer)
        {
            player.OnTriggerFloor();
        }
    }
}
