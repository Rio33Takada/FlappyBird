using KTC.Domain;
using UnityEngine;

namespace KTC.Infrastructure
{
    [CreateAssetMenu(
        menuName = "KTC/Obstacle/Destruction Rules/Indestructible")]
    public class IndestructibleRuleFactory
        : ObstacleDestructionRuleFactory
    {
        public override IObstacleDestructionRule CreateRule()
        {
            return new IndestructibleRule();
        }
    }
}
