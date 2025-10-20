using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class TestMode : MonoBehaviour
{
    [SerializeField] private bool _breakGameMode = true;
    [SerializeField] private bool _breakLastRequest = true;
    [SerializeField] private bool _breakNotificationSettings = true;

    private void Awake()
    {
        if (_breakGameMode)
        {
            PlayerPrefs.SetInt("GameMode", -1);
        }
        if (_breakLastRequest)
        {
            PlayerPrefs.SetString("LastRequest", "");
        }
        if (_breakNotificationSettings)
        {
            PlayerPrefs.SetInt("LastNotificationQuest", -1);
            PlayerPrefs.SetInt("Notification", -1);
        }
    }
}
