using UnityEngine;
using UnityEngine.EventSystems;

namespace Main
{
    [RequireComponent(typeof(CellOccupier))]
    public class CellOccupierDrag : MonoBehaviour,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler
    {
        CellOccupier _occupier = null;
        private void Awake()
        {
            _occupier = GetComponent<CellOccupier>();
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _occupier.Cell.ClearOccupier();
            SetToCursor(eventData);
        }
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            SetToCursor(eventData);
        }
        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            var cell = GameContext.Current.Grid.FindNearbyFreeCell(transform.position);
            cell.SetOccupier(_occupier);
            _occupier.AnimateToCell();
        }

        void SetToCursor(PointerEventData eventData)
        {
            var world = (Vector2)(GameContext.Current.CameraController.Camera.ScreenToWorldPoint(eventData.position));
            transform.position = world;
        }
    }
}
