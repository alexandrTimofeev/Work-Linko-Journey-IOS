using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "InvictibleBonus", menuName = "Bonuses/InvictibleBonus")]
public class BonusInvictibleData : BonusData
{
    public float TimeToInvictible = 4f;

    public override void Use()
    {
        BonusMediator.Instance.Notify(this);
    }
}