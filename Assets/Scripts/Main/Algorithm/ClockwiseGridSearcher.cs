using UnityEngine;

namespace Main.Algorithm
{
    public class ClockwiseGridSearcher : GridSearcher
    {
        public Vector2Int Center { get; private set; }
        public Cell CurrentCell { get; private set; }

        protected int maxRings { get; private set; } = -1;
        protected int ring { get; private set; } = -1;
        protected int index { get; private set; } = -1;

        public ClockwiseGridSearcher(Grid grid, Vector2Int center) : base(grid)
        {
            Center = center;

            maxRings = Mathf.Max(
                Mathf.Max(Mathf.Abs(Center.x), Mathf.Abs(Center.x - Grid.Size.x)),
                Mathf.Max(Mathf.Abs(Center.y), Mathf.Abs(Center.y - Grid.Size.y))
            );
        }

        public override Cell FindNext()
        {
            if (index >= 0)
            {
                index++;
                IterateToNext();
            }

            while (index == -1 && ring <= maxRings)
            {
                ring++;
                index = 0;
                IterateToNext();
            }

            if (index == -1)
                CurrentCell = null;

            return CurrentCell;
        }

        void IterateToNext()
        {
            if (index == -1)
                return;

            if (ring == 0)
            {
                if (index == 0 && CheckCell(Center.x, Center.y)) return;
                index = -1;
                return;
            }

            var edgeSize = (ring * 2) + 1;
            var ringSize = (edgeSize * 4) - 4;
            var sideSize = ringSize / 4;

            var iTop = sideSize * 1;
            var iRight = sideSize * 2;
            var iBottom = sideSize * 3;
            var iLeft = sideSize * 4;

            var min = Center - Vector2Int.one * ring;
            var max = Center + Vector2Int.one * ring;

            if (index < iTop)
            {
                if (max.y < 0 || max.y >= Grid.Size.y) index = iTop;
                else for (; index < iTop; index++) if (CheckCell(min.x + (index), max.y)) return;
            }
            if (index < iRight)
            {
                if (max.x < 0 || max.x >= Grid.Size.x) index = iRight;
                else for (; index < iRight; index++) if (CheckCell(max.x, max.y - (index - iTop))) return;
            }
            if (index < iBottom)
            {
                if (min.y < 0 || min.y >= Grid.Size.y) index = iBottom;
                else for (; index < iBottom; index++) if (CheckCell(max.x - (index - iRight), min.y)) return;
            }
            if (index < iLeft)
            {
                if (min.x < 0 || min.x >= Grid.Size.x) index = iLeft;
                else for (; index < iLeft; index++) if (CheckCell(min.x, min.y + (index - iBottom))) return;
            }

            index = -1;
        }

        bool CheckCell(int x, int y)
        {
            if (x < 0 || x >= Grid.Size.x) return false;
            if (y < 0 || y >= Grid.Size.y) return false;
            CurrentCell = Grid.GetCell(x, y);
            return CurrentCell.IsFree;
        }
    }
}
