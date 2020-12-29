using UnityEngine;

public class Player : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyProjectile"))
        {
            Debug.Log("Hit by " + other.tag);
            
            // for now
            if (other.CompareTag("EnemyProjectile"))
                other.GetComponent<EnemyProjectile>().Die();
        }
    }
}
