using UnityEngine;

public class Killzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("killzone hit");
        other.GetComponent<EnemyProjectile>().Die();
    }
}
