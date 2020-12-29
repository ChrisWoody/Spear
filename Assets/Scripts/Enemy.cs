using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform _player;
    private int _id = -1;
    private EnemySpawner _enemySpawner;
    private Collider2D _collider2d;
    private EnemyProjectile _enemyProjectile;

    private bool _isAlive;

    private float _projectileFiredCooldown;
    private float _projectileFiredCooldownElapsed;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _collider2d = GetComponent<Collider2D>();
        _projectileFiredCooldown = Random.Range(10f, 20f);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GameController.IsGameRunning())
            return;
        
        if (!_isAlive)
            return;

        var diff = -(transform.position - _player.position).normalized;
        transform.position += diff * (Time.deltaTime * 2f);

        if (_enemyProjectile.IsAlive())
            return;
        
        // if too close to the player, dont fire projectile
        var playerMagnitude = Vector3.Magnitude(transform.position - _player.position);
        if (playerMagnitude < 10f)
            return;
        
        _projectileFiredCooldownElapsed += Time.deltaTime;
        if (_projectileFiredCooldownElapsed < _projectileFiredCooldown)
            return;

        _projectileFiredCooldownElapsed = 0f;
        _enemyProjectile.Fire(transform.position, _player.position);
    }

    public void Hydrate(int id, EnemySpawner enemySpawner, EnemyProjectile enemyProjectile)
    {
        _id = id;
        _enemySpawner = enemySpawner;
        _enemyProjectile = enemyProjectile;
    }

    public void Die()
    {
        Deactivate();
        _enemySpawner.ReportEnemyDeath(_id);
    }

    public void Deactivate()
    {
        _isAlive = false;
        transform.position = new Vector3(0f, 300f, 0f);
        _collider2d.enabled = false;
    }

    public void Activate()
    {
        _isAlive = true;
        _collider2d.enabled = true;
    }
}
