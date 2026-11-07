using UnityEngine;

namespace KTC.Infrastructure
{
    [CreateAssetMenu(
        fileName = "StageSettings",
        menuName = "KTC/Stage/StageSettings")]
    public sealed class StageSettings : ScriptableObject
    {
        public const int PERCENT_SCALE = 100;

        [SerializeField, Min(2)]
        private int _obstacleRowCount = 8;

        [SerializeField, Min(1)]
        private int _obstacleGapRowCount = 2;

        [SerializeField, Min(0f)]
        private float _obstacleRowSpacing = 1f;

        [SerializeField, Range(0, PERCENT_SCALE)]
        private int _destructibleGapFillPercent = 0;

        [SerializeField]
        private float _worldLimitX = 0f;

        public int ObstacleRowCount => _obstacleRowCount;

        public int ObstacleGapRowCount => _obstacleGapRowCount;

        public float ObstacleRowSpacing => _obstacleRowSpacing;

        public int DestructibleGapFillPercent => _destructibleGapFillPercent;

        public float WorldLimitX => _worldLimitX;
    }
}
