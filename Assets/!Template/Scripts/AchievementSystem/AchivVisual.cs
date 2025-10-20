using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchivVisual : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI tmpTitle;
    private AchivInfo achivInfo;
    private bool contains;

    public event Action<AchivInfo> OnClickOpen;

    public void Init (AchivInfo achivInfo)
    {
        this.achivInfo = achivInfo;

        contains = AchieviementSystem.IsUnlockAchiv(achivInfo.ID, true);
        image.sprite = achivInfo.GetSprite(contains);
        if (tmpTitle)
            tmpTitle.text = achivInfo.Title;
    }   

    public void OpenViewInfo()
    {
        OnClickOpen?.Invoke(achivInfo);
    }
}