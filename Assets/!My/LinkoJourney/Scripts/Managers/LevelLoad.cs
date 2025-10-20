using System;
using UnityEngine;

public class LevelLoad : MonoBehaviour
{
    [SerializeField] private Transform _spawn;  // Точка появления уровня
    [SerializeField] private Level[] _levels;   // Массив уровней (префабы)

    private Level _currentLevel; // Ссылка на текущий загруженный уровень

    public Action<Level> OnLevelLoad; // Событие загрузки уровня

    public int LevelsCount => _levels.Length;

    public void LoadLevel()
    {
        LoadLevel(GameManager.LevelNow);
    }

    public void LoadLevel(int n)
    {
        if (n < 0 || n >= _levels.Length)
        {
            Debug.LogError($"Уровень {n} вне допустимого диапазона!");
            return;
        }

        if (_currentLevel != null)
        {
            Destroy(_currentLevel.gameObject); // Удаляем старый уровень
        }

        _currentLevel = Instantiate(_levels[n], _spawn.position, Quaternion.identity); // Создаём новый уровень
        OnLevelLoad?.Invoke(_currentLevel); // Вызываем событие загрузки уровня
    }
}