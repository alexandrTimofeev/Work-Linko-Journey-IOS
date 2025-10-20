using UnityEngine;

[CreateAssetMenu(fileName = "StarBonus", menuName = "Bonuses/StarBonus")]
public class StarBonusData : BonusData
{
    public override void Use()
    {
        BonusMediator.Instance.Notify(this);
    }
}