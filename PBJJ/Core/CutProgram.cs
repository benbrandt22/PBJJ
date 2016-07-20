using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBJJ.Core
{
    public class CutProgram
    {
        public CutProgram(List<JointProfileElement> elements, double kerfWidth)
        {
            GenerateFromProfileElements(elements, kerfWidth);
        }

        public List<double> CutPositions { get; set; }

        private void GenerateFromProfileElements(List<JointProfileElement> elements, double kerfWidth)
        {
            // work out the positions of each slot, and where they start and end
            var slotStartsAndEnds = new List<Tuple<double,double>>();
            double pos = 0;
            foreach (var element in elements)
            {
                if (element.Type == JointProfileElement.JointProfileElementType.Finger)
                {
                    // ignore finger, advance to next
                    pos = (pos + element.Width);
                }
                else
                {
                    // slot...
                    var slot = new Tuple<double, double>(pos, (pos + element.Width));
                    slotStartsAndEnds.Add(slot);
                    pos = (pos + element.Width);
                }
            }

            // now work out the cuts to make each slot
            List<double> cuts = new List<double>();
            foreach (var slotCoord in slotStartsAndEnds)
            {
                double slotCutPos = slotCoord.Item1 + kerfWidth;
                cuts.Add(slotCutPos);
                while (slotCutPos < slotCoord.Item2)
                {
                    // advance most of kerf
                    slotCutPos += (0.9*kerfWidth);
                    // ensure we don't overshoot the end of the slot
                    slotCutPos = Math.Min(slotCutPos, slotCoord.Item2);
                    // add the cut
                    cuts.Add(slotCutPos);
                }
            }


            CutPositions = cuts;
        }
    }
}
