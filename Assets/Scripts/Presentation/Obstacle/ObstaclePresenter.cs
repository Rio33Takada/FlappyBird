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

            _model = model;
            _view = view;

            _view.OnLifetimeExpired += HandleLifeTimeExpired;
            _view.OnChargedAttackDetected += HandleChargedAttack;
        }

        private void HandleChargedAttack()
        {
            bool isNewlyDestroyed = _model.ReceiveChargedAttack();

            if (!isNewlyDestroyed)
            {
                return;
            }

            _view.Destruct();
        }

        private void HandleLifeTimeExpired()
        {
            _view.Expire();
        }


        public void Dispose()
        {
            _view.OnLifetimeExpired -= HandleLifeTimeExpired;
            _view.OnChargedAttackDetected -= HandleChargedAttack;
        }
    }
}
