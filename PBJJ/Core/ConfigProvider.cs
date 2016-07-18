using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBJJ.Core
{
    public static class ConfigProvider
    {
        public static PbjjConfigViewModel GetConfiguration()
        {
            var app = ProgrammableBoxJointJigApp.Instance;
            
            return new PbjjConfigViewModel()
            {
                KerfWidthInches = app.KerfWidthInches
            };
        }
    }
}
