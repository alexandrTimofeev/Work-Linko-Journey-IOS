using UnityEngine;
using UnityEngine.Events;

public class OrientationDetector : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent<bool> onAnyPortrait;
    public UnityEvent onPortrait;
    public UnityEvent onPortraitUpsideDown;
    public UnityEvent<bool> onLandscape;
    public UnityEvent onLandscapeLeft;
    public UnityEvent onLandscapeRight;

    private ScreenOrientation _lastOrientation;

    void Start()
    {
        _lastOrientation = Screen.orientation;
        InvokeOrientationEvent(_lastOrientation);
    }

    void Update()
    {
        if (Screen.orientation != _lastOrientation)
        {
            _lastOrientation = Screen.orientation;
            InvokeOrientationEvent(_lastOrientation);
        }
    }

    private void InvokeOrientationEvent(ScreenOrientation orientation)
    {
        switch (orientation)
        {
            case ScreenOrientation.Portrait:
                onPortrait?.Invoke();
                onAnyPortrait?.Invoke(true);
                onLandscape?.Invoke(false);
                break;
            case ScreenOrientation.LandscapeLeft:
                onLandscape?.Invoke(true);
                onLandscapeLeft?.Invoke();
                onAnyPortrait?.Invoke(false);
                break;
            case ScreenOrientation.LandscapeRight:
                onLandscape?.Invoke(true);
                onLandscapeRight?.Invoke();
                onAnyPortrait?.Invoke(false);
                break;
            case ScreenOrientation.PortraitUpsideDown:
                onPortraitUpsideDown?.Invoke();
                onAnyPortrait?.Invoke(true);
                onLandscape?.Invoke(false);
                break;
            default:
                break;
        }
    }
}