using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.CommonFunctions.ParameterClasses;
using IPSO.ParameterClasses;

namespace GalvanScheduling
{
   public class RollGalvan
    {
        // TEMPER
       public int chekChangeWorkRoll(List<Roll> lstRollLocal, ref int doubleChekWorkRoll, Solution currSolution)
        {


            #region if (lstRollLocal.Last().weiOpt != 0)

            if (lstRollLocal.Last().WeiOpt != 0)
            {
                // تغییر غلتک به دلیل سرفصل

                if (Status.IndexSarfasl != -1 && Status.IndexSarfasl != currSolution.IndexSarfasl)
                {
                    //تغییر غلتک به دلیل ظرفیت

                    // فقط با مقدار بهینه مقایسه می شود زیرا  به دلیل سرفصل غلتک عوض شده است بنابراین
                    // غلتک جدید دیگر مقدار فیکس یا ریلیز یا دی بی ندارد

                    if ((lstRollLocal.Last().WeiOpt) * (1 - lstRollLocal.Last().LowerPerc) < (//lstRollLocal.Last().weiDB
                        //  + lstRollLocal.Last().currentTotalFixWei
                        // + lstRollLocal.Last().weiRelease
                                                           +currSolution.WeiProg))
                        return 10;

                    else
                        return 11;
                }


                else
                {
                    if (doubleChekWorkRoll == 1)
                    {
                        //تغییر غلتک به دلیل ظرفیت


                        if ((lstRollLocal.Last().WeiOpt) * (1 - lstRollLocal.Last().LowerPerc) < (lstRollLocal.Last().WeiDB
                                                                + lstRollLocal.Last().CurrentTotalFixWei
                                                                + lstRollLocal.Last().WeiRelease
                                                                + currSolution.WeiProg))

                            return 30;


                        else
                            return 31;

                    }

                    //(doubleChekWorkRoll == 2)
                    else
                    {
                        // فقط با مقدار بهینه مقایسه می شود زیرا  به دلیل غلتک جدید یعنی
                        //(doubleChekWorkRoll == 2)
                        //غلتک عوض شده است بنابراین
                        // غلتک جدید دیگر مقدار فیکس یا ریلیز یا دی بی ندارد

                        if ((lstRollLocal.Last().WeiOpt) * (1 - lstRollLocal.Last().LowerPerc) < (//lstRollLocal.Last().weiDB
                            //  + lstRollLocal.Last().currentTotalFixWei
                            // + lstRollLocal.Last().weiRelease
                                                               +currSolution.WeiProg))
                            return 20;


                        else
                            return 21;


                    }

                }


            }
            #endregion


            #region if (lstRollLocal.Last().lenOpt != 0)

            else
            {

                // تغییر غلتک به دلیل سرفصل

                if (Status.IndexSarfasl != -1 && Status.IndexSarfasl != currSolution.IndexSarfasl)
                {
                    //تغییر غلتک به دلیل ظرفیت

                    // فقط با مقدار بهینه مقایسه می شود زیرا  به دلیل سرفصل غلتک عوض شده است بنابراین
                    // غلتک جدید دیگر مقدار فیکس یا ریلیز یا دی بی ندارد

                    if ((lstRollLocal.Last().LenOpt) * (1 - lstRollLocal.Last().LowerPerc) < (//lstRollLocal.Last().lenDB
                        //  + lstRollLocal.Last().currentTotalFixLen
                        // + lstRollLocal.Last().lenRelease
                                                           +currSolution.LenProg))
                        return 10;

                    else
                        return 11;
                }
                else
                {
                    if (doubleChekWorkRoll == 1)
                    {
                        //تغییر غلتک به دلیل ظرفیت


                        if ((lstRollLocal.Last().LenOpt) * (1 - lstRollLocal.Last().LowerPerc) < (lstRollLocal.Last().LenDB
                                                                + lstRollLocal.Last().CurrentTotalFixLen
                                                                + lstRollLocal.Last().LenRelease
                                                                + currSolution.LenProg))

                            return 30;


                        else
                            return 31;

                    }

                    //(doubleChekWorkRoll == 2)
                    else
                    {
                        // فقط با مقدار بهینه مقایسه می شود زیرا  به دلیل غلتک جدید یعنی
                        //(doubleChekWorkRoll == 2)
                        //غلتک عوض شده است بنابراین
                        // غلتک جدید دیگر مقدار فیکس یا ریلیز یا دی بی ندارد

                        if ((lstRollLocal.Last().LenOpt) * (1 - lstRollLocal.Last().LowerPerc) < (//lstRollLocal.Last().lenDB
                            //  + lstRollLocal.Last().currentTotalFixLen
                            // + lstRollLocal.Last().lenRelease
                                                          +currSolution.LenProg))
                            return 20;


                        else
                            return 21;


                    }

                }


            }
            #endregion


        }

        //TEMPER 
       public void chekWorkRollCurr(int indxSarfaslLoc, int idProgLocal, List<Roll> RollsWork, ref int reasonWorkRoll)
        {
            InnerParameter.weiTotal = 0;
            //weiOpt = 0;
            //lenOpt = 0;
            InnerParameter.lenTotal = 0;
            InnerParameter.weiOpt = RollsWork.Last().WeiOpt;
            InnerParameter.lenOpt = RollsWork.Last().LenOpt;
            TanSkpTemParameter.changeRoll = true;


            int chek = 0;// chekChangeWorkRoll(lstOutputPlan.Last());
            // سر فصل

            // تغییر غلتک به دلیل سرفصل

            if (Status.IndexSarfasl != -1 && Status.IndexSarfasl != indxSarfaslLoc && RollsWork.Last().FirstPlan == true)
            {

                chek = 1;

            }




            if (chek == 1)
            {

                InnerParameter.weiTotal = 0;
                InnerParameter.lenTotal = 0;
                TanSkpTemParameter.changeRoll = true;
                reasonWorkRoll = 1;
            }

            else
            {
                InnerParameter.weiTotal = RollsWork.Last().WeiDB + RollsWork.Last().WeiRelease + RollsWork.Last().CurrentTotalFixWei;
                InnerParameter.lenTotal = RollsWork.Last().LenDB + RollsWork.Last().LenRelease + RollsWork.Last().CurrentTotalFixLen;
                TanSkpTemParameter.changeRoll = false;
                reasonWorkRoll = 2;

                if ((InnerParameter.weiTotal == 0
                    //|| lenTotal == 0
             ) && RollsWork.Last().FirstPlan == false)
                {
                    InnerParameter.weiTotal = 0;
                    InnerParameter.lenTotal = 0;
                    TanSkpTemParameter.changeRoll = true;

                    reasonWorkRoll = 0;
                }

            }



        }


       //TEM

       public void chekWorkRoll(List < Roll > RollsWork , List<Solution> SolutionsOutputPlan)
       {
           Roll rollTem = new Roll();

           // ظرفیت  که 
           //doubleChekWorkRoll=1
           //*******************
           if (SolutionsOutputPlan.Last().ChekChangeWorkRoll == 30)
           {
               RollsWork.Last().CurrentTotalFixWei += SolutionsOutputPlan.Last().WeiProg;
               RollsWork.Last().CurrentTotalFixLen += SolutionsOutputPlan.Last().LenProg;
               RollsWork.Last().FirstPlan = true;
               // به شرطی که ابتدا اوت پوت را پر کرده باشیم
               SolutionsOutputPlan.Last().IndexWorkRoll = RollsWork.Count - 1;

               rollTem = new Roll();


               rollTem.CurrentTotalFixWei = 0;
               rollTem.CurrentTotalFixLen = 0;

               rollTem.DatRollEnter = Status.CurrTime;
               rollTem.FirstPlan = false;
               rollTem.WeiDB = 0;
               rollTem.LenDB = 0;
               rollTem.LenOpt = RollsWork[0].LenOpt;
               rollTem.WeiOpt = RollsWork[0].WeiOpt;
               rollTem.LowerPerc = RollsWork.Last().LowerPerc;
               rollTem.UpperPerc = RollsWork.Last().LowerPerc;
               RollsWork.Add(rollTem);

           }

            //سرفصل و
           //doubleChekWorkRoll=2

           else if (SolutionsOutputPlan.Last().ChekChangeWorkRoll == 10
                    || SolutionsOutputPlan.Last().ChekChangeWorkRoll == 20)
           {
               // اگر غلتک جدید هنوز استفاده نشده است باید با همین غلتک کار شود و سپس غلتک دوم اضافه شود
               if (RollsWork.Last().FirstPlan == false)
               {

                   RollsWork.Last().CurrentTotalFixWei += SolutionsOutputPlan.Last().WeiProg;
                   RollsWork.Last().CurrentTotalFixLen += SolutionsOutputPlan.Last().LenProg;
                   RollsWork.Last().FirstPlan = true;
                   // به شرطی که ابتدا اوت پوت را پر کرده باشیم
                   SolutionsOutputPlan.Last().IndexWorkRoll = RollsWork.Count - 1;

                   rollTem = new Roll();

                   rollTem.DatRollEnter = Status.CurrTime;
                   rollTem.CurrentTotalFixWei = 0;
                   rollTem.CurrentTotalFixLen = 0;
                   rollTem.FirstPlan = false;
                   rollTem.WeiDB = 0;
                   rollTem.LenDB = 0;
                   rollTem.LenOpt = RollsWork[0].LenOpt;
                   rollTem.WeiOpt = RollsWork[0].WeiOpt;
                   rollTem.LowerPerc = RollsWork.Last().LowerPerc;
                   rollTem.UpperPerc = RollsWork.Last().LowerPerc;
                   RollsWork.Add(rollTem);


               }
               else
               {
                   rollTem = new Roll();

                   rollTem.DatRollEnter = SolutionsOutputPlan.Last().StartTimeSelectProg;
                   rollTem.CurrentTotalFixWei += SolutionsOutputPlan.Last().WeiProg;
                   rollTem.CurrentTotalFixLen += SolutionsOutputPlan.Last().LenProg;
                   rollTem.FirstPlan = true;
                   rollTem.WeiDB = 0;
                   rollTem.LenDB = 0;
                   rollTem.LenOpt = RollsWork[0].LenOpt;
                   rollTem.WeiOpt = RollsWork[0].WeiOpt;
                   rollTem.LowerPerc = RollsWork.Last().LowerPerc;
                   rollTem.UpperPerc = RollsWork.Last().LowerPerc;
                   RollsWork.Add(rollTem);
                   // به شرطی که ابتدا اوت پوت را پر کرده باشیم
                   SolutionsOutputPlan.Last().IndexWorkRoll = RollsWork.Count - 1;
                   rollTem = new Roll();
                   rollTem.DatRollEnter = Status.CurrTime;
                   rollTem.CurrentTotalFixWei = 0;
                   rollTem.CurrentTotalFixLen = 0;
                   rollTem.FirstPlan = false;
                   rollTem.WeiDB = 0;
                   rollTem.LenDB = 0;
                   rollTem.LenOpt = RollsWork[0].LenOpt;
                   rollTem.WeiOpt = RollsWork[0].WeiOpt;
                   rollTem.LowerPerc = RollsWork.Last().LowerPerc;
                   rollTem.UpperPerc = RollsWork.Last().LowerPerc;
                   RollsWork.Add(rollTem);

               }

           }


               //doubleChekWorkRoll=1
           else if (SolutionsOutputPlan.Last().ChekChangeWorkRoll == 31)
           {
               RollsWork.Last().CurrentTotalFixLen += SolutionsOutputPlan.Last().LenProg;
               RollsWork.Last().CurrentTotalFixWei += SolutionsOutputPlan.Last().WeiProg;
               RollsWork.Last().FirstPlan = true;
               SolutionsOutputPlan.Last().IndexWorkRoll = RollsWork.Count - 1;
           }

               // سرفصل 
           // و یا 
           // doubleChekWorkRoll=2
           // و اضافه شدن غلتک جدید دوم
           else if (SolutionsOutputPlan.Last().ChekChangeWorkRoll == 11 || SolutionsOutputPlan.Last().ChekChangeWorkRoll == 21)
           {
               // اگر غلتک جدید هنوز استفاده نشده است باید با همین غلتک کار شود و سپس غلتک دوم اضافه شود
               if (RollsWork.Last().FirstPlan == false)
               {
                   RollsWork.Last().CurrentTotalFixLen += SolutionsOutputPlan.Last().LenProg;
                   RollsWork.Last().CurrentTotalFixWei += SolutionsOutputPlan.Last().WeiProg;
                   RollsWork.Last().FirstPlan = true;
                   SolutionsOutputPlan.Last().IndexWorkRoll = RollsWork.Count - 1;
               }

               else
               {

                   rollTem = new Roll();
                   rollTem.DatRollEnter = SolutionsOutputPlan.Last().StartTimeSelectProg;
                   rollTem.CurrentTotalFixWei += SolutionsOutputPlan.Last().WeiProg;
                   rollTem.CurrentTotalFixLen += SolutionsOutputPlan.Last().LenProg;
                   rollTem.FirstPlan = true;
                   rollTem.WeiDB = 0;
                   rollTem.LenDB = 0;
                   rollTem.LenOpt = RollsWork[0].LenOpt;
                   rollTem.WeiOpt = RollsWork[0].WeiOpt;
                   rollTem.LowerPerc = RollsWork.Last().LowerPerc;
                   rollTem.UpperPerc = RollsWork.Last().LowerPerc;
                   RollsWork.Add(rollTem);
                   // به شرطی که ابتدا اوت پوت را پر کرده باشیم
                   SolutionsOutputPlan.Last().IndexWorkRoll = RollsWork.Count - 1;
               }
           }

       }

    }
}
