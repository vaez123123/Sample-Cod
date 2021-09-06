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

                // If the type of program is sensitive, the smallest width in the campaign should be used
            if (TanSkpTemParameter.lstNotSensitive.Contains(idEfrazLocal) != true)

                lstCoilLocal = Lst.Coils.Where(q => q.IdEfraz == idEfrazLocal &&
                                                 q.Width <= Status.MinWidCampain &&
                                                    q.FlagPlan == 1 &&
                                                    q.AvailTime <= Status.CurrTime
                                                     && InnerParameter.lstPfAvail.Contains(q.PfId)
                                                      && q.LstSarfaslGroup.Contains(indexSarfaslLocal)).ToList();
             

            // If the program type is not sensitive
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

            // If the work roll has not changed, it can be increased as much as the width increase jump in the campaign
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
                    int maxLocalSeq = Lst.ReleaseScheds.Max(n => n.SeqSched);

                    int indexMaxLocal = Lst.ReleaseScheds.FindIndex(e => e.SeqSched == maxLocalSeq);

                    Status.LastWid = Lst.CoilReleases[Lst.ReleaseScheds[indexMaxLocal].LstSeqCoil.Last()].Width;
                    Status.LastTks = Lst.CoilReleases[Lst.ReleaseScheds[indexMaxLocal].LstSeqCoil.Last()].Tks;
                    Status.LastTksOut = Lst.CoilReleases[Lst.ReleaseScheds[indexMaxLocal].LstSeqCoil.Last()].TksOutput;
                    Status.IndexSarfasl = Lst.ReleaseScheds[indexMaxLocal].LstIndexSarfasl.First();

                    Status.IdEfraz = Lst.ReleaseScheds[indexMaxLocal].IdEfraz;


                }

                // Determining the latest status if no program is available                
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
       
    }
}
