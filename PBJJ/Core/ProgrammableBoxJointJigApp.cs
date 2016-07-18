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
            LocalSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Profile = ProfileGenerator.GenerateStandardProfile(0.25, 6);
            CutProgram = new CutProgram();
            

            this._kerfWidthInches = (double?) LocalSettings.Values["kerfWidthInches"] ?? 0.125d;

            CutProgram.GenerateFromProfileElements(Profile.Elements, KerfWidthInches);
        }

        public JointProfile Profile { get; set; }
        public CutProgram CutProgram { get; set; }

        public bool ProgramRunning { get; }

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
