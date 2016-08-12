using System;
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
        private double _maxWidthInches;
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
            UsePrimaryProfile = true;
            
            this._kerfWidthInches = (double?) LocalSettings.Values["kerfWidthInches"] ?? 0.125d;
            this._maxWidthInches = (double?)LocalSettings.Values["maxWidthInches"] ?? 8.0d;

            Task.Run(ShowReadyIndicator);
        }

        private async Task ShowReadyIndicator() {
            RedLight.StartBlinking();
            GreenLight.StartBlinking();
            await Task.Delay(3000);
            RedLight.TurnOff();
            GreenLight.TurnOff();
        }

        public JointProfile Profile { get; set; }
        public bool UsePrimaryProfile { get; set; }
        public CutProgram CutProgram { get; set; }
        public string StatusMessage { get; set; }

        private void Status(string message) {
            StatusMessage = message;
        }

        public bool ProgramRunning { get; private set; }
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

        public double MaxWidthInches
        {
            get { return _maxWidthInches; }
            set
            {
                _maxWidthInches = value;
                LocalSettings.Values["maxWidthInches"] = value;
            }
        }

        public async Task ReHome()
        {
            Status("Returning to Home position...");
            RedLight.StartBlinking();
            await Task.Run(Carriage.ReHome);
            RedLight.TurnOff();
            Status("");
        }

        public async Task RunProgram()
        {
            ProgramRunning = true;
            CutProgram = new CutProgram(UsePrimaryProfile ? Profile.Elements : Profile.ReverseElements, KerfWidthInches);
            // make sure we're off the table
            while (OnTable) {
                Status("Waiting: Pull back off the blade");
                await Task.Delay(100);
            }

            // get back to zero if not already
            while (OnTable) {
                Status("Waiting: Pull back off the blade");
                await Task.Delay(100);
            }
            Status("Moving to the Home position");
            RedLight.StartBlinking();
            await Carriage.MoveToPosition(0);
            RedLight.TurnOff();

            int cutCounter = 1;
            var positions = CutProgram.CutPositions.Where(p => p <= MaxWidthInches).ToList();
            foreach (var cutPosition in positions)
            {
                while (OnTable) {
                    Status("Waiting: Pull back off the blade");
                    await Task.Delay(100);
                }
                Status($"Moving to the next cut ({cutCounter} of {CutProgram.CutPositions.Count})");
                RedLight.StartBlinking();
                await Carriage.MoveToPosition(cutPosition);
                RedLight.TurnOff();

                Status($"Ready for cut #{cutCounter} of {CutProgram.CutPositions.Count}");
                await MakeTheCut();

                cutCounter++;
            }

            while (OnTable) {
                Status("Waiting: Pull back off the blade");
                await Task.Delay(100);
            }
            Status("Moving to the Home position");
            RedLight.StartBlinking();
            await Carriage.MoveToPosition(0);
            RedLight.TurnOff();

            Status("");

            ProgramRunning = false;
        }

        private async Task MakeTheCut()
        {
            // signal the operator they can make the cut
            GreenLight.TurnOn();

            // wait until operator moves the unit on to the table
            while (!OnTable) { await Task.Delay(500); }
            // now we just moved onto the table
            // wait until we get off the table
            while (OnTable) {
                Status("Cutting");
                await Task.Delay(500);
            }

            GreenLight.TurnOff();
        }

        public void ToggleProfileMode()
        {
            UsePrimaryProfile = !UsePrimaryProfile;
        }

        public List<string> GetWarnings()
        {
            var warnings = new List<string>();

            if (Profile.Elements.Any(e => e.Width < KerfWidthInches))
            {
                warnings.Add("Certain elements of this cut profile are narrower than your blade kerf.");
            }

            var totalProfileWidth = Profile.Elements.Sum(e => e.Width);
            if (totalProfileWidth > MaxWidthInches)
            {
                warnings.Add($"The current profile is wider ({totalProfileWidth:N3} in) than the maximum width of the jig ({MaxWidthInches:N3} in)");
            }

            return warnings;
        }
    }
}
