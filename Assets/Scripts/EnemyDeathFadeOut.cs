using UnityEngine;

public class EnemyDeathFadeOut : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Play(Transform enemy)
    {
        transform.position = enemy.position;
        transform.localScale = enemy.localScale;
        
        _spriteRenderer.enabled = true;
        _animator.enabled = true;
        _animator.Play("EnemyDeathFadeOut", -1, 0f);
    }

    public void Deactivate()
    {
        transform.position = new Vector3(0f, 0f, -300f);
        _spriteRenderer.enabled = false;
        _animator.enabled = false;
    }
}
