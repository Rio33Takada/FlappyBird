using System;

namespace KTC.Domain
{
    public abstract class ObstacleModel
    {
        private bool _isDestroyed;

        public bool IsDestroyed => _isDestroyed;

        public void ReceiveChargedAttack()
        {
            if (_isDestroyed)
            {
                return;
            }

            OnChargedAttack();
        }

        protected abstract void OnChargedAttack();

        protected void Destroy()
        {
            _isDestroyed = true;
        }
    }
}
