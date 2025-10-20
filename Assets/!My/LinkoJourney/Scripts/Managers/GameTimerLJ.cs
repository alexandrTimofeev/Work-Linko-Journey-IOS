using System;
using UnityEngine;
using TMPro;

public class GameTimerLJ : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;  // Ссылка на компонент TextMeshProUGUI для отображения времени
    private float _elapsedTime;  // Время, прошедшее с начала уровня

    // Пауза, проверяется через GamePause
    private bool IsPaused => GamePause.IsPause;

    public float ElapsedTime => _elapsedTime;

    private void Start()
    {
        _elapsedTime = 0f;
    }

    private void Update()
    {
        // Если не на паузе, увеличиваем время
        if (!IsPaused)
        {
            _elapsedTime += Time.deltaTime;
        }

        // Обновляем отображение времени
        UpdateTimeDisplay();
    }

    // Метод для получения времени в формате mm:ss
    public string GetTimeInText()
    {
        return GameTimerLJ.GetTimeInText(_elapsedTime);
    }

    public static string GetTimeInText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:D2}:{1:D2}", minutes, seconds);  // Форматируем время как "mm:ss"
    }

    // Метод для обновления текста на экране
    private void UpdateTimeDisplay()
    {
        if (_timeText != null)
        {
            _timeText.text = GetTimeInText();  // Устанавливаем текст в компонент TextMeshProUGUI
        }
    }

    // Метод для сброса таймера
    public void ResetTimer()
    {
        _elapsedTime = 0f;
    }
}