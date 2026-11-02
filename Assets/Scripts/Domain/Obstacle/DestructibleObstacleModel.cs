using System;

namespace KTC.Domain
{
    public sealed class DestructibleObstacleModel : ObstacleModel
    {
        protected override void OnChargedAttack()
        {
            Destroy();
        }
    }
}
