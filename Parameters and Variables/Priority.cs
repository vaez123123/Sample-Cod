using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPSO.CMP.CommonFunctions.ParameterClasses
{
    public class Priority
    {
        public int CodMis;
        public string MisProg;
        //0 agar olaviat nadarad
        //1 olaviat darad
        public int PriorityProg;

        public Priority(int codMis, string misProg, int priorityProg)
        {
            this.CodMis = codMis;
            this.MisProg = misProg;
            this.PriorityProg = priorityProg;
        }
    }
}
