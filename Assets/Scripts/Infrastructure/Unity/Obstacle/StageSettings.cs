using UnityEngine;

namespace KTC.Infrastructure
{
    [CreateAssetMenu(
        fileName = "StageSetting",
        menuName = "KTC/Stage/StageSettings")]
    public sealed class StageSettings : ScriptableObject
    {
        [SerializeField]
        private float _worldLimitX = 0f;

        public float WorldLimitX => _worldLimitX;
    }
}
