using KTC.Domain;
using System;

namespace KTC.Presentation
{
    public sealed class GameCyclePresenter : IDisposable
    {
        private readonly GameCycleModel _model = null;

        private readonly IGameCycleBird _bird = null;

        private readonly IGameCycleStage _stage = null;

        public GameCycleState State => _model.State;

        public int Score => _model.Score;

        public event Action<GameCycleState> StateChanged;
        public event Action<int> ScoreChanged;

        public GameCyclePresenter(
            GameCycleModel model,
            IGameCycleBird bird,
            IGameCycleStage stage)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (bird == null)
            {
                throw new ArgumentNullException(nameof(bird));
            }

            if (stage == null)
            {
                throw new ArgumentNullException(nameof(stage));
            }

            _model = model;
            _bird = bird;
            _stage = stage;

            // TODO: 開始入力・死亡の通知を購読する。
            _stage.OnPassedColumn += HandlePassedColumn;
        }

        private void HandleStartRequested()
        {
            if (_model == null)
            {
                return;
            }

            if (!_model.TryStart())
            {
                return;
            }
            // TODO: 鳥を飛行状態にして初回ジャンプを行い、障害物の生成・移動を開始して状態を通知する。

        }

        private void HandlePassedColumn(int columnId)
        {
            if (_model == null)
            {
                return;
            }

            if (!_model.TryScoreColumn(columnId))
            {
                return;
            }

            ScoreChanged?.Invoke(Score);
        }

        private void HandleBirdDied()
        {
            if (_model == null || _stage == null)
            {
                return;
            }

            if (!_model.TryEnd())
            {
                return;
            }

            _stage.StopMoving();

            StateChanged?.Invoke(State);
        }

        public void RequestRetry()
        {
            // TODO: TryPrepareNextRun が成功したら障害物を返却し、鳥を初期位置へ戻して状態とスコアを通知する。
            if (_model == null || _bird == null)
            {
                return;
            }

            if (!_model.TryPrepareNextRun())
            {
                return;
            }

            _bird.Reset();
        }

        public void Dispose()
        {
            // TODO: コンストラクターで登録した開始入力・死亡・列通過の購読を解除する。

        }
    }
}
