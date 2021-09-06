using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.Log;

namespace IPSO.CMP.CommonFunctions.ParameterClasses
{
    public class Solution
    {

        #region
        public int IdEfraz;// idAfraz
        public int IndexSarfasl;//index sarfasl class
        public int IndexCamp; // indexlstBackRoll 
        //Pic
        public int IndexShift;
        //Tan-SRM-Tem
        public int IndexWorkRoll { get; set; }//index list Workroll


        // شماره برنامه فیکس شده از برنامه های لوکال ساخته شده
        // اگر یک عدد منفی خیلی بزرگ باشد یعنی در بهبود ساخته شده است
        public int CountFixProg;
        public double WeiProg; //وزن برنامه
        public double LenProg; // کیلومتر برنامه          

        public DateTime EndTimeSelectProg;
        public DateTime StartTimeSelectProg;
        public TimeSpan TotalTimeProg;

        public int FlagUnableInsertCoil;
        public int FlagUnableInsertCoilNew;
        //1 if prog build in improvement , 0 else(user and algorithm)
        public int FlagImproveProg;

        public List<int> LstSeqCoil = new List<int>();
        public List<bool> LstChekCap = new List<bool>();


        #region obj

        //******************normal obj **********

        public double ObjNormalDatlast;
        public double ObjNormalPrior;
        public double ObjNormalDurability;
        public double ObjNormalCapPlan;
        public double ObjNormalSetupCost;
        public double ObjNormalCountProg;
        public double ObjNormalSarfasl;
        public double ObjNormalLevelStorge;
        public double ObjNormalPriorGroupTypeMis;
        //**felan estefade nemishavad
        public double ObjNormalLenProg;
        public double ObjNormalWeiProg;
        //**
        //Tan-SRM
        public double ObjNormalLenWorkRoll;// 
        //Tan-SRM
        public double ObjNormalWeiWorkRoll;//
        //Pic
        public double ObjNormalStartCamp;
        //Pic
        public double ObjNormalContinuePattern;
        //Pic
        public double ObjNormalPriorStation;
        //SRM
        //public double ObjNormalContinueTypeProg;




        public double TotalObj;// کل تابع هدف برنامه 
        public double ObjDatlast;
        public double ObjPrior;
        public double ObjDurability;
        public double ObjCapPlan;
        public double ObjSetupCost;
        public double ObjCountProg;
        public double ObjSarfasl;
        public double ObjLevelStorge;
        public double ObjPriorGroupTypeMis;
        //**felan estefade nemishavad
        public double ObjLenProg;
        public double ObjWeiProg;
        //**
        //Tan-SRM
        public double ObjLenWorkRoll;// 
        //Tan-SRM
        public double ObjWeiWorkRoll;//
        //Pic
        public double ObjStartCamp;
        //Pic
        public double ObjContinuePattern;
        //Pic
        public double ObjPriorStation;
        //SRM
        //public double ObjContinueTypeProg;

        #endregion

        //Pic-SRM
        public List<int> LstZoneCoil = new List<int>();//MET
        //Pic
        public int NumChangeWid;
        //Tan-SRM-Tem-skp1
        public bool ChangWorkRoll;// // true change  fals no change;
        //Tan
        public int DoubleWorkBefor;
        //Tan-SRM
        public int ChekChangeWorkRollBefor;
        //Tan
        public int DoubleWorkAfter;
        //Tan
        public int ChekChangeWorkRollAfter;
        //Tan
        public int MaxWidSarbarn;
        //Tan
        public bool FlagChangeSarfaslFinal;// به منظور چک کردن تغیر سرفصل  بعد از ساخت برنامه (سرفصل داخلی ) 
        // true == change
        //Tan
        public Boolean FlagChangeStat;//  به منظور چک کردن  تغییر  وضعیت غلتک های کاری قبل و بعد از ساخت برنامه

        //Tem-skp1
        public int ChekChangeWorkRoll;

        //vaez-baraye chap javab
        public int countOutPutSolution;
        public int countProgLocalSolution;
        public int countBigProgSolution;
        public int countCurrProgSolution;

        
        //1 = capplan not allow all remain coils
        //2 = All coil in "CoilsMainCopy" are schedule
        //3 = reach to the max number of the Prog
        //4 = finish roll capacity
        //5 = jump not allowed to schedule
        //6 = access to maximum roll capacity according to the optimal value
        public int flgchekNotSequence;
        #endregion


        public static void sumWeiLenProg(int modelIndexCoil, Solution solutionLocal, List<Coil> Coils)
        {
            solutionLocal.LenProg += Coils[modelIndexCoil].LenKmOutput;
            solutionLocal.WeiProg += Coils[modelIndexCoil].Weight;
        }

       
        public static void replaceSolutions(bool changeRoll, Solution newSolution, Solution oldSolution)
        {
            newSolution.ChangWorkRoll = changeRoll;
            newSolution.ChekChangeWorkRoll = oldSolution.ChekChangeWorkRoll;
            newSolution.ChekChangeWorkRollAfter = oldSolution.ChekChangeWorkRollAfter;
            newSolution.ChekChangeWorkRollBefor = oldSolution.ChekChangeWorkRollBefor;
            newSolution.CountFixProg = oldSolution.CountFixProg;
            newSolution.DoubleWorkAfter = oldSolution.DoubleWorkAfter;
            newSolution.DoubleWorkBefor = oldSolution.DoubleWorkBefor;
            newSolution.FlagChangeSarfaslFinal = oldSolution.FlagChangeSarfaslFinal;
            newSolution.FlagChangeStat = oldSolution.FlagChangeStat;
            newSolution.FlagUnableInsertCoil = oldSolution.FlagUnableInsertCoil;
            newSolution.FlagUnableInsertCoilNew = oldSolution.FlagUnableInsertCoilNew;
            newSolution.IdEfraz = oldSolution.IdEfraz;
            newSolution.IndexCamp = oldSolution.IndexCamp;
            newSolution.IndexSarfasl = oldSolution.IndexSarfasl;
            newSolution.IndexWorkRoll = oldSolution.IndexWorkRoll;
            newSolution.LenProg = oldSolution.LenProg;
            newSolution.MaxWidSarbarn = oldSolution.MaxWidSarbarn;
            newSolution.NumChangeWid = oldSolution.NumChangeWid;

            newSolution.ObjNormalCapPlan = oldSolution.ObjNormalCapPlan;
            newSolution.ObjNormalContinuePattern = oldSolution.ObjNormalContinuePattern;
            //newSolution.ObjNormalContinueTypeProg = oldSolution.ObjNormalContinueTypeProg;
            newSolution.ObjNormalCountProg = oldSolution.ObjNormalCountProg;
            newSolution.ObjNormalDatlast = oldSolution.ObjNormalDatlast;
            newSolution.ObjNormalDurability = oldSolution.ObjNormalDurability;
            newSolution.ObjNormalLenProg = oldSolution.ObjNormalLenProg;
            newSolution.ObjNormalLenWorkRoll = oldSolution.ObjNormalLenWorkRoll;
            newSolution.ObjNormalLevelStorge = oldSolution.ObjNormalLevelStorge;
            newSolution.ObjNormalPrior = oldSolution.ObjNormalPrior;
            newSolution.ObjNormalPriorGroupTypeMis = oldSolution.ObjNormalPriorGroupTypeMis;
            newSolution.ObjNormalPriorStation = oldSolution.ObjNormalPriorStation;
            newSolution.ObjNormalSarfasl = oldSolution.ObjNormalSarfasl;
            newSolution.ObjNormalSetupCost = oldSolution.ObjNormalSetupCost;
            newSolution.ObjNormalStartCamp = oldSolution.ObjNormalStartCamp;
            newSolution.ObjNormalWeiProg = oldSolution.ObjNormalWeiProg;
            newSolution.ObjNormalWeiWorkRoll = oldSolution.ObjNormalWeiWorkRoll;


            newSolution.ObjCapPlan = oldSolution.ObjCapPlan;
            newSolution.ObjContinuePattern = oldSolution.ObjContinuePattern;
            //newSolution.ObjContinueTypeProg = oldSolution.ObjContinueTypeProg;
            newSolution.ObjCountProg = oldSolution.ObjCountProg;
            newSolution.ObjDatlast = oldSolution.ObjDatlast;
            newSolution.ObjDurability = oldSolution.ObjDurability;
            newSolution.ObjLenProg = oldSolution.ObjLenProg;
            newSolution.ObjLenWorkRoll = oldSolution.ObjLenWorkRoll;
            newSolution.ObjLevelStorge = oldSolution.ObjLevelStorge;
            newSolution.ObjPrior = oldSolution.ObjPrior;
            newSolution.ObjPriorGroupTypeMis = oldSolution.ObjPriorGroupTypeMis;
            newSolution.ObjPriorStation = oldSolution.ObjPriorStation;
            newSolution.ObjSarfasl = oldSolution.ObjSarfasl;
            newSolution.ObjSetupCost = oldSolution.ObjSetupCost;
            newSolution.ObjStartCamp = oldSolution.ObjStartCamp;
            newSolution.ObjWeiProg = oldSolution.ObjWeiProg;
            newSolution.ObjWeiWorkRoll = oldSolution.ObjWeiWorkRoll;
            newSolution.TotalObj = oldSolution.TotalObj;
            newSolution.WeiProg = oldSolution.WeiProg;
            newSolution.LstChekCap.Clear();
            newSolution.LstSeqCoil.Clear();
            newSolution.LstZoneCoil.Clear();
            newSolution.LstChekCap.AddRange(oldSolution.LstChekCap);
            newSolution.LstSeqCoil.AddRange(oldSolution.LstSeqCoil);
            newSolution.LstZoneCoil.AddRange(oldSolution.LstZoneCoil);


        }

        // جایگذاری بهترین جواب و منفی کردن فلگ پلن برای کلافهای برنامه
        public static void updatelstOutputPlan(Solution besSolutionlocal, List<Solution> SolutionsOutputPlan, FileLogger fileLogger, List<Coil> Coils)
        {
            if (besSolutionlocal.LstSeqCoil.Count != 0)
            {
                InnerParameter.lstCountProgLocal.Add(InnerParameter.countBestProg);

                Solution b = new Solution();
                SolutionsOutputPlan.Add(b);

                string message ="Prog is fixed"+InnerParameter.symbole+InnerParameter.tab+ SolutionsOutputPlan.Count() + InnerParameter.tab  
                                 + "LstSeqCoil.Count " + InnerParameter.symbole + InnerParameter.tab + besSolutionlocal.LstSeqCoil.Count + InnerParameter.tab
                                 + InnerParameter.idEfraz + InnerParameter.symbole + InnerParameter.tab + besSolutionlocal.IdEfraz + InnerParameter.tab
                                 + InnerParameter.idSarfasl + InnerParameter.symbole + InnerParameter.tab + besSolutionlocal.IndexSarfasl + InnerParameter.tab
                                 + InnerParameter.countLocalSolution + InnerParameter.symbole + InnerParameter.tab + InnerParameter.lstCountProgLocal.Last();

                fileLogger.Log(message,4);

                SolutionsOutputPlan.Last().CountFixProg = InnerParameter.lstCountProgLocal.Last();
                SolutionsOutputPlan.Last().FlagUnableInsertCoil = InnerParameter.RuleBetweenProg;
                SolutionsOutputPlan.Last().FlagUnableInsertCoilNew = InnerParameter.RuleBetweenProg;

                SolutionsOutputPlan.Last().ChangWorkRoll = besSolutionlocal.ChangWorkRoll;
                SolutionsOutputPlan.Last().ChekChangeWorkRoll = besSolutionlocal.ChekChangeWorkRoll;
                SolutionsOutputPlan.Last().ChekChangeWorkRollAfter = besSolutionlocal.ChekChangeWorkRollAfter;
                SolutionsOutputPlan.Last().ChekChangeWorkRollBefor = besSolutionlocal.ChekChangeWorkRollBefor;
                SolutionsOutputPlan.Last().DoubleWorkAfter = besSolutionlocal.DoubleWorkAfter;
                SolutionsOutputPlan.Last().DoubleWorkBefor = besSolutionlocal.DoubleWorkBefor;
                SolutionsOutputPlan.Last().EndTimeSelectProg = besSolutionlocal.EndTimeSelectProg;
                SolutionsOutputPlan.Last().FlagChangeSarfaslFinal = besSolutionlocal.FlagChangeSarfaslFinal;
                SolutionsOutputPlan.Last().FlagChangeStat = besSolutionlocal.FlagChangeStat;
                SolutionsOutputPlan.Last().IdEfraz = besSolutionlocal.IdEfraz;
                SolutionsOutputPlan.Last().IndexCamp = besSolutionlocal.IndexCamp;
                SolutionsOutputPlan.Last().IndexSarfasl = besSolutionlocal.IndexSarfasl;
                SolutionsOutputPlan.Last().IndexShift = besSolutionlocal.IndexShift;
                SolutionsOutputPlan.Last().IndexWorkRoll = besSolutionlocal.IndexWorkRoll;
                SolutionsOutputPlan.Last().LenProg = besSolutionlocal.LenProg;
                SolutionsOutputPlan.Last().MaxWidSarbarn = besSolutionlocal.MaxWidSarbarn;
                SolutionsOutputPlan.Last().NumChangeWid = besSolutionlocal.NumChangeWid;

                SolutionsOutputPlan.Last().ObjNormalCapPlan = besSolutionlocal.ObjNormalCapPlan;
                SolutionsOutputPlan.Last().ObjNormalContinuePattern = besSolutionlocal.ObjNormalContinuePattern;
                //SolutionsOutputPlan.Last().ObjNormalContinueTypeProg = besSolutionlocal.ObjNormalContinueTypeProg;
                SolutionsOutputPlan.Last().ObjNormalCountProg = besSolutionlocal.ObjNormalCountProg;
                SolutionsOutputPlan.Last().ObjNormalDatlast = besSolutionlocal.ObjNormalDatlast;
                SolutionsOutputPlan.Last().ObjNormalDurability = besSolutionlocal.ObjNormalDurability;
                SolutionsOutputPlan.Last().ObjNormalLenProg = besSolutionlocal.ObjNormalLenProg;
                SolutionsOutputPlan.Last().ObjNormalLenWorkRoll = besSolutionlocal.ObjNormalLenWorkRoll;
                SolutionsOutputPlan.Last().ObjNormalLevelStorge = besSolutionlocal.ObjNormalLevelStorge;
                SolutionsOutputPlan.Last().ObjNormalPrior = besSolutionlocal.ObjNormalPrior;
                SolutionsOutputPlan.Last().ObjNormalPriorGroupTypeMis = besSolutionlocal.ObjNormalPriorGroupTypeMis;
                SolutionsOutputPlan.Last().ObjNormalPriorStation = besSolutionlocal.ObjNormalPriorStation;
                SolutionsOutputPlan.Last().ObjNormalSarfasl = besSolutionlocal.ObjNormalSarfasl;
                SolutionsOutputPlan.Last().ObjNormalSetupCost = besSolutionlocal.ObjNormalSetupCost;
                SolutionsOutputPlan.Last().ObjNormalStartCamp = besSolutionlocal.ObjNormalStartCamp;
                SolutionsOutputPlan.Last().ObjNormalWeiProg = besSolutionlocal.ObjNormalWeiProg;
                SolutionsOutputPlan.Last().ObjNormalWeiWorkRoll = besSolutionlocal.ObjNormalWeiWorkRoll;


                SolutionsOutputPlan.Last().ObjCapPlan = besSolutionlocal.ObjCapPlan;
                SolutionsOutputPlan.Last().ObjContinuePattern = besSolutionlocal.ObjContinuePattern;
                //SolutionsOutputPlan.Last().ObjContinueTypeProg = besSolutionlocal.ObjContinueTypeProg;
                SolutionsOutputPlan.Last().ObjCountProg = besSolutionlocal.ObjCountProg;
                SolutionsOutputPlan.Last().ObjDatlast = besSolutionlocal.ObjDatlast;
                SolutionsOutputPlan.Last().ObjDurability = besSolutionlocal.ObjDurability;
                SolutionsOutputPlan.Last().ObjLenProg = besSolutionlocal.ObjLenProg;
                SolutionsOutputPlan.Last().ObjLenWorkRoll = besSolutionlocal.ObjLenWorkRoll;
                SolutionsOutputPlan.Last().ObjLevelStorge = besSolutionlocal.ObjLevelStorge;
                SolutionsOutputPlan.Last().ObjPrior = besSolutionlocal.ObjPrior;
                SolutionsOutputPlan.Last().ObjPriorGroupTypeMis = besSolutionlocal.ObjPriorGroupTypeMis;
                SolutionsOutputPlan.Last().ObjPriorStation = besSolutionlocal.ObjPriorStation;
                SolutionsOutputPlan.Last().ObjSarfasl = besSolutionlocal.ObjSarfasl;
                SolutionsOutputPlan.Last().ObjSetupCost = besSolutionlocal.ObjSetupCost;
                SolutionsOutputPlan.Last().ObjStartCamp = besSolutionlocal.ObjStartCamp;
                SolutionsOutputPlan.Last().ObjWeiProg = besSolutionlocal.ObjWeiProg;
                SolutionsOutputPlan.Last().ObjWeiWorkRoll = besSolutionlocal.ObjWeiWorkRoll;
                SolutionsOutputPlan.Last().StartTimeSelectProg = besSolutionlocal.StartTimeSelectProg;
                SolutionsOutputPlan.Last().TotalObj = besSolutionlocal.TotalObj;
                SolutionsOutputPlan.Last().TotalTimeProg = besSolutionlocal.TotalTimeProg;
                SolutionsOutputPlan.Last().WeiProg = besSolutionlocal.WeiProg;

                SolutionsOutputPlan.Last().LstChekCap.Clear();
                SolutionsOutputPlan.Last().LstSeqCoil.Clear();
                SolutionsOutputPlan.Last().LstZoneCoil.Clear();

                SolutionsOutputPlan.Last().LstChekCap.AddRange(besSolutionlocal.LstChekCap);
                SolutionsOutputPlan.Last().LstSeqCoil.AddRange(besSolutionlocal.LstSeqCoil);
                SolutionsOutputPlan.Last().LstZoneCoil.AddRange(besSolutionlocal.LstZoneCoil);
                          
               
                Coil.flgCoilClass(SolutionsOutputPlan.Last(), Coils);
            }
        }

        public static void resetObjToZeroObj(Solution sol, List<CapPlan> CapPlans, List<CapPlan> CapPlansCurr, List<MaxValueGroup> MaxValueGroups
                                             , List<CapPlanUpDate> CapPlanUpDates)
        {
            CapPlan.updateLstCapPlanCurr(CapPlans, CapPlansCurr);
            MaxValueGroup.setZeroMaxValueGroup(MaxValueGroups);

            sol.ObjDatlast = 0;
            sol.ObjPrior = 0;
            sol.ObjDurability = 0;
            sol.ObjCapPlan = 0;
            sol.ObjSetupCost = 0;
            sol.ObjLenWorkRoll = 0;
            sol.ObjLenProg = 0;
            sol.ObjWeiProg = 0;
            sol.ObjCountProg = 0;
            sol.ObjLevelStorge = 0;
            sol.ObjWeiWorkRoll = 0;
            sol.ObjSarfasl = 0;
            sol.ObjContinuePattern = 0;
            //sol.ObjContinueTypeProg = 0;
            sol.ObjPriorGroupTypeMis = 0;
            sol.ObjPriorStation = 0;
            sol.ObjStartCamp = 0;
            sol.TotalObj = double.MaxValue;
            sol.LenProg = 0;
            sol.WeiProg = 0;
            sol.NumChangeWid = 0;
            sol.MaxWidSarbarn = 0;
            sol.flgchekNotSequence = 0;

            foreach (CapPlanUpDate i in CapPlanUpDates)
                i.RespondTemValueobj = 0;

            sol.LstSeqCoil.Clear();
            sol.LstZoneCoil.Clear();
            sol.LstChekCap.Clear();

        }

        public static void calcuObjCommon(int idEfrazLocal, Solution currSolution, CommonLists Lst)
        {
            string misLoc = Lst.ProgEfrazes.Find(a => a.IdEfraz == currSolution.IdEfraz).ProgMis;

            foreach (var select in currSolution.LstSeqCoil)
            {

                calcuObjDatLast(select, Lst.Coils, currSolution);
                calcuObjPriority(select, Lst.Coils, currSolution);
                calcuObjDurability(select, Lst.Coils, currSolution);
                calcuObjLevelStorge(select, currSolution, Lst.Coils);
                CapPlanUpDate.calcuRespondTemValueobj(select, Lst.Coils, Lst.CapPlanUpDates);
            }

            calcuobjCountProg(Lst.ProgEfrazes, currSolution);
            calcuObjSetupCost(idEfrazLocal, Lst.Schedulings, Lst.Setups, currSolution);
            calcuObjCapPlan(Lst.CapPlanUpDates, Lst.CapPlans, Lst.CapPlansCurr, currSolution);
            calcuObjSarfasl(Lst.DBDCampPlans, currSolution);
            calcuObjPriorMisGroup(misLoc, currSolution, Lst.Sarfasls, Lst.GroupDefs);

            currSolution.ObjLevelStorge = (currSolution.ObjLevelStorge / currSolution.LstSeqCoil.Count * DataBase.MaxLevelStorge);
            currSolution.ObjDurability = currSolution.ObjDurability / currSolution.LstSeqCoil.Count;
            currSolution.ObjDatlast = currSolution.ObjDatlast / currSolution.LstSeqCoil.Count;
            currSolution.ObjPrior = currSolution.ObjPrior / currSolution.LstSeqCoil.Count;

        }

        public static void calcuTotalObjCommon(Solution solution)
        {

            double totalWeight =WeightObjective.CapPlanCoef + WeightObjective.ContinuePatternCoef +
                                WeightObjective.CountProgCoef + WeightObjective.DatLastCoef +
                                WeightObjective.DurabilityCoef +WeightObjective.LevelStorgeCoef +
                                WeightObjective.LenProgCoef + WeightObjective.PriorityCoef +
                                WeightObjective.PriorStationCoef + WeightObjective.PriorGroupTypeMisCoef +
                                WeightObjective.SarfaslCoef + WeightObjective.SetupCoef + 
                                WeightObjective.StartCampCoef + WeightObjective.WeiProgCoef +
                                WeightObjective.WorkRollLenCoef +WeightObjective.WorkRollWeiCoef;


            
            solution.ObjNormalCapPlan = Math.Round(WeightObjective.CapPlanCoef * solution.ObjCapPlan / totalWeight,4)*100;
            solution.ObjNormalContinuePattern = Math.Round((WeightObjective.ContinuePatternCoef * solution.ObjContinuePattern) / totalWeight, 4)*100;
            solution.ObjNormalCountProg = Math.Round((WeightObjective.CountProgCoef * solution.ObjCountProg) / totalWeight, 4)*100;
            solution.ObjNormalDatlast = Math.Round((WeightObjective.DatLastCoef * solution.ObjDatlast) / totalWeight,4)*100;
            solution.ObjNormalDurability = Math.Round((WeightObjective.DurabilityCoef * solution.ObjDurability) / totalWeight, 4)*100;
            solution.ObjNormalLevelStorge = Math.Round((WeightObjective.LevelStorgeCoef * solution.ObjLevelStorge) / totalWeight, 4)*100;
            solution.ObjNormalLenProg = Math.Round((WeightObjective.LenProgCoef * solution.ObjLenProg) / totalWeight, 4)*100;
            solution.ObjNormalPrior = Math.Round((WeightObjective.PriorityCoef * solution.ObjPrior) / totalWeight, 4)*100;
            solution.ObjNormalPriorStation = Math.Round((WeightObjective.PriorStationCoef * solution.ObjPriorStation) / totalWeight, 4)*100;
            solution.ObjNormalPriorGroupTypeMis = Math.Round((WeightObjective.PriorGroupTypeMisCoef * solution.ObjPriorGroupTypeMis) / totalWeight, 4)*100;
            solution.ObjNormalSarfasl = Math.Round((WeightObjective.SarfaslCoef * solution.ObjSarfasl) / totalWeight, 4)*100;
            solution.ObjNormalSetupCost = Math.Round((WeightObjective.SetupCoef * solution.ObjSetupCost) / totalWeight, 4)*100;
            solution.ObjNormalStartCamp = Math.Round((WeightObjective.StartCampCoef * solution.ObjStartCamp) / totalWeight, 4)*100;
            solution.ObjNormalWeiProg = Math.Round((WeightObjective.WeiProgCoef * solution.ObjWeiProg) / totalWeight, 4)*100;
            solution.ObjNormalLenWorkRoll = Math.Round((WeightObjective.WorkRollLenCoef * solution.ObjLenWorkRoll) / totalWeight, 4)*100;
            solution.ObjNormalWeiWorkRoll = Math.Round((WeightObjective.WorkRollWeiCoef * solution.ObjWeiWorkRoll) / totalWeight, 4)*100;
            //solution.ObjNormalContinueTypeProg=Math.Round((WeightObjective.ContinueTypeProgCoef * solution.ObjContinueTypeProg)/ totalWeight,4)*100;


            solution.TotalObj = Math.Round(solution.ObjNormalCapPlan +
                                            solution.ObjNormalContinuePattern +
                                            solution.ObjNormalCountProg +
                                            solution.ObjNormalDatlast +
                                            solution.ObjNormalDurability +
                                            solution.ObjNormalLevelStorge +
                                            solution.ObjNormalLenProg +
                                            solution.ObjNormalPrior +
                                            solution.ObjNormalPriorStation +
                                            solution.ObjNormalPriorGroupTypeMis +
                                            solution.ObjNormalSarfasl +
                                            solution.ObjNormalSetupCost +
                                            solution.ObjNormalStartCamp +
                                            solution.ObjNormalWeiProg +
                                            solution.ObjNormalLenWorkRoll +
                                            solution.ObjNormalWeiWorkRoll 
                                            //solution.ObjNormalContinueTypeProg+
                                            , 4);


        }

        #region calcu obj

        private static void calcuObjSetupCost(int idEfrazLocal, List<Scheduling> Schedulings, List<Setup> Setups, Solution currSolution)
        {
            if (Schedulings.Count != 0 && DataBase.MaxSetupCost != 0)
            {
                double costSetup = Setups.Find(c => c.IdProgFrom == Schedulings.Last().Id && c.IdProgTo == idEfrazLocal).CostSetup;

                currSolution.ObjSetupCost += costSetup / DataBase.MaxSetupCost;
            }

            else
            {
                currSolution.ObjSetupCost += 0;
            }

        }

        private static void calcuObjCapPlan(List<CapPlanUpDate> CapPlanUpDates, List<CapPlan> CapPlans, List<CapPlan> CapPlansCurr,
            Solution currSolution)
        {
            int con = 0;
            foreach (CapPlanUpDate i in CapPlanUpDates)
            {

                double net = CapPlans.Find(c => c.PfId == i.PfId && c.DatePlan.Date == Status.CurrTime.Date).NetValuePf;
                double netCurr = CapPlansCurr.Find(c => c.PfId == i.PfId && c.DatePlan.Date == Status.CurrTime.Date).NetValuePf;
                double respondProg = i.RespondTemValueobj;


                if (respondProg > 0)
                    con++;

                double netFix = CapPlans.Find(c => c.PfId == i.PfId && c.DatePlan.Date == Status.CurrTime.Date).NetValueAfterChangeDay;
                double maxFix = CapPlans.Find(c => c.PfId == i.PfId && c.DatePlan.Date == Status.CurrTime.Date).MaxValueFixChangeDay;

                if (netCurr > 0 || respondProg == 0)
                    currSolution.ObjCapPlan += 0;

                else if (maxFix - netFix != 0)

                    currSolution.ObjCapPlan += (respondProg - net) / (maxFix - netFix);
                else
                    currSolution.ObjCapPlan += 0;



            }

            currSolution.ObjCapPlan = currSolution.ObjCapPlan / con;
        }

        private static void calcuObjDatLast(int modelIndexCoil, List<Coil> Coils, Solution currSolution)
        {

            if (DataBase.MaxDatLast.Date != DataBase.MinDatLast.Date)
            {
                double difDat = ((Status.CurrTime.Date - Coils[modelIndexCoil].DatLast.Date).Days);

                if (difDat >= DataBase.BoundDatlast)
                    currSolution.ObjDatlast += 0;
                else
                {
                    double difDat1 = ((DataBase.MaxDatLast.Date - Coils[modelIndexCoil].DatLast.Date).Days);
                    currSolution.ObjDatlast += Math.Round(1 - (difDat1 / ((DataBase.MaxDatLast.Date - DataBase.MinDatLast.Date).Days)), 4);
                }
            }

            else
                currSolution.ObjDatlast += 0;

        }

        private static void calcuObjDurability(int modelIndexCoil, List<Coil> Coils, Solution currSolution)
        {
            if (DataBase.MaxAvailTime.Date != DataBase.MinAvailTime.Date)
            {
                double difDur = ((Status.CurrTime.Date - Coils[modelIndexCoil].AvailTime.Date).Days);

                if (difDur >= DataBase.BoundDurability)
                    currSolution.ObjDurability += 0;
                else
                {
                    double difDur1 = ((DataBase.MaxAvailTime.Date - Coils[modelIndexCoil].AvailTime.Date).Days);
                    currSolution.ObjDurability += Math.Round(1 - (difDur1 / ((DataBase.MaxAvailTime.Date - DataBase.MinAvailTime.Date).Days)), 4);
                }
            }

            else
                currSolution.ObjDurability += 0;
        }

        private static void calcuObjPriority(int modelIndexCoil, List<Coil> Coils, Solution currSolution)
        {

            if (Coils[modelIndexCoil].Priority != 0)

                currSolution.ObjPrior += 1 - (Coils[modelIndexCoil].Priority / DataBase.MaxPriority);
            else
                currSolution.ObjPrior += 0;

        }

        private static void calcuObjLevelStorge(int modelIndexCoil, Solution currSolution, List<Coil> Coils)
        {
            currSolution.ObjLevelStorge += 1 - (Coils[modelIndexCoil].LevelStorge / DataBase.MaxLevelStorge);
        }

        private static void calcuobjCountProg(List<ProgEfraz> ProgEfrazes, Solution currSolution)
        {
            double countOptLoc = ProgEfrazes.Find(a => a.IdEfraz == currSolution.IdEfraz).CountOpt;
            double countMinLoc = ProgEfrazes.Find(a => a.IdEfraz == currSolution.IdEfraz).CountMin;

            if (countMinLoc <= currSolution.LstSeqCoil.Count() && currSolution.LstSeqCoil.Count() <= countOptLoc)

                currSolution.ObjCountProg = (countOptLoc - currSolution.LstSeqCoil.Count())
                    / (countOptLoc - countMinLoc);

            else// if (countMinLoc > currSolution.lstSeqCoil.Count())

                currSolution.ObjCountProg = 2 * (countOptLoc - currSolution.LstSeqCoil.Count())
                    / (countOptLoc - countMinLoc);
        }

        private static void calcuObjSarfasl(List<DBDCampPlan> DBDCampPlans, Solution currSolution)
        {
            if (DBDCampPlans.Count != 0)
            {

                int sarfaslDBDLocal = DBDCampPlans.Find(c => c.DatCamp.Date == Status.CurrTime.Date).CodCamp;

                int sarfaslCurrLocal = currSolution.IndexSarfasl;


                if (sarfaslCurrLocal == sarfaslDBDLocal)

                    currSolution.ObjSarfasl = 0;
                else
                    currSolution.ObjSarfasl = 1;
            }
            else
            {
                currSolution.ObjSarfasl = 0;
            }


        }

        private static void calcuObjPriorMisGroup(string misLoc, Solution currSolution, List<Sarfasl> Sarfasls, List<GroupDef> GroupDefs)
        {
            List<int> lstGroup = Sarfasls[currSolution.IndexSarfasl].LstGroupSarfasl.ToList();

            int indx = GroupDefs.FindIndex(a => lstGroup.Contains(a.IdGroup) && a.LstgroupMisProg.Contains(misLoc));
            if (indx != -1)
            {
                int priorLoc = GroupDefs[indx].PriorityGroup;
                int maxPriorLoc = GroupDefs.Max(a => a.PriorityGroup);

                currSolution.ObjPriorGroupTypeMis = priorLoc / maxPriorLoc;
            }
            else
                currSolution.ObjPriorGroupTypeMis = 0;
        }

        #endregion calcu obj

        //changeRoll:
        // if change roll does not exits is false 
        // else TanSkpTemParameter.changeRoll
        public static void updateBestsolution(CommonLists Lst, bool changeRoll)
        {
            
            if (RunInformation.NumStation != Station.TanStationId)
            {

                if (Lst.currSolution.LstSeqCoil.Count != 0)
                {
                    if (Lst.currSolution.TotalObj < Lst.bestSolution.TotalObj)
                    {

                        InnerParameter.countBestProg = InnerParameter.countCurrProg;

                        Solution.replaceSolutions(changeRoll, Lst.bestSolution, Lst.currSolution);

                    }
                }
            }

            else
            {
                #region tandem
                if (Lst.currSolution.LstSeqCoil.Count != 0)
                {

                    if (Lst.currSolution.IndexSarfasl != Lst.Sarfasls.Last().IndexSarfasl)
                    {
                        if (Lst.currSolution.TotalObj < Lst.bestSolution.TotalObj)
                        {
                            InnerParameter.countBestProg = InnerParameter.countCurrProg;
                            Solution.replaceSolutions(changeRoll, Lst.bestSolution, Lst.currSolution);

                        }
                    }

                    else
                    {
                        if (Lst.currSolution.MaxWidSarbarn > Lst.bestSolution.MaxWidSarbarn)
                        {
                            InnerParameter.countBestProg = InnerParameter.countCurrProg;
                            Solution.replaceSolutions(changeRoll, Lst.bestSolution, Lst.currSolution);

                        }
                        else if (Lst.currSolution.MaxWidSarbarn == Lst.bestSolution.MaxWidSarbarn
                            && Lst.currSolution.WeiProg > Lst.bestSolution.WeiProg)
                        {
                            InnerParameter.countBestProg = InnerParameter.countCurrProg;
                            Solution.replaceSolutions(changeRoll, Lst.bestSolution, Lst.currSolution);

                        }
                        else if (Lst.currSolution.MaxWidSarbarn == Lst.bestSolution.MaxWidSarbarn
                            && Lst.currSolution.WeiProg == Lst.bestSolution.WeiProg
                            && Lst.currSolution.TotalObj < Lst.bestSolution.TotalObj)
                        {
                            InnerParameter.countBestProg = InnerParameter.countCurrProg;
                            Solution.replaceSolutions(changeRoll, Lst.bestSolution, Lst.currSolution);

                        }


                    }
                }
                #endregion
            }

        }

    }
}
