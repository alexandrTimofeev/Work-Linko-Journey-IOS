using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class InterfaceManagerGame : MonoBehaviour
{
    [SerializeField] private Image _progressBar;
    [SerializeField] private TextMeshProUGUI _coinsTmp;
    [SerializeField] private Image[] _imagesLives; 

    public void Init (GameManager gameManager, ResourceManager resourceManager)
    {
        gameManager.OnProgress += UpdateGameProgress;
        resourceManager.OnChangeCoins += (c) => UpdateCoinsInfo(c, resourceManager.CoinsCountInLevel);
        resourceManager.OnChangeLives += UpdateLivesInfo;

        UpdateGameProgress(0f);
        foreach (var live in _imagesLives)
        {
            live.gameObject.SetActive(false);
        }
    }

    public void UpdateGameProgress (float value)
    {
        _progressBar.fillAmount = value;
    }

    public void UpdateCoinsInfo (int coins, int coinsAll)
    {
        _coinsTmp.text = $"{coins}/{coinsAll}";
    }

    public void UpdateLivesInfo (int lives)
    {
        for (int i = 0; i < _imagesLives.Length; i++)
        {
            if (i < lives)
            {
                if (_imagesLives[i].gameObject.activeInHierarchy == false)
                {
                    _imagesLives[i].gameObject.SetActive(true);
                    _imagesLives[i].transform.localScale = Vector3.zero;
                    _imagesLives[i].transform.DOScale(Vector3.one, 0.8f);
                }
            }
            else
            {
                if (_imagesLives[i].gameObject.activeInHierarchy)
                {
                    int n = i;
                    _imagesLives[i].transform.localScale = Vector3.one;
                    _imagesLives[i].transform.DOPunchScale(Vector3.one * 0.4f, 1f, 10, 1).SetEase(Ease.Linear).OnComplete(() => _imagesLives[n].gameObject.SetActive(false));
                }
            }
        }
    }
}