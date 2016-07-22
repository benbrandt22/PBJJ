using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBJJ.Core
{
    public class PbjjAppStatusViewModel
    {
        public bool ProgramRunning { get; set; }
        public bool OnTable { get; set; }
        public double CurrentPositionInches { get; set; }
        public double KerfWidthInches { get; set; }
        public bool UsePrimaryProfile { get; set; }
        public string ProfileName { get; set; }
        public List<JointProfileElement> ProfileElements { get; set; }
        public string StatusMessage { get; set; }
    }
}
