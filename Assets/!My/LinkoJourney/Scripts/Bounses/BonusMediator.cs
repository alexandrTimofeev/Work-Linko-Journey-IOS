using UnityEngine;
using System;
using System.Collections.Generic;

public class BonusMediator : MonoBehaviour
{
    private static BonusMediator _instance;
    public static BonusMediator Instance
    {
        get
        {
            if (_instance == null) _instance = FindFirstObjectByType<BonusMediator>();
            return _instance;
        }
    }

    public event Action<BonusData> OnBonusUsed; // Универсальное событие для всех бонусов

    private readonly Dictionary<Type, object> _bonusActions = new Dictionary<Type, object>();

    private void ProcessBonus(BonusData bonus)
    {
        OnBonusUsed?.Invoke(bonus);
        Debug.Log($"Бонус {bonus.bonusName} обработан!");
    }

    /// <summary>
    /// Подписывает метод на событие конкретного типа бонуса.
    /// </summary>
    public void Subscribe<T>(Action<T> action) where T : BonusData
    {
        /*if (_bonusActions.ContainsKey(typeof(T)) == false)
        {
            _bonusActions.Add(typeof(T), action);
            return;
        }*/

        if (_bonusActions.TryGetValue(typeof(T), out var existingAction))
        {
            _bonusActions[typeof(T)] = (existingAction as Action<T>) + action;
        }
        else
        {
            _bonusActions[typeof(T)] = action;
        }
    }

    /// <summary>
    /// Отписывает метод от события конкретного типа бонуса.
    /// </summary>
    public void Unsubscribe<T>(Action<T> action) where T : BonusData
    {
        if (_bonusActions.TryGetValue(typeof(T), out var existingAction))
        {
            _bonusActions[typeof(T)] = (existingAction as Action<T>) - action;
        }
    }

    /// <summary>
    /// Вызывает событие для конкретного типа бонуса.
    /// </summary>
    public void Notify<T>(T bonus) where T : BonusData
    {
        ProcessBonus(bonus as BonusData);
        if (_bonusActions.TryGetValue(typeof(T), out var action))
        {
            (action as Action<T>)?.Invoke(bonus);
        }
    }

}


