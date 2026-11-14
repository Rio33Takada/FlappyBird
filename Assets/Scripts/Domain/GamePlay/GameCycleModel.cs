using System;
using System.Collections.Generic;

namespace KTC.Domain
{
    public sealed class GameCycleModel
    {
        private int _score = 0;

        private readonly HashSet<int> _scoredColumnIds = new();

        public int Score => _score;

        public GameCycleState State { get; private set; } = GameCycleState.Ready;

        public bool TryStart()
        {
            if (!(State == GameCycleState.Ready))
            {
                return false;
            }

            State = GameCycleState.Playing;
            return true;
        }

        public bool TryScoreColumn(int columnId)
        {
            if (!(State == GameCycleState.Playing))
            {
                return false;
            }

            if (!_scoredColumnIds.Add(columnId))
            {
                return false;
            }

            _score++;
            return true;
        }

        public bool TryEnd()
        {
            if (!(State == GameCycleState.Playing))
            {
                return false;
            }

            State = GameCycleState.GameOver;
            return true;
        }

        public bool TryPrepareNextRun()
        {
            if (!(State == GameCycleState.GameOver))
            {
                return false;
            }

            _score = 0;
            _scoredColumnIds.Clear();
            return true;
        }
    }
}
