using System;
using System.Collections.Generic;
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

        public Carriage()
        {
            InitStepsPerInch();
            this._motor = new CarriageStepperMotor();
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

        public void MoveToPosition(double inches)
        {
            int targetStepPosition = InchesToSteps(inches);
            int deltaSteps = (targetStepPosition - _currentPositionSteps);
            _motor.MoveSteps(deltaSteps);
            _currentPositionSteps = targetStepPosition;
        }


    }
}
