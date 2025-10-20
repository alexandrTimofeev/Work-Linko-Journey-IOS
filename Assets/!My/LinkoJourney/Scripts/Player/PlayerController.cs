using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMover _playerMover;

    private IInput _input;

    public event Action<float> OnMove;    // Движение влево-вправо
    public event Action OnJumpStart;      // Начало прыжка
    public event Action OnJumpHold;       // Удержание прыжка
    public event Action OnJumpRelease;    // Отпускание прыжка

    public void Init(IInput input)
    {
        _input = input;

        //_input.OnHorizontalMove += HandleMove;
        _input.OnJumpDown += HandleJumpStart;
        _input.OnJumpHold += HandleJumpHold;
        _input.OnJumpUp += HandleJumpRelease;
    }

    public void Update()
    {
        HandleMove(1f);
    }

    private void OnDestroy()
    {
        if (_input != null)
        {
            _input.OnHorizontalMove -= HandleMove;
            _input.OnJumpDown -= HandleJumpStart;
            _input.OnJumpHold -= HandleJumpHold;
            _input.OnJumpUp -= HandleJumpRelease;
        }
    }

    private void HandleMove(float direction)
    {
        if (GamePause.IsPause)
            return;

        OnMove?.Invoke(direction);
        _playerMover.Move(direction);
    }

    private void HandleJumpStart()
    {
        if (GamePause.IsPause)
            return;

        OnJumpStart?.Invoke();
        _playerMover.StartJump();
    }

    private void HandleJumpHold()
    {
        if (GamePause.IsPause)
            return;

        OnJumpHold?.Invoke();
        _playerMover.HoldJump();
    }

    private void HandleJumpRelease()
    {
        if (GamePause.IsPause)
            return;

        OnJumpRelease?.Invoke();
        _playerMover.ReleaseJump();
    }
}
