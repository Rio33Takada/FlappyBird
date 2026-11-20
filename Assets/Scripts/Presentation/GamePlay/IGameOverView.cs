using System;

namespace KTC.Presentation
{
    public interface IGameOverView
    {
        event Action OnRetryRequested;

        void SetVisible(bool visible);
        void SetScore(int score);
    }
}
