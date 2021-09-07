using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.Log;

namespace IPSO.CMP.CommonFunctions.ParameterClasses
{
    public class ProgEfraz
    {

        #region

        public int IdEfraz { get; set; }//DB
        public int FlgAvailable = 1;

        public int CodProgMis { get; set; }//DB
        public string ProgMis { get; set; }//DB

        public int WeiOpt { get; set; }//DB     
        public int LenOpt { get; set; }//DB
        public int CountOpt { get; set; }//DB

        public int CountMin { get; set; }//DB
        //Tin
        public int WeiMin { get; set; }//DB
       
        public int FlgNightPlan { get; set; }//DB  
        //Tan-Tem
        public int FlgSensitiveSur;
        //Pic 
        public List<int> Trim = new List<int>();//DB
        //Pic 
        public int Oil;//DB
        //Pic
        public int MaxNumChangeWid = -1; // DB   
        public double MaxPercChangeWid = -1; // 
        public List<int> ProgEfrazZones = new List<int>();

        #endregion


        //ALL
        public static void chekFlgAvailForProgAfraz(List<ProgEfraz> ProgEfrazes, List<Coil> Coils,int count)
        {
            List<ProgEfraz> lstLocAfraz = new List<ProgEfraz>();

            if (RunInformation.FlgUser == -1)

                lstLocAfraz = ProgEfrazes.Where(c => c.FlgAvailable != -1).ToList();

            else
                lstLocAfraz = ProgEfrazes.Where( c=>c.CodProgMis == RunInformation.lstProg[count] ).ToList();

                foreach (var item in lstLocAfraz)
                {
                    int index = Coils.FindIndex(a => a.IdEfraz == item.IdEfraz && a.FlagPlan != -1);
                    if (index != -1)
                        item.FlgAvailable = 1;
                    else
                        item.FlgAvailable = -1;
                }
            
        }

        public static void insertAvailProg(int sarfaslLocal, CommonLists Lst)
        {
            TimeParameter.timeinsertAvailProg.Start();

            string message;
            Lst.lstAvailProg.Clear();

            if (Lst.Coils.FindIndex(a => InnerParameter.lstPfAvail.Contains(a.PfId) == true) != -1)
            {
                List<int> lstLocAfraz1 = Lst.Coils.Where(c => c.LstSarfaslGroup.Contains(sarfaslLocal)
                                                         && InnerParameter.lstPfAvail.Contains(c.PfId)
                                                         && c.FlagPlan != -1).Select(a => a.IdEfraz).Distinct().OrderBy(a => a).ToList();

                List<ProgEfraz> lstLocAfraz2 = Lst.ProgEfrazes.Where(a => lstLocAfraz1.Contains(a.IdEfraz) && a.FlgAvailable != -1).ToList();


                if (RunInformation.FlgUser == -1)
                {

 
                    foreach (ProgEfraz i in lstLocAfraz2)
                    {

                        int flg = Lst.ShiftWorks[Status.IndexShift].FlgNightShift;

                        if (flg == 0 || (flg == 1 && i.FlgNightPlan == 1))
                        {
                            Lst.lstAvailProg.Add(i.IdEfraz);

                        }

                    }

                    Lst.lstAvailProg = Lst.lstAvailProg.Distinct().ToList();
                    Lst.lstAvailProg = Lst.lstAvailProg.OrderBy(a => a).ToList();
                    
                    //if (lstBackRoll.Last().firstPlan == false)
                    //{
                    //    //chekDecInc = false;
                    //    lstAvailProg = lstNotMbProg;
                    //    //lstAvailSarfasl.Add(lstSarfasl[lstSarfasl.Last().indexSarfasl].indexSarfasl);

                    //}
                    //else
                    //{

                    //        foreach (ProgEFraz i in lstProgEFraz)
                    //        {

                    //            lstAvailProg.Add(i.idEfraz);
                    //        }


                    //}

                }
                else
                {
                    Lst.lstAvailProg.AddRange(Lst.ProgEfrazes.Where(a => a.CodProgMis == RunInformation.lstProg[Lst.SolutionsOutputPlan.Count]).Select(b => b.IdEfraz).OrderBy(r => r).ToList());
                    // RunInformation.chekFlagUser(Lst.Coils, Lst.SolutionsOutputPlan);

                }
            }

            else
            {
                message = "به دلیل عدم وجود برنامه ظرفیت هیچ افرازی انتخاب نشده است";
                Lst.fileLogger.Log(message,-1);
                InnerParameter.lstChekFinishCapplan.Add(true);
            }


            TimeParameter.timeinsertAvailProg.Stop();
        }


    }
}
