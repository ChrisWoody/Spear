using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float runSpeed = 40f;
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private AnimationCurve curve;
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    [SerializeField] private bool airControl = true;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [Range(0, .3f)][SerializeField] private float groundedRadius = 0.1f; 

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private bool _isGrounded;
    private Vector3 _velocity = Vector3.zero;
    
    private const float JumpTimeout = 0.4f;
    private float _jumpElapsed;
    private bool _jumping;

    private float _horizontalMove = 0f;
    private float _currentHorizontalMove;
    private float _currentHorizontalVelocity;

    private static readonly int RunAnimationParam = Animator.StringToHash("Run");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!GameController.IsGameRunning)
            return;
        
        var axisRaw = Input.GetAxisRaw("Horizontal");
        _currentHorizontalMove = Mathf.SmoothDamp(_currentHorizontalMove, axisRaw, ref _currentHorizontalVelocity, 0.1f);
        _horizontalMove = _currentHorizontalMove * runSpeed;

        var playRunAnimation = _currentHorizontalMove >= 0.05f || _currentHorizontalMove <= -0.05f;
        _animator.SetBool(RunAnimationParam, playRunAnimation);

        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
            _jumping = true;

        if (_jumping)
        {
            _jumpElapsed += Time.deltaTime;
            if (_jumpElapsed >= JumpTimeout || Input.GetKeyUp(KeyCode.Space))
            {
                _jumpElapsed = 0f;
                _jumping = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!GameController.IsGameRunning)
            return;

        _isGrounded = false;

        var colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        foreach (var hitCollider in colliders)
        {
            if (hitCollider.gameObject != gameObject)
            {
                _isGrounded = true;
                _jumpElapsed = 0f;
            }
        }

        Move(_horizontalMove * Time.fixedDeltaTime);
    }
    
    private void Move(float move)
    {
        if (_isGrounded || airControl)
        {
            _rigidbody2D.gravityScale = _rigidbody2D.velocity.y <= 0 ? 3f : 1f;

            Vector3 targetVelocity = new Vector2(move * 10f * (_isGrounded ? 1.0f : 0.8f), _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _velocity, movementSmoothing);
        }
        
        if (_jumping)
        {
            var scale = curve.Evaluate(_jumpElapsed / JumpTimeout);
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, (jumpForce * scale) * Time.fixedDeltaTime);
        }
    }
}