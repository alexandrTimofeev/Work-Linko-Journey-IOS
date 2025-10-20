using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private int checkpointIndex;  // Индекс чекпоинта
    private SceneReloader sceneReloader;

    public void Init(int i)
    {
        checkpointIndex = i;
    }

    private void Start()
    {
        // Ищем SceneReloader на сцене и сохраняем его для использования
        sceneReloader = FindObjectOfType<SceneReloader>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var playerSavable = other.GetComponent<PlayerSavable>();
        if (playerSavable != null)
        {
            // Когда объект с компонентом PlayerSavable касается чекпоинта, сохраняем его позицию
            OnPlayerTouch();
        }
    }

    public void OnPlayerTouch()
    {
        if (sceneReloader != null)
        {
            sceneReloader.SaveCheckpoint(checkpointIndex); // Сохраняем номер чекпоинта
        }
    }
}