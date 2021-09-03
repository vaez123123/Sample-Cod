using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.CommonFunctions.ParameterClasses;

namespace SkinPass1Scheduling
{
    public partial class SkinPass1Model
    {
        public void readData()
        {


            readerFunc.readCommonData(Lst, InnerParameter.counterCoil, InnerParameter.counterCoilRelease,
                                     InnerParameter.counterCoilReleaseOtherSt, "Width_IN_SKP1", "THICKNESS", "no_tksOut", "TYPEPROGMIS", "TRIM", "OIL");
        



        
      
            Status.CurrTime = DateTime.Now;
        

            creatNote("capPlanProg.txt");

            creatNote("Main.txt");
            creatNote("local.txt");
            creatNote("capPlanRelease.txt");

            //creatNote("countProg.txt");
            //creatNote("localCoil.txt");
            runSkinPass1Model();

         

            int i = 0;
            string coilmain = "coilmain";
      
            string  workRoll="workRoll.txt";
            string capPlan = "capPlan";
             string snapShot = "snapShot";
             write(coilmain);
            writerworkRoll(RollsWork, workRoll);
            writeCapPlan(capPlan);
            writeSnapShot(snapShot);



        }
        public void writeData()
        {
            writeOutput();

        }
        private void readCoil()
        {

        }
    }



}
