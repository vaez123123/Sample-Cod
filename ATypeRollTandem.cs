using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.CommonFunctions.ParameterClasses;
using IPSO.ParameterClasses;

namespace TandemScheduling
{
    public class ATypeRollTandem
    {
        //TAN
        public void chekWorkRoll(List<Roll> lstRollLocal, CommonLists Lst)
        {
            Roll rollTem = new Roll();

            int secondRollLocal = -1; 


            //  need to new roll  
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
                    // second roll is new  
                    secondRollLocal = 0;
                else
                    //  
                    //     
                    secondRollLocal = 1;

            }

            else
            {
                // old roll 
                secondRollLocal = chekChangworkCapcity(lstRollLocal, Lst);
            }


            // add new roll
            if (secondRollLocal == (int)TandemModel.WorkRollEnum.NewRollbutUseOldRoll)
            {
                Lst.RollsWork.Last().CurrentTotalFixWei += Lst.SolutionsOutputPlan.Last().WeiProg;
                Lst.RollsWork.Last().CurrentTotalFixLen += Lst.SolutionsOutputPlan.Last().LenProg;
                Lst.RollsWork.Last().FirstPlan = true;
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


                // second roll is new  
            else if (secondRollLocal == 0)

            {
                // capacity of old roll is not used completely
                if (Lst.RollsWork.Last().FirstPlan == false)
                {

                    Lst.RollsWork.Last().CurrentTotalFixWei += Lst.SolutionsOutputPlan.Last().WeiProg;
                    Lst.RollsWork.Last().CurrentTotalFixLen += Lst.SolutionsOutputPlan.Last().LenProg;
                    Lst.RollsWork.Last().FirstPlan = true;
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
                    Lst.SolutionsOutputPlan.Last().IndexWorkRoll = Lst.RollsWork.Count - 1;

                    rollTem = new Roll();
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

                // add to previous roll 
            else if (secondRollLocal == (int)TandemModel.WorkRollEnum.OldRoll)
            {
                Lst.RollsWork.Last().CurrentTotalFixLen += Lst.SolutionsOutputPlan.Last().LenProg;
                Lst.RollsWork.Last().CurrentTotalFixWei += Lst.SolutionsOutputPlan.Last().WeiProg;
                Lst.RollsWork.Last().FirstPlan = true;
                Lst.SolutionsOutputPlan.Last().IndexWorkRoll = Lst.RollsWork.Count - 1;
            }

            else if (secondRollLocal == 1)
            {
                // use old roll    
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
                    Lst.SolutionsOutputPlan.Last().IndexWorkRoll = Lst.RollsWork.Count - 1;
                }
            }

        }


        public int chekChangworkCapcity(List<Roll> lstRollLocal, CommonLists Lst)
        {

            if (lstRollLocal.Last().WeiOpt != 0)
            {
                //Roller change due to capacity
                if ((lstRollLocal.Last().WeiOpt) * (1 - lstRollLocal.Last().LowerPerc) < (lstRollLocal.Last().WeiDB
                                                        + lstRollLocal.Last().CurrentTotalFixWei
                                                        + lstRollLocal.Last().WeiRelease
                                                        + Lst.SolutionsOutputPlan.Last().WeiProg))
                    // add new roll
                    return (int)TandemModel.WorkRollEnum.NewRollbutUseOldRoll;


                else
                    // with old roll  
                    return (int)TandemModel.WorkRollEnum.OldRoll;

            }
            else
            {
             //Roller change due to capacity
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
                        firstFlagLocal = lstChangRollLocal[0].FlagChangeRoll;
                        SecondFlagLocal = lstChangRollLocal[1].FlagChangeRoll;

                         
                        if (firstFlagLocal == 1 && SecondFlagLocal == 1)
                        {

                            //  increasing width
                            TanSkpTemParameter.changeRoll = true;
                            reasonWorkRoll = 1;
                            // change roll
                            InnerParameter.weiTotal = 0;
                            InnerParameter.lenTotal = 0;

                            if (Lst.RollsWork.Last().FirstPlan == true)

                                chekChangRoll = (int)TandemModel.WorkRollEnum.DecreaseIncreaseChangeRoll;

                            else //
                            {
                                chekChangRoll = (int)TandemModel.WorkRollEnum.DecreaseIncreaseNewBeforRoll;
                            }

                        }
                       
                        
                        else if (firstFlagLocal != SecondFlagLocal)
                        {
                            chekChangRoll = (int)TandemModel.WorkRollEnum.IncreaseNotChangeRoll;


                            TanSkpTemParameter.changeRoll = false;
                            reasonWorkRoll = 2;
                            
                            // no change roll
                            InnerParameter.weiTotal = Lst.RollsWork.Last().WeiDB + Lst.RollsWork.Last().WeiRelease + Lst.RollsWork.Last().CurrentTotalFixWei;
                            InnerParameter.lenTotal = Lst.RollsWork.Last().LenDB + Lst.RollsWork.Last().LenRelease + Lst.RollsWork.Last().CurrentTotalFixLen;

                       
                            if ((InnerParameter.weiTotal == 0
                                //|| lenTotal == 0
                            ) && Lst.RollsWork.Last().FirstPlan == false)
                            {
                                InnerParameter.weiTotal = 0;
                                InnerParameter.lenTotal = 0;
                                TanSkpTemParameter.changeRoll = true;
                                reasonWorkRoll = 1;

                                chekChangRoll = (int)TandemModel.WorkRollEnum.IncreaseNewBeforRoll;

                            }


                        }
                        else
                        {
                            chekChangRoll = (int)TandemModel.WorkRollEnum.NotDecreaseIncreaseNotChangeRoll;

                            TanSkpTemParameter.changeRoll = true;
                            //Roller change due to capacity                         
                            reasonWorkRoll = 2;

                            InnerParameter.weiTotal = Lst.RollsWork.Last().WeiDB + Lst.RollsWork.Last().WeiRelease + Lst.RollsWork.Last().CurrentTotalFixWei;
                            InnerParameter.lenTotal = Lst.RollsWork.Last().LenDB + Lst.RollsWork.Last().LenRelease + Lst.RollsWork.Last().CurrentTotalFixLen;


                            if ((InnerParameter.weiTotal == 0
                                //|| lenTotal == 0
                            ) && Lst.RollsWork.Last().FirstPlan == false)
                            {
                                InnerParameter.weiTotal = 0;
                                InnerParameter.lenTotal = 0;
                                TanSkpTemParameter.changeRoll = true;
                                reasonWorkRoll = 1;

                                chekChangRoll = (int)TandemModel.WorkRollEnum.NotDecreaseIncreaseNewBeforRoll;

                            }

                        }



                    }

                    else
                    {

                        chekChangRoll = (int)TandemModel.WorkRollEnum.NotDataNotChangeRoll;

                        TanSkpTemParameter.changeRoll = true;
                        reasonWorkRoll = 2;

                        InnerParameter.weiTotal = Lst.RollsWork.Last().WeiDB + Lst.RollsWork.Last().WeiRelease + Lst.RollsWork.Last().CurrentTotalFixWei;
                        InnerParameter.lenTotal = Lst.RollsWork.Last().LenDB + Lst.RollsWork.Last().LenRelease + Lst.RollsWork.Last().CurrentTotalFixLen;

                        if ((InnerParameter.weiTotal == 0
                            //|| lenTotal == 0
                        ) && Lst.RollsWork.Last().FirstPlan == false)
                        {
                            InnerParameter.weiTotal = 0;
                            InnerParameter.lenTotal = 0;
                            TanSkpTemParameter.changeRoll = true;
                            reasonWorkRoll = 1;

                            chekChangRoll = (int)TandemModel.WorkRollEnum.NotDataNewbeforRoll;

                        }

                    }


                }
                else
                {

                    TanSkpTemParameter.changeRoll = true;
                    reasonWorkRoll = 1;
                    InnerParameter.weiTotal = 0;
                    InnerParameter.lenTotal = 0;

                    if (Lst.RollsWork.Last().FirstPlan == true)

                        chekChangRoll = (int)TandemModel.WorkRollEnum.NotProgChangeRoll;

                    else 
                    {
                        chekChangRoll = (int)TandemModel.WorkRollEnum.NotProgNewbeforRoll;
                    }
                }




            }
            else
            {
                TanSkpTemParameter.changeRoll = true;
                reasonWorkRoll = 1;
                // change roll
                InnerParameter.weiTotal = 0;
                InnerParameter.lenTotal = 0;

                if (Lst.RollsWork.Last().FirstPlan == true)

                    chekChangRoll = (int)TandemModel.WorkRollEnum.NotSarfaslChangeRoll;

                else 
                    
                {
                    chekChangRoll = (int)TandemModel.WorkRollEnum.NotSarfaslNewbeforRoll;
                }
            }

            Lst.currSolution.ChekChangeWorkRollBefor = chekChangRoll;
            Lst.currSolution.DoubleWorkBefor = reasonWorkRoll;



        }
    }
}
