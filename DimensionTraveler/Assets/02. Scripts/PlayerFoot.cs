using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MonsterHead"))
        {
            PlayerMovement playerMovement = GetComponentInParent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.Jump(playerMovement.jumpForce * 0.7f);
                Destroy(other.transform.parent.gameObject);
                GameManager.instance.AddScore(100);
            }
        }
    }

}
