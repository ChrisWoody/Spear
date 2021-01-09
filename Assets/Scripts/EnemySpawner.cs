using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public int totalEnemyCount = 40;
    public Transform enemyPrefab;
    public Transform enemyProjectilePrefab;
    public Transform enemyDeathPrefab;
    public Transform enemyDeathFadeOutPrefab;
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

            var enemyDeathFadeOut = Instantiate(enemyDeathFadeOutPrefab);
            enemyDeathFadeOut.GetComponent<EnemyDeathFadeOut>().Deactivate();
            
            var enemy = Instantiate(enemyPrefab);
            enemy.GetComponent<Enemy>().Hydrate(i, this, enemyProjectile.GetComponent<EnemyProjectile>(), enemyDeath.GetComponent<EnemyDeath>(), enemyDeathFadeOut.GetComponent<EnemyDeathFadeOut>());
            enemy.GetComponent<Enemy>().Deactivate();
            
            _deactivatedEnemies.Push(i);

            return enemy;
        }).ToArray();
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

        enemy.position = GetRandomSpawnPosition();
        enemy.GetComponent<Enemy>().Activate();
    }

    public void ReportEnemyDeath(int id)
    {
        _deactivatedEnemies.Push(id);
    }

    private static Vector3 GetRandomSpawnPosition()
    {
        float xPos, yPos;
        if (Random.Range(0f, 1f) > 0.5f)
        {
            xPos = Random.Range(0f, 1f) > 0.5f ? 30f : -30f;
            yPos = (Random.Range(0f, 1f) * 30f) - 15f;
        }
        else
        {
            xPos = (Random.Range(0f, 1f) * 60f) - 30f;
            yPos = Random.Range(0f, 1f) > 0.5f ? 15f : -15f;
        }

        return new Vector3(xPos, yPos, 0f);
    }
}
