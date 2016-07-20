﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace PBJJ.Core
{
    public class ProgrammableBoxJointJigApp
    {
        private static ProgrammableBoxJointJigApp instance;

        public Carriage Carriage;
        private double _kerfWidthInches;
        private LimitSwitch _onTableLimitSwitch;
        private LedLight RedLight;
        private LedLight GreenLight;

        private ApplicationDataContainer LocalSettings { get; set; }

        public static ProgrammableBoxJointJigApp Instance {
            get
            {
                if (instance == null)
                {
                    instance = new ProgrammableBoxJointJigApp();
                }
                return instance;
            }
        }

        private ProgrammableBoxJointJigApp()
        {
            // private constructor to ensure single instance in static "Instance" field
            Carriage = new Carriage();
            _onTableLimitSwitch = new LimitSwitch(GpioConnections.OnTableLimitSwitchPin);
            RedLight = new LedLight(GpioConnections.RedLightGpioPin);
            GreenLight = new LedLight(GpioConnections.GreenLightGpioPin);

            LocalSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Profile = ProfileGenerator.GenerateStandardProfile(0.25, 6);
            
            this._kerfWidthInches = (double?) LocalSettings.Values["kerfWidthInches"] ?? 0.125d;
            
        }

        public JointProfile Profile { get; set; }
        public CutProgram CutProgram { get; set; }

        public bool ProgramRunning { get; }
        public bool OnTable => _onTableLimitSwitch.IsPressed();

        public double KerfWidthInches
        {
            get { return _kerfWidthInches; }
            set
            {
                _kerfWidthInches = value;
                LocalSettings.Values["kerfWidthInches"] = value;
            }
        }
    }
}
