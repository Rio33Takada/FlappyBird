using UnityEngine;
using KTC.Presentation;
using System;

namespace KTC.Infrastructure
{
    public sealed class ObstacleView : MonoBehaviour, IObstacleView
    {
        // TODO: 通常版の土管ペアの移動と画面外判定を実装し、ゲーム終了時に移動を停止できるようにする。
        // TODO: 画面外で OnLifetimeExpired を一度だけ発火し、再利用時に通知済み状態を初期化する。
        // TODO: 鳥の土管ペア通過を通知する。得点加算と重複防止の判断は Model / Presenter 側で行う。
        public event Action OnChargedAttackDetected;
        public event Action OnLifetimeExpired;

        // TODO: 拡張ルール対応時に破壊演出を実装し、演出完了後に所有元プールへ返却する。
        public void Destruct()
        {
            // TODO: 破壊消滅.
        }

        // TODO: 所有元プールへの返却を接続する。直接 Destroy したり、寿命終了通知を再発火したりしない。
        public void Expire()
        {
            // TODO: 寿命消滅.
        }

        // TODO: チャージ攻撃の検出は通常版の完成後に対応する。
        private void OnTriggerEnter2D(Collider2D other)
        {
            // TODO: チャージ攻撃の判定.
        }
    }
}
