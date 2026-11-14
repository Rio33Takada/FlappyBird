using System;

namespace KTC.Presentation
{
    public interface IGameCycleBird
    {
        event Action OnStart;
        event Action OnDeath;

        void InitialJumpInput();
        void Reset();
    }
}
