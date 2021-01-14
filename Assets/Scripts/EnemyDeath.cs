using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        GameController.OnEnemyDeathEffectSet += value =>
        {
            var main = _particleSystem.main;
            main.startSpeed = value ? 8f : 5f;
            var emission = _particleSystem.emission;
            emission.rateOverTime = value ? 5000f : 800f;
        };
    }

    public void Play(Vector2 position)
    {
        transform.position = position;
        _particleSystem.Play();
    }

    public void Deactivate()
    {
        transform.position = new Vector3(-1000, -1000, 0);
        _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
