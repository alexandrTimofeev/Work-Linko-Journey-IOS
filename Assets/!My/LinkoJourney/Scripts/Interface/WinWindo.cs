using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System;

public class WinWindo : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private Transform _frameWindow; // Ссылка на Transform окна
    [SerializeField] private Vector3 _scaleTarget = new Vector3(1.2f, 1.2f, 1f); // Цель для Scale анимации
    [SerializeField] private float _scaleDuration = 0.5f; // Продолжительность Scale анимации
    [SerializeField] private float _punchDuration = 0.3f; // Продолжительность Punch анимации
    [SerializeField] private float _punchStrength = 0.2f; // Сила Punch анимации
    [SerializeField] private int _punchVibrato = 10; // Количество "пульсаций" в Punch анимации
    [SerializeField] private float _punchRandomness = 0.1f; // Случайность Punch анимации

    [Space]
    [SerializeField] private TextMeshProUGUI _scoreTmp;
    [SerializeField] private TextMeshProUGUI _levelTmp;
    [SerializeField] private TextMeshProUGUI _timerTmp;
    [SerializeField] private Image[] _stars;
    [SerializeField] private Color _colorStarDisable = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    // Настройки для звёзд
    [SerializeField] private float _starWaitStart = 1f; // Продолжительность Punch анимации для звезды
    [SerializeField] private float _starSizePunchStrength = 0.2f; // Сила Punch для звезды
    [SerializeField] private float _starPunchDuration = 0.5f; // Продолжительность Punch анимации для звезды
    [SerializeField] private float _starDelayBetweenPunches = 0.2f; // Задержка между анимациями звезд
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _starClip;

    // Событие для активации звезды
    public event Action<int> OnStarActivated;

    // Метод для показа окна с анимацией
    public void Show(int score, int scoreAll, int level, float time, int starsCount)
    {
        gameObject.SetActive(true);

        // Сначала масштабируем окно, а потом применяем Punch анимацию
        _frameWindow.localScale = Vector3.zero; // Начальный масштаб
        for (int i = 0; i < _stars.Length; i++)
        {
            _stars[i].color = _colorStarDisable;
        }

        // Создаем последовательность анимаций
        Sequence sequence = DOTween.Sequence();

        // Масштабируем окно
        sequence.Append(_frameWindow.DOScale(_scaleTarget, _scaleDuration).SetEase(Ease.Linear));

        // Применяем Punch эффект
        sequence.Append(_frameWindow.DOPunchScale(Vector3.one * _punchStrength, _punchDuration, _punchVibrato, _punchRandomness).SetEase(Ease.Linear));

        sequence.AppendInterval(_starWaitStart);

        // Анимация звезд с задержкой
        sequence.AppendCallback(() =>
        {
            ActivateStarsWithDelay(starsCount); // Включаем звезды с задержкой
        });

        // Запускаем последовательность
        sequence.Play();

        // Обновляем текстовые элементы (результаты)
        int current = 0;
        DOTween.To(() => current,
                  x => {
                      current = x;
                      _scoreTmp.text = $"SCORE: {x}/{scoreAll}";
                  },
                  score, Mathf.Clamp(score / 700f, 5f, 10f)).SetEase(Ease.OutExpo);
        _levelTmp.text = $"LEVEL - {level}";
        _timerTmp.text = $"{GameTimerLJ.GetTimeInText(time)}";
    }

    // Метод для активации звезд с задержкой
    private void ActivateStarsWithDelay(int starsCount)
    {
        Sequence starSequence = DOTween.Sequence();

        for (int i = 0; i < _stars.Length; i++)
        {
            if (i < starsCount)
            {
                int n = i;
                // Добавляем задержку и анимацию для каждой звезды
                starSequence.Append(_stars[i].transform.DOPunchScale(Vector3.one * _starSizePunchStrength, _starPunchDuration, 1, _starDelayBetweenPunches)
                    .OnStart(() =>
                    {
                        _stars[n].color = Color.white;
                        _audioSource.pitch += 0.1f;
                        _audioSource.PlayOneShot(_starClip);
                        OnStarActivated?.Invoke(n);
                    })).AppendInterval(_starDelayBetweenPunches); // Вызываем событие активации звезды
            }
        }

        // Запускаем последовательность для звезд
        starSequence.Play();
    }

    // Метод для скрытия окна
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}