using System;

namespace KTC.Domain
{
    public sealed class ObstacleModel
    {
        private readonly IObstacleDestructionRule _destructionRule;

        private bool _isDestroyed = false;

        public bool IsDestroyed => _isDestroyed;

        public ObstacleModel(IObstacleDestructionRule destructionRule)
        {
            if (destructionRule == null)
            {
                throw new ArgumentNullException(
                    nameof(destructionRule));
            }

            _destructionRule = destructionRule;
        }

        public bool ReceiveChargedAttack()
        {
            if (_isDestroyed)
            {
                return false;
            }

            if (!_destructionRule.CanDestroyByChargedAttack())
            {
                return false;
            }

            _isDestroyed = true;
            return true;
        }
    }
}
