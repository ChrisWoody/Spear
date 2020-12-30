using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Collider2D _collider2D;
    private SpriteRenderer _spriteRenderer;
    private bool _isAlive;

    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void FixedUpdate()
    {
        if (!GameController.IsGameRunning)
            return;
        
        if (_isAlive)
            transform.position += transform.right * (Time.deltaTime * 10f);
        
        // crude for now until can get killzone to work
        if (transform.position.x < -50f || transform.position.x > 50f || transform.position.y < -50f || transform.position.y > 50f)
            Die();
    }

    public void Fire(Vector2 position, Vector2 target)
    {
        transform.position = position;
        transform.right = -(position - target).normalized;
        
        _collider2D.enabled = true;
        _spriteRenderer.enabled = true;
        _isAlive = true;
    }

    public void Die()
    {
        _collider2D.enabled = false;
        _spriteRenderer.enabled = false;
        _isAlive = false;
    }

    public bool IsAlive() => _isAlive;
}
