using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "CoinBonus", menuName = "Bonuses/CoinBonus")]
public class CoinBonus : BonusData
{
    public int Coins = 100;

    public override void Use()
    {
        BonusMediator.Instance.Notify(this);
    }
}