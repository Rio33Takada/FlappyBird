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
            // TODO: 鳥の組み立て結果と Inspector で受け取るステージ参照を null チェックし、Model と Presenter を生成して接続する。

            
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
