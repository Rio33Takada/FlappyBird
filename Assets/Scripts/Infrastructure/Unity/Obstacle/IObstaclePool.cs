using UnityEngine;

namespace KTC.Infrastructure
{
    public interface IObstaclePool
    {
        void ReleaseNormal(ObstacleInstaller obstacle);
        void ReleaseDestructible(ObstacleInstaller obstacle);
    }
}
