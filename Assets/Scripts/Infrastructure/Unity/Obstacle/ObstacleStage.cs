using UnityEngine;

namespace KTC.Infrastructure
{
    public sealed class ObstacleStage : MonoBehaviour
    {
        // TODO: IGameCycleStage を実装し、ObstacleColumnSpawner の参照を Inspector から受け取って null チェックする。
        // TODO: 既存の列生成処理から生成に成功した列の位置とタイル一覧を受け取り、プレイ内で一意の ID を割り当てる。
        // TODO: プレイ中は列を定期生成して左へ移動させ、ゲームオーバー時に生成と移動を止める。
        // TODO: 列が鳥の通常位置にある判定線を左向きに越えたら、その列 ID を一度だけ通知する。
        // TODO: リトライで残っている障害物をプールへ返却し、列の追跡情報と生成タイマーを初期化する。
    }
}
