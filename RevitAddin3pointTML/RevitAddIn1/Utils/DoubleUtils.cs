using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddIn1.Utils
{
    public static class DoubleUtils
    {
        public static double FeetToMeet(this double feet) => feet * 0.3048;
        public static double FeetToMeet(this int feet) => feet * 0.3048;
        public static double MeetToFeet(this double met) => met * 3.280840;
        public static double MeetToFeet(this int met) => met * 3.280840;
    }
}
