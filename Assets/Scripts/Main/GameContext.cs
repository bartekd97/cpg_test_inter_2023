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

        public bool IsInitialized { get; private set; } = false;
        public LevelConfig LevelConfig { get; private set; } = null;

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
                throw new Exception("Already initialized.");

            LevelConfig = levelConfigProvider.Provide();

            IsInitialized = true;
        }
    }
}
