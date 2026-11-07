using UnityEngine;

namespace KTC.Infrastructure
{
    public interface IObstaclePool
    {
        ObstacleInstaller Get(ObstacleInstaller prefab);
        void Release(ObstacleInstaller obstacle);
    }
}
