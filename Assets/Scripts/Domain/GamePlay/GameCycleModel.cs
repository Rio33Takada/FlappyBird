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
            throw new NotImplementedException();
        }

        public bool TryScoreColumn(int columnId)
        {
            throw new NotImplementedException();
        }

        public bool TryEnd()
        {
            throw new NotImplementedException();
        }

        public bool TryPrepareNextRun()
        {
            throw new NotImplementedException();
        }
    }
}
