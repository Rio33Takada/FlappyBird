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

        public void StartFlight()
        {
            if (State == BirdState.Ready)
            {
                State = BirdState.Normal;
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

            if (heldSeconds >= _chargeAttackSeconds)
            {
                State = BirdState.ChargeReady;
                return;
            }

            if (heldSeconds >= _chargeStartSeconds)
            {
                State = BirdState.Charging;
            }
        }

        public BirdInputResult Release(double time)
        {
            if (!_pressedAt.HasValue)
            {
                return BirdInputResult.None;
            }

            double heldSeconds = time - _pressedAt.Value;
            CancelInput();

            if (heldSeconds >= _chargeAttackSeconds)
            {
                _isReturning = false;
                State = BirdState.ChargeAttacking;
                return BirdInputResult.ChargeAttack;
            }

            return heldSeconds < _chargeStartSeconds
                ? BirdInputResult.Jump
                : BirdInputResult.None;
        }

        public void CancelInput()
        {
            _pressedAt = null;

            if (State == BirdState.Charging || State == BirdState.ChargeReady)
            {
                State = _isReturning ? BirdState.Returning : BirdState.Normal;
            }
        }

        public void FinishAttack()
        {
            if (State != BirdState.ChargeAttacking)
            {
                return;
            }

            _isReturning = true;
            State = BirdState.Returning;
        }

        public void FinishReturn()
        {
            _isReturning = false;

            if (State == BirdState.Returning)
            {
                State = BirdState.Normal;
            }
        }

        public bool TryDie()
        {
            if (State == BirdState.Ready || State == BirdState.Dead)
            {
                return false;
            }

            _pressedAt = null;
            _isReturning = false;
            State = BirdState.Dead;
            return true;
        }
    }
}
