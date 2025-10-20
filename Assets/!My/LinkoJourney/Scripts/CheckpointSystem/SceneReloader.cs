using UnityEngine;
using System.Collections.Generic;
using System;

public class SceneReloader : MonoBehaviour
{
    public static SceneReloader Instance { get; private set; }

    [SerializeField] private PlayerSavable player;
    [SerializeField] private Checkpoint[] _checkpoints;  // Список всех чекпоинтов на сцене
    private int lastCheckpointIndex = -1;  // Индекс последнего чекпоинта

    private List<ISceneObject> sceneObjects = new List<ISceneObject>(); // Пул объектов для скрытия
    public Checkpoint LastCheckpoint => _checkpoints[_checkpoints.Length - 1];

    public event Action OnSave;
    public event Action OnReload;
    public event Action OnLastCheckpoint;

    public void SetCheckpoints(Checkpoint[] checkpoints)
    {
        _checkpoints = checkpoints;
    }

    private void Awake()
    {
        // Если экземпляр синглтона уже существует, уничтожаем текущий объект
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Убедитесь, что объект сохраняется между сценами
        }

        for (int i = 0; i < _checkpoints.Length; i++)
        {
            _checkpoints[i].Init(i);
        }
    }

    // Перезагружает сцену с заданного чекпоинта
    public void ReloadSceneFromCheckpoint(int n)
    {
        OnReload?.Invoke();

        // Перемещаем игрока на место последнего чекпоинта
        if (n >= 0 && n < _checkpoints.Length)
        {
            player.transform.position = _checkpoints[n].transform.position;
        }

        // Включаем объекты из пулла и вызываем метод Reload()
        foreach (var obj in sceneObjects)
        {
            obj.Reload();
        }
        sceneObjects.Clear(); // Очищаем пул объектов
    }

    // Перезагружает сцену с последнего чекпоинта
    public void ReloadSceneFromCheckpoint()
    {
        ReloadSceneFromCheckpoint(lastCheckpointIndex);
    }

    // Добавляет объект в пул для скрытия
    public static void HideObject(ISceneObject obj)
    {
        Instance.sceneObjects.Add(obj);
        obj.Hide();
    }

    // Сохраняет номер чекпоинта
    public void SaveCheckpoint(int checkpointIndex)
    {
        lastCheckpointIndex = checkpointIndex;
        Debug.Log($"Save Checkpoint {checkpointIndex}");
        OnSave?.Invoke();

        if (checkpointIndex == _checkpoints.Length - 1)
            OnLastCheckpoint?.Invoke();
    }

    public float GetProgress()
    {
        return 1f - (Mathf.Abs(player.transform.position.x - _checkpoints[_checkpoints.Length - 1].transform.position.x) /
            Mathf.Abs(player.StartPoint.x - _checkpoints[_checkpoints.Length - 1].transform.position.x));
    }

    private void OnDestroy()
    {
        // Когда объект уничтожается (например, при загрузке новой сцены), очищаем ссылку
        if (Instance == this)
        {
            Instance = null;
        }
    }
}