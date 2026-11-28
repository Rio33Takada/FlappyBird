using System;
using KTC.Domain;

namespace KTC.Presentation
{
    public interface IGameCycleBird
    {
        float PositionY { get; }
        BirdState State { get; }

        event Action<BirdState> StateChanged;
        event Action OnStart;
        event Action OnDeath;

        void InitialJumpInput();
        void Reset();
        void Die();
    }
}
