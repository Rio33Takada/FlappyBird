using System;
using KTC.Presentation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KTC.Infrastructure
{
    public sealed class GameOverView : MonoBehaviour, IGameOverView
    {
        [SerializeField]
        private GameObject _panel = null;

        [SerializeField]
        private TMPro.TextMeshProUGUI _scoreText = null;

        [SerializeField]
        private UnityEngine.UI.Button _retryButton = null;

        public event Action OnRetryRequested;

        private void OnEnable()
        {
            if (_retryButton != null)
            {
                _retryButton.onClick.AddListener(HandleRetryClicked);
            }
        }

        private void OnDisable()
        {
            if (_retryButton != null)
            {
                _retryButton.onClick.RemoveListener(HandleRetryClicked);
            }
        }

        public void SetVisible(bool visible)
        {
            if (_panel != null)
            {
                _panel.SetActive(visible);
            }

            if (_retryButton == null || EventSystem.current == null)
            {
                return;
            }

            if (visible && _retryButton.isActiveAndEnabled)
            {
                _retryButton.Select();
            }
            else if (EventSystem.current.currentSelectedGameObject == _retryButton.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        public void SetScore(int score)
        {
            if (_scoreText != null)
            {
                _scoreText.text = score.ToString();
            }
        }

        private void HandleRetryClicked()
        {
            OnRetryRequested?.Invoke();
        }
    }
}
