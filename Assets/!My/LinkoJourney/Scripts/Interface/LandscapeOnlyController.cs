using UnityEngine;

public class LandscapeOnlyController : MonoBehaviour
{
    public enum LandscapeMode
    {
        AnyLandscape,
        LeftLandscapeOnly,
        RightLandscapeOnly
    }

    [SerializeField]
    private LandscapeMode _landscapeMode = LandscapeMode.AnyLandscape;

    void Start()
    {
        SetOrientation();
        ValidateOrientation();
    }

    public void ValidateOrientation()
    {
        bool isValid = false;

        switch (_landscapeMode)
        {
            case LandscapeMode.AnyLandscape:
                isValid = Screen.orientation == ScreenOrientation.LandscapeLeft ||
                         Screen.orientation == ScreenOrientation.LandscapeRight;
                break;
            case LandscapeMode.LeftLandscapeOnly:
                isValid = Screen.orientation == ScreenOrientation.LandscapeLeft;
                break;
            case LandscapeMode.RightLandscapeOnly:
                isValid = Screen.orientation == ScreenOrientation.LandscapeRight;
                break;
        }

        if (!isValid)
        {
            SetOrientation();
        }
    }

    private void SetOrientation()
    {
        switch (_landscapeMode)
        {
            case LandscapeMode.AnyLandscape:
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                break;
            case LandscapeMode.LeftLandscapeOnly:
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                break;
            case LandscapeMode.RightLandscapeOnly:
                Screen.orientation = ScreenOrientation.LandscapeRight;
                break;
        }
    }
}