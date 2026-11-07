using KTC.Domain;
using UnityEngine;

namespace KTC.Infrastructure
{
    public sealed class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField]
        private StageSettings _stageSettings = null;

        [SerializeField]
        private ObstaclePrefabCatalog _catalog = null;

        [SerializeField]
        private ObstaclePool _pool = null;

        public StageSettings StageSettings => _stageSettings;

        public bool TrySpawn(
            ObstacleType type,
            Vector3 position,
            out ObstacleInstaller obstacle)
        {
            obstacle = null;

            if (_stageSettings == null || _catalog == null || _pool == null)
            {
                return false;
            }

            ObstacleInstaller prefab;

            if (!_catalog.TryGetPrefab(type, out prefab))
            {
                return false;
            }

            ObstacleInstaller instance = _pool.Get(prefab);

            if (instance == null)
            {
                return false;
            }

            instance.transform.SetPositionAndRotation(
                position,
                Quaternion.identity);

            if (!instance.Install(_pool, _stageSettings.WorldLimitX))
            {
                _pool.Release(instance);
                return false;
            }

            instance.gameObject.SetActive(true);
            obstacle = instance;

            return true;
        }
    }
}