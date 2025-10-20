using UnityEngine;
using System;
using System.Collections;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] private GameObject _hideGO;
    [SerializeField] private GameObject _vfxDead;
    [SerializeField] private GameObject _vfxInvictible;
    private float _invictibleKill;

    public event Action OnDamaged; // Событие для обработки получения урона

    private void Start()
    {
        BonusMediator.Instance.Subscribe<BonusInvictibleData>((inv) => SetInvictible(inv.TimeToInvictible));
    }

    private void SetInvictible(float time)
    {
        _invictibleKill = time;
        _vfxInvictible.SetActive(true);
    }

    private void Update()
    {
        if (_invictibleKill > 0f)
        {
            _invictibleKill -= Time.deltaTime;
            if (_invictibleKill <= 0f)
                NormalizeInvictible();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что объект, с которым произошло столкновение, является источником урона
        if (other.TryGetComponent<DamageTrigger>(out var damageTrigger))
        {
            if (_invictibleKill <= 0 || damageTrigger.AlwaysKill)
                HandleDamage();
        }
    }

    public void HandleDamage()
    {
        if (gameObject.activeInHierarchy == false)
            return;

        OnDamaged?.Invoke();  // Вызываем событие получения урона
        Die();
        Debug.Log($"Damage!");
    }

    public void Die ()
    {
        Destroy(Instantiate(_vfxDead, transform.position, transform.rotation), 5f);
        _hideGO.SetActive(false);
#if UNITY_ANDROID
        if(GameSettings.IsVibrationPlay)
            Handheld.Vibrate();
#endif
    }

    public void NormalizeInvictible()
    {
        _invictibleKill = 0f;
        _vfxInvictible.SetActive(false);
    }
}
