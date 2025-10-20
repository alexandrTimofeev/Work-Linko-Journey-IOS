using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStartClick : MonoBehaviour
{
    [SerializeField] private UnityEvent OnStart;
    private IInput _input;

    public void Init(IInput input)
    {
        _input = input;
        Open();
    }

    public void Open ()
    {
        _input.OnClick += StartGame;
        gameObject.SetActive(true);
        GamePause.SetPause(true);
    }

    private void StartGame()
    {
        _input.OnClick -= StartGame;
        OnStart?.Invoke();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GamePause.SetPause(false);
    }
}
