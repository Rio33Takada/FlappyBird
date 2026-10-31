using System;
using KTC.Domain;

namespace KTC.Presentation
{
    public class BirdPresenter : IDisposable
    {
        private readonly BirdModel _model;
        private readonly IBirdView _view;

        public BirdPresenter(BirdModel model, IBirdView view)
        {
            if (model == null)
            {
                throw new ArgumentNullException(
                    nameof(model));
            }

            if (view == null)
            {
                throw new ArgumentNullException(
                    nameof(view));
            }

            _model = model;
            _view = view;

            _view.OnJumpRequested += HandleJump;
        }

        private void HandleJump()
        {
            if (!_model.IsAlive)
            {
                return;
            }

            _view.Jump();
        }

        private void HandleCollision()
        {
            if (_model.IsAlive)
            {
                return;
            }

            // TODO: BirdModelÇ≈ÇÃé¿ëïå„âèú.
            //_model.Die();
            _view.Die();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
