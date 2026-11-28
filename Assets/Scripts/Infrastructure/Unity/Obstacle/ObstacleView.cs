using UnityEngine;
using KTC.Presentation;
using KTC.Domain;
using System;

namespace KTC.Infrastructure
{
    public sealed class ObstacleView : MonoBehaviour, IObstacleView
    {
        [SerializeField]
        private Collider2D[] _colliders = Array.Empty<Collider2D>();

        [SerializeField]
        private SpriteRenderer[] _spriteRenderers = Array.Empty<SpriteRenderer>();

        [SerializeField, Min(0f)]
        private float _fadeSeconds = 0.25f;

        private bool[] _initialColliderStates = null;
        private Color[] _initialSpriteColors = null;
        private bool _hasLifetimeExpired = false;
        private bool _hasTerminated = false;
        private bool _isDestructing = false;
        private float _fadeElapsed = 0f;

        public event Action OnChargedAttackDetected;
        public event Action OnLifetimeExpired;
        public event Action OnTerminationCompleted;

        public void ResetForReuse()
        {
            _hasLifetimeExpired = false;
            _hasTerminated = false;
            _isDestructing = false;
            _fadeElapsed = 0f;

            if (_colliders != null)
            {
                bool captureInitialStates =
                    _initialColliderStates == null || _initialColliderStates.Length != _colliders.Length;

                if (captureInitialStates)
                {
                    _initialColliderStates = new bool[_colliders.Length];
                }

                for (int index = 0; index < _colliders.Length; index++)
                {
                    Collider2D collider = _colliders[index];
                    if (collider == null)
                    {
                        continue;
                    }

                    if (captureInitialStates)
                    {
                        _initialColliderStates[index] = collider.enabled;
                    }

                    collider.enabled = _initialColliderStates[index];
                }
            }

            if (_spriteRenderers != null)
            {
                bool captureInitialColors =
                    _initialSpriteColors == null || _initialSpriteColors.Length != _spriteRenderers.Length;

                if (captureInitialColors)
                {
                    _initialSpriteColors = new Color[_spriteRenderers.Length];
                }

                for (int index = 0; index < _spriteRenderers.Length; index++)
                {
                    SpriteRenderer spriteRenderer = _spriteRenderers[index];
                    if (spriteRenderer == null)
                    {
                        continue;
                    }

                    if (captureInitialColors)
                    {
                        _initialSpriteColors[index] = spriteRenderer.color;
                    }

                    spriteRenderer.color = _initialSpriteColors[index];
                }
            }
        }

        public void NotifyLifetimeExpired()
        {
            if (_hasLifetimeExpired || _hasTerminated)
            {
                return;
            }

            _hasLifetimeExpired = true;
            OnLifetimeExpired?.Invoke();
        }

        public void Expire()
        {
            if (_hasTerminated)
            {
                return;
            }

            _hasTerminated = true;
            _isDestructing = false;
            DisableColliders();
            OnTerminationCompleted?.Invoke();
        }

        public void Destruct()
        {
            if (_isDestructing || _hasTerminated || _hasLifetimeExpired)
            {
                return;
            }

            _isDestructing = true;
            _fadeElapsed = 0f;
            DisableColliders();

            if (!float.IsFinite(_fadeSeconds) || _fadeSeconds <= 0f)
            {
                Expire();
            }
        }

        private void Update()
        {
            if (!_isDestructing || _hasTerminated)
            {
                return;
            }

            _fadeElapsed += Time.deltaTime;
            float progress = float.IsFinite(_fadeSeconds) && _fadeSeconds > 0f
                ? Mathf.Clamp01(_fadeElapsed / _fadeSeconds)
                : 1f;

            if (_spriteRenderers != null && _initialSpriteColors != null)
            {
                for (int index = 0; index < _spriteRenderers.Length; index++)
                {
                    SpriteRenderer spriteRenderer = _spriteRenderers[index];
                    if (spriteRenderer != null)
                    {
                        Color color = _initialSpriteColors[index];
                        color.a *= 1f - progress;
                        spriteRenderer.color = color;
                    }
                }
            }

            if (progress >= 1f)
            {
                Expire();
            }
        }

        private void DisableColliders()
        {
            if (_colliders == null)
            {
                return;
            }

            foreach (Collider2D collider in _colliders)
            {
                if (collider != null)
                {
                    collider.enabled = false;
                }
            }
        }

        private void DetectChargedAttack(Collider2D other)
        {
            if (!isActiveAndEnabled || _isDestructing || _hasTerminated ||
                _hasLifetimeExpired || other == null)
            {
                return;
            }

            BirdView bird = other.GetComponentInParent<BirdView>();
            if (bird != null && bird.State == BirdState.ChargeAttacking)
            {
                OnChargedAttackDetected?.Invoke();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            DetectChargedAttack(collision.collider);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            DetectChargedAttack(collision.collider);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            DetectChargedAttack(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            DetectChargedAttack(other);
        }

        private void OnDisable()
        {
            _isDestructing = false;
            _fadeElapsed = 0f;
        }
    }
}
