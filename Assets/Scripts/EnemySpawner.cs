using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public Transform enemyPrefab;
    private Vector3[] _spawnPoints;
    private Transform[] _enemies;

    private const float SpawnTimeout = 1f;
    private float _spawnTimeoutElapsed;

    private const int TotalEnemyCount = 30;
    private readonly Stack<int> _deactivatedEnemies = new Stack<int>(TotalEnemyCount);

    private void Awake()
    {
        _enemies = Enumerable.Range(0, TotalEnemyCount).Select(i =>
        {
            var enemy = Instantiate(enemyPrefab);
            enemy.GetComponent<Enemy>().Hydrate(i, this);
            enemy.GetComponent<Enemy>().Deactivate();
            
            _deactivatedEnemies.Push(i);

            return enemy;
        }).ToArray();
        
        _spawnPoints = GetComponentsInChildren<EnemySpawnPoint>().Select(x => x.transform.position).ToArray();
    }

    private void Update()
    {
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
