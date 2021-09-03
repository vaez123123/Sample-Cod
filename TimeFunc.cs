using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.CommonFunctions.ParameterClasses;

namespace IPSO.CMP.CommonFunctions.Functions
{
    public class TimeFunc
    {

        public static void chekTime(int seq, List<Solution> SolutionsOutputPlan, List<ReleaseSched> ReleaseScheds, List<StationStop> StationStops,
           List<Scheduling> Schedulings, List<ShiftWork> ShiftWorks, List<CapPlan> CapPlans, List<MaxValueGroup> MaxValueGroups, List<Coil> Coils,
            List<CoilRelease> CoilReleases, List<int> lstAvailMaxValueGroup, Solution currSolution, List<Setup> Setups)
        {
            int seqSolutions;
            if (seq == -1)
                seqSolutions = SolutionsOutputPlan.Count - 1;
            else//seq==-3 ya -4
                seqSolutions = SolutionsOutputPlan.Count - 2;
            if (seq != -3 && seq != -4)
            {
                if (seq > -1 && ReleaseScheds[seq].StatSched != 2)
                    chekStopStationStat(seq, StationStops, ReleaseScheds,SolutionsOutputPlan,Schedulings, ShiftWorks);  //توقفات را بررسی کن      

                if (seq >= -1)
                {
                    CapPlan.updateCapPlan(seq, seqSolutions, SolutionsOutputPlan, CapPlans, ReleaseScheds,MaxValueGroups, Coils, CoilReleases, lstAvailMaxValueGroup);
                    calcuTimeProg(seq, seqSolutions, SolutionsOutputPlan, ReleaseScheds,Coils,CoilReleases, Schedulings,ShiftWorks, currSolution, Setups);

                }

                //ترتیب

                chekStopStationStat(seq, StationStops, ReleaseScheds, SolutionsOutputPlan, Schedulings, ShiftWorks);  //توقفات را بررسی کن  

                //*
                CapPlan.updateCapPlanPositive(Schedulings, CapPlans);

                if (Schedulings.Count() != 0)
                {
                    if (chekChangeDay(Schedulings.Last(), Parameter.Epsilon) == true)
                        CapPlan.calcuMaxValueCapPlan(CapPlans);
                }
             
                CapPlan.updatePfAvail(InnerParameter.lstPfAvail, CapPlans);
                //*
            }
            else//seq == -3 ya -4
            {
                CapPlan.updateCapPlan(seq, seqSolutions, SolutionsOutputPlan, CapPlans, ReleaseScheds, MaxValueGroups, Coils, CoilReleases,
                    lstAvailMaxValueGroup);
                TimeFunc.calcuTimeProg(seq, seqSolutions, SolutionsOutputPlan, ReleaseScheds,Coils,CoilReleases,Schedulings,ShiftWorks, currSolution, Setups);
            }
        }


        //محاسبه زمان برنامه
        public static void calcuTimeProg(int seqPlanLoc, int seqSolutions, List<Solution> SolutionsOutputPlan, List<ReleaseSched> ReleaseScheds, List<Coil> Coils,
           List<CoilRelease> CoilReleases, List<Scheduling> Schedulings, List<ShiftWork> ShiftWorks,
           Solution currSolution, List<Setup> Setups)
        {
            TimeSpan setupTime = TimeSpan.Zero;
            TimeSpan time = TimeSpan.Zero;

            if (seqPlanLoc == -1)
            {
                int idEfrazLocal = SolutionsOutputPlan[seqSolutions].IdEfraz;
                setupTime = calcuSetupTime(idEfrazLocal, Schedulings, Setups);//زمان آماده سازی

                SolutionsOutputPlan[seqSolutions].StartTimeSelectProg = Status.CurrTime + setupTime;

                time = TimeSpan.Zero;

                foreach (int i in SolutionsOutputPlan[seqSolutions].LstSeqCoil)
                {

                    Coils[i].StartProduceTime = SolutionsOutputPlan[seqSolutions].StartTimeSelectProg + time;
                    time += Coils[i].DuProcessTime;
                    Coils[i].EndProduceTime = SolutionsOutputPlan[seqSolutions].StartTimeSelectProg + time;
                }

                SolutionsOutputPlan[seqSolutions].EndTimeSelectProg = SolutionsOutputPlan[seqSolutions].StartTimeSelectProg + time;
                SolutionsOutputPlan[seqSolutions].TotalTimeProg = SolutionsOutputPlan[seqSolutions].EndTimeSelectProg
                                                                      - SolutionsOutputPlan[seqSolutions].StartTimeSelectProg;

                Scheduling.addLstScheduling(2, -1, setupTime, ReleaseScheds, SolutionsOutputPlan, Schedulings, ShiftWorks);
            }

            else if (seqPlanLoc == -2)
            {

                int idEfrazLocal = currSolution.IdEfraz;
                setupTime = calcuSetupTime(idEfrazLocal, Schedulings, Setups);//زمان آماده سازی


                currSolution.StartTimeSelectProg = Status.CurrTime + setupTime;

                time = TimeSpan.Zero;

                foreach (int i in currSolution.LstSeqCoil)
                {
                    time += Coils[i].DuProcessTime;
                }

                currSolution.EndTimeSelectProg = currSolution.StartTimeSelectProg + time;
                currSolution.TotalTimeProg = currSolution.EndTimeSelectProg - currSolution.StartTimeSelectProg;
            }
            else if (seqPlanLoc == -3)
            {
                time = TimeSpan.Zero;
                int i = SolutionsOutputPlan[seqSolutions].LstSeqCoil.Last();
                Coils[i].StartProduceTime = SolutionsOutputPlan[seqSolutions].EndTimeSelectProg;
                time = Coils[i].DuProcessTime;
                Coils[i].EndProduceTime = SolutionsOutputPlan[seqSolutions].EndTimeSelectProg + time;

                SolutionsOutputPlan[seqSolutions].EndTimeSelectProg = SolutionsOutputPlan[seqSolutions].EndTimeSelectProg + time;
                
                SolutionsOutputPlan[seqSolutions].TotalTimeProg = SolutionsOutputPlan[seqSolutions].EndTimeSelectProg
                                                                      - SolutionsOutputPlan[seqSolutions].StartTimeSelectProg;

                Scheduling.addLstScheduling(7, -3, time, ReleaseScheds, SolutionsOutputPlan, Schedulings, ShiftWorks);
            }
            else if (seqPlanLoc == -4)
            {
                time = TimeSpan.Zero;
                int i = SolutionsOutputPlan[seqSolutions].LstSeqCoil.Last();

                Coils[i].StartProduceTime = SolutionsOutputPlan[seqSolutions-1].EndTimeSelectProg;
                SolutionsOutputPlan[seqSolutions].StartTimeSelectProg = Coils[i].StartProduceTime;

                time = Coils[i].DuProcessTime;
                Coils[i].EndProduceTime = SolutionsOutputPlan[seqSolutions].StartTimeSelectProg + time;

                SolutionsOutputPlan[seqSolutions].EndTimeSelectProg = SolutionsOutputPlan[seqSolutions].StartTimeSelectProg + time;

                SolutionsOutputPlan[seqSolutions].TotalTimeProg = SolutionsOutputPlan[seqSolutions].EndTimeSelectProg
                                                                      - SolutionsOutputPlan[seqSolutions].StartTimeSelectProg;

                Scheduling.addLstScheduling(8, -4, setupTime, ReleaseScheds, SolutionsOutputPlan, Schedulings, ShiftWorks);
            }

            else //release
            {

                int idEfrazLocal = ReleaseScheds[seqPlanLoc].IdEfraz;
                setupTime = calcuSetupTime(idEfrazLocal, Schedulings, Setups);//زمان آماده سازی


                ReleaseScheds[seqPlanLoc].StartTimeSelectProg = Status.CurrTime + setupTime;

                time = TimeSpan.Zero;


                double weiLoc = 0;

                foreach (int i in ReleaseScheds[seqPlanLoc].LstSeqCoil)
                {

                    CoilReleases[i].StartProduceTime = ReleaseScheds[seqPlanLoc].StartTimeSelectProg + time;
                    time += CoilReleases[i].DuProcessTime;
                    CoilReleases[i].EndProduceTime = ReleaseScheds[seqPlanLoc].StartTimeSelectProg + time;

                    weiLoc += CoilReleases[i].Weight;
                }

                ReleaseScheds[seqPlanLoc].EndTimeSelectProg = ReleaseScheds[seqPlanLoc].StartTimeSelectProg + time;
                ReleaseScheds[seqPlanLoc].TotalTimeProg = ReleaseScheds[seqPlanLoc].EndTimeSelectProg - ReleaseScheds[seqPlanLoc].StartTimeSelectProg;

                ReleaseScheds[seqPlanLoc].WeiSched = weiLoc;

                Scheduling.addLstScheduling(1, seqPlanLoc, setupTime, ReleaseScheds, SolutionsOutputPlan, Schedulings, ShiftWorks);

            }

        }


          public static TimeSpan calcuSetupTime(int currProgLocal, List<Scheduling> Schedulings, List<Setup> Setups)
        {
            TimeSpan timeSetupLocal;
            if (Schedulings.Count != 0)
            {
                int beforProgLocal = Schedulings.Last().Id;
                timeSetupLocal = Setups.Find(c => c.IdProgFrom == beforProgLocal && c.IdProgTo == currProgLocal).DurTimeSetup;
            }
            else
                timeSetupLocal = TimeSpan.Zero;

            return timeSetupLocal;
        }

        //بررسی زمان توفقات
        public static void chekStopStationStat(int seq, List<StationStop> StationStops, List<ReleaseSched> ReleaseScheds,
            List<Solution> SolutionsOutputPlan, List<Scheduling> Schedulings, List<ShiftWork> ShiftWorks)
        {
            TimeSpan dur;
            if (seq != -2)
            {
                int indx = StationStops.FindIndex(c => c.DateStop.Date == Status.CurrTime.Date && c.FlgShutDownInactive == 1 && c.DurStopRemain != TimeSpan.Zero);
                if (indx != -1)
                {
                    if (ShiftWorks[Status.IndexShift].FlgShutDown == 1)
                    {
                        dur = StationStops[indx].DurStopRemain;
                        Scheduling.addLstScheduling(3, -1, dur, ReleaseScheds, SolutionsOutputPlan, Schedulings, ShiftWorks);
                        StationStops[indx].DurStopRemain = TimeSpan.Zero;
                        int indx1 = StationStops.FindIndex(c => c.DateStop.Date == Status.CurrTime.Date && c.FlgShutDownInactive == 2 && c.DurStopRemain != TimeSpan.Zero);
                        if (indx1 != -1)
                        {
                            dur = StationStops[indx1].DurStopRemain;
                            Scheduling.addLstScheduling(4, -1, dur, ReleaseScheds, SolutionsOutputPlan, Schedulings, ShiftWorks);
                            StationStops[indx1].DurStopRemain = TimeSpan.Zero;
                        }
                    }
                }
                else
                {
                    int indx1 = StationStops.FindIndex(c => c.DateStop.Date == Status.CurrTime.Date && c.FlgShutDownInactive == 2 && c.DurStopRemain != TimeSpan.Zero);
                    if (indx1 != -1)
                    {
                        dur = StationStops[indx1].DurStopRemain;
                        Scheduling.addLstScheduling(4, -1, dur, ReleaseScheds, SolutionsOutputPlan, Schedulings, ShiftWorks);
                        StationStops[indx1].DurStopRemain = TimeSpan.Zero;
                    }
                }
            }
            else
            {
                dur = Status.CurrTime.Date.AddDays(1) - Status.CurrTime;
                if (dur != TimeSpan.Zero)
                    Scheduling.addLstScheduling(6, -2, dur, ReleaseScheds, SolutionsOutputPlan, Schedulings, ShiftWorks);
            }
        }




        //ALL
        //true = day changed
        public static bool chekChangeDay(Scheduling schedule, double epsilon)
        {
            if (schedule.Start.Date != schedule.End.Date)
                return true;

            else
            {
                if ((schedule.End.Date.AddDays(1) - schedule.End).Hours <= epsilon)
                    return true;
                else
                    return false;
            }
        }

    }
}
