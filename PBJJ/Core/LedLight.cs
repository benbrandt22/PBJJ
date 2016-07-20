using System;
using System.Threading;
using Windows.Devices.Gpio;

namespace PBJJ.Core
{
    /// <summary>
    /// Represents an LED (with inline resistor) with the positive lead connected
    /// to positive voltage and connected to the output pin on the other end.
    /// </summary>
    public class LedLight
    {
        private readonly GpioPin _outputPin;
        private GpioPinValue pinValue;
        private readonly Timer blinkTimer;
        
        public LedLight(GpioPin outputPin)
        {
            _outputPin = outputPin;
            blinkTimer = new Timer(BlinkTimerTick, null, Timeout.InfiniteTimeSpan, TimeSpan.FromSeconds(1));
        }
        
        public void TurnOn() {
            StopBlinking();
            pinValue = GpioPinValue.Low;
            _outputPin.Write(pinValue);
        }

        public void TurnOff() {
            StopBlinking();
            pinValue = GpioPinValue.High;
            _outputPin.Write(pinValue);
        }

        private void BlinkTimerTick(object state)
        {
            pinValue = ((pinValue == GpioPinValue.Low) ? GpioPinValue.High : GpioPinValue.Low);
            _outputPin.Write(pinValue);
        }

        public void StartBlinking(TimeSpan period) {
            if (period <= TimeSpan.Zero)
            {
                TurnOn();
                return;
            }
            blinkTimer.Change(TimeSpan.Zero, period);
        }

        private void StopBlinking()
        {
            blinkTimer.Change(Timeout.InfiniteTimeSpan, TimeSpan.FromSeconds(1));
        }

    }
}
