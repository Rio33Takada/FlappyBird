using System;

namespace KTC.Domain
{
    public sealed class BirdModel
    {
        private readonly double _chargeStartSeconds;
        private readonly double _chargeAttackSeconds;

        private double? _pressedAt = null;
        private bool _isReturning = false;

        public BirdState State { get; private set; } = BirdState.Ready;
        public float ChargeProgress { get; private set; }

        public event Action<BirdState> StateChanged;

        public BirdModel(double chargeStartSeconds, double chargeAttackSeconds)
        {
            if (!double.IsFinite(chargeStartSeconds) || chargeStartSeconds <= 0d)
            {
                throw new ArgumentOutOfRangeException(nameof(chargeStartSeconds));
            }

            if (!double.IsFinite(chargeAttackSeconds) ||
                chargeAttackSeconds <= chargeStartSeconds)
            {
                throw new ArgumentOutOfRangeException(nameof(chargeAttackSeconds));
            }

            _chargeStartSeconds = chargeStartSeconds;
            _chargeAttackSeconds = chargeAttackSeconds;
        }

        public void Reset()
        {
            ClearInput();
            _isReturning = false;
            ChangeState(BirdState.Ready);
        }

        public void StartFlight()
        {
            if (State == BirdState.Ready)
            {
                ChangeState(BirdState.Normal);
            }
        }

        public void Press(double time)
        {
            if ((State == BirdState.Normal || State == BirdState.Returning) &&
                !_pressedAt.HasValue)
            {
                _pressedAt = time;
            }
        }

        public void UpdateHold(double time)
        {
            if (!_pressedAt.HasValue)
            {
                return;
            }

            double heldSeconds = time - _pressedAt.Value;
            ChargeProgress = (float)Math.Clamp(
                (heldSeconds - _chargeStartSeconds) / (_chargeAttackSeconds - _chargeStartSeconds),
                0d, 1d);

            if (heldSeconds >= _chargeAttackSeconds)
            {
                ChangeState(BirdState.ChargeReady);
                return;
            }

            if (heldSeconds >= _chargeStartSeconds)
            {
                ChangeState(BirdState.Charging);
            }
        }

        public BirdInputResult Release(double time)
        {
            if (!_pressedAt.HasValue)
            {
                return BirdInputResult.None;
            }

            double heldSeconds = time - _pressedAt.Value;
            if (heldSeconds >= _chargeAttackSeconds)
            {
                ClearInput();
                _isReturning = false;
                ChangeState(BirdState.ChargeAttacking);
                return BirdInputResult.ChargeAttack;
            }

            CancelInput();

            return heldSeconds < _chargeStartSeconds
                ? BirdInputResult.Jump
                : BirdInputResult.None;
        }

        public void CancelInput()
        {
            ClearInput();

            if (State == BirdState.Charging || State == BirdState.ChargeReady)
            {
                ChangeState(_isReturning ? BirdState.Returning : BirdState.Normal);
            }
        }

        public void FinishAttack()
        {
            if (State != BirdState.ChargeAttacking)
            {
                return;
            }

            _isReturning = true;
            ChangeState(BirdState.Returning);
        }

        public void FinishReturn()
        {
            _isReturning = false;

            if (State == BirdState.Returning)
            {
                ChangeState(BirdState.Normal);
            }
        }

        public bool TryDieByCollision()
        {
            return State != BirdState.ChargeAttacking && TryDie();
        }

        public bool TryDie()
        {
            if (State == BirdState.Ready || State == BirdState.Dead)
            {
                return false;
            }

            ClearInput();
            _isReturning = false;
            ChangeState(BirdState.Dead);
            return true;
        }

        private void ClearInput()
        {
            _pressedAt = null;
            ChargeProgress = 0f;
        }

        private void ChangeState(BirdState state)
        {
            if (State == state)
            {
                return;
            }

            State = state;
            StateChanged?.Invoke(state);
        }
    }
}
