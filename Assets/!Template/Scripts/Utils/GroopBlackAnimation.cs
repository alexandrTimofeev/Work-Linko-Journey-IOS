using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GroopBlackAnimation : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;

    [Header("Durations")]
    [SerializeField] private float fadeInDuration = 1f;     // ���������
    [SerializeField] private float waitDuration = 1f;       // ��������
    [SerializeField] private float fadeOutDuration = 1f;    // ������������

    public void StartAnim()
    {
        group.alpha = 0f;
        group.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();

        // ���������
        seq.Append(group.DOFade(1f, fadeInDuration));

        // ��������
        seq.AppendInterval(waitDuration);

        // ������������
        seq.Append(group.DOFade(0f, fadeOutDuration));

        // �� ���������� � ����� ��������� ������
        seq.OnComplete(() =>
        {
            group.gameObject.SetActive(false);
        });
    }
}