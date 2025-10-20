using System;
using UnityEngine;
using DG.Tweening;

public class WindowUI : MonoBehaviour
{
    [Header("�������� ����")]
    [SerializeField] private Transform target;
    [SerializeField] private float openDuration = 0.3f;
    [SerializeField] private Ease openEase = Ease.OutBack;
    [SerializeField] private bool useBlackScreen = true;

    [Header("Punch ������ ����� ��������")]
    [SerializeField] private bool usePunch = true;
    [SerializeField] private Vector3 punchStrength = new Vector3(0.2f, 0.2f, 0);
    [SerializeField] private float punchDuration = 0.3f;
    [SerializeField] private int punchVibrato = 10;
    [SerializeField] private float punchElasticity = 1f;

    private Tween currentTween;

    public Action<WindowUI> OnClose;
    public Action<WindowUI> OnOpen;

    public static Action OnClickMenu;
    public static Action OnClickRestart;
    public static Action OnClickNextLvl;

    private bool isOpen;
    public bool IsOpen => isOpen;

    public bool UseBlackScreen => useBlackScreen;

    public virtual void Open()
    {
        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();

        if (useBlackScreen)
            InterfaceManager.ShowBlackScreenOnLayer(this);

        target.transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        target.gameObject.SetActive(true);

        // �������� ��������
        currentTween = target.transform.DOScale(Vector3.one, openDuration)
            .SetEase(openEase)
            .OnComplete(() =>
            {
                // ���� ��������
                if (usePunch)
                {
                    currentTween = target.transform.DOPunchScale(
                        punchStrength,
                        punchDuration,
                        punchVibrato,
                        punchElasticity
                    );
                }
            });

        isOpen = true;
        OnOpen?.Invoke(this);

        InterfaceManager.AddInOpenList(this);
    }

    public virtual void Close()
    {
        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();

        isOpen = false;
        gameObject.SetActive(false);
        InterfaceManager.CloseWindowWork(this);

        OnClose?.Invoke(this);

        InterfaceManager.RemoveInOpenList(this);
    }

    public void ClickMenu()
    {
        OnClickMenu?.Invoke();
    }

    public void ClickRestart()
    {
        OnClickRestart?.Invoke();
    }

    public void ClickNextLvl()
    {
        OnClickNextLvl?.Invoke();
    }
}