using Common;
using Config;
using Messaging;
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

        [Header("Dependencies")]
        [SerializeField] CameraController cameraController;

        public bool IsInitialized { get; private set; } = false;
        public LevelConfig LevelConfig { get; private set; } = null;
        public LevelPrefabsStorage LevelPrefabsStorage => levelPrefabsStorage;
        public Grid Grid { get; private set; } = null;
        public Spawner Spawner { get; private set; } = null;
        public CameraController CameraController => cameraController;
        public IObjectPool<Ball> BallPool => _ballPool;
        public IReadOnlyList<Ball> ActiveBalls => _activeBalls;


        List<Ball> _activeBalls = new();
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
            if (Current != this)
                return;

            Initialize();
        }

        private void OnEnable()
        {
            SignalBus.AddListener<ClearBallsSignal>(OnClearBallsSignal);
        }
        private void OnDisable()
        {
            SignalBus.RemoveListener<ClearBallsSignal>(OnClearBallsSignal);
        }

        private void OnDestroy()
        {
            if (Current != this)
                return;

            _ballPool?.Dispose();
            _ballPool = null;

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
                ball => { ball.gameObject.SetActive(true); _activeBalls.Add(ball); },
                ball => { ball.gameObject.SetActive(false); _activeBalls.Remove(ball); },
                ball => { if (ball) Destroy(ball.gameObject); },
                false,
                0,
                Grid.Size.x * Grid.Size.y
            );

            CameraController.SetArea(Grid.WorldRect);

            IsInitialized = true;
        }

        public void MarkBallAdjacent(Ball ball)
        {
            _adjacentBalls.Add(ball);
        }
        public void UnmarkBallAdjacent(Ball ball)
        {
            _adjacentBalls.Remove(ball);
        }

        public void ClearBalls(bool adjacentOnly)
        {
            IEnumerable<Ball> collection = adjacentOnly ? _adjacentBalls : _activeBalls.ToArray();
            foreach (var ball in collection)
            {
                ball.Cell.ClearOccupier(false);
                _ballPool.Release(ball);
            }
            _adjacentBalls.Clear();
        }

        void OnClearBallsSignal(ClearBallsSignal signal)
            => ClearBalls(signal.adjacentOnly);
    }
}
