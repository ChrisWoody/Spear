using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounds : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyProjectile"))
        {
            Debug.Log("Hit by " + other.tag);
            
            // for now
            if (other.CompareTag("EnemyProjectile"))
                other.GetComponent<EnemyProjectile>().Die();
            
            GameController.GameOver();
        }
    }
}
