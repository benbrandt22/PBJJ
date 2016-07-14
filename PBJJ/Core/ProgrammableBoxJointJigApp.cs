using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PBJJ.Core
{
    public class ProgrammableBoxJointJigApp
    {
        private static ProgrammableBoxJointJigApp instance;
        public Carriage Carriage;

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
        }
        
        public bool ProgramRunning { get; }
        public double KerfWidthInches { get; set; }
        
    }
}
