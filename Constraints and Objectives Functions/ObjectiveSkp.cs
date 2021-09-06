using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.CommonFunctions.ParameterClasses;
using IPSO.Functions;

namespace SKPScheduling
{
    public class ObjectiveSkp : ObjectiveL2
    {
        // تابع محاسبه مقدار تابع هدف
        //ALL
        public override void calcuTotalObj(Solution solu, int idEfrazLocal, int idSarfaslLocal, CommonLists Lst, ref int doubleChekWorkRoll,
            List<ChangeRoll> ChangeRolls, ref bool chekChangeCountProg, ref int chekChangRoll, ref int reasonWorkRoll)
        {


            calcuObjRoll(ref doubleChekWorkRoll,Lst.currSolution,Lst.RollsWork);// بین صفر و یک

            calcuObjContinuePattern(Lst);

            Solution.calcuObjCommon(idEfrazLocal, Lst.currSolution, Lst);

            Solution.calcuTotalObjCommon(Lst.currSolution);



        }
   
         //TEM-SKP1
        public void calcuObjRoll(  ref int  doubleChekWorkRoll, Solution currSolution, List< Roll> RollsWork)
        {

            if (doubleChekWorkRoll == 1)
            {
                int chek = chekChangeWorkRoll(RollsWork, ref doubleChekWorkRoll,currSolution);
                currSolution.ChekChangeWorkRoll = chek;
                // غیر سر فصل

                if (chek == 30 || chek == 31)
                {

                    if (RollsWork.Last().LenOpt != 0)
                    {
                        double dif = RollsWork.Last().LenOpt - RollsWork.Last().LenDB - RollsWork.Last().LenRelease -
                           RollsWork.Last().CurrentTotalFixLen - currSolution.LenProg;

                        if (dif > 0)
                            currSolution.ObjLenWorkRoll = (dif / RollsWork.Last().LenOpt);

                        else
                            currSolution.ObjLenWorkRoll = 0;
                    }

                    if (RollsWork.Last().WeiOpt != 0)
                    {
                        double dif = RollsWork.Last().WeiOpt - RollsWork.Last().WeiDB - RollsWork.Last().WeiRelease -
                           RollsWork.Last().CurrentTotalFixWei - currSolution.WeiProg;
                        if (dif > 0)
                            currSolution.ObjWeiWorkRoll = (dif / (RollsWork.Last().WeiOpt));

                        else
                            currSolution.ObjWeiWorkRoll = 0;
                    }
                }
                // سرفصل
                else
                {

                    if (RollsWork.Last().LenOpt != 0)
                    {
                        double objNew = 0;
                        if (RollsWork.Last().LenOpt - InnerParameter.lenTotal > 0)
                        {
                            objNew = ((RollsWork.Last().LenOpt - InnerParameter.lenTotal) / RollsWork.Last().LenOpt);
                        }

                        else
                            objNew = 0;


                        double obj = 0;
                        double dif = RollsWork.Last().LenOpt - RollsWork.Last().LenDB - RollsWork.Last().LenRelease -
                                       RollsWork.Last().CurrentTotalFixLen;

                        if (dif > 0)
                            obj = (dif / RollsWork.Last().LenOpt);

                        else
                            obj = 0;

                        currSolution.ObjLenWorkRoll = obj * DataBase.CoefObjWorkRollOld + objNew * DataBase.CoefObjWorkRollNew;


                    }
                    if (RollsWork.Last().WeiOpt != 0)
                    {
                        double objNew = 0;
                        if (RollsWork.Last().WeiOpt -InnerParameter.weiTotal > 0)

                            objNew = ((RollsWork.Last().WeiOpt - InnerParameter.weiTotal) / (RollsWork.Last().WeiOpt));

                        else
                            objNew = 0;

                        double obj = 0;

                        double dif = RollsWork.Last().WeiOpt - RollsWork.Last().WeiDB - RollsWork.Last().WeiRelease -
                           RollsWork.Last().CurrentTotalFixWei;

                        if (dif > 0)
                            obj = (dif / (RollsWork.Last().WeiOpt));

                        else
                            obj = 0;

                        currSolution.ObjWeiWorkRoll = obj * DataBase.CoefObjWorkRollOld + objNew * DataBase.CoefObjWorkRollNew; ;
                    }
                }
            }

                //
            else
            {
                int chek = chekChangeWorkRoll(RollsWork,ref doubleChekWorkRoll,currSolution);
                currSolution.ChekChangeWorkRoll = chek;


                if (RollsWork.Last().LenOpt != 0)
                {
                    double objNew = 0;
                    if (RollsWork.Last().LenOpt - InnerParameter.lenTotal > 0)
                    {
                        objNew = ((RollsWork.Last().LenOpt -InnerParameter.lenTotal) / RollsWork.Last().LenOpt);
                    }

                    else
                        objNew = 0;


                    double obj = 0;
                    double dif = RollsWork.Last().LenOpt - RollsWork.Last().LenDB - RollsWork.Last().LenRelease -
                                   RollsWork.Last().CurrentTotalFixLen;

                    if (dif > 0)
                        obj = (dif / RollsWork.Last().LenOpt);

                    else
                        obj = 0;

                    currSolution.ObjLenWorkRoll = obj * DataBase.CoefObjWorkRollOld + objNew * DataBase.CoefObjWorkRollNew;


                }
                if (RollsWork.Last().WeiOpt != 0)
                {
                    double objNew = 0;
                    if (RollsWork.Last().WeiOpt -InnerParameter.weiTotal > 0)

                        objNew = ((RollsWork.Last().WeiOpt - InnerParameter.weiTotal) / (RollsWork.Last().WeiOpt));

                    else
                        objNew = 0;

                    double obj = 0;

                    double dif = RollsWork.Last().WeiOpt - RollsWork.Last().WeiDB - RollsWork.Last().WeiRelease -
                       RollsWork.Last().CurrentTotalFixWei;

                    if (dif > 0)
                        obj = (dif / (RollsWork.Last().WeiOpt));

                    else
                        obj = 0;

                    currSolution.ObjWeiWorkRoll = obj * DataBase.CoefObjWorkRollOld + objNew * DataBase.CoefObjWorkRollNew; ;
                }

            }




        }

         //TEM-SKP1
        public int chekChangeWorkRoll(List<Roll> lstRollLocal,ref int  doubleChekWorkRoll,Solution currSolution)
        {
            
            #region if (lstRollLocal.Last().weiOpt != 0)

            if (lstRollLocal.Last().WeiOpt != 0)
            {
                // تغییر غلتک به دلیل سرفصل

                if (Status.IndexSarfasl != -1 && Status.IndexSarfasl !=currSolution.IndexSarfasl)
                {
                    //تغییر غلتک به دلیل ظرفیت

                    // فقط با مقدار بهینه مقایسه می شود زیرا  به دلیل سرفصل غلتک عوض شده است بنابراین
                    // غلتک جدید دیگر مقدار فیکس یا ریلیز یا دی بی ندارد

                    if ((lstRollLocal.Last().WeiOpt) * (1-lstRollLocal.Last().LowerPerc) < (//lstRollLocal.Last().weiDB
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


        // ba data chek shavad
        public void calcuObjContinuePattern(CommonLists Lst)
        {


            if (Lst.currSolution.IndexSarfasl > Status.IndexSarfasl)

                // currSolution.objChangeSarfasl= lstJumpBetweenCrown.Find(c=>c.indexSarfaslFrom == currStat.indexSarfasl && c.indexSarfaslTo==currSolution.indexSarfasl).costBetweenSarfasl;

                Lst.currSolution.ObjSarfasl = 1;
            else
                Lst.currSolution.ObjSarfasl = 0;

        }
    }
}
