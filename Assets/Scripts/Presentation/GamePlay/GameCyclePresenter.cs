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

            _bird.OnStart += HandleStartRequested;
            _bird.OnDeath += HandleBirdDied;
            _stage.OnPassedColumn += HandlePassedColumn;
        }

        public void CheckBirdBounds()
        {
            if (State == GameCycleState.Playing &&
                _stage.IsOutsideVerticalBounds(_bird.PositionY))
            {
                _bird.Die();
            }
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
            _bird.InitialJumpInput();
            _stage.SpawnObstacleColumn();
            _stage.StartRunning();
            StateChanged?.Invoke(State);
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

            _stage.StopRunning();

            StateChanged?.Invoke(State);
        }

        public void RequestRetry()
        {
            if (_model == null || _bird == null)
            {
                return;
            }

            if (!_model.TryPrepareNextRun())
            {
                return;
            }

            _stage.Reset();
            _bird.Reset();
            StateChanged?.Invoke(State);
            ScoreChanged?.Invoke(Score);
        }

        public void Dispose()
        {
            _bird.OnStart -= HandleStartRequested;
            _bird.OnDeath -= HandleBirdDied;
            _stage.OnPassedColumn -= HandlePassedColumn;
        }
    }
}
