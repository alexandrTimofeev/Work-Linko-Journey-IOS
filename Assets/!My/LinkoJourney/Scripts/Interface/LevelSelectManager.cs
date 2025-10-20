using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private int _levelsCount;
    [SerializeField] private LevelsButtonsPanel _buttonsPanel;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private string _privacyLink = "https://linkojourney.com/privacy-policy.html";

    private LevelData[] levelDatas;

    private void Start()
    {
        GameSettings.GlobalMixer = _audioMixer;
        GameSettings.LoadSettings();
        GameSettings.UpdateSettings();

        levelDatas = new LevelData[_levelsCount];
        for (int i = 0; i < levelDatas.Length; i++)
        {
            levelDatas[i] = new LevelData(PlayerPrefs.GetInt($"Level{i}_Stars", -1));
        }

        _buttonsPanel.Init(levelDatas);
        _buttonsPanel.OnLoadLevel += LoadLevel;
    }

    public void LoadLevel()
    {
        for (int i = 0; i < levelDatas.Length; i++)
        {
            if (levelDatas[i].IsComplite == false)
            {
                LoadLevel(i);
                return;
            }
        }
        LoadLevel(levelDatas.Length - 1);
    }

    private void LoadLevel(int level)
    {
        GameManager.LevelNow = level;
        SceneManager.LoadScene("Game");
    }

    public void OpenPolicyPrivicy ()
    {
        BrowserUtils.OpenUrl(_privacyLink);
    }
}

[System.Serializable]
public class LevelData
{
    public int StarsComplite = -1;

    public bool IsComplite => StarsComplite >= 0;

    public LevelData(int starsComplite)
    {
        StarsComplite = starsComplite;
    }
}