using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace PBJJ.Core
{
    public class CarriageStepperMotor
    {
        private readonly Stopwatch _sw;

        public CarriageStepperMotor()
        {
            this._sw = new System.Diagnostics.Stopwatch();
        }

        /// <summary>
        /// Moves the stepper by the specified number of steps.
        /// Negative values go in reverse direction.
        /// </summary>
        public void MoveSteps(int steps, IProgress<int> progress, double stepPeriodMilliseconds = 1)
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
                // easydriver steps for every low-to-high transition
                GpioConnections.StepperStepPin.Write(GpioPinValue.Low);
                GpioConnections.StepperStepPin.Write(GpioPinValue.High);

                _sw.Start();
                while ((_sw.Elapsed).TotalMilliseconds < stepPeriodMilliseconds) { }
                _sw.Reset();

                progress?.Report(directionMultiplier * (i + 1));
            }
        }

    }
}
