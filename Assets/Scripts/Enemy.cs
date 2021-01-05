using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform _player;
    public int Id { get; private set; }
    private EnemySpawner _enemySpawner;
    private Collider2D _collider2d;
    private EnemyProjectile _enemyProjectile;
    private EnemyDeath _enemyDeath;
    private EnemyDeathFadeOut _enemyDeathFadeOut;

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
        if (!GameController.IsGameRunning)
            return;
        
        if (!_isAlive)
            return;

        var diff = -(transform.position - _player.position).normalized;
        transform.position += diff * (Time.deltaTime * 2f);

        transform.localScale = _player.position.x < transform.position.x ? new Vector3(-2.5f, 2.5f, 1f) : new Vector3(2.5f, 2.5f, 1f);

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

    public void Hydrate(int id, EnemySpawner enemySpawner, EnemyProjectile enemyProjectile, EnemyDeath enemyDeath, EnemyDeathFadeOut enemyDeathFadeOut)
    {
        Id = id;
        _enemySpawner = enemySpawner;
        _enemyProjectile = enemyProjectile;
        _enemyDeath = enemyDeath;
        _enemyDeathFadeOut = enemyDeathFadeOut;
    }

    public void Die()
    {
        _enemyDeath.Play(transform.position);
        _enemyDeathFadeOut.Play(transform);
        Deactivate();
        _enemySpawner.ReportEnemyDeath(Id);
        GameController.EnemyKilled();
    }

    public void Deactivate()
    {
        _isAlive = false;
        transform.position = new Vector3(0f, 300f, 0f);
        _collider2d.enabled = false;
        _projectileFiredCooldownElapsed = 0f;
    }

    public void CompletelyDeactivate()
    {
        Deactivate();
        _enemyProjectile.Die();
        _enemyDeath.Deactivate();
        _enemyDeathFadeOut.Deactivate();
    }

    public void Activate()
    {
        _isAlive = true;
        _collider2d.enabled = true;
    }
}
