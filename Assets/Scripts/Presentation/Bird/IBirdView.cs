using System;

namespace KTC.Presentation
{
    public interface IBirdView
    {
        event Action OnJumpRequested;
        event Action OnCollisionDetected;

        void Jump();
        void Die();
    }
}
