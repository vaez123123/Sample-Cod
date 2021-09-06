using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPSO.CMP.CommonFunctions.ParameterClasses
{
    public class Roll
    {
        public DateTime DatRollEnter { get; set; }
        public bool FirstPlan;// false =firstPlan 

        public double LenOpt { get; set; }// مقدار بهینه
        public double WeiOpt { get; set; }
        //Pic
        public int DatOpt { get; set; }

        // کار شده قبل از الگوریتم
        public double WeiDB { get; set; }
        public double LenDB { get; set; }
        //Pic
        public int DatDB { get; set; }

        // کار شده release
        public double WeiRelease { get; set; }
        public double LenRelease { get; set; }
        //Pic
        public int DatRelease { get; set; }

        //کار شده تاحال توسط الگوریتم
        //Tan-SRM
        public double CurrentTotalFixWei { get; set; }
        public double CurrentTotalFixLen { get; set; }

        //Tan-SRM
        public double LowerPerc;
        //Tan-SRM
        public double UpperPerc;
    }
}
