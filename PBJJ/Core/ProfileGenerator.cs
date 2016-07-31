using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBJJ.Core
{
    public static class ProfileGenerator
    {
        public static JointProfile GenerateStandardProfile(double fingerWidthInches, double overallWidthInches)
        {
            string name = $"Standard {fingerWidthInches:N3}in fingers over {overallWidthInches}in";

            var profile = new JointProfile(name);

            JointProfileElement.JointProfileElementType currentElementType = JointProfileElement.JointProfileElementType.Finger;

            double currentWidth = 0;
            do
            {
                profile.Elements.Add(new JointProfileElement(currentElementType, fingerWidthInches));

                currentElementType = currentElementType.Reverse();
                currentWidth = (currentWidth + fingerWidthInches);
            } while (currentWidth < overallWidthInches);

            return profile;
        }
    }
}
