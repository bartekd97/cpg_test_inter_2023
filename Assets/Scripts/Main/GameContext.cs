using Common;
using Config;
using System;
using UnityEngine;

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
            if (Current == this)
            {
                Current = null;
            }
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
            Spawner.AnimateToCell();

            IsInitialized = true;
        }
    }
}
