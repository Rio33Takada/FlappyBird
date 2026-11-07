using System;
using UnityEngine;
using KTC.Domain;

namespace KTC.Infrastructure
{
    [Serializable]
    public sealed class ObstaclePrefabEntry
    {
        [SerializeField]
        private ObstacleType _type = default;

        [SerializeField]
        private ObstacleInstaller _prefab = null;

        public ObstacleType Type => _type;

        public ObstacleInstaller Prefab => _prefab;
    }
}
