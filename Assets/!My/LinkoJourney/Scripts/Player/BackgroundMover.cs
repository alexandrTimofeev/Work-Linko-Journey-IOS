using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    [SerializeField] private float _startX; // Начальная позиция по X
    [SerializeField] private float _endX;   // Конечная позиция по X

    // Метод для обновления позиции фона
    public void UpdatePosition(float t)
    {
        t = Mathf.Clamp01(t); // Ограничиваем значение в диапазоне 0...1
        float newX = Mathf.Lerp(_startX, _endX, t);
        transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
    }
}