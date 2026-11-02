using System;

namespace KTC.Domain
{
    public sealed class BirdModel
    {
        private bool _isAlive = false;
        public bool IsAlive => _isAlive;

        public void Die()
        {
            _isAlive = false;
        }
    }
}