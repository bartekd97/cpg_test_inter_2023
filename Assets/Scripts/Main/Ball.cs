using Config;
using UnityEngine;

namespace Main
{
    public class Ball : CellOccupier
    {
        [Header("Ball config")]
        [SerializeField] SpriteRenderer spriteRenderer;

        public BallVariant Variant { get; private set; }

        public void Bind(BallVariant variant)
        {
            Variant = variant;
            UpdateVisual();
        }

        public void UpdateVisual()
        {
            spriteRenderer.color = Variant.tint;
        }
    }
}
