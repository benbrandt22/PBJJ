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
        public double CurrentPositionInches { get; set; }
        public double KerfWidthInches { get; set; }
        public JointProfile Profile { get; set; }
    }
}
