using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _numText;
    [SerializeField] private Image _buttonImg;
    [SerializeField] private Image[] _stars;

    [Space]
    [SerializeField] private Sprite _spriteLock;
    [SerializeField] private Sprite _spritePlay;
    [SerializeField] private Sprite _spritePast;

    public Button Button => _button;

    public void Init(int num, int starsCount, bool isPast = true, bool isLock = false)
    {
        _button.interactable = isLock == false;
        if (isLock)
        {
            _buttonImg.sprite = _spriteLock;
            _numText.text = $"";
        }
        else
        {
            _numText.text = $"{num + 1}";
            if (isPast)
                _buttonImg.sprite = _spritePast;
            else
                _buttonImg.sprite = _spritePlay;
        }

        for (int i = 0; i < _stars.Length; i++)
        {
            _stars[i].gameObject.SetActive(isLock == false && i < starsCount);
        }
    }
}