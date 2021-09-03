using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.CommonFunctions.ParameterClasses;
using IPSO.CMP.CommonFunctions.Functions;
using IPSO.ParameterClasses;
using IPSO.CMP.Tan_Skp_Tem_Function.ConstraintSequence;
using TandemScheduling;

namespace SkinPass1Scheduling
{
    public partial class SkinPass1Model
    {
        public int reasonWorkRoll;
        public int doubleChekWorkRoll = 1; // 2 double chek 1 one chek

        ObjectiveSkp1 objective = new ObjectiveSkp1();

        public void runSkinPass1Model()
        {

            chekStatBeforAlgorithmOrWhile();
            //string rank="rank.txt";
            // rankAll(rank);

            do
            {
                // if (cheknotCoil == true)

                insertAvailSarfasl();  
                InnerParameter.lstChekFinishCapplan.Clear();
               
                if (InnerParameter.lstPfAvail.Count == 0)
                    InnerParameter.lstChekFinishCapplan.Add(true);

                for (int s = 0; s < Lst.lstAvailSarfasl.Count; s++)
                {


                    int indSarfasl = Lst.lstAvailSarfasl[s];
                    Lst.currSolution.IndexSarfasl = indSarfasl;

                 //   insertAvailProg(indSarfasl);


                    for (int p = 0; p < Lst.lstAvailProg.Count; p++)
                    {


                        int idEfraz = Lst.lstAvailProg[p];
                        Lst.currSolution.IdEfraz = idEfraz;
                        Lst.CoilsRestart.Clear();
                        Lst.bigSolution.TotalObj = double.MaxValue;
                        Lst.bigSolution.LstSeqCoil.Clear();
                        doubleChekWorkRoll = 1;
                        

                        //WidJump.calcuValJumWid(idEfraz, TanSkpTemParameter.idEfrazMisCurr, Lst.ProgEfrazes, LstCom.WidJumps);

                        for (InnerParameter.numCoil = 0; InnerParameter.numCoil < 4; )
                        {


                            Solution.resetObjToZeroObj(Lst.currSolution, Lst.CapPlans, Lst.CapPlansCurr, Lst.MaxValueGroups, Lst.CapPlanUpDates);

                               chekWorkRollCurr(indSarfasl, idEfraz);

                            TimeSpan setTime = TimeFunc.calcuSetupTime(idEfraz, Lst.Schedulings, Lst.Setups);


                               EquipGroupFailureTime.insertEquipGroupFailureList(setTime, Lst.lstAvailEquipGroupFailureTime, Lst.EquipGroupFailureTimes);
                            MaxValueGroup.insertMaxValueGroupsAvailList(Lst.MaxValueGroups, Lst.lstAvailMaxValueGroup);


                            if (InnerParameter.numCoil < 2)
                            {
                                fillMainList(indSarfasl, idEfraz);

                                InnerParameter.mainListCount = Lst.CoilsMain.Count;


                                if (Lst.CoilsMain.Count != 0)
                                      sequencingProgram(idEfraz, indSarfasl, ref InnerParameter.numCoil);
                                //*****************
                                else
                                {

                                  //  CoilsRestart.Clear();

                                    if (//currSolution.lstSeqCoil.Count < y && 
                                        // lenTotal > lenOpt &&
                                         reasonWorkRoll == 2 && doubleChekWorkRoll != 2)
                                    {
                                        //if (flagDoubleWorkRoll == 1)
                                        //{

                                        doubleChekWorkRoll = 2;

                                        InnerParameter.numCoil = 2;
                                        //}
                                        //else
                                        //    InnerParameter.numCoil = 4;
                                    }


                                    else
                                        InnerParameter.numCoil = 4;

                                   
                                }
                            }

                            //*****************
                            if (doubleChekWorkRoll == 2 && InnerParameter.numCoil == 2)
                            {
                                //if (flagDoubleWorkRoll == 1)
                                //{
                                    Solution.resetObjToZeroObj(Lst.currSolution, Lst.CapPlans, Lst.CapPlansCurr, Lst.MaxValueGroups, Lst.CapPlanUpDates);
                                    InnerParameter.weiTotal = 0;
                                    InnerParameter.lenTotal = 0;

                                    TanSkpTemParameter.changeRoll = true;

                                    fillMainList(indSarfasl, idEfraz);
                                    InnerParameter.mainListCount = Lst.CoilsMain.Count;

                                    if (Lst.CoilsMain.Count != 0)
                                    {
                                        // for each plan
                                        sequencingProgram(idEfraz, indSarfasl, ref InnerParameter.numCoil);
                                        //*****************
                                        //doubleChekWorkRoll = 1;
                                    }
                                    else
                                    {
                                        //*****************
                                        doubleChekWorkRoll = 1;
                                        InnerParameter.numCoil = 4;
                                        break;
                                    }
                                //}
                                //else
                                //{
                                //    InnerParameter.numCoil = 4;
                                //}



                            }


                        }


                    }//

                }


                if (Lst.bestSolution.LstSeqCoil.Count == 0)
                {

                    if (InnerParameter.lstChekFinishCapplan.Count == 0)
                    {
                        if (InnerParameter.RuleBetweenProg == -1)
                        {
                            InnerParameter.RuleBetweenProg = 1;

                            TimeFunc.chekTime(-2, Lst.SolutionsOutputPlan, Lst.ReleaseScheds, Lst.StationStops, Lst.Schedulings, Lst.ShiftWorks,
                    Lst.CapPlans, Lst.MaxValueGroups, Lst.Coils, Lst.CoilReleases, Lst.lstAvailMaxValueGroup, Lst.currSolution, Lst.Setups);

                            if (Lst.Schedulings.Last().End > Lst.CapPlans.Max(a => a.DatePlan))
                            {


                                break;
                            }
                            continue;

                        }
                        else
                        {

                            InnerParameter.RuleBetweenProg = -1;
                            continue;
                        }
                    }

                    else if (InnerParameter.lstChekFinishCapplan.Contains(false) == true && InnerParameter.lstChekFinishCapplan.Contains(true) == true)
                    {


                        if (InnerParameter.RuleBetweenProg == 1)
                        {
                            InnerParameter.RuleBetweenProg = -1;
                            continue;
                        }
                        else
                        {
                              break;
                        }


                    }

                            // ALL true
                    else if (InnerParameter.lstChekFinishCapplan.Contains(true) == true)
                    {


                        if (InnerParameter.RuleBetweenProg == 1)
                        {
                            TimeFunc.chekTime(-2, Lst.SolutionsOutputPlan, Lst.ReleaseScheds, Lst.StationStops, Lst.Schedulings, Lst.ShiftWorks,
                    Lst.CapPlans, Lst.MaxValueGroups, Lst.Coils, Lst.CoilReleases, Lst.lstAvailMaxValueGroup, Lst.currSolution, Lst.Setups);
                            if (Lst.Schedulings.Last().End > Lst.CapPlans.Max(a => a.DatePlan))
                                break;

                            continue;
                        }
                        else
                        {
                            // the program logic is wrong
                               break;
                        }


                    }

                     // All False
                    else
                    {

                        if (InnerParameter.RuleBetweenProg == 1)
                        {
                            InnerParameter.RuleBetweenProg = -1;
                            continue;
                        }
                        else
                        {
                            // the program logic is wrong
                           
                            break;
                        }

                    }               
                    
                }
                int hh=0;

                fixSolutionAndUpdate();
                if (Lst.SolutionsOutputPlan.Count == 35)
                  hh  = 35;



                InnerParameter.finiTimeAlgorithm = Status.CurrTime;
            } while (Lst.SolutionsOutputPlan.Count < RunInformation.CountProg

            || (InnerParameter.finiTimeAlgorithm - InnerParameter.starTimeAlgorithm).Hours < RunInformation.Hours

                //&& flagLoop == true
                //  && chekSarbarnameAndFirstSarfasl < 3
                );//   
            CapPlanUpDate.chekCoilWithoutSarfasl(Lst.CapPlanUpDates, Lst.Coils);

       

        }



        public void sequencingProgram(int idEfrazLocal, int idSarfaslLocal, ref int numCoil)
        {
            int sequence = 0;
            int select = -1;
            List<int> lstPfLoc = new List<int>();
            List<int> lstPfLoc1 = new List<int>();

            InnerParameter.chekFinishCapplan = false;

            for (DateTime datePlanLoc = Status.CurrTime.Date; datePlanLoc <= InnerParameter.maxDate.Date; datePlanLoc = datePlanLoc.AddDays(1))
            {

                int chekLocal = 0;
               
               // chekLocal = CapPlan.chekStatCoilsCapDay(lstPfLoc, lstPfLoc1, datePlanLoc, Lst.CoilsMain, Lst.CoilsRestart, ref InnerParameter.chekFinishCapplan,
                  //  Lst.currSolution, Lst.CapPlansCurr, InnerParameter.maxDate, Lst.CoilsTemDelete, Lst.CoilsCapDay,-1);

               
                if (chekLocal == 1)
                    break;
                else if (chekLocal == -1)
                    continue;


    

                #region
                do
                {
                    //int yy = 0;
                    //if ((idAfrazLocal == 421) && idSarfaslLocal == 1 && lstOutputPlan.Count == 5 && countProgLocal == 0)
                    //    yy++;
                    // selsct the best coil
                    select = SequenceFunc.selectCoil(2, 1, 3, 4, datePlanLoc, Lst.CoilsCapDay, Lst.currSolution, Lst.CoilsRestart);

                    if (select == -1)
                    {

                        break;
                    }

                    else  //  if (select == -1)
                    {

                        //double id = 0;
                        //id = lstCoil[select].idSnapshot;
                        //if (id == 3843)
                        //    id = 0;
                        InnerParameter.chekCap = CapPlanFunc.chekMaxCapPlan(select, Lst.CapPlansCurr, Lst.Coils);
                        
                       

                         if (InnerParameter.chekCap == 1)
                        {

                            sequence = SequenceFunc.sequenceSameAttributeCoils(1, -1, -1, select, Lst.currSolution, Lst.Coils, -1);


                            if (sequence == -1)
                            {
                             //   sequence = ConstraintFunc.seqCoilProg(select, TanSkpTemParameter.chekDecInc, idEfrazLocal, Lst.currSolution, Lst.ProgEfrazes, Lst.Coils, Lst.TksJumps);
                                
                                if (sequence == -1)
                                {
                                    SequenceFunc.deleteAddCoil(select, Lst.Coils, Lst.CoilsCapDay, Lst.CoilsTemDelete);

                                }
                                else
                                
                                  
                                        break;
                                    
                                    //    ConstraintFunc.selectSameOrder(select, idEfrazLocal, idSarfaslLocal, datePlanLoc, InnerParameter.maxCount,Lst);

                                
                            }
                            else
                            {


                                Lst.currSolution.LstSeqCoil.Insert(sequence, select);


                               // ConstraintFunc.updateAfterInsertCoil(select,Lst);


                                if (Lst.currSolution.LstSeqCoil.Count >= InnerParameter.maxCount ||
                                    //lenTotal > lenOpt*(1+lstWorkRoll.Last().upperPerc)
                                     InnerParameter.weiTotal > InnerParameter.weiOpt * (1 + Lst.RollsWork.Last().UpperPerc))
     
                                    break;
                                //else
                                   // ConstraintFunc.selectSameOrder(select, idEfrazLocal, idSarfaslLocal, datePlanLoc, InnerParameter.maxCount,Lst);

                            }
                        }
                        




                    }




                }
                while (Lst.currSolution.LstSeqCoil.Count < InnerParameter.maxCount &&
                    //lenTotal < lenOpt*(1+lstWorkRoll.Last().upperPerc)
                    InnerParameter.weiTotal < InnerParameter.weiOpt * (1 + Lst.RollsWork.Last().UpperPerc));
                if (Lst.currSolution.LstSeqCoil.Count == 0 && datePlanLoc == InnerParameter.maxDate)
                {
                    if (InnerParameter.chekFinishCapplan == false)
                    {
                        Lst.currSolution.LstChekCap.Add(InnerParameter.chekFinishCapplan);
                        break;
                    }
                }

                if (Lst.currSolution.LstSeqCoil.Count >= InnerParameter.maxCount ||
                    //lenTotal >= lenOpt* (1 + lstWorkRoll.Last().upperPerc)
                    InnerParameter.weiTotal >= InnerParameter.weiOpt * (1 + Lst.RollsWork.Last().UpperPerc))
                // || (currSolution.LstSeqCoil.Count == 0 && datePlanLoc == maxDate))
                {

                    break;
                }

            }
                #endregion

              if (Lst.currSolution.LstSeqCoil.Count == 0
                //&&
               // coilRankDeleteCount == 1 && 
                //flagDoubleWorkRoll == 1
                )
            {
             if (Lst.currSolution.LstChekCap.Contains(false) == true)
                    InnerParameter.lstChekFinishCapplan.Add(false);
                else
                    InnerParameter.lstChekFinishCapplan.Add(true);  

          
                if (//currSolution.lstSeqCoil.Count < y && 
                    // lenTotal > lenOpt &&
                     reasonWorkRoll == 2 && doubleChekWorkRoll != 2)
                {
                      doubleChekWorkRoll = 2;

                    numCoil = 2;
                }

                   else
                     numCoil = 4;

            }

              else //(currSolution.lstSeqCoil.Count < minCount && currSolution.lstSeqCoil.Count != 0)
            {

                   Lst.bigSolution.LstSeqCoil.Clear();


               if (Lst.currSolution.LstSeqCoil.Count < InnerParameter.minCount)
                       SequenceFunc.selectCoilSameAtrri(InnerParameter.firstCoilBigCoun, Lst.Coils, Lst.CoilsMainCopy, Lst.CoilsRestart);


              InnerParameter.countProgLocal++;

                //string localCoil = "localCoil";
                //writeLocal(localCoil);
                string local = "local.txt";

                //writercurrProg(Lst.currSolution, local, true,Lst.Coils);

                 InnerParameter.countCurrProg = InnerParameter.countProgLocal;

                if (Lst.currSolution.TotalObj < Lst.bigSolution.TotalObj
                    //(currSolution.lstSeqCoil.Count > bigSolution.lstSeqCoil.Count && currSolution.TotalObj < bigSolution.TotalObj)
                    //|| (currSolution.lstSeqCoil.Count == bigSolution.lstSeqCoil.Count && currSolution.TotalObj < bigSolution.TotalObj)
                    )
                {

                     InnerParameter.countBigProg = InnerParameter.countCurrProg;
                    Lst.bigSolution.LstSeqCoil.Clear();
                    Solution.replaceSolutions(TanSkpTemParameter.changeRoll, Lst.bigSolution, Lst.currSolution);


                }

                if (Lst.currSolution.LstSeqCoil.Count == InnerParameter.mainListCount)
                {
                    if (//currSolution.lstSeqCoil.Count < y && 
                        // lenTotal > lenOpt &&
                    reasonWorkRoll == 2 && doubleChekWorkRoll != 2
                    //&& 
                    //flagDoubleWorkRoll == 1
                        )
                    {
                         //*****************
                        doubleChekWorkRoll = 2;
                       // CoilsRestart.Clear();
                     
                        numCoil = 2;
                    }

                     else
                        
                        numCoil = 4;

                }
            

            }



            // 
            if (Lst.bigSolution.LstSeqCoil.Count != 0 &&
                //currSolution.lstSeqCoil.Count != 0 &&

               (InnerParameter.chekFinishCoilRankDelete == true || Lst.currSolution.LstSeqCoil.Count >= InnerParameter.minCount || numCoil == 4)
                //

                )
            {

                InnerParameter.countCurrProg = InnerParameter.countBigProg;
                Solution.replaceSolutions(TanSkpTemParameter.changeRoll, Lst.currSolution, Lst.bigSolution);
        


                //Insert the best answer
                updateBestsolution();      

       



                if (//currSolution.lstSeqCoil.Count < y && 
                    // lenTotal > lenOpt &&
                    reasonWorkRoll == 2 
                   // && flagDoubleWorkRoll == 1
                    )
                {

                    //*****************
                  //  CoilsRestart.Clear();

                    doubleChekWorkRoll = 2;
                    numCoil += 2;
                }

                else
                {
                    //*****************
                    doubleChekWorkRoll = 1;
                    numCoil = 4;
                }



            }
            Solution.resetObjToZeroObj(Lst.currSolution, Lst.CapPlans, Lst.CapPlansCurr, Lst.MaxValueGroups, Lst.CapPlanUpDates);

        }


        public void fixSolutionAndUpdate()
        {


           //Insert the best answer
            Solution.updatelstOutputPlan(Lst.bestSolution, Lst.SolutionsOutputPlan);


            string local = "local.txt";
            //writercurrProg(Lst.currSolution, local, false, Lst.Coils);

             //  calcuWeiCampPlan(lstOutputPlan.Last());

            TimeFunc.chekTime(-1, Lst.SolutionsOutputPlan, Lst.ReleaseScheds, Lst.StationStops, Lst.Schedulings, Lst.ShiftWorks,
                    Lst.CapPlans, Lst.MaxValueGroups, Lst.Coils, Lst.CoilReleases, Lst.lstAvailMaxValueGroup, Lst.currSolution, Lst.Setups); 
            
         

            Coil.flgCoilClass(Lst.SolutionsOutputPlan.Last(), Lst.Coils);

            Solution.resetObjToZeroObj(Lst.bestSolution, Lst.CapPlans, Lst.CapPlansCurr, Lst.MaxValueGroups, Lst.CapPlanUpDates);
            //CapPlanFunc.calcuPffForPlans(-1, Lst.SolutionsOutputPlan, Lst.ReleaseScheds, PathWriter, Lst.CapPlanUpDates, Lst.Coils, Lst.CoilReleases);
            
            
               // updateLstCapPlanCurr();
            //*
            updateCurrStat();
           
           // calcuRespondValueobj(lstOutputPlan.Last());
           // writerCapProg(lstOutputPlan.Last(), "capPlanProg.txt");

            //updateRespondProg();
            InnerParameter.countProgLocal = 0;
            InnerParameter.RuleBetweenProg = 1;

        }


        public void chekStatBeforAlgorithmOrWhile()
        {

            EquipGroupFailureTime.assignEquipGroupCoil(Lst.EquipGroupFailureTimes, Lst.Coils);
            MaxValueGroup.assignMaxValGroupCoil(Lst.MaxValueGroups, Lst.Coils);
            InnerParameter.maxDate = Lst.CapPlans.Max(a => a.DatePlan);

          
            Lst.bestSolution.TotalObj = double.MaxValue;

            DataBase.calcuMaxAttribute(Lst.Coils);

            for (int i = 0; i < Lst.ReleaseScheds.Count; i++)
            {

                TimeFunc.chekTime(i, Lst.SolutionsOutputPlan, Lst.ReleaseScheds, Lst.StationStops, Lst.Schedulings, Lst.ShiftWorks,
                       Lst.CapPlans, Lst.MaxValueGroups, Lst.Coils, Lst.CoilReleases, Lst.lstAvailMaxValueGroup, Lst.currSolution, Lst.Setups);  


                calcuworkRollRelease(i);
                   calcuWeiCampRelease(i);
                calcuMinWidCampRelease(i);
                //CapPlanFunc.calcuPffForPlans(i, Lst.SolutionsOutputPlan, Lst.ReleaseScheds, PathWriter, Lst.CapPlanUpDates, Lst.Coils, Lst.CoilReleases);


            }

            CapPlan.calcuMaxValueCapPlan(Lst.CapPlans);
            CapPlan.updatePfAvail(InnerParameter.lstPfAvail, Lst.CapPlans);
            CapPlan.updateLstCapPlanCurr(Lst.CapPlans, Lst.CapPlansCurr);
            


            updateCurrStat();


            //   calculate  score of coils
            for (int i = 0; i < Lst.Coils.Count; i++)
            {

                Coil.calcuTotalRank(i, Lst.Coils);
            }

              insertnotSensitiveSurFace();

            assignSarfaslGroup();

          //  ProgEfraz.chekFlgAvailForProgAfraz(Lst.ProgEfrazes, Lst.Coils);

            Coil.chekAvailSarfaslForCoils(Lst.Coils);
           


        }

     

    }
}
