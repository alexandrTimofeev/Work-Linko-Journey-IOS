using UnityEngine;

public class PlayerSavable : MonoBehaviour
{
    [SerializeField] private Vector3 lastCheckpointPosition;  // Последняя сохраненная позиция чекпоинта
    private Vector3 _startPoint;

    public Vector3 StartPoint => _startPoint;

    private void Awake()
    {
        _startPoint = transform.position;
    }

    // Сохраняем состояние игрока
    public void SaveState(Vector3 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;  // Сохраняем позицию чекпоинта
    }

    // Восстанавливаем состояние игрока
    public void LoadState()
    {
        transform.position = lastCheckpointPosition;  // Восстанавливаем позицию
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Когда игрок касается чекпоинта, сохраняем его позицию
        Checkpoint checkpoint = other.GetComponent<Checkpoint>();
        if (checkpoint != null)
        {
            checkpoint.OnPlayerTouch();
        }
    }
}