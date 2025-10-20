using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

[DefaultExecutionOrder(-999)]
public class GameInit : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] private GameObject _input;
    [SerializeField] private PlayerController _controller;
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private SpriteRenderer _rendererPlayer;
    [SerializeField] private DamageReceiver _damageReceiver;

    [Header("Game Management")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private SceneReloader _sceneReloader;
    [SerializeField] private GameTimerLJ _gameTimer;
    [SerializeField] private InterfaceManagerGame _interfaceManager;
    [SerializeField] private GameStartClick _gameStartClick;

    [Header("Bonuses & Environment")]
    [SerializeField] private BonusMediator _bonusMediator;
    [SerializeField] private BackgroundMover _backgroundMover;

    [Header("Settings")]
    [SerializeField] private AudioMixer _audioMixer;

    [Header("Level")]
    [SerializeField] private bool _isLoadLevel = false;
    [SerializeField] private LevelLoad _levelLoad;

    private void Awake()
    {
        if (_isLoadLevel)
        {
            _levelLoad.OnLevelLoad += (lvl) => _sceneReloader.SetCheckpoints(lvl.Checkpoints);
            _levelLoad.LoadLevel();
        }
        _gameStartClick.Init(_input.GetComponent<IInput>());
        InitPlayer();
        InitGameManagers();
        InitSceneEvents();
        InitGameEvents();
        InitAudio();
    }

    private void InitPlayer()
    {
        _controller.Init(_input.GetComponent<IInput>());
        _rendererPlayer.sprite = SkinsSystem.GetCurrentSkin();
    }

    private void InitGameManagers()
    {
        _interfaceManager.Init(_gameManager, _resourceManager);
        _gameManager.Init(_damageReceiver, _resourceManager, _sceneReloader);
        _resourceManager.Init();
    }

    private void InitSceneEvents()
    {
        _sceneReloader.OnSave += _resourceManager.SaveCoins;
        _sceneReloader.OnReload += _resourceManager.LoadCoins;
        _sceneReloader.OnLastCheckpoint += () => _gameManager.Win(_gameTimer.ElapsedTime);
        _sceneReloader.OnReload += _playerMover.NormalizeSpeed;
        _sceneReloader.OnReload += _damageReceiver.NormalizeInvictible;
    }

    private void InitGameEvents()
    {
        _gameManager.OnProgress += _interfaceManager.UpdateGameProgress;
        _gameManager.OnProgress += _backgroundMover.UpdatePosition;
    }

    private void InitAudio()
    {
        GameSettings.GlobalMixer = _audioMixer;
        GameSettings.LoadSettings();
        GameSettings.UpdateSettings();
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene("Game");
    }

    public void Home()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void Continue()
    {
        GameManager.LevelNow++;
        if (GameManager.LevelNow < _levelLoad.LevelsCount)
            ReloadLevel();
        else
            Home();
    }
}