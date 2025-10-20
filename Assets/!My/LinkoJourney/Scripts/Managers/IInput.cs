using System;

public interface IInput
{
    public event Action<float> OnHorizontalMove;
    public event Action OnJumpDown;
    public event Action OnJumpHold;
    public event Action OnJumpUp;
    public event Action OnClick;
}