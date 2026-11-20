using System;
using KTC.Domain;

namespace KTC.Presentation
{
    public interface IBirdView
    {
        float PositionY { get; }

        event Action<double> OnPressed;
        event Action<double> OnReleased;
        event Action<double> OnInputTimeUpdated;
        event Action OnInputCanceled;
        event Action OnCollisionDetected;
        event Action OnAttackFinished;
        event Action OnReturnFinished;

        void SetChargeProgress(float progress);
        void SetState(BirdState state);
        void StartFlight();
        void ResetForRun();
        void Jump();
        void BeginChargeAttack();
        void Die();
    }
}
