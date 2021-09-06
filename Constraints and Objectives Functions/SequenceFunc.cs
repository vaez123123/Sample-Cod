using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.CommonFunctions.ParameterClasses;

namespace IPSO.CMP.CommonFunctions.Functions
{
    public class SequenceFunc
    {
        // select coil according to various features and coil scores
        public static int selectCoil(int wid, int rank, int tks, int tksOut, DateTime datePlanLoc, List<Coil> CoilsCapDay,
            Solution currSolution, List<Coil> CoilsRestart)
        {
            int indexMaxlocal = -1;
            int maxWidLocal = -1;
            double maxRankTotal = -1;
            double maxRankTksOut = -1;
            double maxRankTks = -1;
            List<Coil> lstCoilLocal = new List<Coil>();
            List<int> localOrd = new List<int>();
            List<int> orderOper = new List<int>();

            localOrd.Add(wid);
            localOrd.Add(rank);
            localOrd.Add(tks);
            localOrd.Add(tksOut);


            for (int i = 1; i < 5; i++)
            {
                orderOper.Add(localOrd.FindIndex(a => a == i) + 1);
            }


            if (CoilsCapDay.Count == 0)
                return -1;

            else
            {
                if (currSolution.LstSeqCoil.Count == 0)
                {

                    indexMaxlocal = -1;
                    maxWidLocal = -1;
                    maxRankTotal = -1;
                    maxRankTksOut = -1;
                    maxRankTks = -1;

                    lstCoilLocal.AddRange(CoilsCapDay.Where(a => CoilsRestart.Contains(a) != true));

                    for (int h = 0; h < orderOper.Count; h++)
                    {

                        switch (orderOper[h])
                        {
                            case 1://wid
                                {
                                    for (int i = 0; i < lstCoilLocal.Count; i++)
                                    {
                                        if (lstCoilLocal[i].Width > maxWidLocal)
                                        {
                                            maxWidLocal = lstCoilLocal[i].Width;
                                            indexMaxlocal = lstCoilLocal[i].ModelIndexCoil;
                                        }
                                    }

                                    lstCoilLocal.RemoveAll(x => x.Width != maxWidLocal);

                                    break;
                                }
                            case 2:// Rank
                                {
                                    for (int i = 0; i < lstCoilLocal.Count; i++)
                                    {
                                        if (lstCoilLocal[i].RankTotal > maxRankTotal)
                                        {
                                            maxRankTotal = lstCoilLocal[i].RankTotal;
                                            indexMaxlocal = lstCoilLocal[i].ModelIndexCoil;
                                        }
                                    }

                                    lstCoilLocal.RemoveAll(x => x.RankTotal != maxRankTotal);

                                    break;
                                }


                            case 3://TksOut
                                {
                                    for (int i = 0; i < lstCoilLocal.Count; i++)
                                    {
                                        if (lstCoilLocal[i].TksOutput > maxRankTksOut)
                                        {
                                            maxRankTksOut = lstCoilLocal[i].TksOutput;
                                            indexMaxlocal = lstCoilLocal[i].ModelIndexCoil;
                                        }
                                    }

                                    lstCoilLocal.RemoveAll(x => x.TksOutput != maxRankTksOut);

                                    break;
                                }

                            case 4:// tks
                                {                                  
                                   for (int i = 0; i < lstCoilLocal.Count; i++)
                                    {
                                        if (lstCoilLocal[i].Tks > maxRankTks)
                                        {
                                            maxRankTks = lstCoilLocal[i].Tks;
                                            indexMaxlocal = lstCoilLocal[i].ModelIndexCoil;
                                        }
                                    }
                                    break;
                                }
                        }

                    }

                    if (indexMaxlocal == -1 && datePlanLoc == InnerParameter.maxDate.Date)
                    {
                        InnerParameter.chekFinishCoilRankDelete = true;

                    }

                }
                else
                {
                    indexMaxlocal = -1;
                    maxWidLocal = -1;
                    maxRankTotal = -1;
                    maxRankTksOut = -1;
                    maxRankTks = -1;

                    lstCoilLocal.AddRange(CoilsCapDay);

                    for (int h = 0; h < orderOper.Count; h++)
                    {

                        switch (orderOper[h])
                        {
                            case 1:
                                {
                                    //wid
                                    for (int i = 0; i < lstCoilLocal.Count; i++)
                                    {
                                        if (lstCoilLocal[i].Width > maxWidLocal)
                                        {
                                            maxWidLocal = lstCoilLocal[i].Width;
                                            indexMaxlocal = lstCoilLocal[i].ModelIndexCoil;
                                        }
                                    }
                                    lstCoilLocal.RemoveAll(x => x.Width != maxWidLocal);

                                    break;
                                }


                            case 2:// Rank
                                {
                                    for (int i = 0; i < lstCoilLocal.Count; i++)
                                    {
                                        if (lstCoilLocal[i].RankTotal > maxRankTotal)
                                        {
                                            maxRankTotal = lstCoilLocal[i].RankTotal;
                                            indexMaxlocal = lstCoilLocal[i].ModelIndexCoil;
                                        }
                                    }

                                    lstCoilLocal.RemoveAll(x => x.RankTotal != maxRankTotal);

                                    break;
                                }

                            case 3: //TksOut
                                {

                                    for (int i = 0; i < lstCoilLocal.Count; i++)
                                    {
                                        if (lstCoilLocal[i].TksOutput > maxRankTksOut)
                                        {
                                            maxRankTksOut = lstCoilLocal[i].TksOutput;
                                            indexMaxlocal = lstCoilLocal[i].ModelIndexCoil;
                                        }
                                    }

                                    lstCoilLocal.RemoveAll(x => x.TksOutput != maxRankTksOut);

                                    break;

                                }
                            case 4:// tks
                                {

                                    for (int i = 0; i < lstCoilLocal.Count; i++)
                                    {
                                        if (lstCoilLocal[i].Tks > maxRankTks)
                                        {
                                            maxRankTks = lstCoilLocal[i].Tks;
                                            indexMaxlocal = lstCoilLocal[i].ModelIndexCoil;
                                        }
                                    }

                                    break;
                                }
                        }
                    }

                }


            }

            return indexMaxlocal;
        }

        public static void updateMaxValueGroupCurr(int flagCondition, int modelIndexCoilLocal, List<MaxValueGroup> MaxValueGroups, List<int> lstAvailMaxValueGroup
               , List<Coil> Coils)
        {


            foreach (var item in MaxValueGroups)
            {
                if (item.LstCoilMaxVal.Contains(modelIndexCoilLocal) == true && item.Day.Date == Status.CurrTime.Date)
                {
                    if (flagCondition == 1)
                    {
                        item.RespondTemValue += Coils[modelIndexCoilLocal].Weight;
                        if (item.RespondFixValue + item.RespondTemValue + item.RespondValue >= item.MaxValue)
                            foreach (var coil in item.LstCoilMaxVal)
                                lstAvailMaxValueGroup.Remove(coil);
                    }
                    else
                    {
                        item.RespondTemValue -= Coils[modelIndexCoilLocal].Weight;

                        if (item.RespondFixValue + item.RespondTemValue + item.RespondValue < item.MaxValue)

                            foreach (var coil in item.LstCoilMaxVal)
                                lstAvailMaxValueGroup.Add(coil);

                    }
                }
            }
        }


        public static void updateListsAfterInsertCoil(int select, List<Coil> Coils, List<Coil> CoilsMain, List<Coil> CoilsTemDelete,
            List<Coil> CoilsAvailProg, List<Coil> CoilsCapDay)
        {
            CoilsAvailProg.Remove(Coils[select]);
            CoilsMain.Remove(Coils[select]);
            CoilsCapDay.Remove(Coils[select]);
            CoilsCapDay.AddRange(CoilsTemDelete);
            CoilsTemDelete.Clear();
        }

        public static void generalInsertAfterCoil(Solution currSolution, int select, List<Coil> Coils, List<MaxValueGroup> MaxValueGroups,
            List<int> lstAvailMaxValueGroup, List<CapPlan> CapPlansCurr, List<Coil> CoilsMain, List<Coil> CoilsTemDelete,
            List<Coil> CoilsAvailProg, List<Coil> CoilsCapDay)
        {
                // The first selected coil in the program in order to check the minimum number of coils in the program
            if (currSolution.LstSeqCoil.Count == 1)

                InnerParameter.firstCoilBigCoun = select;

            InnerParameter.weiTotal += Coils[select].Weight;
            InnerParameter.lenTotal += Coils[select].Len;

            updateMaxValueGroupCurr(1, select, MaxValueGroups, lstAvailMaxValueGroup, Coils);

            CapPlanFunc.updateCapCurr(select, CapPlansCurr, Coils);

            Solution.sumWeiLenProg(select, currSolution, Coils);

            updateListsAfterInsertCoil(select, Coils, CoilsMain, CoilsTemDelete, CoilsAvailProg, CoilsCapDay);
        }

        public static void removeSamePf(int selectlocal, List<Coil> Coils, List<Coil> CoilsCapDay, List<Coil> CoilsMain, List<CapPlan> CapPlansCurr)
        {
            int pfLoc = Coils[selectlocal].PfId;
            double weiLoc = Coils[selectlocal].Weight;

            double weiMaxLoc = CapPlansCurr.Find(i => i.DatePlan.Date == Status.CurrTime.Date && i.PfId == pfLoc).MaxValueRespond;
            weiLoc = Math.Min(weiLoc, weiMaxLoc);
            CoilsCapDay.RemoveAll(b => b.PfId == pfLoc && b.Weight >= weiLoc);
            CoilsMain.RemoveAll(b => b.PfId == pfLoc && b.Weight >= weiLoc);

        }

        public static void deleteAddCoil(int selectCoilLocal, List<Coil> Coils, List<Coil> CoilsCapDay, List<Coil> CoilsTemDelete)
        {
            List<Coil> lstCoilLoc = CoilsCapDay.Where(a => a.Width == Coils[selectCoilLocal].Width
                && a.Tks == Coils[selectCoilLocal].Tks
                && a.TksOutput == Coils[selectCoilLocal].TksOutput).ToList();

            CoilsTemDelete.AddRange(lstCoilLoc);
            CoilsCapDay.RemoveAll(c => lstCoilLoc.Contains(c));
        }

        public static void selectCoilSameAtrri(int firstCoilBigCoun, List<Coil> Coils, List<Coil> CoilsMainCopy, List<Coil> CoilsRestart)
        {

            int widLoc = Coils[firstCoilBigCoun].Width;
            double tksLoc = Coils[firstCoilBigCoun].Tks;
            double tksOut = Coils[firstCoilBigCoun].TksOutput;

            CoilsRestart.AddRange(CoilsMainCopy.Where(c => c.Width == widLoc
                                                            && c.Tks == tksLoc
                                                            && c.TksOutput == tksOut

                                                            ));

        }

        public static int sequenceSameAttributeCoils(int flgWidth, int flgTks, int flgTksOut, int coilSelection, Solution solution, List<Coil> Coils, int zoneNumber)
        {
            TimeParameter.timeseqSameAtt.Start();
            int indxFirst = 0;

            if (zoneNumber != -1)
                indxFirst = solution.LstZoneCoil.FindIndex(a => a == zoneNumber);

            if (indxFirst != -1)
            {
                int indxLast = solution.LstSeqCoil.Count();

                for (int i = indxFirst; i < indxLast; i++)
                {
                    if (RunInformation.NumStation == 10 || RunInformation.NumStation == 11)
                    {
                        if (Coils[coilSelection].FlgPhosphorus == 1)
                        {
                            if (Coils[solution.LstSeqCoil[i]].FlgPhosphorus == 1)
                            {
                                TimeParameter.timeseqSameAtt.Stop();
                                return -1;
                            }

                            if (i < indxLast - 1)
                            {
                                if (Coils[solution.LstSeqCoil[i + 1]].FlgPhosphorus == 1)
                                {
                                    TimeParameter.timeseqSameAtt.Stop();
                                    return -1;
                                }
                            }
                        }
                    }
                    //end pic

                    double difTks = 0;
                    double difTksOut = 0;
                    int difWid = 0;
                    if (flgTks == 1)
                        difTks = Coils[coilSelection].Tks - Coils[solution.LstSeqCoil[i]].Tks;
                    if (flgTksOut == 1)
                        difTksOut = Coils[coilSelection].TksOutput - Coils[solution.LstSeqCoil[i]].TksOutput;
                    if (flgWidth == 1)
                        difWid = Coils[coilSelection].Width - Coils[solution.LstSeqCoil[i]].Width;

                    if (difTks == 0 && difWid == 0 && difTksOut == 0)
                    {
                        TimeParameter.timeseqSameAtt.Stop();
                        return i + 1;
                    }
                }
            }
            TimeParameter.timeseqSameAtt.Stop();
            return -1;
        }


    }
}
