using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.CommonFunctions.ParameterClasses;
using IPSO.ParameterClasses;
using IPSO.Functions;
using IPSO.CMP.Log;

namespace SKPScheduling
{
    public class RollSKP : RollL2
    {

        /// <summary>
        /// 1 =sarfasl  0 = capacity 2= no change
        /// </summary>
        /// <param name="lstRollLocal"></param>
        /// <returns></returns>

     
        //SKP1
        public override void chekWorkRollCurr(int indxSarfaslLoc, int idProgLocal, CommonLists Lst, List<ChangeRoll> ChangeRolls,
          ref int reasonWorkRoll, ref int chekChangRoll)
        {
            InnerParameter.weiTotal = 0;
            InnerParameter.lenTotal = 0;
            InnerParameter.weiOpt = Lst.RollsWork.Last().WeiOpt;

            TanSkpTemParameter.changeRoll = true;

            int chek = 0;// chekChangeWorkRoll(lstOutputPlan.Last());

            // تغییر غلتک به دلیل سرفصل

            if (Status.IndexSarfasl != -1 && Status.IndexSarfasl != indxSarfaslLoc && Lst.RollsWork.Last().FirstPlan == true)
            {
                int indx = Lst.JumpBetweenCrowns.FindIndex(a => a.IndexSarfaslFrom == Status.IndexSarfasl
                                                    && a.IndexSarfaslTo == indxSarfaslLoc);
                if (indx != -1)
                {
                    int flg = Lst.JumpBetweenCrowns.Find(a => a.IndexSarfaslFrom == Status.IndexSarfasl
                                                      && a.IndexSarfaslTo == indxSarfaslLoc).FlqPossibleJump;
                    if (flg == 1)
                        chek = 1;
                }

            }

            // تغییر غلتک به دلیل سرفصل یا نوع برنامه "ام بی" است
            if (chek == 1)
            {

                InnerParameter.weiTotal = 0;
                InnerParameter.lenTotal = 0;
                TanSkpTemParameter.changeRoll = true;
                reasonWorkRoll = 1;
            }

            else
            {
                InnerParameter.weiTotal = Lst.RollsWork.Last().WeiDB + Lst.RollsWork.Last().WeiRelease + Lst.RollsWork.Last().CurrentTotalFixWei;
                InnerParameter.lenTotal = Lst.RollsWork.Last().LenDB + Lst.RollsWork.Last().LenRelease + Lst.RollsWork.Last().CurrentTotalFixLen;
                TanSkpTemParameter.changeRoll = false;
                reasonWorkRoll = 2;

                // اگر غلتک تعویض شده ولی تا به حال استفاده نشده است پس به دلیل ظرفیت قبلا تغیر کرده است

                if ((InnerParameter.weiTotal == 0
                    //|| lenTotal == 0
                ) && Lst.RollsWork.Last().FirstPlan == false)
                {
                    InnerParameter.weiTotal = 0;
                    InnerParameter.lenTotal = 0;
                    TanSkpTemParameter.changeRoll = true;

                    reasonWorkRoll = 0;
                }

            }

        }

        //TEM
        public static void chekWorkRoll(CommonLists Lst)
        {
            Roll rollTem = new Roll();

            // ظرفیت  که 
            //doubleChekWorkRoll=1
            //*******************
            if (Lst.SolutionsOutputPlan.Last().ChekChangeWorkRoll == 30)
            {
                Lst.RollsWork.Last().CurrentTotalFixWei += Lst.SolutionsOutputPlan.Last().WeiProg;
                Lst.RollsWork.Last().CurrentTotalFixLen += Lst.SolutionsOutputPlan.Last().LenProg;
                Lst.RollsWork.Last().FirstPlan = true;
                // به شرطی که ابتدا اوت پوت را پر کرده باشیم
                Lst.SolutionsOutputPlan.Last().IndexWorkRoll = Lst.RollsWork.Count - 1;

                rollTem = new Roll();
                rollTem.CurrentTotalFixWei = 0;
                rollTem.CurrentTotalFixLen = 0;

                rollTem.DatRollEnter = Status.CurrTime;
                rollTem.FirstPlan = false;
                rollTem.WeiDB = 0;
                rollTem.LenDB = 0;
                rollTem.LenOpt = Lst.RollsWork[0].LenOpt;
                rollTem.WeiOpt = Lst.RollsWork[0].WeiOpt;
                rollTem.LowerPerc = Lst.RollsWork[0].LowerPerc;
                rollTem.UpperPerc = Lst.RollsWork[0].UpperPerc;
                Lst.RollsWork.Add(rollTem);



            }

             //سرفصل و
            //doubleChekWorkRoll=2

            else if (Lst.SolutionsOutputPlan.Last().ChekChangeWorkRoll == 10
                     || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRoll == 20)
            {
                // اگر غلتک جدید هنوز استفاده نشده است باید با همین غلتک کار شود و سپس غلتک دوم اضافه شود
                if (Lst.RollsWork.Last().FirstPlan == false)
                {

                    Lst.RollsWork.Last().CurrentTotalFixWei += Lst.SolutionsOutputPlan.Last().WeiProg;
                    Lst.RollsWork.Last().CurrentTotalFixLen += Lst.SolutionsOutputPlan.Last().LenProg;
                    Lst.RollsWork.Last().FirstPlan = true;
                    // به شرطی که ابتدا اوت پوت را پر کرده باشیم
                    Lst.SolutionsOutputPlan.Last().IndexWorkRoll = Lst.RollsWork.Count - 1;

                    rollTem = new Roll();

                    rollTem.DatRollEnter = Status.CurrTime;
                    rollTem.CurrentTotalFixWei = 0;
                    rollTem.CurrentTotalFixLen = 0;
                    rollTem.FirstPlan = false;
                    rollTem.WeiDB = 0;
                    rollTem.LenDB = 0;
                    rollTem.LenOpt = Lst.RollsWork[0].LenOpt;
                    rollTem.WeiOpt = Lst.RollsWork[0].WeiOpt;
                    rollTem.LowerPerc = Lst.RollsWork[0].LowerPerc;
                    rollTem.UpperPerc = Lst.RollsWork[0].UpperPerc;
                    Lst.RollsWork.Add(rollTem);


                }
                else
                {
                    rollTem = new Roll();
                    rollTem.DatRollEnter = Lst.SolutionsOutputPlan.Last().StartTimeSelectProg;
                    rollTem.CurrentTotalFixWei += Lst.SolutionsOutputPlan.Last().WeiProg;
                    rollTem.CurrentTotalFixLen += Lst.SolutionsOutputPlan.Last().LenProg;
                    rollTem.FirstPlan = true;
                    rollTem.WeiDB = 0;
                    rollTem.LenDB = 0;
                    rollTem.LenOpt = Lst.RollsWork[0].LenOpt;
                    rollTem.WeiOpt = Lst.RollsWork[0].WeiOpt;
                    rollTem.LowerPerc = Lst.RollsWork[0].LowerPerc;
                    rollTem.UpperPerc = Lst.RollsWork[0].UpperPerc;

                    Lst.RollsWork.Add(rollTem);
                    // به شرطی که ابتدا اوت پوت را پر کرده باشیم
                    Lst.SolutionsOutputPlan.Last().IndexWorkRoll = Lst.RollsWork.Count - 1;
                    rollTem = new Roll();
                    rollTem.DatRollEnter = Status.CurrTime;
                    rollTem.CurrentTotalFixWei = 0;
                    rollTem.CurrentTotalFixLen = 0;
                    rollTem.FirstPlan = false;
                    rollTem.WeiDB = 0;
                    rollTem.LenDB = 0;
                    rollTem.LenOpt = Lst.RollsWork[0].LenOpt;
                    rollTem.WeiOpt = Lst.RollsWork[0].WeiOpt;
                    rollTem.LowerPerc = Lst.RollsWork[0].LowerPerc;
                    rollTem.UpperPerc = Lst.RollsWork[0].UpperPerc;
                    Lst.RollsWork.Add(rollTem);

                }




            }


                //doubleChekWorkRoll=1
            else if (Lst.SolutionsOutputPlan.Last().ChekChangeWorkRoll == 31)
            {
                Lst.RollsWork.Last().CurrentTotalFixLen += Lst.SolutionsOutputPlan.Last().LenProg;
                Lst.RollsWork.Last().CurrentTotalFixWei += Lst.SolutionsOutputPlan.Last().WeiProg;
                Lst.RollsWork.Last().FirstPlan = true;
                Lst.SolutionsOutputPlan.Last().IndexWorkRoll = Lst.RollsWork.Count - 1;
            }

                // سرفصل 
            // و یا 
            // doubleChekWorkRoll=2
            // و اضافه شدن غلتک جدید دوم
            else if (Lst.SolutionsOutputPlan.Last().ChekChangeWorkRoll == 11 || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRoll == 21)
            {
                // اگر غلتک جدید هنوز استفاده نشده است باید با همین غلتک کار شود و سپس غلتک دوم اضافه شود
                if (Lst.RollsWork.Last().FirstPlan == false)
                {
                    Lst.RollsWork.Last().CurrentTotalFixLen += Lst.SolutionsOutputPlan.Last().LenProg;
                    Lst.RollsWork.Last().CurrentTotalFixWei += Lst.SolutionsOutputPlan.Last().WeiProg;
                    Lst.RollsWork.Last().FirstPlan = true;
                    Lst.SolutionsOutputPlan.Last().IndexWorkRoll = Lst.RollsWork.Count - 1;
                }

                else
                {
                    rollTem = new Roll();
                    rollTem.DatRollEnter = Lst.SolutionsOutputPlan.Last().StartTimeSelectProg;
                    rollTem.CurrentTotalFixWei += Lst.SolutionsOutputPlan.Last().WeiProg;
                    rollTem.CurrentTotalFixLen += Lst.SolutionsOutputPlan.Last().LenProg;
                    rollTem.FirstPlan = true;
                    rollTem.WeiDB = 0;
                    rollTem.LenDB = 0;
                    rollTem.LenOpt = Lst.RollsWork[0].LenOpt;
                    rollTem.WeiOpt = Lst.RollsWork[0].WeiOpt;
                    rollTem.LowerPerc = Lst.RollsWork[0].LowerPerc;
                    rollTem.UpperPerc = Lst.RollsWork[0].UpperPerc;

                    Lst.RollsWork.Add(rollTem);
                    // به شرطی که ابتدا اوت پوت را پر کرده باشیم
                    Lst.SolutionsOutputPlan.Last().IndexWorkRoll = Lst.RollsWork.Count - 1;
                }
            }

        }
   

    }
}
