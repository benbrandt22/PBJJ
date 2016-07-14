using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace PBJJ.Core
{
    public class CarriageStepperMotor
    {
        /// <summary>
        /// Moves the stepper by the specified number of steps.
        /// Negative values go in reverse direction.
        /// </summary>
        public void MoveSteps(int steps, IProgress<int> progress)
        {
            if (steps == 0) { return; }
            int directionMultiplier;

            if (steps > 0)
            {
                // forward
                directionMultiplier = 1;
                GpioConnections.StepperDirectionPin.Write(GpioPinValue.High);
            }
            else
            {
                // reverse
                directionMultiplier = -1;
                GpioConnections.StepperDirectionPin.Write(GpioPinValue.Low);
            }

            for (int i = 0; i < Math.Abs(steps); i++)
            {
                // motor steps for every low-to-high transition
                GpioConnections.StepperStepPin.Write(GpioPinValue.Low);
                GpioConnections.StepperStepPin.Write(GpioPinValue.High);
                Task.Delay(1).Wait(1);
                progress.Report(directionMultiplier*(i + 1));
            }
        }
    }
}
