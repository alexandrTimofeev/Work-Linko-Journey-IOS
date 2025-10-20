using UnityEngine;
using System;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _pitchSoundUp = 0.1f;
    public event Action<BonusData> OnBonusPicked; // Событие для обработки бонусов

    private float _pitchNow = 1f;
    private float _pitchSoundBreak = 0.1f;

    private void Update()
    {
        if (_pitchSoundBreak <= 0f)        
            _pitchNow = 0.8f;        
        else
            _pitchSoundBreak -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что объект, с которым произошло столкновение, является предметом для подбора
        if (other.TryGetComponent<PickupItem>(out var pickup))
        {
            HandleItemPickup(pickup);
        }
    }

    public void HandleItemPickup(PickupItem pickup)
    {
        pickup.Use();  // Применяем бонус
        OnBonusPicked?.Invoke(pickup.BonusData);  // Генерируем событие для бонуса

        _audioSource.pitch = _pitchNow;
        _pitchSoundBreak = 1f;
        _audioSource.PlayOneShot(pickup.BonusData.clipGrab);
        _pitchNow += _pitchSoundUp;

        if (pickup.BonusData.vfx)
            Instantiate(pickup.BonusData.vfx, pickup.transform.position, transform.rotation);
    }
}
