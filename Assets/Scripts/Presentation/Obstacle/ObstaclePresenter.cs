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

            // TODO: OnLifetimeExpired を購読し、View の終了処理へ接続する。
            _view.OnChargedAttackDetected += HandleChargedAttack;
            //_view.OnLifetimeExpired +=
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


        public void Dispose()
        {
            // TODO: 寿命終了の購読を追加した際は、ここでも解除する。
            _view.OnChargedAttackDetected -= HandleChargedAttack;
        }
    }
}
