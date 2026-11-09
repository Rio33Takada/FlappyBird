using System;
using UnityEngine;
using UnityEngine.InputSystem;
using KTC.Presentation;

namespace KTC.Infrastructure
{
    public class BirdView : MonoBehaviour, IBirdView
    {
        private const string FLIGHT_ACTION_NAME = "Player/Jump";
        private const float RETURN_X_TOLERANCE = 0.001f;

        [SerializeField] private PlayerInput _playerInput = null;
        [SerializeField] private Rigidbody2D _rigidbody2D = null;
        [SerializeField] private UnityEngine.UI.Image _chargeProgressImage = null;
        [SerializeField, Min(0.01f)] private float _jumpSpeed = 5f;
        [SerializeField, Min(0.01f)] private float _dashSpeed = 8f;
        [SerializeField, Min(0.01f)] private float _dashSeconds = 0.2f;
        [SerializeField, Min(0.01f)] private float _returnSpeed = 4f;

        private InputAction _flightAction = null;
        private float _homeX = 0f;
        private float _normalGravityScale = 0f;
        private float _dashRemaining = 0f;
        private bool _isDashing = false;
        private bool _isReturning = false;

        public event Action<double> OnPressed;
        public event Action<double> OnReleased;
        public event Action<double> OnInputTimeUpdated;
        public event Action OnInputCanceled;
        public event Action OnCollisionDetected;
        public event Action OnAttackFinished;
        public event Action OnReturnFinished;

        private void Awake()
        {
            if (_rigidbody2D != null)
            {
                _homeX = _rigidbody2D.position.x;
                _normalGravityScale = _rigidbody2D.gravityScale;
            }
        }

        private void OnEnable()
        {
            if (_playerInput == null || _playerInput.actions == null)
            {
                return;
            }

            _flightAction = _playerInput.actions.FindAction(FLIGHT_ACTION_NAME);

            if (_flightAction != null)
            {
                _flightAction.performed += HandlePressed;
                _flightAction.canceled += HandleCanceled;
            }
        }

        private void OnDisable()
        {
            if (_flightAction != null)
            {
                _flightAction.performed -= HandlePressed;
                _flightAction.canceled -= HandleCanceled;
                _flightAction = null;
            }

            OnInputCanceled?.Invoke();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                OnInputCanceled?.Invoke();
            }
        }

        private void HandlePressed(InputAction.CallbackContext context)
        {
            if (Application.isFocused)
            {
                OnPressed?.Invoke(context.time);
            }
        }

        private void HandleCanceled(InputAction.CallbackContext context)
        {
            if (Application.isFocused && context.action.WasReleasedThisFrame())
            {
                OnReleased?.Invoke(context.time);
                return;
            }

            OnInputCanceled?.Invoke();
        }

        private void Update()
        {
            if (Application.isFocused &&
                _flightAction != null &&
                _flightAction.IsPressed())
            {
                OnInputTimeUpdated?.Invoke(Time.realtimeSinceStartupAsDouble);
            }
        }

        public void SetChargeProgress(float progress)
        {
            if (_chargeProgressImage != null)
            {
                _chargeProgressImage.fillAmount = progress;
            }
        }

        public void Jump()
        {
            if (_rigidbody2D != null)
            {
                Vector2 velocity = _rigidbody2D.linearVelocity;
                velocity.y = _jumpSpeed;
                _rigidbody2D.linearVelocity = velocity;
            }
        }

        public void BeginChargeAttack()
        {
            if (_rigidbody2D == null)
            {
                return;
            }

            _isReturning = false;
            _isDashing = true;
            _dashRemaining = _dashSeconds;
            _rigidbody2D.gravityScale = 0f;
            _rigidbody2D.linearVelocity = new Vector2(_dashSpeed, 0f);
        }

        private void FixedUpdate()
        {
            if (_rigidbody2D == null)
            {
                return;
            }

            if (_isDashing)
            {
                if (_dashRemaining > 0f)
                {
                    _dashRemaining -= Time.fixedDeltaTime;
                    return;
                }

                _isDashing = false;
                _isReturning = true;
                _rigidbody2D.gravityScale = _normalGravityScale;
                OnAttackFinished?.Invoke();
            }

            if (!_isReturning)
            {
                return;
            }

            Vector2 velocity = _rigidbody2D.linearVelocity;
            float remainingX = _homeX - _rigidbody2D.position.x;

            if (Mathf.Abs(remainingX) <= RETURN_X_TOLERANCE)
            {
                _isReturning = false;
                velocity.x = 0f;
                _rigidbody2D.linearVelocity = velocity;
                OnReturnFinished?.Invoke();
                return;
            }

            velocity.x = Mathf.Clamp(
                remainingX / Time.fixedDeltaTime, -_returnSpeed, _returnSpeed);
            _rigidbody2D.linearVelocity = velocity;
        }

        public void Die()
        {
            _isDashing = false;
            _isReturning = false;

            if (_rigidbody2D != null)
            {
                _rigidbody2D.gravityScale = _normalGravityScale;
                _rigidbody2D.linearVelocity = Vector2.zero;
                _rigidbody2D.simulated = false;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionDetected?.Invoke();
        }
    }
}
