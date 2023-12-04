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
        public CellOccupier Occupier { get; private set; } = null;
        public bool IsBlocked { get; private set; } = false;
        public bool HasOccupier => Occupier != null;
        public bool IsFree => !IsBlocked && !HasOccupier;

        public void Initialize(Vector2Int position)
        {
            if (Position != UNINITIALIZED_POSITION)
                throw new Exception("Cell already initialized");

            Position = position;
            transform.position = GameContext.Current.Grid.CellToWorld(position);

            gameObject.name = $"{position.x},{position.y}";

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

        public void SetOccupier(CellOccupier occupier)
        {
            if (HasOccupier)
                throw new Exception($"There is already an occupier on cell {gameObject.name}");
            if (occupier.IsOnCell)
                throw new Exception($"Target occupier {occupier.gameObject.name} is already on {occupier.Cell.gameObject.name} cell");

            Occupier = occupier;
            occupier.Cell = this;
        }
        public void ClearOccupier()
        {
            if (!HasOccupier)
                throw new Exception($"There is no occupier on cell {gameObject.name}");

            Occupier.Cell = null;
            Occupier = null;
        }
    }
}
