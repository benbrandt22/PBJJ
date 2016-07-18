using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBJJ.Core
{
    public class JointProfile
    {
        public string Name { get; set; }

        public JointProfile(string name)
        {
            Name = name;
            Elements = new List<JointProfileElement>();
        }

        public List<JointProfileElement> Elements { get; set; }
    }

    public class JointProfileElement
    {
        public JointProfileElement(JointProfileElementType type, double width)
        {
            this.Type = type;
            this.Width = width;
        }

        public double Width { get; set; }

        public JointProfileElementType Type { get; set; }

        public enum JointProfileElementType
        {
            Slot = 0,
            Finger = 1
        }

        public JointProfileElement Reverse()
        {
            return new JointProfileElement(this.Type.Reverse(), this.Width);
        }
    }

    public static class JointProfileExtensions
    {
        public static JointProfileElement.JointProfileElementType Reverse(this JointProfileElement.JointProfileElementType elementType)
        {
            return (elementType == JointProfileElement.JointProfileElementType.Finger
                ? JointProfileElement.JointProfileElementType.Slot
                : JointProfileElement.JointProfileElementType.Finger);
        }
    }

}
