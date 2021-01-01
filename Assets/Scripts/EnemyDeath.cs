using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
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
