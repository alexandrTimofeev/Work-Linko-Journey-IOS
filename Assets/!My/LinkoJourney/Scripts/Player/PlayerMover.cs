using System;
using UnityEngine;
using System.Collections;

public class PlayerMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float jumpHoldForce = 3f;
    public float jumpTime = 0.2f;

    private Rigidbody2D _rb;
    private ConstantForce2D _constForce;
    private bool isGrounded;
    private bool isJumping;
    private bool canHoldJumping;
    private float jumpTimeCounter;

    [Space]
    [SerializeField] private Transform _shadow;
    [SerializeField] private float _offsetShadow;
    [SerializeField] private LayerMask _layersShadow;

    [Space]
    [SerializeField] private GameObject _slowVfx;
    [SerializeField] private AudioSource _jumpSource;
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioClip _groundClip;

    private float _flyTime;
    private Vector3 _memoryVelocity;
    private float _memorySpeed = 5f;
    private float _memoryGravity = 1f;
    private float _slowMoTimer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _constForce = GetComponent<ConstantForce2D>();
        _memorySpeed = moveSpeed;
        _memoryGravity = _constForce.force.y;

        GamePause.OnPause += PauseMove;
        GamePause.OnUnpause += UnpauseMove;
        BonusMediator.Instance.Subscribe<SlowMoBonusData>((s) => StartSlowMo(s.SpeedCoef, s.Time));
    }

    private void Update()
    {
        if (GamePause.IsPause)
            return;

        if (isGrounded)
            _flyTime = 0f;
        else
            _flyTime += Time.deltaTime;
    }

    private void UnpauseMove()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rb.linearVelocity = _memoryVelocity;
    }

    private void PauseMove()
    {
        _memoryVelocity = _rb.linearVelocity;
        _rb.linearVelocity = Vector2.zero;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Move(float direction)
    {
        if (GamePause.IsPause)
            return;

        _rb.linearVelocity = new Vector2(direction * moveSpeed, _rb.linearVelocity.y);
        TestSlowMo();
        UpdateShadow();
    }

    private void UpdateShadow()
    {
        if (_shadow == null) return;

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.3f, Vector2.down, Mathf.Infinity, _layersShadow);

        if (hit.collider != null)
        {
            _shadow.gameObject.SetActive(true);
            _shadow.position = hit.point + new Vector2(0, _offsetShadow);
        }
        else
        {
            _shadow.gameObject.SetActive(false);
        }
    }

    public void StartJump()
    {
        if (GamePause.IsPause)
            return;

        if (isGrounded)
        {
            isJumping = true;
            canHoldJumping = true;
            jumpTimeCounter = jumpTime;
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            _jumpSource.PlayOneShot(_jumpClip);
        }
    }

    public void HoldJump()
    {
        if (GamePause.IsPause)
            return;

        if (isJumping && canHoldJumping && jumpTimeCounter > 0)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, (jumpForce + jumpHoldForce));
            jumpTimeCounter -= Time.deltaTime;
        }
    }

    public void ReleaseJump()
    {
        isJumping = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (isGrounded == false)
            {
                _jumpSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                _jumpSource.PlayOneShot(_groundClip, Mathf.Lerp(0.2f, 1.5f, _flyTime / 3f));
            }
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    private void StartSlowMo(float speedCoef, float time)
    {
        _slowMoTimer = time;
        moveSpeed = _memorySpeed * speedCoef;
        _constForce.force = new Vector2(_constForce.force.x, _memoryGravity * speedCoef * speedCoef);
        _slowVfx.SetActive(true);
    }

    public void NormalizeSpeed ()
    {
        moveSpeed = _memorySpeed;
        _constForce.force = new Vector2(_constForce.force.x, _memoryGravity);
        _slowMoTimer = 0f;
        _slowVfx.SetActive(false);
    }

    private void TestSlowMo()
    {
        if (_slowMoTimer > 0f)
        {
            _slowMoTimer -= Time.deltaTime;
            if (_slowMoTimer <= 0f)
                NormalizeSpeed();
        }
    }

    private void OnDestroy()
    {
        GamePause.OnPause -= PauseMove;
        GamePause.OnUnpause -= UnpauseMove;
    }
}