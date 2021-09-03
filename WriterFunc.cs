using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using IPSO.CMP.CommonFunctions.ParameterClasses;

namespace IPSO.CMP.CommonFunctions.Functions
{
    public class WriterFunc
    {
        public static void writerCapProg(int number, string name, string pathWriter, List<CapPlanUpDate> CapPlanUpDates)
        {
            int z = -1;
            double wei = 0;
            string route;
            string exten = ".txt";

            route = pathWriter  + name + exten;
            FileStream fk2;

            fk2 = new FileStream(route, FileMode.Append, FileAccess.Write);
            StreamWriter stream2 = new StreamWriter(fk2);

            foreach (var i in CapPlanUpDates)
            {
                int Pf = i.PfId;
                if (z != Pf)
                {
                    z = Pf;
                    wei = i.RespondProgPf;
                    if (wei > 0)
                    {
                        stream2.Write(Convert.ToString(number) + "\t" + Convert.ToString(Pf) + "\t" + Convert.ToString(wei));
                        stream2.WriteLine();
                    }
                }
            }
            //stream2.WriteLine();
            stream2.Close();
        }

       
    }
}
