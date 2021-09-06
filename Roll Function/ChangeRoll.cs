using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.CommonFunctions.ParameterClasses;

namespace IPSO.CMP.RollFunctions
{
    public class ChangeRoll
    {
        public int IndexSarfaslFrom;
        public int IndexSarfaslTo;
        public int IdMisProgFrom;
        public int IdMisProgTo;
        public int FlagIncreaseWid;// یک نشان دهنده افزایش عرضی
        public int FlagChangeRoll;//یک نیاز به تغییر غلتک دارد
        public int FlagMinCamp;//برای برنامه های حساس باید از مینیم عرض کمپین شروع شوند 
        public static bool changeRoll = true; // true change  fals no change

        public enum WorkRollEnum
        {
            NotDecreaseIncreaseNewBeforRoll = 60,
            NotDecreaseIncreaseNotChangeRoll = 6,

            NotSarfaslNewbeforRoll = 50,
            NotSarfaslChangeRoll = 5,

            NotProgNewbeforRoll = 40,
            NotProgChangeRoll = 4,

            NotDataNewbeforRoll = 30,
            NotDataNotChangeRoll = 3,

            DecreaseIncreaseNewBeforRoll = 20,
            DecreaseIncreaseChangeRoll = 2,

            IncreaseNewBeforRoll = 10,
            IncreaseNotChangeRoll = 1,

            AfterSolutionIncreaseChangeNewRoll = 100,
            AfterSolutionIncreaseChangeRoll = 101,
            AfterSolutionIncreaseNotChangeNewRoll = 200,
            AfterSolutionIncreaseNotChangeRoll = 201,

            AfterSolutionDecreaseChangeNewRoll = 300,
            AfterSolutionDecreaseChangeRoll = 301,
            AfterSolutionDecreaseNotChangeNewRoll = 400,
            AfterSolutionDecreaseNotChangeRoll = 401,

            NewRollbutUseOldRoll = 1000,
            OldRoll = 1001
        };

        public ChangeRoll()
        { }

        public ChangeRoll(int indexSarfaslFrom, int indexSarfaslTo, int idMisProgFrom, int idMisProgTo, int flagIncreaseWid, int flagChangeRoll, int flagMinCamp)
        {
            this.IndexSarfaslFrom = indexSarfaslFrom;
            this.IndexSarfaslTo = indexSarfaslTo;
            this.IdMisProgFrom = idMisProgFrom;
            this.IdMisProgTo = idMisProgTo;
            this.FlagChangeRoll = flagChangeRoll;
            this.FlagIncreaseWid = flagIncreaseWid;
            this.FlagMinCamp = flagMinCamp;

        }
        //TAN
        public void chekWorkRoll(List<Roll> lstRollLocal, CommonLists Lst)
        {
            Roll rollTem = new Roll();

            int secondRollLocal = -1; // 0 =نیاز به غلتک دوم جدید دارد
            // 1 = غلتک جدید یا موجود که جدید است
            // 30 =اضافه شدن غلتک جد ید اما این برنامه با غلتک قبلی زده می شود و بعد جدید اضافه می شود
            // 31 = عدم تغییر غلتک 


            // نیاز به غلتک جدید داردند
            if (Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)WorkRollEnum.AfterSolutionIncreaseNotChangeNewRoll
                || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)WorkRollEnum.AfterSolutionDecreaseChangeNewRoll
                || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)WorkRollEnum.AfterSolutionIncreaseChangeNewRoll
                || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)WorkRollEnum.AfterSolutionDecreaseNotChangeNewRoll
                || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)WorkRollEnum.AfterSolutionDecreaseChangeRoll
                || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)WorkRollEnum.AfterSolutionIncreaseChangeRoll
                //  || SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)WorkRollEnum.NotDataNotChangeRoll// moshabe 6
                || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)WorkRollEnum.NotProgChangeRoll
                || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)WorkRollEnum.NotSarfaslChangeRoll
                // || SolutionsOutputPlan.Last().ChekChangeWorkRollAfter ==  (int)WorkRollEnum.NotDataNewbeforRoll// moshabe 60
                || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)WorkRollEnum.NotProgNewbeforRoll
                || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRollAfter == (int)WorkRollEnum.NotSarfaslNewbeforRoll

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
            if (secondRollLocal == (int)WorkRollEnum.NewRollbutUseOldRoll)
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
            else if (secondRollLocal == (int)WorkRollEnum.OldRoll)
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
                    return (int)WorkRollEnum.NewRollbutUseOldRoll;


                else
                    // با غلتک قبلی
                    return (int)WorkRollEnum.OldRoll;



            }
            else
            {

                //تغییر غلتک به دلیل ظرفیت


                if ((lstRollLocal.Last().LenOpt) * (1 - lstRollLocal.Last().LowerPerc) < (lstRollLocal.Last().LenDB
                                                        + lstRollLocal.Last().CurrentTotalFixLen
                                                        + lstRollLocal.Last().LenRelease
                                                        + Lst.SolutionsOutputPlan.Last().WeiProg))

                    return (int)WorkRollEnum.NewRollbutUseOldRoll;


                else
                    return (int)WorkRollEnum.OldRoll;



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
            ChangeRoll.changeRoll = true;

            if (Status.IndexSarfasl != -1)
            {

                int indexFrom = Lst.ProgEfrazes.FindIndex(c => c.IdEfraz == Status.IdEfraz);
                int indexTo = Lst.ProgEfrazes.FindIndex(c => c.IdEfraz == idProgLocal);

                if (indexFrom != -1)
                {
                    int misFrom = Lst.ProgEfrazes[indexFrom].CodProgMis;
                    int misTo = Lst.ProgEfrazes[indexTo].CodProgMis;



                    List<ChangeRoll> lstChangRollLocal = new List<ChangeRoll>();
                    TimeParameter.TimechangeRoll.Start();
                    lstChangRollLocal = ChangeRolls.Where(c => c.IdMisProgFrom == misFrom
                                                             && c.IdMisProgTo == misTo
                                                             && c.IndexSarfaslFrom == Status.IndexSarfasl
                                                             && c.IndexSarfaslTo == indxSarfaslLoc).ToList();

                    TimeParameter.TimechangeRoll.Stop();



                    int firstFlagLocal = -1;
                    int SecondFlagLocal = -1;

                    if (lstChangRollLocal.Count() != 0)
                    {
                        if (lstChangRollLocal.Count > 2)
                        {

                            throw new Exception(" Change roll information is  not true ");
                            // چاپ اشتباه بودن نتایج وارد شده
                        }

                        if (lstChangRollLocal.Count() == 1)
                        {
                            firstFlagLocal = lstChangRollLocal[0].FlagChangeRoll;
                            SecondFlagLocal = 0;

                        }

                        firstFlagLocal = lstChangRollLocal[0].FlagChangeRoll;
                        SecondFlagLocal = lstChangRollLocal[1].FlagChangeRoll;

                        // هر دو حالت افزایش و کاهش عرض نیاز به تغییر غلتک کاری دارند
                        if (firstFlagLocal == 1 && SecondFlagLocal == 1)
                        {

                            //امکان افزایش عرض
                            ChangeRoll.changeRoll = true;
                            // نیاز به غلتک دوم نیست
                            reasonWorkRoll = 1;
                            // تغییر غلتک
                            InnerParameter.weiTotal = 0;
                            InnerParameter.lenTotal = 0;

                            if (Lst.RollsWork.Last().FirstPlan == true)

                                chekChangRoll = (int)WorkRollEnum.DecreaseIncreaseChangeRoll;

                            else // غلتک موجود جدید است
                            {
                                chekChangRoll = (int)WorkRollEnum.DecreaseIncreaseNewBeforRoll;
                            }

                        }


                            // برای افزایش عرض تنها  نیاز به تغییر غلتک می باشد که این حالت در غلتک دوم دیده می شود
                        else if (firstFlagLocal != SecondFlagLocal)
                        {
                            chekChangRoll = (int)WorkRollEnum.IncreaseNotChangeRoll;


                            //عدم امکان افزایش عرض
                            ChangeRoll.changeRoll = false;
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
                                ChangeRoll.changeRoll = true;
                                //عدم غلتک دوم
                                reasonWorkRoll = 1;

                                chekChangRoll = (int)WorkRollEnum.IncreaseNewBeforRoll;

                            }


                        }
                        // عدم تغییر غلتک برای هر دو حالت افزایش و کاهش عرض
                        else
                        {
                            chekChangRoll = (int)WorkRollEnum.NotDecreaseIncreaseNotChangeRoll;

                            //امکان افزایش عرض
                            ChangeRoll.changeRoll = true;
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
                                ChangeRoll.changeRoll = true;
                                //عدم غلتک دوم
                                reasonWorkRoll = 1;

                                chekChangRoll = (int)WorkRollEnum.NotDecreaseIncreaseNewBeforRoll;

                            }

                        }



                    }

                        // عدم وجود اطلاعات در دیتا بیس
                    else
                    {

                        chekChangRoll = (int)WorkRollEnum.NotDataNotChangeRoll;

                        //امکان افزایش عرض
                        ChangeRoll.changeRoll = true;
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
                            ChangeRoll.changeRoll = true;
                            //عدم غلتک دوم
                            reasonWorkRoll = 1;

                            chekChangRoll = (int)WorkRollEnum.NotDataNewbeforRoll;

                        }

                    }


                }
                // نوع برنامه قبلی وجود ندارد
                else
                {

                    //امکان افزایش عرض
                    ChangeRoll.changeRoll = true;
                    // نیاز به غلتک دوم نیست
                    reasonWorkRoll = 1;
                    // تغییر غلتک
                    InnerParameter.weiTotal = 0;
                    InnerParameter.lenTotal = 0;

                    if (Lst.RollsWork.Last().FirstPlan == true)

                        chekChangRoll = (int)WorkRollEnum.NotProgChangeRoll;

                    else // غلتک موجود جدید است
                    {
                        chekChangRoll = (int)WorkRollEnum.NotProgNewbeforRoll;
                    }
                }




            }
            // سرفصل قبلی وجود ندراد
            else
            {
                //امکان افزایش عرض
                ChangeRoll.changeRoll = true;
                // نیاز به غلتک دوم نیست
                reasonWorkRoll = 1;
                // تغییر غلتک
                InnerParameter.weiTotal = 0;
                InnerParameter.lenTotal = 0;

                if (Lst.RollsWork.Last().FirstPlan == true)

                    chekChangRoll = (int)WorkRollEnum.NotSarfaslChangeRoll;

                else // غلتک موجود جدید است
                {
                    chekChangRoll = (int)WorkRollEnum.NotSarfaslNewbeforRoll;
                }
            }

            Lst.currSolution.ChekChangeWorkRollBefor = chekChangRoll;
            Lst.currSolution.DoubleWorkBefor = reasonWorkRoll;



        }

      

        public void local(int misFrom, int misTo, List<ChangeRoll> ChangeRolls, int indxSarfaslLoc)
        {

            List<ChangeRoll> lstChangRollLocal = new List<ChangeRoll>();

            lstChangRollLocal = ChangeRolls.Where(c => (c.IdMisProgFrom == misFrom || c.IdMisProgFrom == Parameter.CodMisNull)
                                                              && (c.IdMisProgTo == misTo || c.IdMisProgTo == Parameter.CodMisNull)
                                                              && (c.IndexSarfaslFrom == Status.IndexSarfasl || c.IndexSarfaslFrom == Parameter.CodSarfaslNull)
                                                              && (c.IndexSarfaslTo == indxSarfaslLoc || c.IndexSarfaslTo == Parameter.CodSarfaslNull)).ToList();

        }
    }
}
