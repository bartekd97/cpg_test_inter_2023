using UnityEngine;

namespace Main
{
    [RequireComponent(typeof(CellOccupier))]
    public class CellOccupierDrag : MonoBehaviour
    {
        CellOccupier _occupier = null;
        private void Awake()
        {
            _occupier = GetComponent<CellOccupier>();
        }

        private void OnMouseDown()
        {
            _occupier.Cell.ClearOccupier();
            SetToCursor();
        }
        private void OnMouseUp()
        {
            var cell = GameContext.Current.Grid.FindNearbyFreeCell(transform.position);
            cell.SetOccupier(_occupier);
            _occupier.AnimateToCell();
        }
        private void OnMouseDrag()
        {
            SetToCursor();
        }

        void SetToCursor()
        {
            var cursor = Input.mousePosition;
            var world = (Vector2)Camera.main.ScreenToWorldPoint(cursor);
            transform.position = world;
        }
    }
}
