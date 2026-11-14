using UnityEngine;
using KTC.Presentation;
using KTC.Domain;

namespace KTC.Infrastructure
{
    public class BirdInstaller : MonoBehaviour
    {
        [SerializeField]
        private BirdView _birdPrefab = null;

        [SerializeField]
        private Transform _spawnPoint = null;

        [SerializeField, Min(0.01f)]
        private float _chargeStartSeconds = 0.15f;

        [SerializeField, Min(0.01f)]
        private float _chargeAttackSeconds = 0.6f;

        private BirdPresenter _presenter = null;

        public void Install()
        {
            if (_birdPrefab == null)
            {
                return;
            }

            BirdModel model = new(_chargeStartSeconds, _chargeAttackSeconds);
            Vector3 spawnPosition = _spawnPoint != null
                ? _spawnPoint.position
                : _birdPrefab.transform.position;

            BirdView birdView = Instantiate(
                _birdPrefab,
                spawnPosition,
                _birdPrefab.transform.rotation);

            birdView.Initialize();

            _presenter = new BirdPresenter(model, birdView);
            // TODO: 生成直後は開始待ちにし、StartFlight の呼び出しをゲーム開始時へ移す。
            // TODO: 組み立てた鳥の IGameCycleBird を GameSceneInstaller に渡せるようにする。
            model.StartFlight();
        }

        private void OnDestroy()
        {
            _presenter?.Dispose();
            _presenter = null;
        }
    }
}
