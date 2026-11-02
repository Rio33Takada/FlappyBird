using System;
using UnityEngine;
using UnityEngine.InputSystem;
using KTC.Presentation;

namespace KTC.Infrastructure
{
    public class BirdView : MonoBehaviour, IBirdView
    {
        [SerializeField]
        private PlayerInput _playerInput;

        [SerializeField]
        private Rigidbody2D _rigidbody2D;

        private InputAction _jumpAction;

        public event Action OnJumpRequested;
        public event Action OnCollisionDetected;

        private void Awake()
        {
            _jumpAction = _playerInput.actions["Jump"];
        }

        private void OnEnable()
        {
            if (_jumpAction == null)
            {
                return;
            }

            _jumpAction.performed += OnJumpPerformed;
        }

        private void OnDisable()
        {
            if (_jumpAction == null)
            {
                return;
            }

            _jumpAction.performed -= OnJumpPerformed;
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            OnJumpRequested?.Invoke();
        }

        public void Jump()
        {
            throw new NotImplementedException();
        }

        public void Die()
        {
            // TODO: アニメーションとか.
            throw new NotImplementedException();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionDetected?.Invoke();
        }
    }
}
