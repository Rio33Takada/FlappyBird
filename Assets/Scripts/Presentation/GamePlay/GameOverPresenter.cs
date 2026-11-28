using System;
using KTC.Domain;

namespace KTC.Presentation
{
    public sealed class GameOverPresenter : IDisposable
    {
        private readonly GameCyclePresenter _cycle;
        private readonly IGameOverView _view;

        public GameOverPresenter(GameCyclePresenter cycle, IGameOverView view)
        {
            if (cycle == null)
            {
                throw new ArgumentNullException(nameof(cycle));
            }

            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            _cycle = cycle;
            _view = view;

            _cycle.StateChanged += HandleStateChanged;
            _cycle.ScoreChanged += _view.SetScore;
            _view.OnRetryRequested += _cycle.RequestRetry;

            HandleStateChanged(_cycle.State);
        }

        private void HandleStateChanged(GameCycleState state)
        {
            _view.SetScore(_cycle.Score);
            _view.SetVisible(state == GameCycleState.GameOver);
        }

        public void Dispose()
        {
            _cycle.StateChanged -= HandleStateChanged;
            _cycle.ScoreChanged -= _view.SetScore;
            _view.OnRetryRequested -= _cycle.RequestRetry;
        }
    }
}
