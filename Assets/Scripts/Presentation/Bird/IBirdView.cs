using System;

namespace KTC.Presentation
{
    public interface IBirdView
    {
        event Action<double> OnPressed;
        event Action<double> OnReleased;
        event Action<double> OnInputTimeUpdated;
        event Action OnInputCanceled;
        event Action OnCollisionDetected;
        event Action OnAttackFinished;
        event Action OnReturnFinished;

        void SetChargeProgress(float progress);
        void Jump();
        void BeginChargeAttack();
        void Die();
    }
}
