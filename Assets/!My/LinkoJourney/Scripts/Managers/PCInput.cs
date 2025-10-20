using System;
using System.Collections;
using UnityEngine;

public class PCInput : MonoBehaviour, IInput
{
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

    public event Action<float> OnHorizontalMove;
    public event Action OnJumpDown;
    public event Action OnJumpHold;
    public event Action OnJumpUp;
    public event Action OnClick;

    private void Update()
    {
        OnHorizontalMove?.Invoke(Input.GetAxis("Horizontal"));

        if (Input.GetKeyDown(_jumpKey))
        {
            OnClick?.Invoke();
            OnJumpDown?.Invoke();
        }
        if (Input.GetKey(_jumpKey))
            OnJumpHold?.Invoke();
        if (Input.GetKeyUp(_jumpKey))
            OnJumpUp?.Invoke();
    }
}