using KTC.Presentation;
using KTC.Domain;
using UnityEngine;

namespace KTC.Infrastructure
{
    public sealed class GameSceneInstaller : MonoBehaviour
    {
        [SerializeField]
        private BirdInstaller _birdInstaller = null;

        [SerializeField]
        private ObstacleStage _obstacleStage = null;

        [SerializeField]
        private GameOverView _gameOverView = null;

        private GameCyclePresenter _presenter = null;
        private GameOverPresenter _gameOverPresenter = null;

        private void Start()
        {
            if (_birdInstaller == null || _obstacleStage == null)
            {
                return;
            }

            _birdInstaller.Install();
            if (_birdInstaller.Bird == null)
            {
                return;
            }

            _presenter = new GameCyclePresenter(
                new GameCycleModel(), _birdInstaller.Bird, _obstacleStage);

            if (_gameOverView != null)
            {
                _gameOverPresenter = new GameOverPresenter(_presenter, _gameOverView);
            }
        }

        private void LateUpdate()
        {
            _presenter?.CheckBirdBounds();
        }

        public void RequestRetry()
        {
            _presenter?.RequestRetry();
        }

        public void OnDestroy()
        {
            _gameOverPresenter?.Dispose();
            _gameOverPresenter = null;
            _presenter?.Dispose();
            _presenter = null;
        }
    }
}
