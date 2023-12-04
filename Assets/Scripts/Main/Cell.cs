using System;
using System.Collections.Generic;
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

        List<Cell> _neighbours = null;

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

        public void SetOccupier(CellOccupier occupier, bool notify = true)
        {
            if (HasOccupier)
                throw new Exception($"There is already an occupier on cell {gameObject.name}");
            if (occupier.IsOnCell)
                throw new Exception($"Target occupier {occupier.gameObject.name} is already on {occupier.Cell.gameObject.name} cell");

            Occupier = occupier;
            occupier.Cell = this;

            if (notify)
                occupier.OnCellChanged();
        }
        public void ClearOccupier(bool notifyChanges = true)
        {
            if (!HasOccupier)
                throw new Exception($"There is no occupier on cell {gameObject.name}");

            var occupier = Occupier;

            Occupier.Cell = null;
            Occupier = null;

            if (notifyChanges)
                occupier.OnCellChanged();
        }

        public IReadOnlyList<Cell> GetNeighbours()
        {
            if (_neighbours == null)
            {
                var grid = GameContext.Current.Grid;

                _neighbours = new(4);

                if (Position.y < grid.Size.y - 1) _neighbours.Add(grid.GetCell(Position + Vector2Int.up));
                if (Position.x < grid.Size.x - 1) _neighbours.Add(grid.GetCell(Position + Vector2Int.right));
                if (Position.y > 0) _neighbours.Add(grid.GetCell(Position + Vector2Int.down));
                if (Position.x > 0) _neighbours.Add(grid.GetCell(Position + Vector2Int.left));
            }

            return _neighbours;
        }
    }
}
