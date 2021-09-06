using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPSO.CMP.CommonFunctions.ParameterClasses
{
    public static  class WeightObjective
    {

        #region
        public static float DatLastCoef { get; set; }
        public static float PriorityCoef { get; set; }
        public static float DurabilityCoef { get; set; }
        public static float CapPlanCoef { get; set; }
        public static float SetupCoef { get; set; }
        public static float LenProgCoef { get; set; }
        public static float WeiProgCoef { get; set; }
        public static  float SarfaslCoef { get; set; }
        public static float LevelStorgeCoef { get; set; }
        public static float CountProgCoef { get; set; }
        //Tan- SRM-skp1
        public static float WorkRollWeiCoef { get; set; }
        //Tan- SRM-skp1
        public static float WorkRollLenCoef { get; set; }
        //Pic
        public static float StartCampCoef { get; set; }
        //SRM
        //public static float ContinueTypeProgCoef { get; set; }
        //Pic- TAn-skp1
        public static float ContinuePatternCoef { get; set; }
        //Pic
        public static float PriorStationCoef { get; set; }
        //Tan- Pic-skp1
        public static float PriorGroupTypeMisCoef { get; set; }

        
    

        #endregion
      
    }
}
