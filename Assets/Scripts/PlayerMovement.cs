using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D Controller;

    //private Animator _animator;

    public float RunSpeed = 40f;

    private float _horizontalMove = 0f;
    private float _currentHorizontalMove;
    private bool _jump = false;
    private float _currentHorizontalVelocity;

    private void Awake()
    {
        //_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.IsGameRunning())
            return;
        
        var axisRaw = Input.GetAxisRaw("Horizontal");

        //_currentHorizontalMove = Mathf.Lerp(_currentHorizontalMove, axisRaw, Time.deltaTime / 2f);
        _currentHorizontalMove = Mathf.SmoothDamp(_currentHorizontalMove, axisRaw, ref _currentHorizontalVelocity, 0.1f);

        //if (_currentHorizontalMove != 0f)
            //_animator.SetBool("Run", true);
        //else
            //_animator.SetBool("Run", false);

        _horizontalMove = _currentHorizontalMove * RunSpeed;
        if (!_jump)
        {
            _jump = Input.GetButtonDown("Jump");
        }
    }

    void FixedUpdate()
    {
        if (!GameController.IsGameRunning())
            return;
        
        // Move our character
        Controller.Move(_horizontalMove * Time.fixedDeltaTime, _jump);
        _jump = false;
    }
}