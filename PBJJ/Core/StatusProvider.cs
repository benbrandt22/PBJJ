using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBJJ.Core
{
    public static class StatusProvider
    {
        public static PbjjAppStatusViewModel GetCurrentStatus()
        {
            var app = ProgrammableBoxJointJigApp.Instance;

            return new PbjjAppStatusViewModel()
            {
                ProgramRunning = app.ProgramRunning
            };
        }
    }
}
