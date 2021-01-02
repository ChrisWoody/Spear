using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spear : MonoBehaviour
{
    public Transform ThrownSpear;
    public SpearTrail SpearTrail;
    public Slider slider;
    
    private const float AttackTimeout = 0.2f;
    private const float HalfAttackTimeout = AttackTimeout / 2f;
    private float _attackMeleeOutElapsed;
    private float _attackMeleeInElapsed;
    private bool _attacking;
    private bool _attackingMelee;
    private bool _attackingMeleeOut;
    private bool _attackingThrow;
    private const float AttackThrowChargeTime = 0.5f;
    private float _attackThrowChargeElapsed;
    private bool _attackThrowCharging;
    private readonly RaycastHit2D[] _enemyHits = new RaycastHit2D[30];
    private bool _attackingThrowSkipFrame;

    private Vector3 _originalPosition;
    private Vector3 _attackPosition;
    private SpriteRenderer _spriteRenderer;
    private ParticleSystem _spearEffects;
    private Collider2D _collider2D;
    private Transform _player;
    private Camera _mainCamera;

    // Start is called before the first frame update
    private void Awake()
    {
        _originalPosition = transform.localPosition;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spearEffects = GetComponentInChildren<ParticleSystem>();
        ThrownSpear.GetComponent<SpriteRenderer>().enabled = false;
        ThrownSpear.GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _collider2D = GetComponent<Collider2D>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _mainCamera = Camera.main;
        slider.maxValue = AttackThrowChargeTime;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GameController.IsGameRunning)
            return;
        
        if (!_attacking && Input.GetMouseButtonDown(0))
        {
            _attacking = true;
            _attackingMelee = true;
            _attackingMeleeOut = true;
            _attackPosition = _originalPosition + (Vector3.right * 2f);
        }
        else if (!_attacking && Input.GetMouseButtonDown(1))
        {
            _attackThrowCharging = true;
        }

        if (_attackThrowCharging)
        {
            _attackThrowChargeElapsed += Time.deltaTime;
            if (_attackThrowChargeElapsed >= AttackThrowChargeTime)
            {
                if (Input.GetMouseButtonUp(1))
                {
                    _attacking = true;
                    _attackingThrow = true;
                    _attackingThrowSkipFrame = true;
                    _attackThrowChargeElapsed = 0f;
                    _attackThrowCharging = false;
                    slider.value = 0f;

                    var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    var pos = transform.position;
                    var right = mousePos.x < _player.position.x ? -transform.right : transform.right;
                    var thrownHit = Physics2D.Raycast(pos, right, 100f, LayerMask.GetMask("Ground"));
                    var numOfEnemyHits = Physics2D.RaycastNonAlloc(pos, right, _enemyHits, thrownHit.distance, LayerMask.GetMask("Enemy"));

                    for (var i = 0; i < numOfEnemyHits; i++)
                    {
                        var enemy = _enemyHits[i];
                        enemy.transform.GetComponent<Enemy>().Die();
                    }

            

                    _spriteRenderer.enabled = false;
                    _spearEffects.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    _collider2D.enabled = false;
                    ThrownSpear.position = thrownHit.point;
                    //ThrownSpear.right = mousePos.x < _player.position.x ? transform.right : -transform.right;
                    ThrownSpear.right = right;
                    ThrownSpear.GetComponent<SpriteRenderer>().enabled = true;
                    ThrownSpear.GetComponentInChildren<ParticleSystem>().Play(true);
                    SpearTrail.ShowTrail(transform.position, thrownHit.point);
                }
            }
            else
            {
                slider.value = _attackThrowChargeElapsed;
                if (Input.GetMouseButtonUp(1))
                {
                    _attackThrowCharging = false;
                    _attackThrowChargeElapsed = 0f;
                    slider.value = 0f;
                }
            }
        }
        
        if (_attacking)
        {
            if (_attackingMelee)
            {
                if (_attackingMeleeOut)
                {
                    _attackMeleeOutElapsed += Time.deltaTime;
                    if (_attackMeleeOutElapsed >= HalfAttackTimeout)
                    {
                        _attackMeleeOutElapsed = 0f;
                        _attackingMeleeOut = false;
                    }
                    else
                    {
                        var scale = _attackMeleeOutElapsed / HalfAttackTimeout;
                        var newPos = Vector3.Lerp(_originalPosition, _attackPosition, scale);
                        transform.localPosition = newPos;
                    }
                }
                else
                {
                    _attackMeleeInElapsed += Time.deltaTime;
                    if (_attackMeleeInElapsed >= HalfAttackTimeout)
                    {
                        _attackMeleeInElapsed = 0f;
                        _attackingMelee = false;
                        _attacking = false;
                    }
                    else
                    {
                        var scale = _attackMeleeInElapsed / HalfAttackTimeout;
                        var newPos = Vector3.Lerp(_attackPosition, _originalPosition, scale);
                        transform.localPosition = newPos;
                    }
                }
            }
            else if (_attackingThrow)
            {
                if (_attackingThrowSkipFrame)
                {
                    _attackingThrowSkipFrame = false;
                }
                else
                {
                    // basically waiting for player to return the spear
                    if (Input.GetMouseButtonDown(1))
                    {
                        // return it
                        _attacking = false;
                        _attackingThrow = false;
                        _spriteRenderer.enabled = true;
                        _spearEffects.Play();
                        _collider2D.enabled = true;
                        ThrownSpear.GetComponent<SpriteRenderer>().enabled = false;
                        ThrownSpear.GetComponentInChildren<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                        SpearTrail.ShowTrail(ThrownSpear.position, transform.position);
                        var diff = transform.position - ThrownSpear.position;
                        var numOfEnemyHits = Physics2D.RaycastNonAlloc(ThrownSpear.position, diff.normalized, _enemyHits, diff.magnitude, LayerMask.GetMask("Enemy"));
                        for (var i = 0; i < numOfEnemyHits; i++)
                        {
                            _enemyHits[i].transform.GetComponent<Enemy>().Die();
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_attackingMelee && other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Die();
        }
    }
}