using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBJJ.Core
{
    public static class ProfileGenerator
    {
        public static JointProfile GenerateStandardProfile(decimal fingerWidthInches, decimal overallWidthInches)
        {
            string name = $"Standard {fingerWidthInches:N3}in fingers over {overallWidthInches}in";

            var profile = new JointProfile(name);

            JointProfileElement.JointProfileElementType currentElementType = JointProfileElement.JointProfileElementType.Finger;

            decimal currentWidth = 0;
            do
            {
                profile.Elements.Add(new JointProfileElement(currentElementType, fingerWidthInches));

                currentElementType = currentElementType.Reverse();
                currentWidth = (currentWidth + fingerWidthInches);
            } while (currentWidth < overallWidthInches);

            if (profile.Elements.Any() && profile.TotalWidth > overallWidthInches)
            {
                // ensure the profile doesn't exceed the target width by shortening the last element
                profile.Elements.Last().Width -= (currentWidth - overallWidthInches);
            }

            return profile;
        }

        public static JointProfile GenerateFingerSlotCountProfile(int fingerSlotCount, decimal overallWidthInches) {
            string name = $"{fingerSlotCount} fingers over {overallWidthInches}in";

            var profile = new JointProfile(name);

            JointProfileElement.JointProfileElementType currentElementType = JointProfileElement.JointProfileElementType.Finger;

            for (int positionNumber = 1; positionNumber <= fingerSlotCount; positionNumber++) {
                decimal totalPreviousWidth = profile.TotalWidth;
                decimal thisElementWidth = Math.Round( (positionNumber*(overallWidthInches/fingerSlotCount)) - totalPreviousWidth , 3);
                profile.Elements.Add(new JointProfileElement(currentElementType, thisElementWidth));
                currentElementType = currentElementType.Reverse();
            }
            
            return profile;
        }
        

    }
}
