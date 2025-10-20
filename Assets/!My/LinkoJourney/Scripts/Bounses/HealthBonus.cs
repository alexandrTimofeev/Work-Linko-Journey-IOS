using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Health", menuName = "Bonuses/HealthBonus")]
public class HealthBonus : BonusData
{
    public override void Use()
    {
        BonusMediator.Instance.Notify(this);
    }
}