using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform _player;
    private int _id = -1;
    private EnemySpawner _enemySpawner;
    private Collider2D _collider2d;

    private bool _isAlive;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _collider2d = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_isAlive)
            return;

        var diff = -(transform.position - _player.position).normalized;
        transform.position += diff * (Time.deltaTime * 2f);
    }

    public void Hydrate(int id, EnemySpawner enemySpawner)
    {
        _id = id;
        _enemySpawner = enemySpawner;
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
