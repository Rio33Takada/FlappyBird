using KTC.Domain;
using System;

namespace KTC.Presentation
{
    public sealed class GameCyclePresenter : IDisposable
    {
        private readonly GameCycleModel _model = null;

        private readonly IGameCycleBird _bird = null;

        //private readonly IGameCycleStage _stage = null;

        public GameCycleState State => _model.State;

        public int Score => _model.Score;

        public event Action<GameCycleState> StateChanged;
        public event Action<int> ScoreChanged;

        public GameCyclePresenter()
        {
            // TODO: Model・鳥・ステージを受け取ってフィールドに保持し、開始入力・死亡・列通過の通知を購読する。
            
        }

        private void HandleStartRequested()
        {
            if (_model == null)
            {
                return;
            }

            if (_model.TryStart())
            {
                // TODO: 鳥を飛行状態にして初回ジャンプを行い、障害物の生成・移動を開始して状態を通知する。

            }
        }

        private void HandleColumnPassed(int columnId)
        {
            // TODO: TryScoreColumn が成功したら、更新後のスコアを通知する。

        }

        private void HandleBirdDied()
        {
            // TODO: TryEnd が成功したら障害物の生成・移動を止め、状態を通知する。

        }

        public void RequestRetry()
        {
            // TODO: TryPrepareNextRun が成功したら障害物を返却し、鳥を初期位置へ戻して状態とスコアを通知する。

        }

        public void Dispose()
        {
            // TODO: コンストラクターで登録した開始入力・死亡・列通過の購読を解除する。

        }
    }
}
