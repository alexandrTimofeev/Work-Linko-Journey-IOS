using UnityEngine;
using System;

public class PlayerCharacter : MonoBehaviour
{
    public event Action OnDamaged;  // Вызывается при столкновении с DamageTrigger
    public event Action<BonusData> OnBonusPicked; // Вызывается при подборе бонуса
    public GameObject deathEffect; // Объект, создаваемый при смерти

    private bool isDead = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.TryGetComponent<DamageTrigger>(out var damageTrigger))
        {
            OnDamaged?.Invoke();
            HandleDeath();
        }
        else if (other.TryGetComponent<PickupItem>(out var pickup))
        {
            pickup.Use();
            OnBonusPicked?.Invoke(pickup.BonusData);
        }
    }

    private void HandleDeath()
    {
        isDead = true;
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}