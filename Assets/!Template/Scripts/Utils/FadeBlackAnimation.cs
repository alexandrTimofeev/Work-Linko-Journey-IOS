using DG.Tweening;
using System.Collections;
using UnityEngine;

public class FadeBlackAnimation : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;

    [Header("Durations")]
    [SerializeField] private float showDuration = 1f;     // Появление
    [SerializeField] private float fadeDuration = 1f;    // Исчезновение
    [SerializeField] private bool useInteractible = true;
    [SerializeField] private bool useBlockRaycast = true;

    public void Fade()
    {
        group.DOKill();
        group.DOFade(0f, fadeDuration);

        if (useInteractible)
            group.interactable = false;
        if (useBlockRaycast)
            group.blocksRaycasts = false;
    }

    public void Show()
    {
        group.DOKill();
        group.DOFade(1f, showDuration);

        if (useInteractible)
            group.interactable = true;
        if (useBlockRaycast)
            group.blocksRaycasts = true;
    }
}