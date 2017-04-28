using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBJJ.Core
{
    public class CutProgram
    {
        public CutProgram(List<JointProfileElement> elements, decimal kerfWidth)
        {
            CutPositions = GenerateFromProfileElements(elements, kerfWidth);
        }

        public List<decimal> CutPositions { get; set; }

        private static List<decimal> GenerateFromProfileElements(List<JointProfileElement> elements, decimal kerfWidth)
        {
            // work out the positions of each slot, and where they start and end
            var slots = new List<SlotCoordinate>();
            decimal pos = 0;
            foreach (var element in elements)
            {
                if (element.Type == JointProfileElement.JointProfileElementType.Slot) {
                    slots.Add(new SlotCoordinate(pos, (pos + element.Width)));
                }
                pos = (pos + element.Width);
            }

            // now work out the cuts to make each slot
            var cuts = new List<decimal>();
            foreach (var slotCoord in slots)
            {
                var cutsForSlot = GetCutsForSlot(slotCoord, kerfWidth);
                cuts.AddRange(cutsForSlot);
            }
            
            return cuts;
        }

        private static List<decimal> GetCutsForSlot(SlotCoordinate slot, decimal kerfWidth)
        {
            var cuts = new List<decimal>();
            if (slot.Width == 0) { return cuts; }
            // first cut
            cuts.Add(slot.Left + kerfWidth);
            if (slot.Width <= kerfWidth) { return cuts; }
            // slot requires multiple cuts, space them out evenly
            decimal widthAfterFirstCut = (slot.Width - kerfWidth);
            var maxAdvance = (kerfWidth*0.9M);
            var remainingCutsRequired = (int)Math.Ceiling(widthAfterFirstCut/maxAdvance);
            for (int i = 1; i <= remainingCutsRequired; i++) {
                var cutPos = (slot.Left + kerfWidth) + (i*(widthAfterFirstCut/remainingCutsRequired));
                cuts.Add(cutPos);
            }
            return cuts;
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
