using System;
using UnityEngine;

namespace Main
{
    public class Cell : MonoBehaviour
    {
        static readonly Vector2Int UNINITIALIZED_POSITION = new(int.MinValue, int.MinValue);

        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Color colorOdd;
        [SerializeField] Color colorEven;
        [SerializeField] Color colorBlocked;

        public Vector2Int Position { get; private set; } = UNINITIALIZED_POSITION;
        public bool IsBlocked { get; private set; } = false;

        public void Initialize(Vector2Int position)
        {
            if (Position != UNINITIALIZED_POSITION)
                throw new Exception("Cell already initialized");

            Position = position;
            transform.position = GameContext.Current.Grid.CellToWorld(position);

            UpdateVisual();
        }

        public void SetBlocked(bool blocked)
        {
            IsBlocked = blocked;
            UpdateVisual();
        }

        public void UpdateVisual()
        {
            if (IsBlocked)
            {
                spriteRenderer.color = colorBlocked;
            }
            else
            {
                var v = (Position.x % 2) + (Position.y % 2);
                spriteRenderer.color = v == 1 ? colorOdd : colorEven;
            }
        }
    }
}
