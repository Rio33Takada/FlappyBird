using KTC.Domain;
using UnityEngine;

namespace KTC.Infrastructure
{
    [CreateAssetMenu(
        menuName = "KTC/Obstacle/Destruction Rules/Destroy By Charged Attack")]
    public class DestroyByChargedAttackRuleFactory
        : ObstacleDestructionRuleFactory
    {
        public override IObstacleDestructionRule CreateRule()
        {
            return new DestroyByChargedAttackRule();
        }
    }
}
