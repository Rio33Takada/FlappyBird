using System;
using UnityEngine;

namespace KTC.Infrastructure
{
    public sealed class ObstacleLifetimeDetector : MonoBehaviour
    {
        private float _despawnX = 0f;
        private bool _isMonitoring = false;

        public event Action OnLifetimeExpired;

        public void Initialize(float despawnX)
        {
            _despawnX = despawnX;
            _isMonitoring = true;
        }

        public void StopMonitoring()
        {
            _isMonitoring = false;
        }

        private void LateUpdate()
        {
            if (!_isMonitoring)
            {
                return;
            }

            if (transform.position.x > _despawnX)
            {
                return;
            }

            _isMonitoring = false;
            OnLifetimeExpired?.Invoke();
        }

        private void OnDisable()
        {
            StopMonitoring();
        }
    }
}
