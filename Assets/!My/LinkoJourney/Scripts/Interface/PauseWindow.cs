using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class PauseWindow : MonoBehaviour
{
    [SerializeField] private UnityEvent<bool> SetPause;
    
    public void Open ()
    {
        gameObject.SetActive(true);
        GamePause.SetPause(true);
        SetPause?.Invoke(true);
    }

    public void Close ()
    {
        gameObject.SetActive(false);
        GamePause.SetPause(false);
        SetPause?.Invoke(false);
    }
}