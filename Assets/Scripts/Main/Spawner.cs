using Common;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Main
{
    public class Spawner : CellOccupier
    {
        ObjectPool<Ball> _ballPool = null;
        Coroutine _spawnerCoroutine = null;

        private void Start()
        {
            var ballPrefab = GameContext.Current.LevelPrefabsStorage.ball;
            _ballPool = new(
                () => ballPrefab.Instantiate<Ball>(),
                ball => { ball.gameObject.SetActive(true); },
                ball => { ball.gameObject.SetActive(false); },
                ball => { Destroy(ball.gameObject); },
                false,
                0,
                GameContext.Current.Grid.Size.x * GameContext.Current.Grid.Size.y
            );
        }

        private void OnDisable()
        {
            StopSpawner();
        }

        private void OnDestroy()
        {
            _ballPool?.Dispose();
            _ballPool = null;
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                StartSpawner();
            if (Input.GetKeyUp(KeyCode.Space))
                StopSpawner();
        }


        public bool IsSpawnerActive()
        {
            return _spawnerCoroutine != null;
        }
        public void StartSpawner()
        {
            if (IsSpawnerActive()) return;

            _spawnerCoroutine = StartCoroutine(SpawnerCoroutine());
        }
        public void StopSpawner()
        {
            if (!IsSpawnerActive()) return;

            StopCoroutine(_spawnerCoroutine);
            _spawnerCoroutine = null;
        }

        IEnumerator SpawnerCoroutine()
        {
            var searcher = new ClockwiseGridSearcher(GameContext.Current.Grid, Cell.Position);

            while (true)
            {
                var cell = searcher.TryGetNext();
                if (cell != null)
                {
                    SpawnBall(cell);
                }
                yield return null;
            }
        }

        void SpawnBall(Cell targetCell)
        {
            var ball = _ballPool.Get();
            ball.transform.position = transform.position;

            var variant = GameContext.Current.LevelConfig.ballVariants.GetRandom();
            ball.Bind(variant);

            targetCell.SetOccupier(ball);
            ball.AnimateToCell();
        }
    }
}
