using Windows.Devices.Gpio;

namespace PBJJ.Core
{
    /// <summary>
    /// Represents a limit switch with the Normally Closed contacts
    /// connected to ground and the GPIO input pin.
    /// </summary>
    public class NormallyClosedLimitSwitch
    {
        private readonly GpioPin _inputPin;

        public NormallyClosedLimitSwitch(GpioPin inputPin)
        {
            _inputPin = inputPin;
        }

        public bool IsPressed()
        {
            var pinValue = _inputPin.Read();
            return pinValue == GpioPinValue.High;
        }

        public bool IsNotPressed()
        {
            return !IsPressed();
        }
    }
}
