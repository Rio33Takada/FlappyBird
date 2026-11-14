using KTC.Presentation;
using UnityEngine;

namespace KTC.Infrastructure
{
    public sealed class GameSceneInstaller : MonoBehaviour
    {
        [SerializeField]
        private BirdInstaller _birdInstaller = null;

        private GameCyclePresenter _presenter = null;

        private void Start()
        {
            if (_birdInstaller == null)
            {
                return;
            }

            _birdInstaller.Install();

            
        }

        public void RequestRetry()
        {
            _presenter?.RequestRetry();
        }

        public void OnDestroy()
        {
            _presenter?.Dispose();
        }
    }
}
