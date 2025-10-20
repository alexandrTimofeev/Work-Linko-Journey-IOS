using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG_PhysicsDraw2D
{
    public class PD2_FreezableElement : MonoBehaviour
    {
        Rigidbody2D _rigidbody2;
        Vector2 _velocity;
        float _angularVelocity;

        void Awake()
        {
            _rigidbody2 = GetComponent<Rigidbody2D>();
        }

        void OnEnable()
        {
            if (this.gameObject != PD2_DrawingManager.Instance.drawingTemplate.gameObject)
            {
                PD2_DrawingManager.Instance.freezableElementList.Add(this);
            }
        }

        void OnDisable()
        {
            if (PD2_DrawingManager.Instance && PD2_DrawingManager.Instance.freezableElementList.Contains(this))
                PD2_DrawingManager.Instance.freezableElementList.Remove(this);
        }

        public void Freeze()
        {
            _velocity = _rigidbody2.linearVelocity;
            _angularVelocity = _rigidbody2.angularVelocity;
            _rigidbody2.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public void Unfreeze()
        {
            if (_rigidbody2.bodyType != RigidbodyType2D.Static)
            {
                _rigidbody2.constraints = RigidbodyConstraints2D.None;
                _rigidbody2.linearVelocity = _velocity;
                _rigidbody2.angularVelocity = _angularVelocity;
                _rigidbody2.WakeUp();
            }
        }
    }
}