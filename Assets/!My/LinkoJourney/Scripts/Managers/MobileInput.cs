using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MobileInput : MonoBehaviour, IInput
{
    public event Action<float> OnHorizontalMove;
    public event Action OnJumpDown;
    public event Action OnJumpHold;
    public event Action OnJumpUp;
    public event Action OnClick;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
                OnClick?.Invoke();

            if (IsPointerOverUIObject(Input.touches[0].position) == false)
            {
                switch (Input.touches[0].phase)
                {
                    case TouchPhase.Began:
                        OnJumpDown?.Invoke();
                        break;
                    case TouchPhase.Stationary:
                        OnJumpHold?.Invoke();
                        break;
                    case TouchPhase.Ended:
                        OnJumpUp?.Invoke();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private bool IsPointerOverUIObject(Vector3 point)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = point
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }

}