using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBJJ.Core
{
    public class Carriage
    {
        private double _stepsPerInch;
        private int _currentPositionSteps;
        private CarriageStepperMotor _motor;
        private LimitSwitch _homeLimitSwitch;

        public Carriage()
        {
            InitStepsPerInch();
            this._motor = new CarriageStepperMotor();
            this._homeLimitSwitch = new LimitSwitch(GpioConnections.CarriageHomeLimitSwitchPin);
        }
        
        private void InitStepsPerInch()
        {
            double inchesPerRevolution = (8d/25.4d); // lead screw is 8mm per revolution
            double stepsPerRevolution = (200d*8d); // 200 steps with 1/8 microstepping
            double revolutionsPerInch = (1d/inchesPerRevolution);
            _stepsPerInch = (revolutionsPerInch*stepsPerRevolution);
        }

        private int InchesToSteps(double inches) { return Convert.ToInt32(inches * _stepsPerInch); }
        private double StepsToInches(int steps) { return (steps / _stepsPerInch); }

        public double CurrentPositionInches => StepsToInches(_currentPositionSteps);

        public async Task MoveToPosition(double inches)
        {
            int targetStepPosition = InchesToSteps(inches);
            int deltaSteps = (targetStepPosition - _currentPositionSteps);

            int positionBeforeMove = _currentPositionSteps;
            var moveProgress = new Progress<int>(stepsMoved => {
                _currentPositionSteps = (positionBeforeMove + stepsMoved);
            });

            await _motor.MoveSteps(deltaSteps, moveProgress);
            
            _currentPositionSteps = targetStepPosition;
        }

        public async Task ReHome()
        {
            while (_homeLimitSwitch.IsPressed())
            {
                // move away from switch first in case we're already on it
                await _motor.MoveSteps(1, null);
            }

            await Task.Delay(TimeSpan.FromSeconds(0.5));

            // move on to switch until it engages
            while (_homeLimitSwitch.IsNotPressed())
            {
                // move toward home switch
                await _motor.MoveSteps(-1, null);
            }

            await Task.Delay(TimeSpan.FromSeconds(0.5));

            // switch is now pressed, back off until it's not pressed
            while (_homeLimitSwitch.IsPressed())
            {
                await _motor.MoveSteps(1, null);
            }

            // set our zero point
            _currentPositionSteps = 0;
        }

    }
}
