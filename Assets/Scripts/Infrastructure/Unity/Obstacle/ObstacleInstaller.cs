using UnityEngine;
using KTC.Presentation;
using KTC.Domain;
using KTC.Infrastructure;
using System;

namespace KTC.Infrastructure
{
    public sealed class ObstacleInstaller : MonoBehaviour
    {
        private ObstaclePresenter _presenter;
        private ObstacleView _view;

        public void Install(ObstacleModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(
                    nameof(model));
            }
        }

        public void Release()
        {
            _presenter?.Dispose();
            _presenter = null;

            gameObject.SetActive(false);
        }
    }
}
