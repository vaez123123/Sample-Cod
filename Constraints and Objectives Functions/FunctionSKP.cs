using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections;
using System.Xml;
using System.Data;
using IPSO.CMP.CommonFunctions.ParameterClasses;
using IPSO.ParameterClasses;
using IPSO.CMP.Log;
using IPSO.CMP.CommonFunctions.Functions;
using IPSO.Functions;

namespace SKPScheduling
{
    public class FunctionSKP : FunctionL2
    {

      //SKP1
        public override void fillMainList(int indexSarfaslLocal, int idEfrazLocal, CommonLists Lst)
        {


            List<Coil> lstCoilLocal;
            Lst.CoilsMain.Clear();
            Lst.CoilsTemDelete.Clear();
            Lst.CoilsMainCopy.Clear();


            // اگر نوع برنامه حساس  باشد باید از کوچکترین عرض در کمپین استفاده نمود
            if (TanSkpTemParameter.lstNotSensitive.Contains(idEfrazLocal) != true)

                lstCoilLocal = Lst.Coils.Where(q => q.IdEfraz == idEfrazLocal &&
                                                 q.Width <= Status.MinWidCampain &&
                                                    q.FlagPlan == 1 &&
                                                    q.AvailTime <= Status.CurrTime
                                                     && InnerParameter.lstPfAvail.Contains(q.PfId)
                                                      && q.LstSarfaslGroup.Contains(indexSarfaslLocal)).ToList();
            // 

            //اگر نوع برنامه  حساس  نباشد

            else

                lstCoilLocal = Lst.Coils.Where(q => q.IdEfraz == idEfrazLocal &&
                                                    q.FlagPlan == 1 &&
                                                    q.AvailTime <= Status.CurrTime
                                                     && InnerParameter.lstPfAvail.Contains(q.PfId)
                                                     && q.LstSarfaslGroup.Contains(indexSarfaslLocal)).ToList();



            foreach (int i in Lst.lstAvailEquipGroupFailureTime)
            {
                lstCoilLocal.RemoveAll(c => c.LstEquipGroupFailureTime.Contains(i));
            }

            foreach (int j in Lst.lstAvailMaxValueGroup)
            {
                lstCoilLocal.RemoveAll(b => b.LstMaxValueGroup.Contains(j) != true);
            }


            lstCoilLocal = lstCoilLocal.Where(a => a.LstSarfaslGroup.Contains(indexSarfaslLocal) == true && a.FlagPlan == 1 && InnerParameter.lstPfAvail.Contains(a.PfId)).ToList();




            // اگر غلتک کاری تغییر نکرده است به اندازه پرش افزایشی عرض در کمپین می توان بالا رفت
            // اگر غلتک تغییر کرده باشد  با توجه به دستورات بالا( بعد از " ام بی") تمام کلاف ها انتخاب شده اند
            if (TanSkpTemParameter.changeRoll == false)
            {

                int idMis = Lst.ProgEfrazes.Find(c => c.IdEfraz == idEfrazLocal).CodProgMis;
                WidthJump.calcuJumpWidth(1, idMis, idEfrazLocal, Lst.WidthJumps);
                int widJump = InnerParameter.widJumpLocalOutAsc;


                lstCoilLocal = lstCoilLocal.Where(b => b.Width <= (Status.LastWid + widJump) && InnerParameter.lstPfAvail.Contains(b.PfId)).ToList();
            }




            lstCoilLocal = lstCoilLocal.Distinct().ToList();


            foreach (var a in lstCoilLocal)
            {

                Lst.CoilsMain.Add(a);
                Lst.CoilsMainCopy.Add(a);
            }
      
            
        }

        //SKP1
        public override void updateCurrStat(CommonLists Lst)
        {


            #region  if (lstOutputPlan.Count != 0)

            if (Lst.SolutionsOutputPlan.Count != 0)
            {


                Status.LastWid = Lst.Coils[Lst.SolutionsOutputPlan.Last().LstSeqCoil.Last()].Width;
                Status.LastTks = Lst.Coils[Lst.SolutionsOutputPlan.Last().LstSeqCoil.Last()].Tks;
                Status.LastTksOut = Lst.Coils[Lst.SolutionsOutputPlan.Last().LstSeqCoil.Last()].TksOutput;
                Status.IndexSarfasl = Lst.SolutionsOutputPlan.Last().IndexSarfasl;
                Status.IdEfraz = Lst.SolutionsOutputPlan.Last().IdEfraz;




                if (Status.MinWidCampain > Lst.Coils[Lst.SolutionsOutputPlan.Last().LstSeqCoil.Last()].Width)

                    Status.MinWidCampain = Lst.Coils[Lst.SolutionsOutputPlan.Last().LstSeqCoil.Last()].Width;


            }

            #endregion


            #region else (lstOutputPlan.Count = 0)

            else
            {
                int indexLocal;

                indexLocal = Lst.Schedulings.FindLastIndex(c => c.TypId == 1 || c.TypId == 2);

                if (indexLocal != -1)
                {
                    // اخرین ریلیز
                    int maxLocalSeq = Lst.ReleaseScheds.Max(n => n.SeqSched);

                    int indexMaxLocal = Lst.ReleaseScheds.FindIndex(e => e.SeqSched == maxLocalSeq);

                    Status.LastWid = Lst.CoilReleases[Lst.ReleaseScheds[indexMaxLocal].LstSeqCoil.Last()].Width;
                    Status.LastTks = Lst.CoilReleases[Lst.ReleaseScheds[indexMaxLocal].LstSeqCoil.Last()].Tks;
                    Status.LastTksOut = Lst.CoilReleases[Lst.ReleaseScheds[indexMaxLocal].LstSeqCoil.Last()].TksOutput;
                    Status.IndexSarfasl = Lst.ReleaseScheds[indexMaxLocal].LstIndexSarfasl.First();

                    Status.IdEfraz = Lst.ReleaseScheds[indexMaxLocal].IdEfraz;


                }

                // عرض اخر در صورتی که هیچ برنامه ای در دسترس نباشد
                else
                {

                    Status.LastWid = Lst.Coils.Max(c => c.Width);
                    Status.MinWidCampain = Lst.Coils.Max(c => c.Width);
                    Status.LastTks = Lst.Coils[Lst.Coils.Find(c => c.Width == Status.LastWid).ModelIndexCoil].Tks;
                    Status.LastTksOut = Lst.Coils[Lst.Coils.Find(c => c.Width == Status.LastWid).ModelIndexCoil].TksOutput;
                    Status.IndexSarfasl = 0;

                }


            }

            #endregion



        }

     
        public override void chekStatBeforAlgorithm(CommonLists Lst, ReleasePackage releaseSKP,
                                          GeneralFunc functionSKP, string pathWriter, int flgWriter)
        {

            base.chekStatBeforAlgorithm(Lst, releaseSKP, functionSKP, pathWriter, flgWriter);

            Status.lastWidStartAlgorithm = Status.LastWid;

            InnerParameter.initialValue(1, -1, 2, 3, 4, -1, -1, 1, 1, 1, -1, -1);


        }

        //public void fixSolutionAndUpdate(CommonLists Lst, FileLogger fileLogger)
        //{

        //    // جایگذاری بهترین جواب
        //    Solution.updatelstOutputPlan(Lst.bestSolution, Lst.SolutionsOutputPlan, fileLogger, Lst.Coils);



        //    string local = "local.txt";
        //    WriterSKP.writercurrProg(Lst.currSolution, local, false, Lst.Coils, Lst);



        //    TimeFunc.chekTime(-1, Lst.SolutionsOutputPlan, Lst.ReleaseScheds, Lst.StationStops, Lst.Schedulings, Lst.ShiftWorks,
        //            Lst.CapPlans, Lst.MaxValueGroups, Lst.Coils, Lst.CoilReleases, Lst.lstAvailMaxValueGroup, Lst.currSolution, Lst.Setups);

        //    //چک کردن  ورک رول ها
        //    RollSKP.chekWorkRoll(Lst);
        //    // چک کردن وضعیت کمپین 
        //    //  calcuWeiCampPlan(lstOutputPlan.Last());


        //    Solution.resetObjToZeroObj(Lst.bestSolution, Lst.CapPlans, Lst.CapPlansCurr, Lst.MaxValueGroups, Lst.CapPlanUpDates);// صفر کردن بهترین جواب  

        //    CapPlanFunc.calcuPffForPlans(-1, Lst.SolutionsOutputPlan, Lst.ReleaseScheds, WriterSKP.PathWriter, Lst.CapPlanUpDates, Lst.Coils, Lst.CoilReleases, WriterSKP.flgWriter);



        //    //در صفر کردن جواب( تابع بالایی) هست
        //    // updateLstCapPlanCurr();
        //    //*
        //    updateCurrStat(Lst);// تعیین وضعیت مینمم عرض کمپین و کلاف اخر و سرفصل

        //    WriterSKP.writerBestProg(Lst.SolutionsOutputPlan.Last(), "Main.txt", true, Lst.Coils, Lst);

        //    InnerParameter.countProgLocal = 0;
        //    InnerParameter.RuleBetweenProg = 1;
        //    RunInformation.chekFlagUser(Lst.Coils, Lst.SolutionsOutputPlan);

        //    ProgEfraz.chekFlgAvailForProgAfraz(Lst.ProgEfrazes, Lst.Coils, Lst.SolutionsOutputPlan.Count());

        //}

    }
}
