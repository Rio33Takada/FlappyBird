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
            
        }

        private void HandleStartRequested()
        {
            if (_model == null)
            {
                return;
            }

            if (_model.TryStart())
            {

            }
        }

        private void HandleColumnPassed(int columnId)
        {

        }

        private void HandleBirdDied()
        {

        }

        public void RequestRetry()
        {

        }

        public void Dispose()
        {

        }
    }
}
