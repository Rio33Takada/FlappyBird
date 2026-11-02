using UnityEngine;
using UnityEngine.Pool;

namespace KTC.Infrastructure
{
    public sealed class ObstaclePool : MonoBehaviour, IObstaclePool
    {
        [SerializeField]
        private ObstacleInstaller _normalPrefab;

        [SerializeField]
        private ObstacleInstaller _destructiblePrefab;

        private ObjectPool<ObstacleInstaller> _normalPool;
        private ObjectPool<ObstacleInstaller> _destructiblePool;

        private void Awake()
        {
            _normalPool = CreatePool(_normalPrefab);
            _destructiblePool = CreatePool(_destructiblePrefab);
        }

        private ObjectPool<ObstacleInstaller> CreatePool(
            ObstacleInstaller prefab)
        {
            return new ObjectPool<ObstacleInstaller>(
                createFunc: () => Instantiate(prefab, transform),
                actionOnGet: obstacle => obstacle.gameObject.SetActive(true),
                actionOnRelease: obstacle => obstacle.Release(),
                actionOnDestroy: obstacle => Destroy(obstacle.gameObject),
                collectionCheck: true,
                defaultCapacity: 10,
                maxSize: 30);
        }

        public ObstacleInstaller GetNormal()
        {
            return _normalPool.Get();
        }

        public ObstacleInstaller GetDestructible()
        {
            return _destructiblePool.Get();
        }

        public void ReleaseNormal(ObstacleInstaller obstacle)
        {
            _normalPool.Release(obstacle);
        }

        public void ReleaseDestructible(ObstacleInstaller obstacle)
        {
            _destructiblePool.Release(obstacle);
        }
    }
}
