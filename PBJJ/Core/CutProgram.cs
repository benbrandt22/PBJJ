using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBJJ.Core
{
    public class CutProgram
    {
        public CutProgram(List<JointProfileElement> elements, decimal kerfWidth)
        {
            GenerateFromProfileElements(elements, kerfWidth);
        }

        public List<decimal> CutPositions { get; set; }

        private void GenerateFromProfileElements(List<JointProfileElement> elements, decimal kerfWidth)
        {
            // work out the positions of each slot, and where they start and end
            var slotStartsAndEnds = new List<SlotCoordinate>();
            decimal pos = 0;
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
                    var slot = new SlotCoordinate(pos, (pos + element.Width));
                    slotStartsAndEnds.Add(slot);
                    pos = (pos + element.Width);
                }
            }

            // now work out the cuts to make each slot
            var cuts = new List<decimal>();
            foreach (var slotCoord in slotStartsAndEnds)
            {
                decimal slotCutPos = slotCoord.Left + kerfWidth;
                cuts.Add(slotCutPos);
                while (slotCutPos < slotCoord.Right)
                {
                    // advance most of kerf
                    slotCutPos += (0.9M*kerfWidth);
                    // ensure we don't overshoot the end of the slot
                    slotCutPos = Math.Min(slotCutPos, slotCoord.Right);
                    // add the cut
                    cuts.Add(slotCutPos);
                }
            }


            CutPositions = cuts;
        }

        private class SlotCoordinate {
            public SlotCoordinate(decimal left, decimal right)
            {
                if (left > right) {
                    throw new ArgumentOutOfRangeException(nameof(left),
                        "SlotCoordinate left value cannot be greater than right value");
                }
                Left = left;
                Right = right;
            }
            public decimal Left { get; private set; }
            public decimal Right { get; private set; }
            public decimal Width => (Right-Left);
        }
    }
}
