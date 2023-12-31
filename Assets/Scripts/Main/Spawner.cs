﻿using Common;
using Main.Algorithm;
using Messaging;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Main
{
    public class Spawner : CellOccupier
    {
        Coroutine _spawnerCoroutine = null;

        private void OnEnable()
        {
            SignalBus.AddListener<StartSpawnerSignal>(OnSpawnerSignal);
            SignalBus.AddListener<StopSpawnerSignal>(OnSpawnerSignal);
        }
        private void OnDisable()
        {
            StopSpawner();

            SignalBus.RemoveListener<StartSpawnerSignal>(OnSpawnerSignal);
            SignalBus.RemoveListener<StopSpawnerSignal>(OnSpawnerSignal);
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
                var cell = searcher.FindNext();
                if (cell != null)
                {
                    SpawnBall(cell);
                }
                yield return null;
            }
        }

        void SpawnBall(Cell targetCell)
        {
            var ball = GameContext.Current.BallPool.Get();
            ball.transform.position = transform.position;

            var variant = GameContext.Current.LevelConfig.ballVariants.GetRandom();
            ball.Bind(variant);

            targetCell.SetOccupier(ball);
            ball.AnimateToCell();
        }


        void OnSpawnerSignal(StartSpawnerSignal _)
            => StartSpawner();
        void OnSpawnerSignal(StopSpawnerSignal _)
            => StopSpawner();
    }
}
