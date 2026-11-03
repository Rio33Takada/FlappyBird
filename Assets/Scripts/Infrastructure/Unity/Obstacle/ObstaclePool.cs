using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace KTC.Infrastructure
{
    public sealed class ObstaclePool : MonoBehaviour, IObstaclePool
    {
        private const int OBJECTPOOL_DEFAULT_CAPACITY = 10;

        private const int OBJECTPOOL_MAX_SIZE = 30;

        private readonly Dictionary<
            ObstacleInstaller,
            ObjectPool<ObstacleInstaller>> _pools =
                new Dictionary<
                    ObstacleInstaller,
                    ObjectPool<ObstacleInstaller>>();

        private readonly Dictionary<
            ObstacleInstaller,
            ObjectPool<ObstacleInstaller>> _rentedObstacles =
                new Dictionary<
                    ObstacleInstaller,
                    ObjectPool<ObstacleInstaller>>();

        public ObstacleInstaller Get(ObstacleInstaller prefab)
        {
            if (prefab == null)
            {
                return null;
            }

            ObjectPool<ObstacleInstaller> pool;

            if (!_pools.TryGetValue(prefab, out pool))
            {
                pool = CreatePool(prefab);
                _pools.Add(prefab, pool);
            }

            ObstacleInstaller obstacle = pool.Get();
            _rentedObstacles.Add(obstacle, pool);

            return obstacle;
        }

        public void Release(ObstacleInstaller obstacle)
        {
            if (obstacle == null)
            {
                return;
            }

            ObjectPool<ObstacleInstaller> pool;

            if (!_rentedObstacles.TryGetValue(obstacle, out pool))
            {
                return;
            }

            _rentedObstacles.Remove(obstacle);
            pool.Release(obstacle);
        }

        private ObjectPool<ObstacleInstaller> CreatePool(
            ObstacleInstaller prefab)
        {
            return new ObjectPool<ObstacleInstaller>(
                createFunc: () => Instantiate(prefab, transform),
                actionOnGet: null,
                actionOnRelease: obstacle => obstacle.Release(),
                actionOnDestroy: obstacle =>
                    Destroy(obstacle.gameObject),
                collectionCheck: true,
                defaultCapacity: OBJECTPOOL_DEFAULT_CAPACITY,
                maxSize: OBJECTPOOL_MAX_SIZE);
        }

        private void OnDestroy()
        {
            foreach (ObjectPool<ObstacleInstaller> pool in _pools.Values)
            {
                pool.Dispose();
            }

            _pools.Clear();
            _rentedObstacles.Clear();
        }
    }
}
