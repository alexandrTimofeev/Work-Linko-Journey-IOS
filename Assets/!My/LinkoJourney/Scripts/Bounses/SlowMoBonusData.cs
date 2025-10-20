using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SlowMo", menuName = "Bonuses/SlowMo")]
public class SlowMoBonusData : BonusData
{
    public float SpeedCoef = 0.5f;
    public float Time = 4f;

    public override void Use()
    {
        BonusMediator.Instance.Notify(this);
    }
}