using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.CommonFunctions.Functions;

namespace IPSO.CMP.CommonFunctions.ParameterClasses
{
    public class CapPlan
    {

        #region
        public int PfId { get; set; }
        public DateTime DatePlan { get; set; 
        public double NetValuePf { get; set; }
        public double MaxValueRespond;
        public double MaxValueFixChangeDay;
        public double NetValueAfterChangeDay { get; set; }
         public double NetValuePfFix { get; set; }

        public int FlgInactive;  // 0 = inactive 

        public CapPlan(int pfId, DateTime datePlan, double netValuePf, double netValuePfFix)
        {
            this.PfId = pfId;
            this.DatePlan = datePlan;
            this.NetValuePf = netValuePf;
            this.NetValuePfFix = netValuePfFix;


        }

        //constructor
        public CapPlan() { }

        #endregion

        //ALL
        public static void updateMaxValueCapPlan(List<CapPlan> CapPlansCurr, int modelIndex, List<Coil> Coils)
        {
            int index = CapPlansCurr.FindIndex(a => a.DatePlan.Date == Status.CurrTime.Date && a.PfId == Coils[modelIndex].PfId);
            CapPlansCurr[index].MaxValueRespond -= Coils[modelIndex].Weight;
        }

        //ALL
        public static void calcuMaxValueCapPlan(List<CapPlan> CapPlans)
        {
            foreach (var i in CapPlans)
            {
                i.NetValueAfterChangeDay = i.NetValuePf;

                if (i.FlgInactive == 1)
                {
                    if (i.NetValueAfterChangeDay == 0)
                    {
                        i.MaxValueRespond = DataBase.MinCapPlanDefault;
                        i.MaxValueFixChangeDay = DataBase.MinCapPlanDefault;
                    }
                    else
                    {
                        i.MaxValueRespond = DataBase.CoefMaxCapPlan * i.NetValueAfterChangeDay;
                        i.MaxValueFixChangeDay = DataBase.CoefMaxCapPlan * i.NetValueAfterChangeDay;
                    }
                }
                else
                {
                    i.MaxValueFixChangeDay = 0;
                    i.MaxValueRespond = 0;
                }
            }
        }

        //ALL
        public static void updateLstCapPlanCurr(List<CapPlan> CapPlans, List<CapPlan> CapPlansCurr)
        {
            CapPlansCurr.Clear();

            foreach (var i in CapPlans)
            {
                CapPlan loc = new CapPlan();
                loc.DatePlan = i.DatePlan;
                loc.NetValuePf = i.NetValuePf;
                loc.PfId = i.PfId;
                loc.MaxValueRespond = i.MaxValueRespond;
                loc.MaxValueFixChangeDay = i.MaxValueFixChangeDay;
                loc.NetValuePfFix = i.NetValuePfFix;
                loc.NetValueAfterChangeDay = i.NetValueAfterChangeDay;
                loc.FlgInactive = i.FlgInactive;
                CapPlansCurr.Add(loc);
            }
        }
        //ALL
        public static void updateCapPlanPositive(List<Scheduling> Schedulings, List<CapPlan> CapPlans)
        {
            if (Schedulings.Count() > 0)
            {
                if (TimeFunc.chekChangeDay(Schedulings.Last(), Parameter.Epsilon) == true)
                {
                    List<CapPlan> capLocal = new List<CapPlan>();

                    capLocal = CapPlans.Where(i => i.DatePlan.Date < Status.CurrTime.Date && i.NetValuePf > 0).ToList();

                    if (capLocal.Count != 0)
                    {
                        foreach (var item in capLocal)
                        {
                            int index = CapPlans.FindIndex(j => j.PfId == item.PfId && j.DatePlan.Date == Status.CurrTime.Date);
                            if (index != -1)
                            {
                                CapPlans[index].NetValuePf += item.NetValuePf;
                                item.NetValuePf = 0;
                            }
                        }
                    }
                }
            }
        }
        public static void updatePfAvail(List<int> lstPfAvail, List<CapPlan> CapPlans)
        {
            if (RunInformation.FlgUser == -1)
            {
                lstPfAvail.Clear();

                List<int> pfLoc = new List<int>();
                pfLoc = CapPlans.Where(c => c.NetValuePf > 0).Select(a => a.PfId).Distinct().ToList();

                List<CapPlan> lstAvailCapLoc = CapPlans.Where(c => c.MaxValueRespond > Parameter.EpsilonCapPlan
                                                                        && c.DatePlan.Date == Status.CurrTime.Date
                                                                        && pfLoc.Contains(c.PfId)).ToList();
                lstPfAvail.AddRange(lstAvailCapLoc.Select(a => a.PfId).Distinct().ToList());
            }
            else
            {
                lstPfAvail.AddRange(CapPlans.Select(a => a.PfId).Distinct().ToList());
 
            }


        }

        public static void updateCapPlan(int seq, int seqSolutions, List<Solution> SolutionsOutputPlan, List<CapPlan> CapPlans, List<ReleaseSched> ReleaseScheds,
            List<MaxValueGroup> MaxValueGroups, List<Coil> Coils, List<CoilRelease> CoilReleases, List<int> lstAvailMaxValueGroup)
        {
            #region if=> SolutionsOutputPlan.Count != 0

            if (SolutionsOutputPlan.Count != 0)
            {
                List<int> lstIndxCoil = new List<int>();
                if (seq == -1)
                    lstIndxCoil.AddRange(SolutionsOutputPlan[seqSolutions].LstSeqCoil);
                else//seq == -3
                    lstIndxCoil.Add(SolutionsOutputPlan[seqSolutions].LstSeqCoil.Last());

                foreach (int c in lstIndxCoil)
                {
                    double weiLocal = Coils[c].Weight;
                    MaxValueGroup.updateMaxValueGroup(c, MaxValueGroups, Coils, lstAvailMaxValueGroup);
                    do
                    {
                        int indx = CapPlans.FindIndex(i => i.PfId == Coils[c].PfId && i.NetValuePf > 0 && i.DatePlan.Date >= Status.CurrTime.Date &&
                            i.DatePlan == CapPlans.Where(j => j.PfId == Coils[c].PfId && j.NetValuePf > 0 && j.DatePlan.Date >= Status.CurrTime.Date).Min(a => a.DatePlan));

                        if (indx != -1)
                        {
                            int indexCurr = CapPlans.FindIndex(i => i.PfId == Coils[c].PfId && i.DatePlan.Date == Status.CurrTime.Date);

                            //if (indexCurr != -1)
                            //{

                                if (weiLocal > CapPlans[indx].NetValuePf)
                                {
                                    weiLocal -= CapPlans[indx].NetValuePf;
                                    CapPlans[indexCurr].MaxValueRespond -= CapPlans[indx].NetValuePf;
                                    CapPlans[indx].NetValuePf = 0;
                                }
                                else
                                {
                                    CapPlans[indx].NetValuePf -= weiLocal;
                                    CapPlans[indexCurr].MaxValueRespond -= weiLocal;
                                    weiLocal = 0;
                                    break;
                                }
                            //}
                        }
                        else
                            break;

                    } while (weiLocal > 0);


                }
            }

            #endregion


            else if (ReleaseScheds.Count != 0)
            {
                foreach (int c in ReleaseScheds[seq].LstSeqCoil)
                {
                    double weiLocal = CoilReleases[c].Weight;
                    MaxValueGroup.updateMaxValueGroup(c, MaxValueGroups, Coils, lstAvailMaxValueGroup);
                    do
                    {
                        int indx = CapPlans.FindIndex(i => i.PfId == CoilReleases[c].PfId && i.NetValuePf > 0
                            && i.DatePlan == CapPlans.Where(j => j.PfId == CoilReleases[c].PfId && j.NetValuePf > 0).Min(a => a.DatePlan));

                        if (indx != -1)
                        {

                            if (weiLocal > CapPlans[indx].NetValuePf)
                            {
                                weiLocal -= CapPlans[indx].NetValuePf;
                                CapPlans[indx].NetValuePf = 0;
                            }
                            else
                            {
                                CapPlans[indx].NetValuePf -= weiLocal;
                                weiLocal = 0;
                                break;
                            }
                        }
                        else
                            break;
                    } while (weiLocal > 0);
                }
            }
        }

        public static int chekStatCoilsCapDay(List<int> lstPfLoc, List<int> lstPfLoc1, ref DateTime datePlanLocref, List<Coil> CoilsMain,
                        List<Coil> CoilsRestart,  ref bool chekFinishCapplan, Solution currSolution, List<CapPlan> CapPlansCurr,
                        DateTime maxDate, List<Coil> CoilsTemDelete, List<Coil> CoilsCapDay, int numZone)
        {

            DateTime datePlanLoc = datePlanLocref;
            List<Coil> lstLoc = CoilsMain.Where(a => !CoilsRestart.Contains(a)).ToList();

            if (lstLoc.Count == 0)
            {
                chekFinishCapplan = true;

                return 1;
                //break;
            }

            if (CoilsMain.Count == 0)
            {
                chekFinishCapplan = true;
                currSolution.LstChekCap.Add(chekFinishCapplan);

                return 1;
                //break;
            }

            if (RunInformation.FlgUser == -1)
            {
                List<CapPlan> lstCap = CapPlansCurr.Where(c => c.MaxValueRespond > Parameter.EpsilonCapPlan
                                            && c.DatePlan.Date == Status.CurrTime.Date).ToList();
                if (lstCap.Count == 0)
                {
                    chekFinishCapplan = true;
                    currSolution.LstChekCap.Add(chekFinishCapplan);

                    return 1;
                    //break;
                }

                lstPfLoc.Clear();
                lstPfLoc1.Clear();
                lstPfLoc1 = CapPlansCurr.Where(a => a.NetValuePf > 0 && a.DatePlan.Date <=  datePlanLoc).Select(a => a.PfId).Distinct().ToList();
                lstPfLoc.AddRange(lstCap.Where(c => lstPfLoc1.Contains(c.PfId)
                                                    && CoilsMain.Select(a => a.PfId).Contains(c.PfId)).Select(a => a.PfId).ToList());

                lstPfLoc = lstPfLoc.Distinct().ToList();

                if (lstPfLoc.Count() == 0)
                {
                    if (datePlanLoc == maxDate && (currSolution.LstSeqCoil.Count() == 0
                        || (currSolution.LstZoneCoil.Count != 0 && currSolution.LstZoneCoil.Last() != numZone)))
                    {
                        chekFinishCapplan = true;
                        currSolution.LstChekCap.Add(chekFinishCapplan);

                        return 1;
                        // break;
                    }
                    else
                        return -1;
                    //continue;

                }



                if (CoilsMain.Count() == CoilsTemDelete.Count)
                {

                    chekFinishCapplan = false;
                    currSolution.LstChekCap.Add(chekFinishCapplan);

                    return 1;
                    // break;
                }

                else
                {
                    int countRestart = CoilsMain.Count() - lstLoc.Count;
                    if (CoilsMain.Count() == CoilsTemDelete.Count + countRestart)
                    {
                        chekFinishCapplan = true;

                        return 1;
                        // break;
                    }
                }


                CoilsCapDay.Clear();
                CoilsCapDay.AddRange(CoilsMain.Where(b => lstPfLoc.Contains(b.PfId) == true &&
                                                            CoilsTemDelete.Contains(b) == false
                                                            ));

                if (CoilsCapDay.Count() == 0)
                {
                    if (datePlanLoc == maxDate && currSolution.LstSeqCoil.Count() == 0)
                    {
                        chekFinishCapplan = true;
                        currSolution.LstChekCap.Add(chekFinishCapplan);

                        return 1;
                        //break;
                    }
                    else
                        return -1;
                    //  continue;

                }

                else
                    return 0;
            }

            else
            {


                CoilsCapDay.AddRange(CoilsMain.Where(b => CoilsTemDelete.Contains(b) == false ));

                if (CoilsCapDay.Count() == 0)
                    return 1;
                //break;
                else
                {
                    datePlanLocref = InnerParameter.maxDate.Date;
                    return 0;
                }
            }
        }
    }
}
