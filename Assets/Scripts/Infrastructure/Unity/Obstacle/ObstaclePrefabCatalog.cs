using System.Collections.Generic;
using UnityEngine;
using KTC.Domain;

namespace KTC.Infrastructure
{
    [CreateAssetMenu(
        fileName = "ObstaclePrefabCatalog",
        menuName = "KTC/Obstacle/ObstaclePrefabCatalog")]
    public sealed class ObstaclePrefabCatalog : ScriptableObject
    {
        [SerializeField]
        private List<ObstaclePrefabEntry> _entries = new();

        public bool TryGetPrefab(
            ObstacleType type,
            out ObstacleInstaller prefab)
        {
            prefab = null;

            if (_entries == null)
            {
                return false;
            }

            bool hasMatch = false;

            foreach (ObstaclePrefabEntry entry in _entries)
            {
                if (entry == null || entry.Type != type)
                {
                    continue;
                }

                if (hasMatch)
                {
                    prefab = null;
                    return false;
                }

                hasMatch = true;
                prefab = entry.Prefab;
            }

            return prefab != null;
        }
    }
}
