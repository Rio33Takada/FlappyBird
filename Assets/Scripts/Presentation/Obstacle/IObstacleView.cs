using System;

namespace KTC.Presentation
{
    public interface IObstacleView
    {
        event Action OnChargedAttackDetected;
        event Action OnLifetimeExpired;

        void Destruct();
        void Expire();
    }
}
