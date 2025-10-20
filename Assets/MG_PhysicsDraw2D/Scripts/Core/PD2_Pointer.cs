using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG_PhysicsDraw2D
{
    public class PD2_Pointer : MonoBehaviour
    {
        [HideInInspector] public CircleCollider2D circleCollider;
        TargetJoint2D _targetJoint;
        // v5.1.1 - added RigdBody reference in Pointer script
        Rigidbody2D _rigbody2D;
        public Transform Transform { get; set; }
        PD2_DrawingManager _drawingManager;
        public Vector3 Position
        {
            get
            {
                return Transform.position;
            }
        }

        void Awake()
        {
            circleCollider = GetComponent<CircleCollider2D>();
            _targetJoint = GetComponent<TargetJoint2D>();
            _rigbody2D = GetComponent<Rigidbody2D>();
            Transform = transform;
            _drawingManager = PD2_DrawingManager.Instance;

            // v5.1 - bugfix: fixed new layer "CurrentDrawing" being set
            if (LayerMask.LayerToName(gameObject.layer) == "")
                gameObject.layer = LayerMask.NameToLayer("CurrentDrawing");
        }

        void OnEnable()
        {
            PD2_DrawingManager.Instance.eventManager.StartListening(PD2Event.OnUpdate, FollowPointer);
        }

        void OnDisable()
        {
            PD2_DrawingManager.Instance.eventManager.StopListening(PD2Event.OnUpdate, FollowPointer);
        }

        // v5.1.1 - added BringPointerToTouchPoint method to the Pointer script
        public void BringPointerToTouchPoint()
        {
            Vector3 pointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pointerPosition.z = _drawingManager.Transform.position.z;
            _targetJoint.target = pointerPosition;
            this.transform.position = pointerPosition;
            _rigbody2D.linearVelocity = Vector2.zero;
        }

        public void FollowPointer()
        {
            Vector3 pointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pointerPosition.z = _drawingManager.Transform.position.z;
            _targetJoint.target = pointerPosition;
        }
    }
}