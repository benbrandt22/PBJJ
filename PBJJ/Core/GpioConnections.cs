using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI.Notifications;

namespace PBJJ.Core
{
    /// <summary>
    /// Class for keeping all the pin configuration code in one place.
    /// Static to ensure it's only set up once.
    /// </summary>
    public static class GpioConnections
    {
        public static GpioPin StepperDirectionPin;
        public static GpioPin StepperStepPin;

        static GpioConnections()
        {
            // static constructor initializes pins only once the first time they are called for.
            InitializeGpio();
        }

        private static void InitializeGpio()
        {
            var gpio = GpioController.GetDefault();

            StepperDirectionPin = gpio.OpenPin(5);
            StepperDirectionPin.SetDriveMode(GpioPinDriveMode.Output);

            StepperStepPin = gpio.OpenPin(13);
            StepperStepPin.SetDriveMode(GpioPinDriveMode.Output);

        }
    }
}
