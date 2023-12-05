using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main.Algorithm
{
    public class NearbyGridSearcher : ClockwiseGridSearcher
    {
        public Vector2 World { get; private set; }

        List<Cell> _currentRing = new();

        public NearbyGridSearcher(Grid grid, Vector2 world) : base(grid, grid.WorldToCell(world))
        {
            World = world;
        }

        public override Cell FindNext()
        {
            if (_currentRing.Count == 0)
                CollectRing();

            if (_currentRing.Count == 0)
                return null;

            var closest = _currentRing.OrderBy(c => Vector2.Distance(c.transform.position, World)).First();
            _currentRing.Remove(closest);
            return closest;
        }

        void CollectRing()
        {
            if (ring == -1)
                base.FindNext();

            if (CurrentCell == null)
                return;

            var r = ring;
            while (ring == r)
            {
                _currentRing.Add(CurrentCell);
                base.FindNext();
            }
        }
    }
}
