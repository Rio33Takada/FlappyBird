using System;

namespace KTC.Presentation
{
    public interface IGameCycleStage
    {
        void SpawnObstacleColumn();

        void StartMoving();

        void StopMoving();
        // TODO: 列 ID を伴う通過通知と、生成の開始・停止およびリトライ時のリセットの契約を定義する。


    }
}
