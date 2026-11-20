using System;

namespace KTC.Presentation
{
    public interface IGameCycleStage
    {
        void SpawnObstacleColumn();

        void StartRunning();

        void StopRunning();

        void Reset();

        bool IsOutsideVerticalBounds(float positionY);

        event Action<int> OnPassedColumn;
    }
}
