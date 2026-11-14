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
            // TODO: Ready のときだけ Playing に遷移して true を返す。
            throw new NotImplementedException();
        }

        public bool TryScoreColumn(int columnId)
        {
            // TODO: Playing 中に初めて通過した列 ID を記録し、1 点加算して true を返す。
            throw new NotImplementedException();
        }

        public bool TryEnd()
        {
            // TODO: Playing のときだけ GameOver に遷移し、スコアを保持して true を返す。
            throw new NotImplementedException();
        }

        public bool TryPrepareNextRun()
        {
            // TODO: GameOver のときだけスコアと加点済みの列 ID を初期化し、Ready に戻して true を返す。
            throw new NotImplementedException();
        }
    }
}
