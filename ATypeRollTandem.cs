using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.CommonFunctions.ParameterClasses;
using IPSO.ParameterClasses;

namespace TandemScheduling
{
    public class RollTandem
    {
        //TAN
        public void chekWorkRoll(List<Roll> lstRollLocal, CommonLists Lst)
        {
            Roll rollTem = new Roll();

            int secondRollLocal = -1; // 0 =نیاز به غلتک دوم جدید دارد
            // 1 = غلتک جدید یا موجود که جدید است
            // 30 =اضافه شدن غلتک جد ید اما این برنامه با غلتک قبلی زده می شود و بعد جدید اضافه می شود
            // 31 = عدم تغییر غلتک 


            // نیاز به غلتک جدید داردند
            if (Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)TandemModel.WorkRollEnum.AfterSolutionIncreaseNotChangeNewRoll
                || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)TandemModel.WorkRollEnum.AfterSolutionDecreaseChangeNewRoll
                || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)TandemModel.WorkRollEnum.AfterSolutionIncreaseChangeNewRoll
                || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)TandemModel.WorkRollEnum.AfterSolutionDecreaseNotChangeNewRoll
                || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)TandemModel.WorkRollEnum.AfterSolutionDecreaseChangeRoll
                || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)TandemModel.WorkRollEnum.AfterSolutionIncreaseChangeRoll
                //  || SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)WorkRollEnum.NotDataNotChangeRoll// moshabe 6
                || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)TandemModel.WorkRollEnum.NotProgChangeRoll
                || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)TandemModel.WorkRollEnum.NotSarfaslChangeRoll
                // || SolutionsOutputPlan.Last().ChekChangeWorkRollAfter ==  (int)WorkRollEnum.NotDataNewbeforRoll// moshabe 60
                || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)TandemModel.WorkRollEnum.NotProgNewbeforRoll
                || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)TandemModel.WorkRollEnum.NotSarfaslNewbeforRoll

                //|| ((lstOutputPlan.Last().chekChangeWorkRoll == 1 || lstOutputPlan.Last().chekChangeWorkRoll == (int)WorkRollEnum.NotDecreaseIncreaseNotChangeRoll) && lstOutputPlan.Last().doubleWork == 2)
                )
            {
                if ((((lstRollLocal.Last().WeiOpt) * (1 - lstRollLocal.Last().LowerPerc) < (Lst.currSolution.WeiProg)) && lstRollLocal.Last().WeiOpt != 0)
                    || (((lstRollLocal.Last().LenOpt) * (1 - lstRollLocal.Last().LowerPerc) < (Lst.currSolution.LenProg)) && lstRollLocal.Last().LenOpt != 0))
                    // غلتک دوم جدید
                    secondRollLocal = 0;
                else
                    // غلتک جدید
                    // یا غلتک موجود که جدید است
                    secondRollLocal = 1;

            }

            else
            {
                // غلتک قدیمی
                secondRollLocal = chekChangworkCapcity(lstRollLocal, Lst);
            }


            // اضافه شدن غلتک جدید اما این برنامه با غلتک قبلی زده می شود
            if (secondRollLocal == (int)TandemModel.WorkRollEnum.NewRollbutUseOldRoll)
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


                // غلتک دوم جدید
            else if (secondRollLocal == 0)
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
                    rollTem.DatRollEnter = Status.CurrTime;
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
                    // زمان شروع برنامه با زمان شروع غلتک یکسان است
                    rollTem.DatRollEnter = Lst.SolutionsOutputPlan.Last().StartTimeSelectProg;
                    //rollTem.datRollEnter = Status.CurrTime;

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

                // به غلتک قبلی اضافه می شود 
            else if (secondRollLocal == (int)TandemModel.WorkRollEnum.OldRoll)
            {
                Lst.RollsWork.Last().CurrentTotalFixLen += Lst.SolutionsOutputPlan.Last().LenProg;
                Lst.RollsWork.Last().CurrentTotalFixWei += Lst.SolutionsOutputPlan.Last().WeiProg;
                Lst.RollsWork.Last().FirstPlan = true;
                Lst.SolutionsOutputPlan.Last().IndexWorkRoll = Lst.RollsWork.Count - 1;
            }

            else if (secondRollLocal == 1)
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


        public int chekChangworkCapcity(List<Roll> lstRollLocal, CommonLists Lst)
        {

            if (lstRollLocal.Last().WeiOpt != 0)
            {

                //تغییر غلتک به دلیل ظرفیت


                if ((lstRollLocal.Last().WeiOpt) * (1 - lstRollLocal.Last().LowerPerc) < (lstRollLocal.Last().WeiDB
                                                        + lstRollLocal.Last().CurrentTotalFixWei
                                                        + lstRollLocal.Last().WeiRelease
                                                        + Lst.SolutionsOutputPlan.Last().WeiProg))
                    // اضافه شدن غلتک جد ید اما این برنامه با غلتک قبلی زده می شود و بعد جدید اضافه می شود
                    return (int)TandemModel.WorkRollEnum.NewRollbutUseOldRoll;


                else
                    // با غلتک قبلی
                    return (int)TandemModel.WorkRollEnum.OldRoll;



            }
            else
            {

                //تغییر غلتک به دلیل ظرفیت


                if ((lstRollLocal.Last().LenOpt) * (1 - lstRollLocal.Last().LowerPerc) < (lstRollLocal.Last().LenDB
                                                        + lstRollLocal.Last().CurrentTotalFixLen
                                                        + lstRollLocal.Last().LenRelease
                                                        + Lst.SolutionsOutputPlan.Last().WeiProg))

                    return (int)TandemModel.WorkRollEnum.NewRollbutUseOldRoll;


                else
                    return (int)TandemModel.WorkRollEnum.OldRoll;



            }
        }


        //TAN
        public void chekWorkRollCurr(int indxSarfaslLoc, int idProgLocal, CommonLists Lst, List<ChangeRoll> ChangeRolls,
            ref int reasonWorkRoll, ref int chekChangRoll)
        {
            InnerParameter.weiTotal = 0;
            //weiOpt = 0;
            //lenOpt = 0;
            InnerParameter.lenTotal = 0;
            InnerParameter.weiOpt = Lst.RollsWork.Last().WeiOpt;
            InnerParameter.lenOpt = Lst.RollsWork.Last().LenOpt;
            TanSkpTemParameter.changeRoll = true;

            if (Status.IndexSarfasl != -1)
            {

                int indexFrom = Lst.ProgEfrazes.FindIndex(c => c.IdEfraz == Status.IdEfraz);
                int indexTo = Lst.ProgEfrazes.FindIndex(c => c.IdEfraz == idProgLocal);

                if (indexFrom != -1)
                {
                    int misFrom = Lst.ProgEfrazes[indexFrom].CodProgMis;
                    int misTo = Lst.ProgEfrazes[indexTo].CodProgMis;



                    List<ChangeRoll> lstChangRollLocal = new List<ChangeRoll>();
                    lstChangRollLocal = ChangeRolls.Where(c => c.IdMisProgFrom == misFrom
                                                             && c.IdMisProgTo == misTo
                                                             && c.IndexSarfaslFrom == Status.IndexSarfasl
                                                             && c.IndexSarfaslTo == indxSarfaslLoc).ToList();



                    int firstFlagLocal = -1;
                    int SecondFlagLocal = -1;

                    if (lstChangRollLocal.Count() != 0)
                    {
                        if (lstChangRollLocal.Count > 2)
                        {

                            // چاپ اشتباه بودن نتایج وارد شده
                        }

                        firstFlagLocal = lstChangRollLocal[0].FlagChangeRoll;
                        SecondFlagLocal = lstChangRollLocal[1].FlagChangeRoll;

                        // هر دو حالت افزایش و کاهش عرض نیاز به تغییر غلتک کاری دارند
                        if (firstFlagLocal == 1 && SecondFlagLocal == 1)
                        {

                            //امکان افزایش عرض
                            TanSkpTemParameter.changeRoll = true;
                            // نیاز به غلتک دوم نیست
                            reasonWorkRoll = 1;
                            // تغییر غلتک
                            InnerParameter.weiTotal = 0;
                            InnerParameter.lenTotal = 0;

                            if (Lst.RollsWork.Last().FirstPlan == true)

                                chekChangRoll = (int)TandemModel.WorkRollEnum.DecreaseIncreaseChangeRoll;

                            else // غلتک موجود جدید است
                            {
                                chekChangRoll = (int)TandemModel.WorkRollEnum.DecreaseIncreaseNewBeforRoll;
                            }

                        }


                            // برای افزایش عرض تنها  نیاز به تغییر غلتک می باشد که این حالت در غلتک دوم دیده می شود
                        else if (firstFlagLocal != SecondFlagLocal)
                        {
                            chekChangRoll = (int)TandemModel.WorkRollEnum.IncreaseNotChangeRoll;


                            //عدم امکان افزایش عرض
                            TanSkpTemParameter.changeRoll = false;
                            // غلتک دوم
                            reasonWorkRoll = 2;
                            // عدم تغییر غلتک
                            InnerParameter.weiTotal = Lst.RollsWork.Last().WeiDB + Lst.RollsWork.Last().WeiRelease + Lst.RollsWork.Last().CurrentTotalFixWei;
                            InnerParameter.lenTotal = Lst.RollsWork.Last().LenDB + Lst.RollsWork.Last().LenRelease + Lst.RollsWork.Last().CurrentTotalFixLen;

                            // اگر غلتک تعویض شده ولی تا به حال استفاده نشده است پس به دلیل ظرفیت قبلا تغیر کرده است

                            if ((InnerParameter.weiTotal == 0
                                //|| lenTotal == 0
                            ) && Lst.RollsWork.Last().FirstPlan == false)
                            {
                                // غلتک جدید
                                InnerParameter.weiTotal = 0;
                                InnerParameter.lenTotal = 0;
                                // امکان افزایش عرض
                                TanSkpTemParameter.changeRoll = true;
                                //عدم غلتک دوم
                                reasonWorkRoll = 1;

                                chekChangRoll = (int)TandemModel.WorkRollEnum.IncreaseNewBeforRoll;

                            }


                        }
                        // عدم تغییر غلتک برای هر دو حالت افزایش و کاهش عرض
                        else
                        {
                            chekChangRoll = (int)TandemModel.WorkRollEnum.NotDecreaseIncreaseNotChangeRoll;

                            //امکان افزایش عرض
                            TanSkpTemParameter.changeRoll = true;
                            // نیاز به غلتک دوم هست به دلیل ظرفیت بیشتر
                            reasonWorkRoll = 2;

                            // عدم تغییر غلتک
                            InnerParameter.weiTotal = Lst.RollsWork.Last().WeiDB + Lst.RollsWork.Last().WeiRelease + Lst.RollsWork.Last().CurrentTotalFixWei;
                            InnerParameter.lenTotal = Lst.RollsWork.Last().LenDB + Lst.RollsWork.Last().LenRelease + Lst.RollsWork.Last().CurrentTotalFixLen;

                            // اگر غلتک تعویض شده ولی تا به حال استفاده نشده است پس به دلیل ظرفیت قبلا تغیر کرده است

                            if ((InnerParameter.weiTotal == 0
                                //|| lenTotal == 0
                            ) && Lst.RollsWork.Last().FirstPlan == false)
                            {
                                // غلتک جدید
                                InnerParameter.weiTotal = 0;
                                InnerParameter.lenTotal = 0;
                                // امکان افزایش عرض
                                TanSkpTemParameter.changeRoll = true;
                                //عدم غلتک دوم
                                reasonWorkRoll = 1;

                                chekChangRoll = (int)TandemModel.WorkRollEnum.NotDecreaseIncreaseNewBeforRoll;

                            }

                        }



                    }

                        // عدم وجود اطلاعات در دیتا بیس
                    else
                    {

                        chekChangRoll = (int)TandemModel.WorkRollEnum.NotDataNotChangeRoll;

                        //امکان افزایش عرض
                        TanSkpTemParameter.changeRoll = true;
                        // نیاز به غلتک دوم هست به دلیل ظرفیت بیشتر
                        reasonWorkRoll = 2;

                        // عدم تغییر غلتک
                        InnerParameter.weiTotal = Lst.RollsWork.Last().WeiDB + Lst.RollsWork.Last().WeiRelease + Lst.RollsWork.Last().CurrentTotalFixWei;
                        InnerParameter.lenTotal = Lst.RollsWork.Last().LenDB + Lst.RollsWork.Last().LenRelease + Lst.RollsWork.Last().CurrentTotalFixLen;

                        // اگر غلتک تعویض شده ولی تا به حال استفاده نشده است پس به دلیل ظرفیت قبلا تغیر کرده است

                        if ((InnerParameter.weiTotal == 0
                            //|| lenTotal == 0
                        ) && Lst.RollsWork.Last().FirstPlan == false)
                        {
                            // غلتک جدید
                            InnerParameter.weiTotal = 0;
                            InnerParameter.lenTotal = 0;
                            // امکان افزایش عرض
                            TanSkpTemParameter.changeRoll = true;
                            //عدم غلتک دوم
                            reasonWorkRoll = 1;

                            chekChangRoll = (int)TandemModel.WorkRollEnum.NotDataNewbeforRoll;

                        }

                    }


                }
                // نوع برنامه قبلی وجود ندارد
                else
                {

                    //امکان افزایش عرض
                    TanSkpTemParameter.changeRoll = true;
                    // نیاز به غلتک دوم نیست
                    reasonWorkRoll = 1;
                    // تغییر غلتک
                    InnerParameter.weiTotal = 0;
                    InnerParameter.lenTotal = 0;

                    if (Lst.RollsWork.Last().FirstPlan == true)

                        chekChangRoll = (int)TandemModel.WorkRollEnum.NotProgChangeRoll;

                    else // غلتک موجود جدید است
                    {
                        chekChangRoll = (int)TandemModel.WorkRollEnum.NotProgNewbeforRoll;
                    }
                }




            }
            // سرفصل قبلی وجود ندراد
            else
            {
                //امکان افزایش عرض
                TanSkpTemParameter.changeRoll = true;
                // نیاز به غلتک دوم نیست
                reasonWorkRoll = 1;
                // تغییر غلتک
                InnerParameter.weiTotal = 0;
                InnerParameter.lenTotal = 0;

                if (Lst.RollsWork.Last().FirstPlan == true)

                    chekChangRoll = (int)TandemModel.WorkRollEnum.NotSarfaslChangeRoll;

                else // غلتک موجود جدید است
                {
                    chekChangRoll = (int)TandemModel.WorkRollEnum.NotSarfaslNewbeforRoll;
                }
            }

            Lst.currSolution.ChekChangeWorkRollBefor = chekChangRoll;
            Lst.currSolution.DoubleWorkBefor = reasonWorkRoll;



        }
    }
}
