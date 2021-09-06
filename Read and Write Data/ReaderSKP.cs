using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.CommonFunctions.ParameterClasses;
using IPSO.ParameterClasses;
using IPSO.CMP.CommonFunctions.Functions;
using IPSO.Functions;
using IPSO.CMP.DataBaseMODEL;
using System.Data;

namespace SKPScheduling
{
    public class ReaderSKP
    {
        public MODELDataBase WorkData = new MODELDataBase();
        public DataTable dt = new DataTable();
        public DataTable dt2 = new DataTable();
        public ReaderFunc readerFunc = new ReaderFunc();
        public ReaderFunL2 readerFunL2 = new ReaderFunL2();

        public void readFromDataBase(CommonLists Lst, Lists LstCom)
        {
            WriterSKP.PathWriter = @"C:\Users\p.vaez\Desktop\output\SKP1\" + RunInformation.RunId + "\\";
            WriterSKP.creatNotes(WriterSKP.PathWriter);

            readerFunc.readCommonData(Lst, WriterSKP.PathWriter, InnerParameter.counterCoilRelease,
                                     InnerParameter.counterCoilReleaseOtherSt, "Width_IN_SKP", "No_THICKNESS", "no_tksOut",
                                     "No-TYPEPROGMIS", "No surfaceRough", "No-TRIM", "OIL", "no_product_family_gal");


            if (RunInformation.flgStopAlgorithm == 1)
                readerFunL2.readProgSensitive(Lst);
        }
    }
}
