using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isUpsideDown;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _maxVerticalSpeed;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private AudioClip _loseSound;

    private bool _isFacingRight = true;
    private bool _isGrounded;
    private bool _wasGrounded;
    private float _maxFuel = 100.0f;
    private static float _fuelAmmount = 100.0f;
    private float _fuelConsumeRate = 2.0f;
    private float _fuelRefillRate = 1.0f;
    private Coroutine _refillFuelCoroutine;
    private Lever _leverInRange;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private Animator _animator;
    private AudioSource _audioSource;
    private ParticleSystem _ps;
    private UIManager _uiManager;

    private float movement;
    private bool jumpPressed;
    private bool actionPressed;

    private int _jumpHash = Animator.StringToHash("Jumping");
    private int _movementHash = Animator.StringToHash("Movement");
    private int _groundedHash = Animator.StringToHash("Grounded");

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _ps = GetComponent<ParticleSystem>();
        _uiManager = FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        if(LevelManager.Instance.controlsEnabled)
        {
            movement = Input.GetAxisRaw("Horizontal");
            jumpPressed = Input.GetButton("Jump") || Input.GetAxisRaw("Vertical") == 1;
            actionPressed = Input.GetButtonDown("Action");

            Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, .1f, _whatIsGround);
            _isGrounded = colliders.Length > 0;

            if(_isGrounded && actionPressed && _leverInRange && IsFacingLever())
            {
                _leverInRange.Interact();
            }

            if(!isUpsideDown)
            {
                if(!_isGrounded && _refillFuelCoroutine != null)
                {
                    StopCoroutine(_refillFuelCoroutine);
                    _refillFuelCoroutine = null;
                }
                else if(_isGrounded && !_wasGrounded)
                {
                    _refillFuelCoroutine = StartCoroutine(RefillFuel());
                }
            }

            _wasGrounded = _isGrounded;

            if((movement > 0 && !_isFacingRight) || (movement < 0 && _isFacingRight))
            {
                Flip();
            }
        }
    }

    private void FixedUpdate()
    {
        if(LevelManager.Instance.controlsEnabled)
        {
            _rb.velocity = new Vector2(movement * _moveSpeed * Time.deltaTime, _rb.velocity.y);
            if(jumpPressed && _fuelAmmount > 0)
            {
                if(!isUpsideDown)
                {
                    _fuelAmmount -= _fuelConsumeRate;
                    _uiManager.UpdateFuel(_fuelAmmount/_maxFuel);
                }
                
                if(Mathf.Abs(_rb.velocity.y) < _maxVerticalSpeed)
                {
                    _rb.AddForce(new Vector2(0.0f, _jumpForce * (isUpsideDown ? -1.0f : 1.0f) * Time.deltaTime));
                }
                if(!_audioSource.isPlaying)
                {
                    _audioSource.Play();
                }
            }
            else
            {
                _audioSource.Stop();
            }

            _animator.SetFloat(_movementHash, Mathf.Abs(_rb.velocity.x));
            _animator.SetBool(_jumpHash, jumpPressed && _fuelAmmount > 0);
            _animator.SetBool(_groundedHash, _isGrounded);
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }

    private void Flip()
	{
		_isFacingRight = !_isFacingRight;

		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

    private IEnumerator RefillFuel()
    {
        yield return new WaitForSeconds(1.0f);

        while(_fuelAmmount < _maxFuel)
        {
            _fuelAmmount += +_fuelRefillRate;
            _uiManager.UpdateFuel(_fuelAmmount/_maxFuel);
            yield return null;
        }
    }

    private bool IsFacingLever()
    {
        var leverPos = _leverInRange.transform.position.x;
        var playerPos = transform.position.x;

        return (_isFacingRight && leverPos >= playerPos) ||
            (!_isFacingRight && leverPos <= playerPos);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Platform")
        {
            transform.parent = col.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.tag == "Platform")
        {
            transform.parent = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Spike")
        {
            _ps.Play();
            _sr.enabled = false;
            AudioSource.PlayClipAtPoint(_loseSound, Camera.main.transform.position);
            _fuelAmmount = _maxFuel;
            LevelManager.Instance.GameOver();
            Invoke(nameof(DestroyMe), _ps.main.duration);
        }
        else if(other.gameObject.tag == "Lever")
        {
            _leverInRange = other.GetComponent<Lever>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Lever")
        {
            _leverInRange = null;
        }
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }
}
