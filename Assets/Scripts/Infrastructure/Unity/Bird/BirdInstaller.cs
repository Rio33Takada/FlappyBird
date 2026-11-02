using System;
using UnityEngine;
using KTC.Presentation;
using KTC.Domain;

namespace KTC.Infrastructure
{
    public class BirdInstaller : MonoBehaviour
    {
        [SerializeField]
        private BirdView _birdPrefab;

        private BirdView _birdView;
        private BirdPresenter _presenter;

        private void Awake()
        {
            BirdModel model = new();

            _birdView = Instantiate(_birdPrefab);

            _presenter = new BirdPresenter(
                model,
                _birdView);
        }

        private void OnDestroy()
        {
            _presenter.Dispose();
            _presenter = null;
        }
    }
}
