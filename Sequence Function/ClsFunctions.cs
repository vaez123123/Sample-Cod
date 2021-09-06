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

namespace SkinPass1Scheduling
{
    public partial class SkinPass1Model
    {



        #region CalcuObjective




        // تابع محاسبه مقدار تابع هدف
        //ALL
        public void calcuTotalObj(Solution solu, int idEfrazLocal, int idSarfaslLocal)
        {

            calcuobjPriorMisGroup(idEfrazLocal, idSarfaslLocal);
       
            calcuObjRoll();// بین صفر و یک
         
            calcuObjChangSarfasl();

            Solution.calcuObjCommon(idEfrazLocal, solu, Lst.Coils, Lst.CapPlanUpDates, Lst.ProgEfrazes, Lst.Schedulings,
              Lst.Setups, Lst.CapPlans, Lst.CapPlansCurr, Lst.DBDCampPlans);

            Solution.calcuTotalObjCommon(solu);
          


        }

        //TEM-SKP1
        public void calcuObjRoll()
        {

            if (doubleChekWorkRoll == 1)
            {
                int chek = chekChangeWorkRoll(Lst.RollsWork);
                Lst.currSolution.ChekChangeWorkRoll = chek;
                // غیر سر فصل

                if (chek == 30 || chek == 31)
                {

                    if (Lst.RollsWork.Last().LenOpt != 0)
                    {
                        double dif = Lst.RollsWork.Last().LenOpt - Lst.RollsWork.Last().LenDB - Lst.RollsWork.Last().LenRelease -
                           Lst.RollsWork.Last().CurrentTotalFixLen - Lst.currSolution.LenProg;

                        if (dif > 0)
                            Lst.currSolution.ObjLenWorkRoll = (dif / Lst.RollsWork.Last().LenOpt);

                        else
                            Lst.currSolution.ObjLenWorkRoll = 0;
                    }

                    if (Lst.RollsWork.Last().WeiOpt != 0)
                    {
                        double dif = Lst.RollsWork.Last().WeiOpt - Lst.RollsWork.Last().WeiDB - Lst.RollsWork.Last().WeiRelease -
                           Lst.RollsWork.Last().CurrentTotalFixWei - Lst.currSolution.WeiProg;
                        if (dif > 0)
                            Lst.currSolution.ObjWeiWorkRoll = (dif / (Lst.RollsWork.Last().WeiOpt));

                        else
                            Lst.currSolution.ObjWeiWorkRoll = 0;
                    }
                }
                // سرفصل
                else
                {

                    if (Lst.RollsWork.Last().LenOpt != 0)
                    {
                        double objNew = 0;
                        if (Lst.RollsWork.Last().LenOpt - InnerParameter.lenTotal > 0)
                        {
                            objNew = ((Lst.RollsWork.Last().LenOpt - InnerParameter.lenTotal) / Lst.RollsWork.Last().LenOpt);
                        }

                        else
                            objNew = 0;


                        double obj = 0;
                        double dif = Lst.RollsWork.Last().LenOpt - Lst.RollsWork.Last().LenDB - Lst.RollsWork.Last().LenRelease -
                                       Lst.RollsWork.Last().CurrentTotalFixLen;

                        if (dif > 0)
                            obj = (dif / Lst.RollsWork.Last().LenOpt);

                        else
                            obj = 0;

                        Lst.currSolution.ObjLenWorkRoll = obj * DataBase.CoefObjWorkRoll + objNew * DataBase.CoefObjWorkRollNew;


                    }
                    if (Lst.RollsWork.Last().WeiOpt != 0)
                    {
                        double objNew = 0;
                        if (Lst.RollsWork.Last().WeiOpt -InnerParameter.weiTotal > 0)

                            objNew = ((Lst.RollsWork.Last().WeiOpt - InnerParameter.weiTotal) / (Lst.RollsWork.Last().WeiOpt));

                        else
                            objNew = 0;

                        double obj = 0;

                        double dif = Lst.RollsWork.Last().WeiOpt - Lst.RollsWork.Last().WeiDB - Lst.RollsWork.Last().WeiRelease -
                           Lst.RollsWork.Last().CurrentTotalFixWei;

                        if (dif > 0)
                            obj = (dif / (Lst.RollsWork.Last().WeiOpt));

                        else
                            obj = 0;

                        Lst.currSolution.ObjWeiWorkRoll = obj * DataBase.CoefObjWorkRoll + objNew * DataBase.CoefObjWorkRollNew; ;
                    }
                }
            }

                //
            else
            {
                int chek = chekChangeWorkRoll(Lst.RollsWork);
                Lst.currSolution.ChekChangeWorkRoll = chek;


                if (Lst.RollsWork.Last().LenOpt != 0)
                {
                    double objNew = 0;
                    if (Lst.RollsWork.Last().LenOpt - InnerParameter.lenTotal > 0)
                    {
                        objNew = ((Lst.RollsWork.Last().LenOpt -InnerParameter.lenTotal) / Lst.RollsWork.Last().LenOpt);
                    }

                    else
                        objNew = 0;


                    double obj = 0;
                    double dif = Lst.RollsWork.Last().LenOpt - Lst.RollsWork.Last().LenDB - Lst.RollsWork.Last().LenRelease -
                                   Lst.RollsWork.Last().CurrentTotalFixLen;

                    if (dif > 0)
                        obj = (dif / Lst.RollsWork.Last().LenOpt);

                    else
                        obj = 0;

                    Lst.currSolution.ObjLenWorkRoll = obj * DataBase.CoefObjWorkRoll + objNew * DataBase.CoefObjWorkRollNew;


                }
                if (Lst.RollsWork.Last().WeiOpt != 0)
                {
                    double objNew = 0;
                    if (Lst.RollsWork.Last().WeiOpt -InnerParameter.weiTotal > 0)

                        objNew = ((Lst.RollsWork.Last().WeiOpt - InnerParameter.weiTotal) / (Lst.RollsWork.Last().WeiOpt));

                    else
                        objNew = 0;

                    double obj = 0;

                    double dif = Lst.RollsWork.Last().WeiOpt - Lst.RollsWork.Last().WeiDB - Lst.RollsWork.Last().WeiRelease -
                       Lst.RollsWork.Last().CurrentTotalFixWei;

                    if (dif > 0)
                        obj = (dif / (Lst.RollsWork.Last().WeiOpt));

                    else
                        obj = 0;

                    Lst.currSolution.ObjWeiWorkRoll = obj * DataBase.CoefObjWorkRoll + objNew * DataBase.CoefObjWorkRollNew; ;
                }

            }




        }


  

        // TAN-SKP1
        public void calcuobjPriorMisGroup(int idprog, int indexSarfasl)
        {
            string prgMisLocal = Lst.ProgEfrazes.Find(c => c.IdEfraz == idprog).ProgMis;

            int index = Lst.Sarfasls[indexSarfasl].LstgroupMisProg.FindIndex(c => c.Contains(prgMisLocal) == true);
            if (index != -1)
            {
                double maxRankMis = Lst.Sarfasls[indexSarfasl].PriorityGroup.Max();
                Lst.currSolution.ObjPriorGroupTypeMis = Lst.Sarfasls[indexSarfasl].PriorityGroup[index] / maxRankMis;
            }
            else
                Lst.currSolution.ObjPriorGroupTypeMis = 0;


        }

        #endregion CalcuObjective


        #region Only One Time Run befor Run Algorithm


        //TAN-SKP1
        public void assignSarfaslGroup()
        {

            foreach (var i in Lst.Sarfasls)
            {

                List<Coil> sarfaslCoil = Lst.Coils.Where(a => a.Width <= i.WidTo && a.Width >= i.WidFrom).ToList();

                foreach (var j in sarfaslCoil)
                {
                    // در کلاس کویل

                    j.LstSarfaslGroup.Add(i.IndexSarfasl);

                    // در کلاس سرفصل 
                    i.LstCoilSarfasl.Add(j.ModelIndexCoil);

                }


            }



        }



        #endregion Only One Time Run in Algorithm  befor Run Algorithm



        #region//Methode sequencingProgram

     
      //SKP1
        public void fillMainList(int indexSarfaslLocal, int idProgLocal)
        {


            List<Coil> lstCoilLocal;
            Lst.CoilsMain.Clear();
            Lst.CoilsTemDelete.Clear();
            Lst.CoilsMainCopy.Clear();


            // اگر نوع برنامه حساس  باشد باید از کوچکترین عرض در کمپین استفاده نمود
            if (lstNotMbProg.Contains(idProgLocal) != true)

                lstCoilLocal = Lst.Coils.Where(q => q.IdEfraz == idProgLocal &&
                                                 q.Width <= Status.MinWidCampain &&
                                                    q.FlagPlan == 1 &&
                                                    q.AvailTime <= Status.CurrTime
                                                     && lstPfAvail.Contains(q.PfId)
                                                      && q.LstSarfaslGroup.Contains(indexSarfaslLocal)).ToList();
            // 

            //اگر نوع برنامه  حساس  نباشد

            else

                lstCoilLocal = Lst.Coils.Where(q => q.IdEfraz == idProgLocal &&
                                                    q.FlagPlan == 1 &&
                                                    q.AvailTime <= Status.CurrTime
                                                     && lstPfAvail.Contains(q.PfId)
                                                     && q.LstSarfaslGroup.Contains(indexSarfaslLocal)).ToList();



            foreach (int i in Lst.lstAvailEquipGroupFailureTime)
            {
                lstCoilLocal.RemoveAll(c => c.LstEquipGroupFailureTime.Contains(i));
            }

            foreach (int j in Lst.lstAvailMaxValueGroup)
            {
                lstCoilLocal.RemoveAll(b => b.LstMaxValueGroup.Contains(j) != true);
            }


            lstCoilLocal = lstCoilLocal.Where(a => a.LstSarfaslGroup.Contains(indexSarfaslLocal) == true && a.FlagPlan == 1 && lstPfAvail.Contains(a.PfId)).ToList();




            // اگر غلتک کاری تغییر نکرده است به اندازه پرش افزایشی عرض در کمپین می توان بالا رفت
            // اگر غلتک تغییر کرده باشد  با توجه به دستورات بالا( بعد از " ام بی") تمام کلاف ها انتخاب شده اند
            if (TanSkpTemParameter.changeRoll == false)

                lstCoilLocal = lstCoilLocal.Where(b => b.Width <= (Status.LastWid + DataBase.widJumpIncrease) && lstPfAvail.Contains(b.PfId)).ToList();




            lstCoilLocal = lstCoilLocal.Distinct().ToList();


            foreach (var a in lstCoilLocal)
            {

                Lst.CoilsMain.Add(a);
                Lst.CoilsMainCopy.Add(a);
            }
      
            
        }

       

     
        
        #endregion




        #region  Release

        //SKP1
        public void updateCurrStat()
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



                    //var query = from c1 in ReleaseScheds.Last().lstSeqCoil.AsEnumerable()
                    //            join c2 in CoilReleases.AsEnumerable()
                    //            on Convert.ToInt32(c1) equals c2.coilIndex
                    //            select new
                    //            {
                    //                c2.width,
                    //                c2.coilIndex
                    //            };


                    //var qq = (from c3 in query.AsEnumerable()
                    //          let mx = query.Min(q => q.width)
                    //          where c3.width == mx
                    //          select c3).FirstOrDefault();




                    //if (Status.MinWidCampain > CoilReleases[qq.coilIndex].width)

                    //    Status.MinWidCampain = CoilReleases[qq.coilIndex].width;




                }

                // عرض اخر در صورتی که هیچ برنامه ای در دسترس نباشد
                else
                {

                    Status.LastWid = Lst.Coils.Max(c => c.Width);

                    Status.MinWidCampain = Lst.Coils.Max(c => c.Width);


                    Status.LastTks = Lst.Coils[Lst.Coils.Find(c => c.Width == Status.LastWid).ModelIndexCoil].Tks;
                    Status.LastTksOut = Lst.Coils[Lst.Coils.Find(c => c.Width == Status.LastWid).ModelIndexCoil].TksOutput;
                    Status.IndexSarfasl = 0;


                    // currStat.lastTksIn = 3.3;
                    // currStat.lastTksOut = 0.98;
                    //currStat.minWidCampain = 900;
                    //currStat.lastWid = 1000;


                }


            }

            #endregion



        }

        public void calcuMinWidCampRelease(int seq)
        {

            if (Lst.ReleaseScheds[seq].FlagStartCamp == 0)
            {
                var widMin = (from c in Lst.CoilReleases.AsEnumerable()
                              where Lst.ReleaseScheds[seq].LstSeqCoil.Contains(c.CoilIndex)
                              select c.Width).Min();
                int minWid = widMin;


                if (Status.MinWidCampain > minWid)

                    Status.MinWidCampain = minWid;
            }
            // مینیمم عرض در سربرنامه به عنوان کمترین عرض کمپین حساب نمی شود
            else
            {
                Status.MinWidCampain = int.MaxValue;

            }

        }

        //TAN
        public void calcuworkRollRelease(int indexLstReleaseSchedul)
        {

            if (Lst.SolutionsOutputPlan.Count == 0)
            {
                double weiLocal = Lst.ReleaseScheds[indexLstReleaseSchedul].WeiSched;
                double lenLocal = Lst.ReleaseScheds[indexLstReleaseSchedul].LenSched;

                //برنامه در حال تولید وجود دارد
                if (Lst.ReleaseScheds[indexLstReleaseSchedul].StatSched == 2)
                {
                    // با همان غلتک قبلی
                    chekWorkRollRelease(weiLocal, lenLocal, false);
                }

                else
                {
                    // عدم وجود برنامه قبلی
                    if (indexLstReleaseSchedul == 0)
                    {
                        // با غلتک جدید
                        chekWorkRollRelease(weiLocal, lenLocal, true);

                    }
                    // وجود برنامه قبلی
                    else
                    {

                        // چک کردن تغییر سرفصل

                        // true == change 
                        bool chekSarfaslLoc = chekChangeSarfasl(indexLstReleaseSchedul - 1, indexLstReleaseSchedul);

                        // تغییر سرفصل
                        if (chekSarfaslLoc == true)
                        {
                            // با غلتک جدید
                            chekWorkRollRelease(weiLocal, lenLocal, true);
                        }
                        else
                            // با همان غلتک قبلی
                            chekWorkRollRelease(weiLocal, lenLocal, false);


                    }


                }
            }

        }

        //ALL
        public void chekWorkRollRelease(double weiLocal, double lenLocal, bool flagLocal)
        {
            if (flagLocal == true)
            {
                if (Lst.RollsWork.Last().FirstPlan == true)
                {
                    Roll r = new Roll();
                    r.CurrentTotalFixLen = 0;
                    r.CurrentTotalFixWei = 0;
                    r.DatRollEnter = Status.CurrTime;
                    // مطمئنا در حلقه بالا یا همین حلقه مجددا رفته و اول غلتک را رد می کند
                    r.FirstPlan = true;
                    r.LenDB = 0;
                    r.LenOpt = Lst.RollsWork.Last().LenOpt;
                    r.LenRelease = 0;
                    r.WeiDB = 0;
                    r.WeiOpt = Lst.RollsWork.Last().WeiOpt;
                    r.WeiRelease = 0;
                    r.LowerPerc = Lst.RollsWork[0].LowerPerc;
                    r.UpperPerc = Lst.RollsWork[0].UpperPerc;
                    Lst.RollsWork.Add(r);
                }

            }

            if (Lst.RollsWork.Last().WeiOpt != 0)
            {

                do
                {
                    // 
                    if ((Lst.RollsWork.Last().WeiOpt * (1 + Lst.RollsWork.Last().UpperPerc)) - Lst.RollsWork.Last().WeiDB - Lst.RollsWork.Last().WeiRelease >= weiLocal)
                    {
                        Lst.RollsWork.Last().WeiRelease += weiLocal;
                        Lst.RollsWork.Last().FirstPlan = true;
                        weiLocal = 0;
                    }

                    else
                    {
                        double weiloc = Lst.RollsWork.Last().WeiOpt - Lst.RollsWork.Last().WeiDB - Lst.RollsWork.Last().WeiRelease;
                        Lst.RollsWork.Last().WeiRelease += weiloc;
                        weiLocal -= weiloc;
                        Roll r = new Roll();
                        r.CurrentTotalFixLen = 0;
                        r.CurrentTotalFixWei = 0;
                        r.DatRollEnter = Status.CurrTime;
                        // مطمئنا در حلقه بالا یا همین حلقه مجددا رفته و اول غلتک را رد می کند
                        r.FirstPlan = true;
                        r.LenDB = 0;
                        r.LenOpt = Lst.RollsWork.Last().LenOpt;
                        r.LenRelease = 0;
                        r.WeiDB = 0;
                        r.WeiOpt = Lst.RollsWork.Last().WeiOpt;
                        r.WeiRelease = 0;
                        r.LowerPerc = Lst.RollsWork[0].LowerPerc;
                        r.UpperPerc = Lst.RollsWork[0].UpperPerc;
                        Lst.RollsWork.Add(r);
                    }

                } while (weiLocal > 0);
            }
            else
            {

                // 
                if ((Lst.RollsWork.Last().LenOpt * (1 + Lst.RollsWork.Last().UpperPerc)) - Lst.RollsWork.Last().LenDB - Lst.RollsWork.Last().LenRelease >= lenLocal)
                {
                    Lst.RollsWork.Last().LenRelease += lenLocal;
                    Lst.RollsWork.Last().FirstPlan = true;
                    weiLocal = 0;
                }

                else
                {
                    double lenLoc = Lst.RollsWork.Last().LenOpt - Lst.RollsWork.Last().LenDB - Lst.RollsWork.Last().LenRelease;
                    Lst.RollsWork.Last().LenRelease += lenLoc;
                    lenLocal -= lenLoc;
                    Roll r = new Roll();
                    r.CurrentTotalFixLen = 0;
                    r.CurrentTotalFixWei = 0;
                    r.DatRollEnter = Status.CurrTime;
                    // مطمئنا در حلقه بالا یا همین حلقه مجددا رفته و اول غلتک را رد می کند
                    r.FirstPlan = true;
                    r.LenDB = 0;
                    r.LenOpt = Lst.RollsWork.Last().LenOpt;
                    r.LenRelease = 0;
                    r.WeiDB = 0;
                    r.WeiOpt = Lst.RollsWork.Last().WeiOpt;
                    r.WeiRelease = 0;
                    r.LowerPerc = Lst.RollsWork[0].LowerPerc;
                    r.UpperPerc = Lst.RollsWork[0].UpperPerc;
                    Lst.RollsWork.Add(r);
                }

            } while (lenLocal > 0) ;
        }

        //skp
        public bool chekChangeSarfasl(int indexLstSchBefor, int indexLstSchAft)
        {

            foreach (int item in Lst.ReleaseScheds[indexLstSchBefor].LstIndexSarfasl)
            {
                if (Lst.ReleaseScheds[indexLstSchAft].LstIndexSarfasl.Contains(item) == true)
                {
                    Lst.ReleaseScheds[indexLstSchAft].LstIndexSarfasl.Clear();
                    Lst.ReleaseScheds[indexLstSchBefor].LstIndexSarfasl.Clear();

                    Lst.ReleaseScheds[indexLstSchAft].LstIndexSarfasl.Add(item);
                    Lst.ReleaseScheds[indexLstSchBefor].LstIndexSarfasl.Add(item);
                    // سرفصل تغییر نکرده
                    return false;
                }
            }

            return true;

        }

        //  به روز رسانی ایندکس وضعیت کمپین در برنامه پذیرفته شده


        //TAN
        public void calcuWeiCampPlan(Solution outplan)
        {

            rollTem = new Roll();
            double weiCamp = 0;

            Lst.RollsBack.Last().CurrentTotalFixWei += outplan.WeiProg;

            weiCamp = Lst.RollsBack.Last().CurrentTotalFixWei + Lst.RollsBack.Last().WeiRelease + Lst.RollsBack.Last().WeiDB;

            if (Lst.RollsBack.Last().WeiOpt <= weiCamp)
            {

                Lst.SolutionsOutputPlan.Last().IndexCamp = Lst.RollsBack.Count - 1;
                // اضافه شدن کمپین جدید
                rollTem.DatRollEnter = Status.CurrTime;
                rollTem.CurrentTotalFixLen = 0;
                rollTem.CurrentTotalFixWei = 0;
                rollTem.LenDB = 0;
                rollTem.LenOpt = Lst.RollsBack.Last().LenOpt;
                rollTem.LenRelease = 0;
                rollTem.WeiDB = 0;
                rollTem.WeiOpt = Lst.RollsBack.Last().WeiOpt;
                rollTem.WeiRelease = 0;
                rollTem.FirstPlan = false;

                Lst.RollsBack.Add(rollTem);
                Status.MinWidCampain = int.MaxValue;


            }

            else
            {

                Lst.SolutionsOutputPlan.Last().IndexCamp = Lst.RollsBack.Count - 1;
                Lst.RollsBack.Last().FirstPlan = true;

            }

            Status.IndexCamp = Lst.RollsBack.Count - 1;


        }


        //TAN
        public void calcuWeiCampRelease(int indexLstRelease)
        {
            rollTem = new Roll();
            double weiCamp = 0;


            Lst.RollsBack.Last().WeiRelease += Lst.ReleaseScheds[indexLstRelease].WeiSched;


            weiCamp = Lst.RollsBack.Last().CurrentTotalFixWei + Lst.RollsBack.Last().WeiRelease + Lst.RollsBack.Last().WeiDB;



            if (Lst.RollsBack.Last().WeiOpt <= weiCamp)
            {
                // با همان غلتک یا کمپین قبلی

                Lst.SolutionsOutputPlan.Last().IndexCamp = Lst.RollsBack.Count - 1;
                // اضافه شدن کمپین جدید
                rollTem.DatRollEnter = Status.CurrTime;
                rollTem.CurrentTotalFixLen = 0;
                rollTem.CurrentTotalFixWei = 0;
                rollTem.LenDB = 0;
                rollTem.LenOpt = Lst.RollsBack.Last().LenOpt;
                rollTem.LenRelease = 0;
                rollTem.WeiDB = 0;
                rollTem.WeiOpt = Lst.RollsBack.Last().WeiOpt;
                rollTem.WeiRelease = 0;
                rollTem.FirstPlan = false;
                Lst.RollsBack.Add(rollTem);
                Status.IndexCamp = Lst.RollsBack.Count - 1;

            }

            else
            {

                Lst.RollsBack.Last().FirstPlan = true;
                Status.IndexCamp = Lst.RollsBack.Count - 1;

            }


        }

        #endregion

     

        //TEMPER/TAN/skp1
        public void updateBestsolution()
        {

            if (Lst.currSolution.LstSeqCoil.Count != 0)
            {
                if (Lst.currSolution.TotalObj < Lst.bestSolution.TotalObj)
                {

                    InnerParameter.countBestProg = InnerParameter.countCurrProg;

                    Solution.replaceSolutions(TanSkpTemParameter.changeRoll, Lst.bestSolution, Lst.currSolution);

                }
            }

        }


 


    

        //SkP1
        public void insertAvailSarfasl()
        {
            Lst.lstAvailSarfasl.Clear();

            if (Lst.Coils.FindIndex(a => lstPfAvail.Contains(a.PfId) == true) != -1)
            {
                List<int> sarfaalLocAvail = Lst.Coils.Where(b => b.FlagPlan == 1).SelectMany(a => a.LstSarfaslGroup).Distinct().ToList();



                foreach (var item in Lst.Sarfasls)
                {
                    if (sarfaalLocAvail.Contains(item.IndexSarfasl))
                        Lst.lstAvailSarfasl.Add(item.IndexSarfasl);
                }



                Lst.lstAvailSarfasl = Lst.lstAvailSarfasl.Distinct().ToList();
                Lst.lstAvailSarfasl = Lst.lstAvailSarfasl.OrderBy(a => a).ToList();
            }
            else
                InnerParameter.lstChekFinishCapplan.Add(true);

        }

        //TAN
        public void insertAvailProg(int sarfaslLocal)
        {
           Lst.lstAvailProg.Clear();
            if (Lst.Coils.FindIndex(a => InnerParameter.lstPfAvail.Contains(a.PfId) == true) != -1)
            {
            sarfaslLocal = 0;
     


            List<ProgEfraz> lstLoc = Lst.ProgEfrazes.Where(a => Lst.Sarfasls[sarfaslLocal].LstgroupMisProg.Contains(a.ProgMis) == true && a.FlgAvailable == 1).ToList();

            //اگر برنامه ای به شرایطی از برنامه های قبل وابسته باشد باید در اینجا شرایط اضافه گردد

            foreach (ProgEfraz i in lstLoc)
            {

                int flg =Lst.ShiftWorks[Status.IndexShift].FlgNightShift;

                if (flg == 0 || (flg == 1 && i.FlgNightPlan == 1))
                {

                    //int index = lstCoil.FindIndex(c => c.idAfraz == i.idAfraz && c.flagPlan != -1);
                    //if (index != -1)
                    //{

                    Lst.lstAvailProg.Add(i.IdEfraz);

                    //}

                    //else
                    // index++;

                }

            }

            Lst.lstAvailProg = Lst.lstAvailProg.Distinct().ToList();
            Lst.lstAvailProg = Lst.lstAvailProg.OrderBy(a => a).ToList();
            }
            else
                 InnerParameter.lstChekFinishCapplan.Add(true);


        }


     
      

    

        /// <summary>
        /// 1 =sarfasl  0 = capacity 2= no change
        /// </summary>
        /// <param name="lstRollLocal"></param>
        /// <returns></returns>

        //TEM-SKP1
        public int chekChangeWorkRoll(List<Roll> lstRollLocal)
        {
            
            #region if (lstRollLocal.Last().weiOpt != 0)

            if (lstRollLocal.Last().WeiOpt != 0)
            {
                // تغییر غلتک به دلیل سرفصل

                if (Status.IndexSarfasl != -1 && Status.IndexSarfasl != Lst.currSolution.IndexSarfasl)
                {
                    //تغییر غلتک به دلیل ظرفیت

                    // فقط با مقدار بهینه مقایسه می شود زیرا  به دلیل سرفصل غلتک عوض شده است بنابراین
                    // غلتک جدید دیگر مقدار فیکس یا ریلیز یا دی بی ندارد

                    if ((lstRollLocal.Last().WeiOpt) * (1-lstRollLocal.Last().LowerPerc) < (//lstRollLocal.Last().weiDB
                        //  + lstRollLocal.Last().currentTotalFixWei
                        // + lstRollLocal.Last().weiRelease
                                                           +Lst.currSolution.WeiProg))
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
                                                                + Lst.currSolution.WeiProg))

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
                                                               +Lst.currSolution.WeiProg))
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

                if (Status.IndexSarfasl != -1 && Status.IndexSarfasl != Lst.currSolution.IndexSarfasl)
                {
                    //تغییر غلتک به دلیل ظرفیت

                    // فقط با مقدار بهینه مقایسه می شود زیرا  به دلیل سرفصل غلتک عوض شده است بنابراین
                    // غلتک جدید دیگر مقدار فیکس یا ریلیز یا دی بی ندارد

                    if ((lstRollLocal.Last().LenOpt) * (1 - lstRollLocal.Last().LowerPerc) < (//lstRollLocal.Last().lenDB
                        //  + lstRollLocal.Last().currentTotalFixLen
                        // + lstRollLocal.Last().lenRelease
                                                           +Lst.currSolution.LenProg))
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
                                                                + Lst.currSolution.LenProg))

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
                                                          +Lst.currSolution.LenProg))
                            return 20;


                        else
                            return 21;


                    }

                }




            }
            #endregion


        }
        //SKP1
        public void chekWorkRollCurr(int indxSarfaslLoc, int idProgLocal)
        {
            InnerParameter.weiTotal = 0;
            InnerParameter.lenTotal = 0;
            InnerParameter.weiOpt = Lst.RollsWork.Last().WeiOpt;
            lenOpt =Lst.RollsWork.Last().LenOpt;
            TanSkpTemParameter.changeRoll = true;

            int chek = 0;// chekChangeWorkRoll(lstOutputPlan.Last());

            // تغییر غلتک به دلیل سرفصل

            if (Status.IndexSarfasl != -1 && Status.IndexSarfasl != indxSarfaslLoc && Lst.RollsWork.Last().FirstPlan == true)
            {
                int indx = Lst.JumpBetweenCrowns.FindIndex(a => a.IndexSarfaslFrom == Status.IndexSarfasl
                                                    && a.IndexSarfaslTo == indxSarfaslLoc);
                if (indx != -1)
                {
                    int flg = Lst.JumpBetweenCrowns.Find(a => a.IndexSarfaslFrom == Status.IndexSarfasl
                                                      && a.IndexSarfaslTo == indxSarfaslLoc).FlqPossibleJump;
                    if (flg == 1)
                        chek = 1;
                }

            }

            // تغییر غلتک به دلیل سرفصل یا نوع برنامه "ام بی" است
            if (chek == 1)
            {

                InnerParameter.weiTotal = 0;
                InnerParameter.lenTotal = 0;
                TanSkpTemParameter.changeRoll = true;
                reasonWorkRoll = 1;
            }

            else
            {
                InnerParameter.weiTotal = Lst.RollsWork.Last().WeiDB + Lst.RollsWork.Last().WeiRelease + Lst.RollsWork.Last().CurrentTotalFixWei;
                InnerParameter.lenTotal = Lst.RollsWork.Last().LenDB + Lst.RollsWork.Last().LenRelease + Lst.RollsWork.Last().CurrentTotalFixLen;
                TanSkpTemParameter.changeRoll = false;
                reasonWorkRoll = 2;

                // اگر غلتک تعویض شده ولی تا به حال استفاده نشده است پس به دلیل ظرفیت قبلا تغیر کرده است

                if ((InnerParameter.weiTotal == 0
                    //|| lenTotal == 0
                ) && Lst.RollsWork.Last().FirstPlan == false)
                {
                    InnerParameter.weiTotal = 0;
                    InnerParameter.lenTotal = 0;
                    TanSkpTemParameter.changeRoll = true;

                    reasonWorkRoll = 0;
                }

            }

        }

        //TEM
        public void chekWorkRoll()
        {
            rollTem = new Roll();

            // ظرفیت  که 
            //doubleChekWorkRoll=1
            //*******************
            if (Lst.SolutionsOutputPlan.Last().ChekChangeWorkRoll == 30)
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

             //سرفصل و
            //doubleChekWorkRoll=2

            else if (Lst.SolutionsOutputPlan.Last().ChekChangeWorkRoll == 10
                     || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRoll == 20)
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




            }


                //doubleChekWorkRoll=1
            else if (Lst.SolutionsOutputPlan.Last().ChekChangeWorkRoll == 31)
            {
                Lst.RollsWork.Last().CurrentTotalFixLen += Lst.SolutionsOutputPlan.Last().LenProg;
                Lst.RollsWork.Last().CurrentTotalFixWei += Lst.SolutionsOutputPlan.Last().WeiProg;
                Lst.RollsWork.Last().FirstPlan = true;
                Lst.SolutionsOutputPlan.Last().IndexWorkRoll = Lst.RollsWork.Count - 1;
            }

                // سرفصل 
            // و یا 
            // doubleChekWorkRoll=2
            // و اضافه شدن غلتک جدید دوم
            else if (Lst.SolutionsOutputPlan.Last().ChekChangeWorkRoll == 11 || Lst.SolutionsOutputPlan.Last().ChekChangeWorkRoll == 21)
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
   


        //TAN-SKP1
        public void calcuSarfaslRelease()
        {
            for (int i = 0; i < Lst.ReleaseScheds.Count; i++)
            {

                List<Sarfasl> lstSarfaslLocal = Lst.Sarfasls.Where(c => c.WidFrom <= Lst.CoilReleases[Lst.ReleaseScheds[i].LstSeqCoil.Last()].Width &&
                                     c.WidTo >= Lst.CoilReleases[Lst.ReleaseScheds[i].LstSeqCoil.Last()].Width
                                     && c.LstgroupMisProg.Contains(Lst.ReleaseScheds[i].MisProg) == true
                                     ).ToList();

                foreach (Sarfasl item in lstSarfaslLocal)
                {
                    if (item.IndexSarfasl != Lst.Sarfasls.Last().IndexSarfasl)
                        Lst.ReleaseScheds[i].LstIndexSarfasl.Add(item.IndexSarfasl);
                }
                Lst.ReleaseScheds[i].LstIndexSarfasl.OrderBy(a => a).ToList();
            }
        }

 
        public void calcuObjChangSarfasl()
        {


            if (Lst.currSolution.IndexSarfasl > Status.IndexSarfasl)

                // currSolution.objChangeSarfasl= lstJumpBetweenCrown.Find(c=>c.indexSarfaslFrom == currStat.indexSarfasl && c.indexSarfaslTo==currSolution.indexSarfasl).costBetweenSarfasl;

                Lst.currSolution.ObjSarfasl = 1;
            else
                Lst.currSolution.ObjSarfasl = 0;

        }

      
       

        public void insertnotSensitiveSurFace()
        {
            //List<ProgAFraz> Mblist = lstProgAFraz.Where(a => !a.progMis.ToUpper().Contains("MB")).ToList();

            List<ProgEfraz> Mblist = Lst.ProgEfrazes.Where(c => c.FlgSensitiveSur == 0).ToList();

            lstNotSensitive = Mblist.Select(a => a.IdEfraz).ToList();

        }

       



    }
}
