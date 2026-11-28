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
        private BirdView _birdView = null;

        public IGameCycleBird Bird => _birdView != null ? _presenter : null;

        public void Install()
        {
            if (_presenter != null && _birdView != null)
            {
                return;
            }

            if (_birdPrefab == null)
            {
                return;
            }

            if (!float.IsFinite(_chargeStartSeconds) || _chargeStartSeconds <= 0f ||
                !float.IsFinite(_chargeAttackSeconds) || _chargeAttackSeconds <= _chargeStartSeconds)
            {
                Debug.LogError("チャージ時間は 0 < 開始秒数 < 攻撃秒数に設定してください。", this);
                return;
            }

            _presenter?.Dispose();
            _presenter = null;

            BirdModel model = new(_chargeStartSeconds, _chargeAttackSeconds);
            Vector3 spawnPosition = _spawnPoint != null
                ? _spawnPoint.position
                : _birdPrefab.transform.position;

            _birdView = Instantiate(
                _birdPrefab,
                spawnPosition,
                _birdPrefab.transform.rotation);

            _birdView.Initialize();

            _presenter = new BirdPresenter(model, _birdView);
        }

        private void OnDestroy()
        {
            _presenter?.Dispose();
            _presenter = null;

            if (_birdView != null)
            {
                Destroy(_birdView.gameObject);
                _birdView = null;
            }
        }
    }
}
