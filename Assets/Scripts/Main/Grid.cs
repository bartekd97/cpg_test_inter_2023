using Common;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main
{
    public class Grid : MonoBehaviour
    {
        public Vector2Int Size { get; private set; } = Vector2Int.zero;
        public Rect WorldRect { get; private set; } = Rect.zero;

        Cell[,] _cells = null;

        public void Initialize()
        {
            if (Size != Vector2Int.zero)
                throw new Exception("Grid already initialized.");

            var config = GameContext.Current.LevelConfig.gridSetup;
            var storage = GameContext.Current.LevelPrefabsStorage;

            Size = new(config.width, config.height);
            WorldRect = new(-(Vector2)Size * 0.5f, Size);

            var randomState = Random.state;
            if (config.seed != 0)
                Random.InitState(config.seed);

            _cells = new Cell[config.width, config.height];
            for (int x = 0; x < config.width; x++)
            {
                for (int y = 0; y < config.height; y++)
                {
                    _cells[x, y] = storage.cell.Instantiate<Cell>(transform);
                    _cells[x, y].Initialize(new(x, y));
                    _cells[x, y].SetBlocked(Random.value < config.blockChance);
                }
            }

            if (config.seed != 0)
                Random.state = randomState;
        }

        public Vector2Int WorldToCell(Vector2 world)
        {
            var offset = world - (WorldRect.position + Vector2.one * 0.5f);
            var x = Mathf.RoundToInt(offset.x);
            var y = Mathf.RoundToInt(offset.y);
            return new(
                Mathf.Clamp(x, 0, Size.x - 1),
                Mathf.Clamp(y, 0, Size.y - 1)
            );
        }

        public Vector2 CellToWorld(Vector2Int cell)
        {
            return (WorldRect.position + Vector2.one * 0.5f) + cell;
        }
    }
}
