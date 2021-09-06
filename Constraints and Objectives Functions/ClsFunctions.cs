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

        #region Only One Time Run befor Run Algorithm

        // assign sarfasl
        public void assignSarfaslGroup()
        {
            foreach (var i in Lst.Sarfasls)
            {
                // select coil with specific feature
                List<Coil> sarfaslCoil = Lst.Coils.Where(a => a.Width <= i.WidTo && a.Width >= i.WidFrom).ToList();

                foreach (var j in sarfaslCoil)
                {
                    // In the class of coil  

                    j.LstSarfaslGroup.Add(i.IndexSarfasl);

                    //  In the class of sarfasl  
                    i.LstCoilSarfasl.Add(j.ModelIndexCoil);

                }
            }
        }

        #endregion Only One Time Run in Algorithm  befor Run Algorithm

        #region//Methode sequencingProgram
    
        public void fillMainList(int indexSarfaslLocal, int idEfrazLocal)
        {
            List<Coil> lstCoilLocal;
            Lst.CoilsMain.Clear();
            Lst.CoilsTemDelete.Clear();
            Lst.CoilsMainCopy.Clear();
            //      type of program is sensitive
            if (TanSkpTemParameter.lstNotSensitive.Contains(idEfrazLocal) != true)

                lstCoilLocal = Lst.Coils.Where(q => q.IdEfraz == idEfrazLocal &&
                                                 q.Width <= Status.MinWidCampain &&
                                                    q.FlagPlan == 1 &&
                                                    q.AvailTime <= Status.CurrTime
                                                     && InnerParameter.lstPfAvail.Contains(q.PfId)
                                                      && q.LstSarfaslGroup.Contains(indexSarfaslLocal)).ToList();
  
                //type of program is not sensitive
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

            // if work roll does not change
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
         #endregion

        #region  Release

        // update current status
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
                    //  last release
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

               }
                
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

          public void calcuworkRollRelease(int indexLstReleaseSchedul)
        {

            if (Lst.SolutionsOutputPlan.Count == 0)
            {
                double weiLocal = Lst.ReleaseScheds[indexLstReleaseSchedul].WeiSched;
                double lenLocal = Lst.ReleaseScheds[indexLstReleaseSchedul].LenSched;

                //The program is in production
                if (Lst.ReleaseScheds[indexLstReleaseSchedul].StatSched == 2)
                {
                    // with old work roll   
                    chekWorkRollRelease(weiLocal, lenLocal, false);
                }

                else
                {
                       
                    if (indexLstReleaseSchedul == 0)
                    {
                        // with new work roll  
                        chekWorkRollRelease(weiLocal, lenLocal, true);

                    }
                    else
                    {
                    
                        // true == change 
                        bool chekSarfaslLoc = chekChangeSarfasl(indexLstReleaseSchedul - 1, indexLstReleaseSchedul);

                        if (chekSarfaslLoc == true)
                        {
                            // with old work roll   
                 
                            chekWorkRollRelease(weiLocal, lenLocal, true);
                        }
                        else
                            // with new work roll  
                      
                            chekWorkRollRelease(weiLocal, lenLocal, false);
                    }
                }
            }
        }

        // update work roll
        public void chekWorkRollRelease(double weiLocal, double lenLocal, bool flagLocal)
        {
            if (flagLocal == true)
            {
                // Roller capacity not used 
                if (Lst.RollsWork.Last().FirstPlan == true)
                {
                    Roll r = new Roll();
                    r.CurrentTotalFixLen = 0;
                    r.CurrentTotalFixWei = 0;
                    r.DatRollEnter = Status.CurrTime;
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
                    // The remaining capacity of the roller is as required
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

                // The remaining capacity of the roller is as required
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
                    // sarfasl does not change   
                    return false;
                }
            }
          // sarfasl has to be changed   
            return true;

        }

  
        public void calcuWeiCampPlan(Solution outplan)
        {

            rollTem = new Roll();
            double weiCamp = 0;

            Lst.RollsBack.Last().CurrentTotalFixWei += outplan.WeiProg;

            weiCamp = Lst.RollsBack.Last().CurrentTotalFixWei + Lst.RollsBack.Last().WeiRelease + Lst.RollsBack.Last().WeiDB;
            // The remaining capacity of the roller is as required
            if (Lst.RollsBack.Last().WeiOpt <= weiCamp)
            {

                Lst.SolutionsOutputPlan.Last().IndexCamp = Lst.RollsBack.Count - 1;
              
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


        
        public void calcuWeiCampRelease(int indexLstRelease)
        {
            rollTem = new Roll();
            double weiCamp = 0;


            Lst.RollsBack.Last().WeiRelease += Lst.ReleaseScheds[indexLstRelease].WeiSched;


            weiCamp = Lst.RollsBack.Last().CurrentTotalFixWei + Lst.RollsBack.Last().WeiRelease + Lst.RollsBack.Last().WeiDB;


            // The remaining capacity of the roller is as required
            if (Lst.RollsBack.Last().WeiOpt <= weiCamp)
            {

                Lst.SolutionsOutputPlan.Last().IndexCamp = Lst.RollsBack.Count - 1;
            
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

        public void insertAvailSarfasl()
        {
            Lst.lstAvailSarfasl.Clear();

            if (Lst.Coils.FindIndex(a => InnerParameter.lstPfAvail.Contains(a.PfId) == true) != -1)
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

        /// <summary>
        /// 1 =sarfasl  0 = capacity 2= no change
        /// </summary>
        /// <param name="lstRollLocal"></param>
        /// <returns></returns>
   
        public void chekWorkRollCurr(int indxSarfaslLoc, int idProgLocal)
        {
            InnerParameter.weiTotal = 0;
            InnerParameter.lenTotal = 0;
            InnerParameter.weiOpt = Lst.RollsWork.Last().WeiOpt;
           
            TanSkpTemParameter.changeRoll = true;

            int chek = 0;// chekChangeWorkRoll(lstOutputPlan.Last());

            // change roll because of sarfasl
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
             // change roll because of sarfasl or type of programm is "MB"
      
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

            TanSkpTemParameter.lstNotSensitive = Mblist.Select(a => a.IdEfraz).ToList();

        }
    }
}
