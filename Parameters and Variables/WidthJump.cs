using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.Log;

namespace IPSO.CMP.CommonFunctions.ParameterClasses
{
  public  class WidthJump
    {
        public int MaxValWidIncrease { get; set; }
        public int MaxValWidDecrease { get; set; }
        public double MaxPcnWidIncrease { get; set; }
        public double MaxPcnWidDecrease { get; set; }
        public string MisProg;
        public int CodMis;
        public int FlgInputOutput; // 0 =in  1= out
        public int IdEfraz { get; set; }
        /// <summary>
        /// نزولی =2 
        /// 1= صعودی
        /// 0= نوسانی
        /// </summary>
        public int FlgSeqWid { get; set; }

     
        //constructor
        public WidthJump() { }


        public static void calcuValJumWid(int idEfrazLocal,int idSarfaslLocal, int codMisLocal, CommonLists Lst)
        {
            string statusLoc;
            string messageLoc;
            string progMis;

            int indexloc = Lst.ProgEfrazes.FindIndex(c => c.IdEfraz == idEfrazLocal);

            InnerParameter.maxCount = Lst.ProgEfrazes[indexloc].CountOpt;
            InnerParameter.minCount = Lst.ProgEfrazes[indexloc].CountMin;
            progMis = Lst.ProgEfrazes[indexloc].ProgMis;
            calcuJumpWidth(0, codMisLocal, idEfrazLocal, Lst.WidthJumps);

            calcuJumpWidth(1, codMisLocal, idEfrazLocal, Lst.WidthJumps);

            #region  log
            if (InnerParameter.flgSeqWidIn == (int)Parameter.EnumStatusJump.Swinging)
                statusLoc = "سینوسی";
            else if(InnerParameter.flgSeqWidIn == (int)Parameter.EnumStatusJump.Ascending)
                statusLoc = "صعودی";
            else
                statusLoc = "نزولی";


            messageLoc = "###############" +InnerParameter.tab+ InnerParameter.countFixProg + InnerParameter.symbole + Lst.SolutionsOutputPlan.Count +InnerParameter.tab+ "###############";
                Lst.fileLogger.Log(messageLoc, 1);

                messageLoc = "idSarfaslLocal " + InnerParameter.symbole + idSarfaslLocal + InnerParameter.tab+InnerParameter.tab+
                         "idEfraz " + InnerParameter.symbole + idEfrazLocal + InnerParameter.tab+InnerParameter.tab+
                          "progMis " + InnerParameter.symbole + progMis + InnerParameter.tab+InnerParameter.tab+
                          "maxCount " + InnerParameter.symbole + InnerParameter.maxCount + InnerParameter.tab+InnerParameter.tab+
                         "minCount " + InnerParameter.symbole + InnerParameter.minCount ;
                Lst.fileLogger.Log(messageLoc, 2);

                messageLoc = "MaxValWidIncrease " + "Input" + InnerParameter.symbole + InnerParameter.widJumpLocalInAsc + InnerParameter.tab
                 + "MaxValWidDecrease " + "Input" + InnerParameter.symbole + InnerParameter.widJumpLocalInDes + InnerParameter.tab+ InnerParameter.tab
                 + "MaxPcnWidIncrease " + "Input" + InnerParameter.symbole + InnerParameter.widJumpLocalInAscPcn + InnerParameter.tab + InnerParameter.tab
                 + "MaxPcnWidDecrease " + "Input" + InnerParameter.symbole + InnerParameter.widJumpLocalInDesPcn + InnerParameter.tab + InnerParameter.tab
                 + "Status " + InnerParameter.symbole + statusLoc + InnerParameter.tab
                 + "MaxValWidIncrease " + "Output" + InnerParameter.symbole + InnerParameter.widJumpLocalOutAsc + InnerParameter.tab
                 + "MaxValWidDecrease " + "Output" + InnerParameter.symbole + InnerParameter.widJumpLocalOutDes + InnerParameter.tab
                 + "MaxPcnWidIncrease " + "Output" + InnerParameter.symbole + InnerParameter.widJumpLocalOutAscPcn + InnerParameter.tab
                 + "MaxPcnWidDecrease " + "Output" + InnerParameter.symbole + InnerParameter.widJumpLocalOutDesPcn;


            Lst.fileLogger.Log(messageLoc, 2);
            
            #endregion


        }

        public static void calcuJumpWidth(int FlgInputOutput, int codMisLoc, int idEfrazLoc, List<WidthJump> WidthJumps)
        {
            int maxIncrease = 0;
            int maxDecrease = 0;
            double maxPcnIncrease = 0;
            double maxPcnDecrease = 0;
            int flgSeqWid = 0;

            #region عرض داخل برنامه
            if (FlgInputOutput == 0)
            {
                var qryInp = from widJump in WidthJumps
                             where (widJump.IdEfraz == idEfrazLoc || widJump.CodMis == codMisLoc) && widJump.FlgInputOutput == FlgInputOutput
                             select new
                             {
                                 valueJumpDecInput = widJump.MaxValWidDecrease,
                                 valueJumpIncInput = widJump.MaxValWidIncrease,
                                 pcnJumpDecInput = widJump.MaxPcnWidDecrease,
                                 pcnJumpIncInput = widJump.MaxPcnWidIncrease,
                                 flgSeq = widJump.FlgSeqWid
                             };

                //اگر پرش وابسته به نوع برنامه نباشد
                if (qryInp.Count() <= 0)
                {
                    var qry2 = from widJump in WidthJumps
                               where (widJump.IdEfraz == Parameter.CodEfrazNull || widJump.CodMis == Parameter.CodMisNull)
                                    && widJump.FlgInputOutput == FlgInputOutput
                               select new
                               {
                                   valueJumpDecInput = widJump.MaxValWidDecrease,
                                   valueJumpIncInput = widJump.MaxValWidIncrease,
                                   pcnJumpDecInput = widJump.MaxPcnWidDecrease,
                                   pcnJumpIncInput = widJump.MaxPcnWidIncrease,
                                   flgSeq = widJump.FlgSeqWid
                               };

                    if (qry2.Count() <= 0)
                    {
                    
                        maxIncrease = DataBase.MaxJumpWid;
                        maxDecrease = DataBase.MaxJumpWid;
                        maxPcnIncrease = DataBase.MaxJumpWid;
                        maxPcnDecrease = DataBase.MaxJumpWid;
                        flgSeqWid = DataBase.FlgSequenceIncDec;
                    }

                    else
                    {
                        
                        maxIncrease = qry2.FirstOrDefault().valueJumpIncInput;
                        maxDecrease = qry2.FirstOrDefault().valueJumpDecInput;
                        maxPcnIncrease = qry2.FirstOrDefault().pcnJumpIncInput;
                        maxPcnDecrease = qry2.FirstOrDefault().pcnJumpDecInput;
                        flgSeqWid = qry2.FirstOrDefault().flgSeq;
                    }
                }
                else
                {
                    maxIncrease = qryInp.FirstOrDefault().valueJumpIncInput;
                    maxDecrease = qryInp.FirstOrDefault().valueJumpDecInput;
                    maxPcnIncrease = qryInp.FirstOrDefault().pcnJumpIncInput;
                    maxPcnDecrease = qryInp.FirstOrDefault().pcnJumpDecInput;
                    flgSeqWid = qryInp.FirstOrDefault().flgSeq;
                }

                InnerParameter.widJumpLocalInAsc = maxIncrease;
                InnerParameter.widJumpLocalInDes = maxDecrease;
                InnerParameter.widJumpLocalInAscPcn = maxPcnIncrease;
                InnerParameter.widJumpLocalInDesPcn = maxPcnDecrease;
                InnerParameter.flgSeqWidIn = flgSeqWid;

            }
            #endregion عرض داخل برنامه

            #region عرض بین برنامه ها
            else //flgInputOutput = 1
            {
                var qryInp = from widJump in WidthJumps
                             where (widJump.IdEfraz == idEfrazLoc || widJump.CodMis == codMisLoc) && widJump.FlgInputOutput == FlgInputOutput
                             select new
                             {
                                 valueJumpDecOutput = widJump.MaxValWidDecrease,
                                 valueJumpIncOutput = widJump.MaxValWidIncrease,
                                 pcnJumpDecOutput = widJump.MaxPcnWidDecrease,
                                 pcnJumpIncOutput = widJump.MaxPcnWidIncrease,
                                 flgSeq = widJump.FlgSeqWid
                             };

                //اگر پرش وابسته به نوع برنامه نباشد
                if (qryInp.Count() <= 0)
                {
                    var qry2 = from widJump in WidthJumps
                               where (widJump.IdEfraz == Parameter.CodEfrazNull || widJump.CodMis == Parameter.CodMisNull)
                                    && widJump.FlgInputOutput == FlgInputOutput
                               select new
                               {
                                   valueJumpDecOutput = widJump.MaxValWidDecrease,
                                   valueJumpIncOutput = widJump.MaxValWidIncrease,
                                   pcnJumpDecOutput = widJump.MaxPcnWidDecrease,
                                   pcnJumpIncOutput = widJump.MaxPcnWidIncrease,
                                   flgSeq = widJump.FlgSeqWid
                               };

                    if (qry2.Count() <= 0)
                    {
                        maxIncrease = DataBase.MaxJumpWid;
                        maxDecrease = DataBase.MaxJumpWid;
                        maxPcnIncrease = DataBase.MaxJumpWid;
                        maxPcnDecrease = DataBase.MaxJumpWid;
                        flgSeqWid = DataBase.FlgSequenceIncDec;
                    }

                    else
                    {
                        maxIncrease = qry2.FirstOrDefault().valueJumpIncOutput;
                        maxDecrease = qry2.FirstOrDefault().valueJumpDecOutput;
                        maxPcnIncrease = qry2.FirstOrDefault().pcnJumpIncOutput;
                        maxPcnDecrease = qry2.FirstOrDefault().pcnJumpDecOutput;
                        flgSeqWid = qry2.FirstOrDefault().flgSeq;
                    }
                }
                else
                {
                    maxIncrease = qryInp.FirstOrDefault().valueJumpIncOutput;
                    maxDecrease = qryInp.FirstOrDefault().valueJumpDecOutput;
                    maxPcnIncrease = qryInp.FirstOrDefault().pcnJumpIncOutput;
                    maxPcnDecrease = qryInp.FirstOrDefault().pcnJumpDecOutput;
                    flgSeqWid = qryInp.FirstOrDefault().flgSeq;
                }

                InnerParameter.widJumpLocalOutAsc = maxIncrease;
                InnerParameter.widJumpLocalOutDes = maxDecrease;
                InnerParameter.widJumpLocalOutAscPcn = maxPcnIncrease;
                InnerParameter.widJumpLocalOutDesPcn = maxPcnDecrease;
                
            }
            #endregion //عرض بین برنامه ها

        }

    }
}
