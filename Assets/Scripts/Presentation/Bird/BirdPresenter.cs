using System;
using KTC.Domain;

namespace KTC.Presentation
{
    public sealed class BirdPresenter : IDisposable
    {
        private readonly BirdModel _model;
        private readonly IBirdView _view;

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

            // TODO: 開始待ちの押下をゲームサイクルに通知し、その押下とリリースを通常の飛行入力から除外する。
            _view.OnPressed += _model.Press;
            _view.OnReleased += HandleRelease;
            _view.OnInputTimeUpdated += HandleInputTimeUpdated;
            _view.OnInputCanceled += HandleInputCanceled;
            _view.OnCollisionDetected += HandleCollision;
            _view.OnAttackFinished += _model.FinishAttack;
            _view.OnReturnFinished += _model.FinishReturn;
        }

        private void HandleInputTimeUpdated(double time)
        {
            _model.UpdateHold(time);
            _view.SetChargeProgress(_model.ChargeProgress);
        }

        private void HandleInputCanceled()
        {
            _model.CancelInput();
            _view.SetChargeProgress(_model.ChargeProgress);
        }

        private void HandleRelease(double time)
        {
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

        private void HandleCollision()
        {
            if (_model.TryDie())
            {
                _view.SetChargeProgress(_model.ChargeProgress);
                _view.Die();
                // TODO: 死亡確定後、ゲームサイクルに死亡を通知する。
            }
        }

        public void Dispose()
        {
            _view.OnPressed -= _model.Press;
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
