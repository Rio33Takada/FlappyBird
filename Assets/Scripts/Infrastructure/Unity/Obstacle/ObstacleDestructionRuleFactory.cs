using KTC.Domain;
using UnityEngine;

namespace KTC.Infrastructure
{
    public abstract class ObstacleDestructionRuleFactory : ScriptableObject
    {
        public abstract IObstacleDestructionRule CreateRule();
    }
}
