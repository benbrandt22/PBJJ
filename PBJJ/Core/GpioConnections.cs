﻿using System;
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
        private static GpioController gpioController;

        public static GpioPin StepperDirectionPin;
        public static GpioPin StepperStepPin;

        public static GpioPin CarriageHomeLimitSwitchPin;

        public static GpioPin OnTableLimitSwitchPin;

        public static GpioPin RedLightGpioPin;
        public static GpioPin GreenLightGpioPin;

        static GpioConnections()
        {
            // static constructor initializes pins only once the first time they are called for.
            InitializeGpio();
        }

        private static void InitializeGpio()
        {
            gpioController = GpioController.GetDefault();

            // stepper driver control
            StepperDirectionPin = InitializeOutput(27);
            StepperStepPin = InitializeOutput(22);

            // switch inputs
            CarriageHomeLimitSwitchPin = InitializeInput(19);
            OnTableLimitSwitchPin = InitializeInput(21);

            // output LEDs
            RedLightGpioPin = InitializeOutput(4);
            GreenLightGpioPin = InitializeOutput(18);
        }

        private static GpioPin InitializeOutput(int gpioPinNumber)
        {
            var gpioPin = gpioController.OpenPin(gpioPinNumber);
            gpioPin.SetDriveMode(GpioPinDriveMode.Output);
            return gpioPin;
        }

        private static GpioPin InitializeInput(int gpioPinNumber)
        {
            var gpioPin = gpioController.OpenPin(gpioPinNumber);
            gpioPin.SetDriveMode(
                gpioPin.IsDriveModeSupported(GpioPinDriveMode.InputPullUp)
                    ? GpioPinDriveMode.InputPullUp
                    : GpioPinDriveMode.Input);
            gpioPin.DebounceTimeout = TimeSpan.FromMilliseconds(50);
            return gpioPin;
        }
    }
}
