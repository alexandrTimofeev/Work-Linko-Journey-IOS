using UnityEngine;

public class PickupItem : MonoBehaviour, ISceneObject
{
    [SerializeField] private GameObject _vfxDeadObject; // Объект, создаваемый на месте после подбора
    [SerializeField] private BonusData _bonusData; // ScriptableObject с бонусом
    [SerializeField] private bool _reload = true;

    public BonusData BonusData => _bonusData;

    public void Use()
    {
        if (_bonusData != null)        
            _bonusData.Use();        
        if(_vfxDeadObject)
            Instantiate(_vfxDeadObject, transform.position, Quaternion.identity);  // Создаём объект замены
        Die();
    }

    public void Die ()
    {
        if (_reload)
            SceneReloader.HideObject(this);
        else
            Hide();
        //Destroy(gameObject);  // Удаляем объект
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Reload()
    {
        gameObject.SetActive(true);
    }
}
