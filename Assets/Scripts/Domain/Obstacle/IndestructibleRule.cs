namespace KTC.Domain
{
    public sealed class IndestructibleRule : IObstacleDestructionRule
    {
        public bool CanDestroyByChargedAttack()
        {
            return false;
        }
    }
}
