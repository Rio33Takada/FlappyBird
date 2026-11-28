using KTC.Domain;
using UnityEngine;

namespace KTC.Infrastructure
{
    public sealed class BirdAnimationView : MonoBehaviour
    {
        private static readonly int Fly = Animator.StringToHash("Fly");
        private static readonly int IsCharging = Animator.StringToHash("IsCharging");
        private static readonly int IsChargeAttacking = Animator.StringToHash("IsChargeAttacking");
        private static readonly int ChargeProgress = Animator.StringToHash("ChargeProgress");
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Idle = Animator.StringToHash("Base Layer.Idle");

        [SerializeField]
        private Animator _animator = null;

        public void SetState(BirdState state)
        {
            if (_animator == null)
            {
                return;
            }

            _animator.SetBool(
                IsCharging,
                state == BirdState.Charging || state == BirdState.ChargeReady);
            _animator.SetBool(IsChargeAttacking, state == BirdState.ChargeAttacking);

            if (state == BirdState.Dead)
            {
                _animator.ResetTrigger(Fly);
                _animator.SetTrigger(Die);
            }
        }

        public void SetChargeProgress(float progress)
        {
            if (_animator != null)
            {
                _animator.SetFloat(ChargeProgress, Mathf.Clamp01(progress));
            }
        }

        public void PlayJump(BirdState state)
        {
            if (_animator == null ||
                (state != BirdState.Normal && state != BirdState.Returning))
            {
                return;
            }

            _animator.SetTrigger(Fly);
        }

        public void ResetForRun()
        {
            if (_animator == null)
            {
                return;
            }

            _animator.ResetTrigger(Fly);
            _animator.ResetTrigger(Die);
            _animator.SetBool(IsCharging, false);
            _animator.SetBool(IsChargeAttacking, false);
            _animator.SetFloat(ChargeProgress, 0f);
            _animator.Play(Idle, 0, 0f);
        }
    }
}
