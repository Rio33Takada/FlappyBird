using KTC.Domain;
using System;

namespace KTC.Presentation
{
    public class ObstaclePresenter : IDisposable
    {
        private readonly ObstacleModel _model;
        private readonly IObstacleView _view;

        public ObstaclePresenter(
            ObstacleModel model,
            IObstacleView view)
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

            _view.OnChargedAttackDetected += HandleChargedAttack;
        }

        private void HandleChargedAttack()
        {
            _model.ReceiveChargedAttack();

            if (_model.IsDestroyed)
            {
                _view.Destruct();
            }
        }


        public void Dispose()
        {
            _view.OnChargedAttackDetected -= HandleChargedAttack;
        }
    }
}
