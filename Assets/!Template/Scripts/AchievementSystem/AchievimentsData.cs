using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "AchievimentsData", menuName = "SGames/Achieviments")]
public class AchievimentsData : ScriptableObject
{
    private static AchievimentsData achivFromResource;
    public static AchievimentsData AchivFromResource
    {
        get
        {
            if (achivFromResource == null)
                achivFromResource = Resources.Load<AchievimentsData>("AchievimentsData");
            return achivFromResource;
        }
    }

    [SerializeField] private Achiviement[] allAchiviements = new Achiviement[0];
    public Achiviement[] AllAchiviements => allAchiviements;

    public void ForceUnlock(string id)
    {
        foreach (var achiviement in allAchiviements)
        {
            if (achiviement.Info.ID == id)
                achiviement.ForceUnlock();
        }
    }

    public bool IsUnlockAchiv(string id, bool testCondition = false)
    {
        return allAchiviements.Any((x) => x.Info.ID == id &&
        (x.IsUnlocked || (testCondition && x.CheckUnlock(true))));
    }
}


[System.Serializable]
public class AchivInfo
{
    public string ID;
    public string Title;
    public string InfoTxt;
    public Sprite spriteYes;
    public Sprite spriteNo;

    public Sprite GetSprite(bool contains)
    {
        return contains ? spriteYes : spriteNo;
    }
}

[System.Serializable]
public class Achiviement
{
    public AchivInfo Info;
    [SerializeReference] private AchievementBehaviour achievementBehaviour;

    public bool CheckUnlock(bool isTestCondition = false)
    {
        return achievementBehaviour == null ? true : achievementBehaviour.CheckUnlock(isTestCondition);
    }

    public void ForceUnlock ()
    {
        if (achievementBehaviour == null)
            return;
        achievementBehaviour.ForceUnlock();
    }

    public bool IsUnlocked => achievementBehaviour == null ? true : achievementBehaviour.IsUnlocked;
}