using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public int totalEnemyCount = 30;
    public Transform enemyPrefab;
    public Transform enemyProjectilePrefab;
    public Transform enemyDeathPrefab;
    private Vector3[] _spawnPoints;
    private Transform[] _enemies;

    private const float SpawnTimeout = 1f;
    private float _spawnTimeoutElapsed;
    
    private Stack<int> _deactivatedEnemies;

    private void Awake()
    {
        GameController.OnStartGame += GameControllerOnOnStartGame;

        _deactivatedEnemies = new Stack<int>(totalEnemyCount);
        _enemies = Enumerable.Range(0, totalEnemyCount).Select(i =>
        {
            var enemyProjectile = Instantiate(enemyProjectilePrefab);
            enemyProjectile.GetComponent<EnemyProjectile>().Die();

            var enemyDeath = Instantiate(enemyDeathPrefab);
            enemyDeath.GetComponent<EnemyDeath>().Deactivate();
            
            var enemy = Instantiate(enemyPrefab);
            enemy.GetComponent<Enemy>().Hydrate(i, this, enemyProjectile.GetComponent<EnemyProjectile>(), enemyDeath.GetComponent<EnemyDeath>());
            enemy.GetComponent<Enemy>().Deactivate();
            
            _deactivatedEnemies.Push(i);

            return enemy;
        }).ToArray();
        
        _spawnPoints = GetComponentsInChildren<EnemySpawnPoint>().Select(x => x.transform.position).ToArray();
    }

    private void GameControllerOnOnStartGame()
    {
        _deactivatedEnemies.Clear();
        foreach (var enemy in _enemies)
        {
            enemy.GetComponent<Enemy>().CompletelyDeactivate();
            _deactivatedEnemies.Push(enemy.GetComponent<Enemy>().Id);
        }

        _spawnTimeoutElapsed = 0f;
    }

    private void Update()
    {
        if (!GameController.IsGameRunning)
            return;
        
        _spawnTimeoutElapsed += Time.deltaTime;
        if (_spawnTimeoutElapsed < SpawnTimeout)
            return;
        
        _spawnTimeoutElapsed = 0f;
        
        if (_deactivatedEnemies.Count == 0)
            return;
        
        var enemyCacheIndex = _deactivatedEnemies.Pop();
        var enemy = _enemies[enemyCacheIndex];

        var spawnPointIndex = Random.Range(0, _spawnPoints.Length);
        var spawnPoint = _spawnPoints[spawnPointIndex];

        enemy.position = spawnPoint;
        enemy.GetComponent<Enemy>().Activate();
    }

    public void ReportEnemyDeath(int id)
    {
        _deactivatedEnemies.Push(id);
    }
}
