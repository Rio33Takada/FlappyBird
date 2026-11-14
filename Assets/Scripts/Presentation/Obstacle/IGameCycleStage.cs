using System;

namespace KTC.Presentation
{
    public interface IGameCycleStage
    {
        void SpawnObstacleColumn();

        void StartMoving();

        void StopMoving();


    }
}
