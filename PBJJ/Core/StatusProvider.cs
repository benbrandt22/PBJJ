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
                ProgramRunning = app.ProgramRunning,
                OnTable = app.OnTable,
                CurrentPositionInches = app.Carriage.CurrentPositionInches,
                KerfWidthInches = app.KerfWidthInches,
                ProfileName = $"{app.Profile.Name}{(app.UsePrimaryProfile ? "" : " (Reverse)")}",
                ProfileElements = (app.UsePrimaryProfile ? app.Profile.Elements : app.Profile.ReverseElements),
                UsePrimaryProfile = app.UsePrimaryProfile,
                StatusMessage = app.StatusMessage,
                Warnings = app.GetWarnings(),
            };
        }
    }
}
