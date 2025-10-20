using UnityEngine;

[DisallowMultipleComponent]
public abstract class BonusData : ScriptableObject
{
    public string bonusName;
    public string description;
    public AudioClip clipGrab;
    public GameObject vfx;

    public abstract void Use();
}
