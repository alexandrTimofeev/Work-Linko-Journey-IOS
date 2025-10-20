using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;

public class LevelsButtonsPanel : MonoBehaviour
{
    [SerializeField] private LevelButton[] _levelButtons;
    public event Action<int> OnLoadLevel;

    public void Init (LevelData[] levelDatas)
    {
        for (int i = 0; i < levelDatas.Length && i < _levelButtons.Length; i++)
        {
            _levelButtons[i].Init(i, levelDatas[i].StarsComplite, levelDatas[i].IsComplite, i != 0 && levelDatas[i - 1].IsComplite == false);
            int n = i;
            _levelButtons[i].Button.onClick.AddListener(() => OnLoadLevel?.Invoke(n));
        }
    }
}