using UnityEngine;
using UnityEngine.UI;

public class TileScaleUp : MonoBehaviour
{
    [SerializeField] private Vector2 speed;

    private RawImage rawImage;

    void Start()
    {
        rawImage = GetComponent<RawImage>(); 
    }

    void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.x + (speed.x * Time.deltaTime), rawImage.uvRect.y + (speed.y * Time.deltaTime),
            rawImage.uvRect.width, rawImage.uvRect.height);
    }
}
