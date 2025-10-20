using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Collider2DOptimization;

namespace MG_PhysicsDraw2D
{
    public class PD2_Drawing : MonoBehaviour
    {
        [HideInInspector] public LineRenderer lineRenderer;
        [HideInInspector] public Rigidbody2D rigidbody2;
        [HideInInspector] public PolygonCollider2D polygonCollider;
        [HideInInspector] public int pointsCount = 0;

        [HideInInspector] public Vector3 startPosition;
        [HideInInspector] public float length = 0;

        public float lifeTime = 0;
        public bool fixedPosition = false;
        public bool stopDrawingIfCollide = true;
        public float maxLength = 0;
        public float optimization = 0.01f;

        List<Vector2> vertices0 = new List<Vector2>();
        List<Vector2> vertices1 = new List<Vector2>();
        PD2_DrawingManager _drawingManager;
        PD2_FreezableElement _freezableElement;

        private void OnValidate()
        {
            // v5.1 - bugfix: fixed new layer "CurrentDrawing" not being set
            if (LayerMask.LayerToName(gameObject.layer) == "")
            {
                gameObject.layer = LayerMask.NameToLayer("CurrentDrawing");
                PD2_DrawingManager.Instance.noStopDrawingLayers = (1 << gameObject.layer);
            }
        }

        void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            rigidbody2 = GetComponent<Rigidbody2D>();
            polygonCollider = GetComponent<PolygonCollider2D>();
            _drawingManager = PD2_DrawingManager.Instance;
            _freezableElement = GetComponent<PD2_FreezableElement>();
        }

        LayerMask _colLayer;

        void OnEnable()
        {
            _drawingManager.AddDrawing(this);

            _colLayer = ~_drawingManager.noStopDrawingLayers;
        }

        void OnDisable()
        {
            // v5.1 - bugfix: fixed Drawing Manager null when resetting scene if lifeTime > 0
            if (lifeTime > 0)
                PD2_DrawingManager.Instance?.eventManager.StopListening(PD2Event.OnUpdate, ManageLifeTime);

            _drawingManager.RemoveDrawing(this);
        }

        public static PD2_Drawing StartDrawing(Vector3 position)
        {
            PD2_DrawingManager manager = PD2_DrawingManager.Instance;
            PD2_Drawing drawing = Instantiate(manager.drawingTemplate, position, Quaternion.identity, manager.drawingContainer);
            manager.AddDrawing(drawing);
            drawing.startPosition = position;

            int layerMaskNotCurrentDrawing = ~(LayerMask.GetMask("CurrentDrawing"));
            RaycastHit2D hit = Physics2D.Raycast(position, Vector3.forward, 100, layerMaskNotCurrentDrawing);
            if (hit.collider != null)
            {
                drawing._drawingManager.OnPointerUp();
            }

            return drawing;
        }

        public void AddPoint(Vector3 position)
        {
            Vector3 worldPos = position - startPosition;
            pointsCount++;
            lineRenderer.positionCount = pointsCount;
            lineRenderer.SetPosition(pointsCount - 1, worldPos);

            if (pointsCount > 1)
            {
                float lengthToAdd = Vector3.Distance(lineRenderer.GetPosition(pointsCount - 2), worldPos);
                length += lengthToAdd;
            }

            UpdateCollider();
        }

        void UpdateCollider(bool skipTest = false)
        {
            if (!skipTest && stopDrawingIfCollide)
            {
                for (int i = 1; i < lineRenderer.positionCount; i++)
                {
                    Vector3 vp1 = lineRenderer.GetPosition(i - 1);
                    Vector3 vp2 = lineRenderer.GetPosition(i);
                    for (float t = 0; t <= 1; t += 0.1f)
                    {
                        Vector3 point = Vector3.Lerp(vp1, vp2, t);


                        RaycastHit2D hit = Physics2D.Raycast(point + startPosition, Vector3.forward, 100, _colLayer);

                        if (hit.collider != null)
                        {
                            Vector3 newPos = (Vector3)Vector3.Lerp(vp1, vp2, t - 0.1f);
                            lineRenderer.SetPosition(pointsCount - 1, newPos);

                            UpdateCollider(true);

                            _drawingManager.OnPointerUp();
                            return;
                        }
                    }
                }
            }

            vertices0.Clear();
            vertices1.Clear();
            List<Vector2> vertices = new List<Vector2>();

            Mesh mesh = new Mesh();
            lineRenderer.BakeMesh(mesh);

            Matrix4x4 localToWorld = _drawingManager.Transform.localToWorldMatrix;
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                if (!skipTest && (stopDrawingIfCollide && i > 0))
                {
                    Vector3 vp1 = mesh.vertices[i - 1];
                    Vector3 vp2 = mesh.vertices[i];
                    for (float t = 0; t <= 1; t += 0.1f)
                    {
                        Vector3 point = Vector3.Lerp(vp1, vp2, t);

                        RaycastHit2D hit = Physics2D.Raycast(localToWorld.MultiplyPoint3x4(point + startPosition), Vector3.forward, 100, _colLayer);

                        if (hit.collider != null)
                        {

                            Vector3 newPos = (Vector3)localToWorld.MultiplyPoint3x4(Vector3.Lerp(vp1, vp2, t - 0.1f) + startPosition) - startPosition;
                            lineRenderer.SetPosition(pointsCount - 1, newPos);

                            UpdateCollider(true);

                            _drawingManager.OnPointerUp();
                            return;
                        }
                    }
                }

                if (i % 2 == 0)
                {
                    vertices0.Add(localToWorld.MultiplyPoint3x4(mesh.vertices[i]));
                }
                else
                {
                    vertices1.Insert(0, localToWorld.MultiplyPoint3x4(mesh.vertices[i]));
                }
            }

            vertices.AddRange(vertices0);
            vertices.AddRange(vertices1);
            polygonCollider.SetPath(0, vertices);

            // PolygonColliderOptimizer.Optimize(polygonCollider, optimization);
        }

        public void FinishDrawing()
        {
            rigidbody2.constraints = RigidbodyConstraints2D.None;
            if (!fixedPosition)
            {
                rigidbody2.bodyType = RigidbodyType2D.Dynamic;
            }
            else
            {
                rigidbody2.bodyType = RigidbodyType2D.Static;
            }

            polygonCollider.isTrigger = false;
            PolygonColliderOptimizer.Optimize(polygonCollider, optimization);

            if (lifeTime > 0)
                PD2_DrawingManager.Instance.eventManager.StartListening(PD2Event.OnUpdate, ManageLifeTime);
        }

        public void SetLifeTime(float lifeTime, float lifeTimeDelayPerPoint)
        {
            this.lifeTime = lifeTime;

            if (lifeTime > 0)
            {
                PD2_DrawingManager.Instance.eventManager.StartListening(PD2Event.OnUpdate, ManageLifeTime);
            }
            else
            {
                PD2_DrawingManager.Instance.eventManager.StopListening(PD2Event.OnUpdate, ManageLifeTime);
            }
        }

        void ManageLifeTime()
        {
            if (lifeTime > 0)
            {
                lifeTime -= Time.deltaTime;
            }
            else
            {
                PD2_DrawingManager.Instance.eventManager.StopListening(PD2Event.OnUpdate, ManageLifeTime);
                _drawingManager.RemoveDrawing(this);
            }
        }

        public void Destroy()
        {
            _drawingManager.RemoveDrawing(this);
        }
    }
}