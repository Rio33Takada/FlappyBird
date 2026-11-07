namespace KTC.Domain
{
    public sealed class DestroyByChargedAttackRule : IObstacleDestructionRule
    {
        public bool CanDestroyByChargedAttack()
        {
            return true;
        }
    }
}
