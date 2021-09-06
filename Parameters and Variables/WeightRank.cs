using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPSO.CMP.CommonFunctions.ParameterClasses
{
    public static class WeightRank
    {
        public static float DatLastCoef { get; set; }
        public static float PriorityCoef { get; set; }
        public static float DurabilityCoef { get; set; }
        public static float LevelStorgeCoef { get; set; }
        //Pic
        public static float SameWidGroupCoef = 1;
        //Pic
        public static float RankTotalCoef = 1;
    }
}
