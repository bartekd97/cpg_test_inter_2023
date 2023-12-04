using Common;
using Config;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Main
{
    [DefaultExecutionOrder(int.MinValue)]
    public class GameContext : MonoBehaviour
    {
        public static GameContext Current { get; private set; } = null;

        [Header("Configuration")]
        [SerializeField] LevelConfigProvider levelConfigProvider;
        [SerializeField] LevelPrefabsStorage levelPrefabsStorage;

        public bool IsInitialized { get; private set; } = false;
        public LevelConfig LevelConfig { get; private set; } = null;
        public LevelPrefabsStorage LevelPrefabsStorage => levelPrefabsStorage;
        public Grid Grid { get; private set; } = null;
        public Spawner Spawner { get; private set; } = null;
        public IObjectPool<Ball> BallPool => _ballPool;


        ObjectPool<Ball> _ballPool = null;
        HashSet<Ball> _adjacentBalls = new();

        private void Awake()
        {
            if (Current != null)
            {
                Destroy(gameObject);
                return;
            }

            Current = this;
        }

        private void Start()
        {
            if (Current == this)
            {
                Initialize();
            }
        }

        private void OnDestroy()
        {
            if (Current != this)
                return;

            _ballPool?.Dispose();
            Current = null;
        }

        void Initialize()
        {
            if (IsInitialized)
                throw new Exception("Game Context already initialized.");

            LevelConfig = levelConfigProvider.Provide();

            Grid = new GameObject("Grid").AddComponent<Grid>();
            Grid.Initialize();

            Spawner = LevelPrefabsStorage.spawner.Instantiate<Spawner>();
            var spawnerCell = Grid.FindNearbyFreeCell(Vector2.zero);
            spawnerCell.SetOccupier(Spawner);
            Spawner.WarpToCell();

            _ballPool = new(
                () => LevelPrefabsStorage.ball.Instantiate<Ball>(),
                ball => { ball.gameObject.SetActive(true); },
                ball => { ball.gameObject.SetActive(false); },
                ball => { Destroy(ball.gameObject); },
                false,
                0,
                Grid.Size.x * Grid.Size.y
            );

            IsInitialized = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Spawner.StartSpawner();
            if (Input.GetKeyUp(KeyCode.Space))
                Spawner.StopSpawner();

            if (Input.GetKeyDown(KeyCode.C))
                ClearAdjacentBalls();
        }

        public void MarkBallAdjacent(Ball ball)
        {
            _adjacentBalls.Add(ball);
        }
        public void UnmarkBallAdjacent(Ball ball)
        {
            _adjacentBalls.Remove(ball);
        }

        public void ClearAdjacentBalls()
        {
            foreach (var ball in _adjacentBalls)
            {
                ball.Cell.ClearOccupier(false);
                _ballPool.Release(ball);
            }
            _adjacentBalls.Clear();
        }
    }
}
