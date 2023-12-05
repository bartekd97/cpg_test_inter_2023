using Common;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Main
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour,
        IInitializePotentialDragHandler,
        IDragHandler
    {
        [SerializeField] BoxCollider2D backgroundCollider;
        [SerializeField] float maxZoomAreaMultiplier = 0.25f;
        [SerializeField] float minZoomAreaMultiplier = 1.25f;

        public Camera Camera { get; private set; } = null;
        public Rect Area { get; private set; } = Rect.zero;
        public float Zoom { get; private set; } = 0.0f;
        public float AspectRatio => (float)Camera.pixelWidth / Camera.pixelHeight;


        Rect _currentMovementArea = Rect.zero;

        private void Awake()
        {
            Camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus)) SetZoom(Zoom + 0.2f);
            if (Input.GetKeyDown(KeyCode.KeypadMinus)) SetZoom(Zoom - 0.2f);
        }

        public void SetArea(Rect area)
        {
            Area = area;
            UpdateCameraArea();
        }

        public void SetZoom(float zoom)
        {
            Zoom = Mathf.Clamp01(zoom);
            UpdateCameraArea();
        }

        public void UpdateCameraArea()
        {
            if (Area.size.x == 0 || Area.size.y == 0)
                return;

            var zoomMultiplier = Mathf.Lerp(minZoomAreaMultiplier, maxZoomAreaMultiplier, Zoom);
            var visibleAreaSize = Area.size * zoomMultiplier;
            var areaRatio = visibleAreaSize.x / visibleAreaSize.y;

            if (areaRatio > AspectRatio)
            {
                Camera.orthographicSize = visibleAreaSize.y * 0.5f * (areaRatio / AspectRatio);
                visibleAreaSize.y *= (areaRatio / AspectRatio);
            }
            else
            {
                visibleAreaSize.x *= (AspectRatio / areaRatio);
                Camera.orthographicSize = visibleAreaSize.y * 0.5f;
            }

            var clampedVisibleArea = new Vector2(
                Mathf.Min(Area.size.x, visibleAreaSize.x),
                Mathf.Min(Area.size.y, visibleAreaSize.y)
            );
            _currentMovementArea = new(
                Area.position + clampedVisibleArea * 0.5f,
                Area.size - clampedVisibleArea
            );

            AdjustBackgroundCollider();
            MoveCameraTo(transform.position);
        }


        void AdjustBackgroundCollider()
        {
            backgroundCollider.size = new(
                Camera.orthographicSize * 2.0f * AspectRatio,
                Camera.orthographicSize * 2.0f
            );
        }

        void MoveCameraTo(Vector3 position)
        {
            var clamped = _currentMovementArea.Clamp(position);
            transform.position = new(clamped.x, clamped.y, transform.position.z);
        }

        void IInitializePotentialDragHandler.OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            var from = Camera.ScreenToWorldPoint(eventData.position);
            var to = Camera.ScreenToWorldPoint(eventData.position + eventData.delta);
            var delta = to - from;
            MoveCameraTo(transform.position - delta);
        }
    }
}
