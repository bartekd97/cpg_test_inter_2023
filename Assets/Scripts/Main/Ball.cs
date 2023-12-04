using Config;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class Ball : CellOccupier
    {
        [Header("Ball config")]
        [SerializeField] SpriteRenderer spriteRenderer;

        public BallVariant Variant { get; private set; }

        public void Bind(BallVariant variant)
        {
            Variant = variant;
            UpdateVisual();
        }

        public void UpdateVisual()
        {
            spriteRenderer.color = Variant.tint;
        }

        public override void OnCellChanged()
        {
            base.OnCellChanged();

            var cell = IsOnCell ? Cell : RecentCell;
            foreach (var nb in cell.GetNeighbours())
            {
                if (nb.HasOccupier && nb.Occupier is Ball ball)
                {
                    ball.UpdateBallAdjacency();
                }
            }

            if (IsOnCell)
            {
                UpdateBallAdjacency();
            }
            else
            {
                GameContext.Current.UnmarkBallAdjacent(this);
            }
        }

        void UpdateBallAdjacency()
        {
            var adjacent = false;
            foreach (var nb in Cell.GetNeighbours())
            {
                if (nb.HasOccupier && nb.Occupier is Ball ball && ball.Variant == Variant)
                {
                    adjacent = true;
                    break;
                }
            }

            if (adjacent)
                GameContext.Current.MarkBallAdjacent(this);
            else
                GameContext.Current.UnmarkBallAdjacent(this);
        }
    }
}
