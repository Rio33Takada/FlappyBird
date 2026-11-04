using KTC.Domain;
using KTC.Presentation;
using UnityEngine;

namespace KTC.Infrastructure
{
    public sealed class ObstacleInstaller : MonoBehaviour
    {
        [SerializeField]
        private ObstacleView _view = null;

        [SerializeField]
        private ObstacleDestructionRuleFactory _ruleFactory = null;

        private ObstaclePresenter _presenter = null;
        private IObstaclePool _ownerPool = null;

        public bool Install(IObstaclePool ownerPool)
        {
            if (_view == null || _ruleFactory == null || ownerPool == null)
            {
                return false;
            }

            IObstacleDestructionRule destructionRule =
                _ruleFactory.CreateRule();

            if (destructionRule == null)
            {
                return false;
            }

            DisposePresenter();

            _ownerPool = ownerPool;

            ObstacleModel model = new(destructionRule);
            _presenter = new ObstaclePresenter(model, _view);

            // TODO: 再利用時の演出・当たり判定・通過判定を初期化する。
            return true;
        }

        public void ReturnToPool()
        {
            if (_ownerPool == null)
            {
                return;
            }

            IObstaclePool ownerPool = _ownerPool;
            _ownerPool = null;

            ownerPool.Release(this);
        }

        public void Release()
        {
            _ownerPool = null;
            DisposePresenter();

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            DisposePresenter();
        }

        private void DisposePresenter()
        {
            _presenter?.Dispose();
            _presenter = null;
        }
    }
}