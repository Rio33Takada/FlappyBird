using System;
using KTC.Domain;

namespace KTC.Presentation
{
    public sealed class BirdPresenter : IGameCycleBird, IDisposable
    {
        private readonly BirdModel _model;
        private readonly IBirdView _view;

        private bool _ignoreInputUntilRelease = false;

        public float PositionY => _view.PositionY;
        public BirdState State => _model.State;

        public event Action<BirdState> StateChanged;
        public event Action OnStart;
        public event Action OnDeath;

        public BirdPresenter(BirdModel model, IBirdView view)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            _model = model;
            _view = view;

            _view.SetState(_model.State);
            _model.StateChanged += HandleStateChanged;
            _view.OnPressed += HandlePressed;
            _view.OnReleased += HandleRelease;
            _view.OnInputTimeUpdated += HandleInputTimeUpdated;
            _view.OnInputCanceled += HandleInputCanceled;
            _view.OnCollisionDetected += HandleCollision;
            _view.OnAttackFinished += _model.FinishAttack;
            _view.OnReturnFinished += _model.FinishReturn;
        }

        private void HandleStateChanged(BirdState state)
        {
            _view.SetState(state);
            StateChanged?.Invoke(state);
        }

        public void InitialJumpInput()
        {
            if (_model.State != BirdState.Ready)
            {
                return;
            }

            _model.StartFlight();
            _view.StartFlight();
            _view.Jump();
        }

        public void Reset()
        {
            _ignoreInputUntilRelease = false;
            _model.Reset();
            _view.ResetForRun();
        }

        private void HandlePressed(double time)
        {
            if (_ignoreInputUntilRelease)
            {
                return;
            }

            if (_model.State == BirdState.Ready)
            {
                _ignoreInputUntilRelease = true;
                OnStart?.Invoke();
                return;
            }

            _model.Press(time);
        }

        private void HandleInputTimeUpdated(double time)
        {
            _model.UpdateHold(time);
            _view.SetChargeProgress(_model.ChargeProgress);
        }

        private void HandleInputCanceled()
        {
            _ignoreInputUntilRelease = false;
            _model.CancelInput();
            _view.SetChargeProgress(_model.ChargeProgress);
        }

        private void HandleRelease(double time)
        {
            if (_ignoreInputUntilRelease)
            {
                _ignoreInputUntilRelease = false;
                return;
            }

            BirdInputResult result = _model.Release(time);
            _view.SetChargeProgress(_model.ChargeProgress);
            ApplyInputResult(result);
        }

        private void ApplyInputResult(BirdInputResult result)
        {
            switch (result)
            {
                case BirdInputResult.Jump:
                    _view.Jump();
                    break;

                case BirdInputResult.ChargeAttack:
                    _view.BeginChargeAttack();
                    break;
            }
        }

        public void Die()
        {
            if (_model.TryDie())
            {
                ApplyDeath();
            }
        }

        private void HandleCollision()
        {
            if (_model.TryDieByCollision())
            {
                ApplyDeath();
            }
        }

        private void ApplyDeath()
        {
            _view.SetChargeProgress(_model.ChargeProgress);
            _view.Die();
            OnDeath?.Invoke();
        }

        public void Dispose()
        {
            _model.StateChanged -= HandleStateChanged;
            _view.OnPressed -= HandlePressed;
            _view.OnReleased -= HandleRelease;
            _view.OnInputTimeUpdated -= HandleInputTimeUpdated;
            _view.OnInputCanceled -= HandleInputCanceled;
            _view.OnCollisionDetected -= HandleCollision;
            _view.OnAttackFinished -= _model.FinishAttack;
            _view.OnReturnFinished -= _model.FinishReturn;
            HandleInputCanceled();
        }
    }
}
