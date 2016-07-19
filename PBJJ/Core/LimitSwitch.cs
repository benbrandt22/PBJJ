using Windows.Devices.Gpio;

namespace PBJJ.Core
{
    /// <summary>
    /// Represents a limit switch with the Normally Open contacts
    /// connected to ground and the GOPIO input pin.
    /// </summary>
    public class LimitSwitch
    {
        private readonly GpioPin _inputPin;

        public LimitSwitch(GpioPin inputPin)
        {
            _inputPin = inputPin;
        }

        public bool IsPressed()
        {
            var pinValue = _inputPin.Read();
            return pinValue == GpioPinValue.Low;
        }

        public bool IsNotPressed()
        {
            return !IsPressed();
        }
    }
}
