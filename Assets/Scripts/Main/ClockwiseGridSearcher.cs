using UnityEngine;

namespace Main
{
    public class ClockwiseGridSearcher : GridSearcher
    {
        public Vector2Int Center { get; private set; }

        int _ring = -1;
        int _index = -1;
        Cell _cell = null;

        public ClockwiseGridSearcher(Grid grid, Vector2Int center) : base(grid)
        {
            Center = center;
        }

        public override Cell TryGetNext()
        {
            var maxRings = Mathf.Max(
                Mathf.Max(Mathf.Abs(Center.x), Mathf.Abs(Center.x - Grid.Size.x)),
                Mathf.Max(Mathf.Abs(Center.y), Mathf.Abs(Center.y - Grid.Size.y))
            );

            if (_index >= 0)
            {
                _index++;
                IterateToNext();
            }

            while (_index == -1 && _ring <= maxRings)
            {
                _ring++;
                _index = 0;
                IterateToNext();
            }

            return _index == -1 ? null : _cell;
        }

        void IterateToNext()
        {
            if (_index == -1)
                return;

            if (_ring == 0)
            {
                if (_index == 0 && CheckCell(Center.x, Center.y)) return;
                _index = -1;
                return;
            }

            var edgeSize = (_ring * 2) + 1;
            var ringSize = (edgeSize * 4) - 4;
            var sideSize = ringSize / 4;

            var iTop = sideSize * 1;
            var iRight = sideSize * 2;
            var iBottom = sideSize * 3;
            var iLeft = sideSize * 4;

            var min = Center - Vector2Int.one * _ring;
            var max = Center + Vector2Int.one * _ring;

            if (_index < iTop)
            {
                if (max.y < 0 || max.y >= Grid.Size.y) _index = iTop;
                else for (; _index < iTop; _index++) if (CheckCell(min.x + (_index), max.y)) return;
            }
            if (_index < iRight)
            {
                if (max.x < 0 || max.x >= Grid.Size.x) _index = iRight;
                else for (; _index < iRight; _index++) if (CheckCell(max.x, max.y - (_index - iTop))) return;
            }
            if (_index < iBottom)
            {
                if (min.y < 0 || min.y >= Grid.Size.y) _index = iBottom;
                else for (; _index < iBottom; _index++) if (CheckCell(max.x - (_index - iRight), min.y)) return;
            }
            if (_index < iLeft)
            {
                if (min.x < 0 || min.x >= Grid.Size.x) _index = iLeft;
                else for (; _index < iLeft; _index++) if (CheckCell(min.x, min.y + (_index - iBottom))) return;
            }

            _index = -1;
        }

        bool CheckCell(int x, int y)
        {
            if (x < 0 || x >= Grid.Size.x) return false;
            if (y < 0 || y >= Grid.Size.y) return false;
            _cell = Grid.GetCell(x, y);
            return _cell.IsFree;
        }
    }
}
