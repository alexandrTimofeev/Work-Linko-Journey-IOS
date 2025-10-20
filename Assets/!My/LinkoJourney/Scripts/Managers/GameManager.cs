using System.Collections;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _dieDuration;
    [SerializeField] private GameObject _player;
    [SerializeField] private LoseWindow _loseWindow;
    [SerializeField] private WinWindo _winWindow;
    [SerializeField] private AudioSource _sourceMusic;

    private ResourceManager _resourceManager;
    private SceneReloader _sceneReloader;
    private float _progress = 0f;

    public static int LevelNow = 6;

    public event Action<float> OnProgress;

    // Инициализация
    public void Init(DamageReceiver damageReceiver, ResourceManager resourceManager, SceneReloader sceneReloader)
    {
        _resourceManager = resourceManager;
        _sceneReloader = sceneReloader;
        damageReceiver.OnDamaged += OnPlayerDead;

        StartCoroutine(UpdateProgressCoroutine());
    }

    // Когда игрок умирает, проверяем его жизни
    public void OnPlayerDead()
    {
        _resourceManager.DecreaseLives();
        if (_resourceManager.CurrentLives > 0)
        {
            // Уменьшаем жизни и перезагружаем сцену
            _resourceManager.BreakCoins();
            StartCoroutine(HitReloadScene());
        }
        else
        {
            // Если жизни закончились, вызываем Lose
            Lose();
        }
    }

    // Перезагружаем сцену после задержки
    public IEnumerator HitReloadScene()
    {
        yield return new WaitForSeconds(_dieDuration);
        _player.SetActive(true);
        SceneReloader.Instance.ReloadSceneFromCheckpoint();
    }

    // Метод, вызываемый при окончании жизней
    private void Lose()
    {
        _loseWindow.gameObject.SetActive(true);
        _loseWindow.Show(_resourceManager.CurrentCoins, _resourceManager.CoinsCountInLevel, LevelNow + 1);
        GamePause.SetPause(true);
        Debug.Log("Game Over! No more lives.");
        _sourceMusic.Stop();
        // Здесь можно вызвать какой-то UI, показать экран Game Over или другие действия
    }

    public void Win(float time)
    {
        int stars = GetStars();
        PlayerPrefs.SetInt($"Level{LevelNow}_Stars", stars);
        _winWindow.gameObject.SetActive(true);
        _winWindow.Show(_resourceManager.CurrentCoins, _resourceManager.CoinsCountInLevel, LevelNow + 1, time, stars);
        GamePause.SetPause(true);
        _sourceMusic.Stop();

        AchieviementSystem.ForceUnlock($"Level{LevelNow}");
        if (stars >= 3)
            SkinsSystem.UnlockSkin($"Level{LevelNow}Stars");
    }

    public int GetStars()
    {
        float winCoef = (float)_resourceManager.CurrentCoins / _resourceManager.CoinsCountInLevel;
        return winCoef >= 1f ? 3 : (winCoef > 0.6f ? 2 : 1); 
    }

    private IEnumerator UpdateProgressCoroutine ()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            UpdateProgress();
        }
    }

    public void UpdateProgress ()
    {
        OnProgress.Invoke(_sceneReloader.GetProgress());
    } 
}