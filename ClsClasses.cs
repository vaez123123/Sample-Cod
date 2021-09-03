using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections;
using System.Xml;
using System.Data;
using IPSO.CMP.CommonFunctions.ParameterClasses;
using IPSO.ParameterClasses;
using IPSO.CMP.CommonFunctions.Functions;

namespace SkinPass1Scheduling
{

    public partial class SkinPass1Model
    {


        #region  LIST

        CommonLists Lst = new CommonLists();

        public ReaderFunc readerFunc = new ReaderFunc();



        public Roll rollTem;
        Lists LstCom = new Lists();


      
        public List<CapPlan> lstCapNotCoil = new List<CapPlan>();


           #endregion



    }
}
