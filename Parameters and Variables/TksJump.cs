using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPSO.CMP.CommonFunctions.ParameterClasses
{
    public class TksJump    //DB
    {
        public int CodMis { get; private set; }

        public float TksFrom { get; private set; }
        public float TksTo { get; private set; }
        public float ValueJumpInput { get; set; }
        public float PercentJumpInput { get; set; }
        public float ValueJumpOutput { get; set; }
        public float PercentJumpOutput { get; set; }
        public int FlgInputOutput { get; set; }
        //Tan
        public float LimitedJumpInput;
        //Tan
        public string MisProg;
        /// <summary>
        /// نزولی =2 
        /// 1= صعودی
        /// 0= نوسانی
        /// </summary>
        public int flgSeqThick;


        public TksJump(int codMis, string mis,float tksFrom, float tksTo, int flgInputOutput)
        {
            this.CodMis = codMis;
            this.MisProg = mis;
            this.TksFrom = tksFrom;
            this.TksTo = tksTo;
            this.FlgInputOutput = flgInputOutput;
        }

        public TksJump() { }
    }
}
