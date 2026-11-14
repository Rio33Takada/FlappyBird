using System;

namespace KTC.Presentation
{
    public interface IGameCycleStage
    {
        void SpawnObstacleColumn();

        void StartMoving();

        void StopMoving();

        event Action<int> OnPassedColumn;

        // TODO: 生成の開始・停止およびリトライ時のリセットの契約を定義する。

    }
}
