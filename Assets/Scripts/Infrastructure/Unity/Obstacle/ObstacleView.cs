using UnityEngine;
using KTC.Presentation;
using System;

namespace KTC.Infrastructure
{
    public sealed class ObstacleView : MonoBehaviour, IObstacleView
    {
        public event Action OnChargedAttackDetected;
        public event Action OnLifetimeExpired;

        public void Destruct()
        {
            // TODO: ”j‰óÁ–Å.
        }

        public void Expire()
        {
            // TODO: õ–½Á–Å.
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // TODO: ƒ`ƒƒ[ƒWUŒ‚‚Ì”»’è.
        }
    }
}
