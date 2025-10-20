using System;

public static class GamePause
{
    public static bool IsPause { get; private set; }

    public static Action OnPause;
    public static Action OnUnpause;

    // Метод для установки состояния паузы
    public static void SetPause(bool isPaused)
    {
        if (IsPause == isPaused)
            return;

        IsPause = isPaused;
        if (isPaused)        
            OnPause?.Invoke();        
        else
            OnUnpause?.Invoke();        
    }
}
