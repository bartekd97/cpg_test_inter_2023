using System;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    [Serializable]
    public class BallVariant
    {
        public Color tint;
    }

    [Serializable]
    public class GridSetup
    {
        public int width;
        public int height;
        public float blockChance;
        public int seed;
    }

    [Serializable]
    public class LevelConfig
    {
        public GridSetup gridSetup = new();
        public List<BallVariant> ballVariants = new();
    }
}