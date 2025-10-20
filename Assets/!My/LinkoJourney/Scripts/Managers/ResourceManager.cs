using System;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private int _maxLives = 3;  // Максимальное количество жизней
    private int _currentLives = 3;
    private int _currentCoins;
    public int CoinsCountInLevel { get; private set; }

    public int CurrentLives => _currentLives;  // Свойство для получения текущего количества жизней
    public int CurrentCoins => _currentCoins;  // Свойство для получения текущего количества монет

    public event Action<int> OnChangeLives;    // Событие, вызываемое при изменении количества жизней
    public event Action<int> OnLivesUp;        // Событие, вызываемое при увеличении количества жизней
    public event Action<int> OnTakeDamage;     // Событие, вызываемое при получении урона
    public event Action<int> OnChangeCoins;    // Событие, вызываемое при изменении количества монет

    public void Init()
    {
        BonusMediator.Instance.Subscribe<CoinBonus>((c) => AddCoins(c.Coins));
        BonusMediator.Instance.Subscribe<HealthBonus>((h) => AddLives(1));
    }

    private void Start()
    {
        PlayerPrefs.SetInt("Coins", 0);
        PickupItem[] pickupItems = FindObjectsOfType<PickupItem>(false);
        foreach (var item in pickupItems)
        {
            CoinBonus coinBonus = item.BonusData as CoinBonus;
            if (coinBonus)
                CoinsCountInLevel += coinBonus.Coins;
        }

        ChangeLives(_currentLives);
        ChangeCoins(_currentCoins);
    }

    // Метод для изменения количества жизней (уменьшение или увеличение)
    public void ChangeLives(int value)
    {
        int prevLives = _currentLives;
        _currentLives = value;

        // Ограничиваем количество жизней максимумом
        if (_currentLives > _maxLives)
        {
            _currentLives = _maxLives;
        }
        if (_currentLives < 0)
        {
            _currentLives = 0;
        }

        // Обрабатываем события в зависимости от того, прибавили или отняли жизни
        if (prevLives < _currentLives)
        {
            OnTakeDamage?.Invoke(_currentLives);  // Если отняли жизни, вызываем событие TakeDamage
        }
        else if (prevLives > _currentLives)
        {
            OnLivesUp?.Invoke(_currentLives);  // Если прибавили жизни, вызываем событие LivesUp
        }

        // Всегда вызываем событие изменения количества жизней
        OnChangeLives?.Invoke(_currentLives);

        // Если жизни закончились, вызываем метод проигрыша
        if (_currentLives <= 0)
        {
            OnLose();  // Метод проигрыша
        }
    }

    // Метод для уменьшения количества жизней
    public void DecreaseLives()
    {
        ChangeLives(_currentLives - 1);  // Уменьшаем жизни на 1
    }

    // Метод для прибавления жизней
    public void AddLives(int amount)
    {
        ChangeLives(_currentLives + amount);  // Прибавляем жизни
    }

    // Метод для изменения количества монет
    public void ChangeCoins(int value)
    {
        _currentCoins = value;

        // Вызываем событие изменения монет
        OnChangeCoins?.Invoke(_currentCoins);
    }

    // Метод для увеличения монет
    public void AddCoins(int amount)
    {
        ChangeCoins(_currentCoins + amount);  // Прибавляем монеты
    }

    // Метод для уменьшения монет
    public void SubtractCoins(int amount)
    {
        ChangeCoins(_currentCoins - amount);  // Отнимаем монеты
    }

    public void BreakCoins()
    {
        ChangeCoins(0);
    }

    // Метод для сохранения монет
    public void SaveCoins()
    {
        PlayerPrefs.SetInt("Coins", _currentCoins);
    }

    // Метод для восстановления монет
    public void LoadCoins()
    {
        ChangeCoins(PlayerPrefs.GetInt("Coins", 0));
    }

    // Метод для обработки проигрыша
    private void OnLose()
    {
        // Здесь можно добавить действия при проигрыше, например, показ экрана Game Over
        Debug.Log("Game Over! No more lives.");
    }

    // Метод для сохранения состояния игры (например, жизней)
    public void SaveGame()
    {
        PlayerPrefs.SetInt("Lives", _currentLives);
        PlayerPrefs.Save();
    }

    // Метод для загрузки состояния игры
    public void LoadGame()
    {
        _currentLives = PlayerPrefs.GetInt("Lives", _maxLives);
        OnChangeLives?.Invoke(_currentLives);
    }
}