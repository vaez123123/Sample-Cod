using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using IPSO.CMP.CommonFunctions.ParameterClasses;
using SkinPass1Scheduling;

namespace TandemScheduling
{
     public  static class WriterSkp1
    {
        public static string PathWriter;
        public static SKINPASS1DataBase WorkData = new SKINPASS1DataBase();
        static DataTable dt = new DataTable();
        public static int ffff = 0;

        public static void write(string name, CommonLists Lst)
        {
            string fullnameoutput;


            string exten = ".txt";



            fullnameoutput = PathWriter + name + exten;
             FileStream fk = new FileStream(fullnameoutput, FileMode.Create, FileAccess.Write);

            StreamWriter sh = new StreamWriter(fk);
            int i = 0;
            int j = 0;

            sh.Write(TimeParameter.timeRunModel.EllapsedTimeString);
            sh.WriteLine();
            for (i = 0; i < Lst.SolutionsOutputPlan.Count; i++)
            {
                for (j = 0; j < Lst.SolutionsOutputPlan[i].LstSeqCoil.Count; j++)
                {


                    sh.Write(i + 1);
                    sh.Write("\t");

                    sh.Write(InnerParameter.lstCountProgLocal[i]);
                    sh.Write("\t");

                    sh.Write(Lst.Coils[Lst.SolutionsOutputPlan[i].LstSeqCoil[j]].ModelIndexCoil);
                    sh.Write("\t");

                    sh.Write(Lst.Coils[Lst.SolutionsOutputPlan[i].LstSeqCoil[j]].IdSnapshot);
                    sh.Write("\t");


                    sh.Write(Lst.SolutionsOutputPlan[i].IndexSarfasl);
                    sh.Write("\t");
                    sh.Write(Lst.SolutionsOutputPlan[i].IdEfraz);
                    sh.Write("\t");
                    sh.Write(Math.Round(Lst.Coils[Lst.SolutionsOutputPlan[i].LstSeqCoil[j]].Tks, 4));
                    sh.Write("\t");
                    sh.Write(Math.Round(Lst.Coils[Lst.SolutionsOutputPlan[i].LstSeqCoil[j]].TksOutput, 4));
                    sh.Write("\t");
                    sh.Write(Lst.Coils[Lst.SolutionsOutputPlan[i].LstSeqCoil[j]].Width);
                    sh.Write("\t");
                    sh.Write(Lst.Coils[Lst.SolutionsOutputPlan[i].LstSeqCoil[j]].Weight);
                    sh.Write("\t");
                    sh.Write(Lst.Coils[Lst.SolutionsOutputPlan[i].LstSeqCoil[j]].PfId);
                    sh.Write("\t");
                    sh.Write(Lst.SolutionsOutputPlan[i].IndexWorkRoll);


                    sh.WriteLine();
                }
                // sh.WriteLine();
                //  sh.WriteLine();
                //  sh.WriteLine();
            }

            sh.Close();

        }


        public static void writeSnapShot(string name, List<Coil> Coils)
        {
            string fullnameoutput;


            string exten = ".txt";

            fullnameoutput = PathWriter + name + exten;


           // fullnameoutput = @"C:\Documents and Settings\p.vaez\Desktop\Output\SKP1\" + DataBase.runId2 + "\\" + name + exten;
                 FileStream fk = new FileStream(fullnameoutput, FileMode.Create, FileAccess.Write);

            StreamWriter sh = new StreamWriter(fk);
            int i = 0;
            int j = 0;

            //sh.Write(durTimeTest);
            // sh.WriteLine();
            for (i = 0; i < Coils.Count; i++)
            {

                sh.Write(Coils[i].IdSnapshot);
                sh.Write("\t");
                sh.Write(Coils[i].IdOrder);
                sh.Write("\t");
                sh.Write(Coils[i].IdEfraz);
                sh.Write("\t");
                sh.Write(Coils[i].DatLast);
                sh.Write("\t");
                sh.Write(Coils[i].Len);
                sh.Write("\t");
                sh.Write(Coils[i].Weight);
                sh.Write("\t");
                sh.Write(Coils[i].PfId);
                sh.Write("\t");
                sh.Write(Coils[i].RankTotal);
                sh.Write("\t");
                //sh.Write(lstCoil[i].rankAvailtime);
                //sh.Write("\t");
                sh.Write(Coils[i].RankDatLast);
                sh.Write("\t");
                sh.Write(Coils[i].RankDurability);
                sh.Write("\t");
                //sh.Write(lstCoil[i].rankEnterStorge);
                //sh.Write("\t");
                sh.Write(Coils[i].RankLevelStorge);
                sh.Write("\t");
                sh.Write(Coils[i].RankPriority);
                sh.Write("\t");
                sh.Write(Coils[i].Tks);
                sh.Write("\t");
                //sh.Write(lstCoil[i].tksOutput);
                //sh.Write("\t");
                sh.Write(Coils[i].Width);
                sh.Write("\t");

                for (int n = 0; n < Coils[i].LstSarfaslGroup.Count; n++)
                {
                    sh.Write(Coils[i].LstSarfaslGroup[n]);
                    sh.Write(" \\ ");
                }

                sh.Write("\t");

                sh.WriteLine();


            }

            sh.Close();

        }


        public static void writerBestProg(Solution progLoc, string route, bool breakline, List<Coil> Coils, CommonLists Lst)
        {

            string fullnameoutput;


            string exten = ".txt";



            fullnameoutput = PathWriter + route + exten;
            //string route2 = @"C:\Documents and Settings\p.vaez\Desktop\Output\SKP1\" + DataBase.runId2 + "\\" + route;
            FileStream fk2;

            fk2 = new FileStream(fullnameoutput, FileMode.Append, FileAccess.Write);
            StreamWriter stream2 = new StreamWriter(fk2);



            if (breakline == false)
                stream2.WriteLine();
            else
            {

                var tksMaxx = (from c in Coils.AsEnumerable()
                               where progLoc.LstSeqCoil.Contains(c.ModelIndexCoil)
                               select c.Tks).Max();
                var tksMinn = (from c in Coils.AsEnumerable()
                               where progLoc.LstSeqCoil.Contains(c.ModelIndexCoil)
                               select c.Tks).Min();

                var tksOutMaxx = (from c in Coils.AsEnumerable()
                                  where progLoc.LstSeqCoil.Contains(c.ModelIndexCoil)
                                  select c.TksOutput).Max();

                var tksOutMinn = (from c in Coils.AsEnumerable()
                                  where progLoc.LstSeqCoil.Contains(c.ModelIndexCoil)
                                  select c.TksOutput).Min();

                var widMaxx = (from c in Coils.AsEnumerable()
                               where progLoc.LstSeqCoil.Contains(c.ModelIndexCoil)
                               select c.Width).Max();

                var widMinn = (from c in Coils.AsEnumerable()
                               where progLoc.LstSeqCoil.Contains(c.ModelIndexCoil)
                               select c.Width).Min();

                double tksMin = tksMinn;
                double tksMax = tksMaxx;
                double tksOutMax = tksOutMaxx;
                double tksOutMin = tksOutMinn;
                double maxWid = widMaxx;
                double minWid = widMinn;




                stream2.Write(Convert.ToString(Lst.SolutionsOutputPlan.Count - 1)
                     + "\t" + Convert.ToString(InnerParameter.lstCountProgLocal.Last())
                     + "\t" + Convert.ToString(progLoc.FlagUnableInsertCoil)
                   + "\t" + Convert.ToString(progLoc.IdEfraz)

                      + "\t" + Convert.ToString(Lst.ProgEfrazes.Find(c => c.IdEfraz == progLoc.IdEfraz).ProgMis)
                         + "\t" + Convert.ToString(progLoc.IndexSarfasl)
                         + "\t" + Convert.ToString(progLoc.LstSeqCoil.Count)


                      + "\t" + Convert.ToString(maxWid)
                      + "\t" + Convert.ToString(minWid)

                       + "\t" + Convert.ToString(tksMax)
                        + "\t" + Convert.ToString(tksMin)


                     //  + "\t" + Convert.ToString(tksOutMax)
                    //+ "\t" + Convert.ToString(tksOutMin)


                      + "\t" + Convert.ToString(InnerParameter.weiTotal)
                      // + "\t" + Convert.ToString(weiOpt)

                     + "\t" + Convert.ToString(progLoc.IndexWorkRoll)
                      + "\t" + Convert.ToString(progLoc.ChekChangeWorkRoll)
                        + "\t" + Convert.ToString(progLoc.LenProg)
                     + "\t" + Convert.ToString(progLoc.WeiProg)


                  + "\t" + Convert.ToString(progLoc.TotalObj)
                   + "\t" + Convert.ToString(progLoc.ObjCapPlan * WeightObjective.CapPlanCoef)
                    + "\t" + Convert.ToString(progLoc.ObjCountProg * WeightObjective.CountProgCoef)

                     + "\t" + Convert.ToString(progLoc.ObjDatlast * WeightObjective.DatLastCoef)

                      + "\t" + Convert.ToString(progLoc.ObjDurability * WeightObjective.DurabilityCoef)
                    //   + "\t" + Convert.ToString(progLoc.objStartCamp)
                        + "\t" + Convert.ToString(progLoc.ObjLenWorkRoll * WeightObjective.WorkRollLenCoef)
                         + "\t" + Convert.ToString(progLoc.ObjLevelStorge * WeightObjective.LevelStorgeCoef)
                          + "\t" + Convert.ToString(progLoc.ObjPrior * WeightObjective.PriorityCoef)
                           + "\t" + Convert.ToString(progLoc.ObjSarfasl * WeightObjective.SarfaslCoef)
                            + "\t" + Convert.ToString(progLoc.ObjSetupCost * WeightObjective.SetupCoef)
                            + "\t" + Convert.ToString(progLoc.ObjSarfasl * WeightObjective.ContinuePatternCoef)
                    // + "\t" + Convert.ToString(progLoc.objContinueMA)
                              + "\t" + Convert.ToString(progLoc.ObjWeiWorkRoll * WeightObjective.WorkRollWeiCoef)
                              + "\t" + Convert.ToString(progLoc.ObjPriorGroupTypeMis * WeightObjective.PriorGroupTypeMisCoef)
                               + "\t" + Convert.ToString(progLoc.StartTimeSelectProg)
                               + "\t" + Convert.ToString(progLoc.EndTimeSelectProg)


                    );

                // List<int> pfLocal= new List<int>();
                //for (int i = 0; i < lstOutputPlan.Count; i++)
                //{


                //}
                //= lstOutputPlan.Where(c => lstCoil[c.lstSeqCoil].pfId).ToList();

                int z = -1;
                double wei = 0;
                //foreach (int i in progLoc.lstSeqCoil)
                //{
                //    int Pf = lstCoil[i].pfId;
                //    //if (z != Pf)
                //    //{
                //    //    if(z!= -1)
                //    stream2.Write("\t" + Convert.ToString(Pf))
                //        ;//+ "=" + "\t" + Convert.ToString(wei));
                //    //    z = Pf;
                //    //    wei = 0;
                //    //}
                //    //wei += lstCoil[i].weight;

                //}
                stream2.WriteLine();
            }



            stream2.Close();
        }

        public static void writercurrProg(Solution progLoc, string route, bool breakline, List<Coil> Coils, CommonLists Lst)
        {

            string fullnameoutput;


            string exten = ".txt";



            fullnameoutput = PathWriter + route + exten;

           // string route2 = @"C:\Documents and Settings\p.vaez\Desktop\Output\SKP1\" + DataBase.runId2 + "\\" + route;
            FileStream fk2;

            fk2 = new FileStream(fullnameoutput, FileMode.Append, FileAccess.Write);
            StreamWriter stream2 = new StreamWriter(fk2);


            if (breakline == false)
                stream2.WriteLine();
            else
            {

                var tksMaxx = (from c in Coils.AsEnumerable()
                               where progLoc.LstSeqCoil.Contains(c.ModelIndexCoil)
                               select c.Tks).Max();
                var tksMinn = (from c in Coils.AsEnumerable()
                               where progLoc.LstSeqCoil.Contains(c.ModelIndexCoil)
                               select c.Tks).Min();

                var widMaxx = (from c in Coils.AsEnumerable()
                               where progLoc.LstSeqCoil.Contains(c.ModelIndexCoil)
                               select c.Width).Max();

                var widMinn = (from c in Coils.AsEnumerable()
                               where progLoc.LstSeqCoil.Contains(c.ModelIndexCoil)
                               select c.Width).Min();

                //var tksOutMaxx = (from c in lstCoil.AsEnumerable()
                //         where progLoc.lstSeqCoil.Contains(c.modelIndexCoil)
                //         select c.tksOutput).Max();

                //var tksOutMinn = (from c in lstCoil.AsEnumerable()
                //              where progLoc.lstSeqCoil.Contains(c.modelIndexCoil)
                //              select c.tksOutput).Min();
                //double tksOutMax = tksOutMaxx;
                //double tksOutMin = tksOutMinn;

                double tksMin = tksMinn;
                double tksMax = tksMaxx;
                double maxWid = widMaxx;
                double minWid = widMinn;





                stream2.Write(Convert.ToString(Lst.SolutionsOutputPlan.Count)
                     + "\t" + Convert.ToString(InnerParameter.countProgLocal)

                   + "\t" + Convert.ToString(progLoc.IdEfraz)

                      + "\t" + Convert.ToString(Lst.ProgEfrazes.Find(c => c.IdEfraz == progLoc.IdEfraz).ProgMis)
                         + "\t" + Convert.ToString(progLoc.IndexSarfasl)
                         + "\t" + Convert.ToString(progLoc.LstSeqCoil.Count)


                      + "\t" + Convert.ToString(maxWid)
                      + "\t" + Convert.ToString(minWid)

                       + "\t" + Convert.ToString(tksMax)
                        + "\t" + Convert.ToString(tksMin)


                      // + "\t" + Convert.ToString(tksOutMax)
                    // + "\t" + Convert.ToString(tksOutMin)


                      + "\t" + Convert.ToString(InnerParameter.weiTotal)
                     //  + "\t" + Convert.ToString(weiOpt)

                     + "\t" + Convert.ToString(progLoc.IndexWorkRoll)
                      + "\t" + Convert.ToString(progLoc.ChekChangeWorkRoll)
                        + "\t" + Convert.ToString(progLoc.LenProg)
                     + "\t" + Convert.ToString(progLoc.WeiProg)


                  + "\t" + Convert.ToString(progLoc.TotalObj)
                   + "\t" + Convert.ToString(progLoc.ObjCapPlan * WeightObjective.CapPlanCoef)
                    + "\t" + Convert.ToString(progLoc.ObjCountProg * WeightObjective.CountProgCoef)

                     + "\t" + Convert.ToString(progLoc.ObjDatlast * WeightObjective.DatLastCoef)

                      + "\t" + Convert.ToString(progLoc.ObjDurability * WeightObjective.DurabilityCoef)
                    //   + "\t" + Convert.ToString(progLoc.objStartCamp)
                        + "\t" + Convert.ToString(progLoc.ObjLenWorkRoll * WeightObjective.WorkRollLenCoef)
                         + "\t" + Convert.ToString(progLoc.ObjLevelStorge * WeightObjective.LevelStorgeCoef)
                          + "\t" + Convert.ToString(progLoc.ObjPrior * WeightObjective.PriorityCoef)
                           + "\t" + Convert.ToString(progLoc.ObjSarfasl * WeightObjective.SarfaslCoef)
                            + "\t" + Convert.ToString(progLoc.ObjSetupCost * WeightObjective.SetupCoef)
                            + "\t" + Convert.ToString(progLoc.ObjSarfasl * WeightObjective.ContinuePatternCoef)
                    // + "\t" + Convert.ToString(progLoc.objContinueMA)
                              + "\t" + Convert.ToString(progLoc.ObjWeiWorkRoll * WeightObjective.WorkRollWeiCoef)
                              + "\t" + Convert.ToString(progLoc.ObjPriorGroupTypeMis * WeightObjective.PriorGroupTypeMisCoef)


                    );

                // List<int> pfLocal= new List<int>();
                //for (int i = 0; i < lstOutputPlan.Count; i++)
                //{


                //}
                //= lstOutputPlan.Where(c => lstCoil[c.lstSeqCoil].pfId).ToList();

                int z = -1;
                double wei = 0;
                //foreach (int i in progLoc.lstSeqCoil)
                //{
                //    int Pf = lstCoil[i].pfId;
                //    //if (z != Pf)
                //    //{
                //    //    if(z!= -1)
                //    stream2.Write("\t" + Convert.ToString(Pf))
                //        ;//+ "=" + "\t" + Convert.ToString(wei));
                //    //    z = Pf;
                //    //    wei = 0;
                //    //}
                //    //wei += lstCoil[i].weight;

                //}
                stream2.WriteLine();
            }



            stream2.Close();
        }

        public static void writerworkRoll(List<Roll> lstWorkRolllocal, string route)
        {

            string fullnameoutput;


            string exten = ".txt";



            fullnameoutput = PathWriter + route + exten;

           // string route2 = @"C:\Documents and Settings\p.vaez\Desktop\Output\SKP1\" + DataBase.runId2 + "\\" + route;
            FileStream fk3;

            fk3 = new FileStream(fullnameoutput, FileMode.Create, FileAccess.Write);
            StreamWriter stream2 = new StreamWriter(fk3);

            for (int i = 0; i < lstWorkRolllocal.Count; i++)
            {
                stream2.Write(Convert.ToString(i)

                   + "\t" + Convert.ToString(lstWorkRolllocal[i].CurrentTotalFixLen)
               + "\t" + Convert.ToString(lstWorkRolllocal[i].LenOpt)
                + "\t" + Convert.ToString(lstWorkRolllocal[i].LenRelease)
                + "\t" + Convert.ToString(lstWorkRolllocal[i].LenDB)
                + "\t" + Convert.ToString(lstWorkRolllocal[i].CurrentTotalFixWei)
                + "\t" + Convert.ToString(lstWorkRolllocal[i].WeiOpt)
                + "\t" + Convert.ToString(lstWorkRolllocal[i].WeiRelease)
                + "\t" + Convert.ToString(lstWorkRolllocal[i].WeiDB)
                 + "\t" + Convert.ToString(lstWorkRolllocal[i].DatRollEnter)
                  + "\t" + Convert.ToString(lstWorkRolllocal[i].FirstPlan));
                stream2.WriteLine();
            }





            stream2.WriteLine();

            stream2.Close();
        }


        public static void rankAll(string name, CommonLists Lst)
        {
            string fullnameoutput;


            string exten = ".txt";



            fullnameoutput = PathWriter + name + exten;



           // fullnameoutput = @"C:\Documents and Settings\p.vaez\Desktop\Output\SKP1\" + DataBase.runId2 + "\\" + name + exten;
             FileStream fk = new FileStream(fullnameoutput, FileMode.Create, FileAccess.Write);

            StreamWriter sh = new StreamWriter(fk);


            for (int i = 0; i < Lst.Coils.Count; i++)
            {
                sh.Write(Lst.Coils[i].IdCoil + "\t" +
                Lst.Coils[i].IdSnapshot + "\t" +
                 Lst.Coils[i].Width + "\t" +
                 Lst.Coils[i].Tks + "\t" +

                   Lst.Coils[i].TksOutput + "\t" +
                   Lst.Coils[i].RankTotal + "\t" +
                Lst.Coils[i].IdOrder + "\t" +
                Lst.Coils[i].IdEfraz + "\t" +
                Lst.Coils[i].DatLast + "\t" +
                Lst.Coils[i].AvailTime + "\t");


                double difDatLast = (DataBase.MaxDatLast - Lst.Coils[i].DatLast).Days;
                sh.Write(difDatLast + "\t");

                //double DatLastR = Math.Round(difDatLast / ((dataBase.maxDatLast - dataBase.minDatLast).Days), 4);
                sh.Write(Lst.Coils[i].RankDatLast + "\t");

                double difEnter = (DateTime.Now - Lst.Coils[i].AvailTime).Days;
                sh.Write(difEnter + "\t");

                //double EnterR = 1 - Math.Round(difEnter / (currStat.currTime - dataBase.minEnterStorge).Days, 4);
                sh.Write(Lst.Coils[i].RankDurability + "\t");

                sh.Write(Lst.Coils[i].Priority + "\t");

                sh.Write(Lst.Coils[i].RankPriority + "\t");

                sh.WriteLine();
            }
            sh.Close();

        }

        public static void writeCapPlan(string capPlan, CommonLists Lst)
        {
            string fullnameoutput;

            string exten = ".txt";

            fullnameoutput = PathWriter + capPlan + exten;

            FileStream fk = new FileStream(fullnameoutput, FileMode.Create, FileAccess.Write);

            StreamWriter sh = new StreamWriter(fk);
            int i = 0;


            double dif = 0;
            double used = 0;


            DateTime firstDate = Lst.CapPlans[0].DatePlan.Date;
            for (i = 0; i < Lst.CapPlans.Count; i++)
            {

                if (firstDate == Lst.CapPlans[i].DatePlan.Date)
                {
                    sh.Write(Lst.CapPlans[i].PfId + "\t"
                    + Lst.CapPlans[i].DatePlan.Date + "\t"
                    + Lst.CapPlans[i].NetValuePfFix + "\t");

                    double res = Lst.CapPlanUpDates.Find(a => a.PfId == Lst.CapPlans[i].PfId).RespondValueobj;
                    if (res > 0 && Lst.CapPlans[i].NetValuePfFix > 0)
                    {
                        dif = Lst.CapPlans[i].NetValuePfFix - res;
                        if (dif >= 0)
                        {
                            used = res;
                            Lst.CapPlanUpDates.Find(a => a.PfId == Lst.CapPlans[i].PfId).RespondValueobj -= res;
                        }
                        else
                        {
                            dif = 0;
                            used = Lst.CapPlans[i].NetValuePfFix;
                            Lst.CapPlanUpDates.Find(a => a.PfId == Lst.CapPlans[i].PfId).RespondValueobj -= Lst.CapPlans[i].NetValuePfFix;
                        }
                    }
                    else
                    {
                        used = 0;
                        dif = Lst.CapPlans[i].NetValuePfFix;
                    }

                    sh.Write(dif + "\t" + used + "\t" + Lst.CapPlans[i].NetValuePf + "\t");

                    sh.WriteLine();
                }
                else
                {
                    sh.WriteLine();
                    firstDate = Lst.CapPlans[i].DatePlan.Date;
                    sh.Write(Lst.CapPlans[i].PfId + "\t"
                     + Lst.CapPlans[i].DatePlan.Date + "\t"
                     + Lst.CapPlans[i].NetValuePfFix + "\t");

                    double res = Lst.CapPlanUpDates.Find(a => a.PfId == Lst.CapPlans[i].PfId).RespondValueobj;
                    if (res > 0 && Lst.CapPlans[i].NetValuePfFix > 0)
                    {
                        dif = Lst.CapPlans[i].NetValuePfFix - res;
                        if (dif >= 0)
                        {
                            used = res;
                            Lst.CapPlanUpDates.Find(a => a.PfId == Lst.CapPlans[i].PfId).RespondValueobj -= res;
                        }
                        else
                        {
                            dif = 0;
                            used = Lst.CapPlans[i].NetValuePfFix;
                            Lst.CapPlanUpDates.Find(a => a.PfId == Lst.CapPlans[i].PfId).RespondValueobj -= Lst.CapPlans[i].NetValuePfFix;
                        }
                    }
                    else
                    {
                        used = 0;
                        dif = Lst.CapPlans[i].NetValuePfFix;
                    }

                    sh.Write(dif + "\t" + used + "\t" + Lst.CapPlans[i].NetValuePf + "\t");

                    sh.WriteLine();

                }

            }

            sh.Close();

        }

        public static void writeCapPlanpa(string capPlan, List<CapPlan> CapPlans)
        {
            string fullnameoutput;

            string exten = ".txt";

            fullnameoutput = PathWriter + capPlan + exten;

          //  fullnameoutput = @"C:\Documents and Settings\p.vaez\Desktop\Output\SKP1\" + DataBase.runId2 + "\\" + capPlan + exten;

            FileStream fk = new FileStream(fullnameoutput, FileMode.Create, FileAccess.Write);

            StreamWriter sh = new StreamWriter(fk);
            int i = 0;
            int j = 0;


            DateTime firstDate = CapPlans[0].DatePlan.Date;
            for (i = 0; i < CapPlans.Count; i++)
            {

                if (firstDate == CapPlans[i].DatePlan.Date)
                {
                    sh.Write(CapPlans[i].PfId);
                    sh.Write("\t");
                    sh.Write(CapPlans[i].DatePlan.Date);
                    sh.Write("\t");
                    sh.Write(CapPlans[i].NetValuePfFix);
                    sh.Write("\t");
                    sh.Write(CapPlans[i].NetValuePf);
                    sh.Write("\t");
                    sh.Write(CapPlans[i].NetValuePfFix - CapPlans[i].NetValuePf);
                    sh.Write("\t");
                    sh.WriteLine();
                }
                else
                {
                    sh.WriteLine();
                    firstDate = CapPlans[i].DatePlan.Date;
                    sh.Write(CapPlans[i].PfId);
                    sh.Write("\t");
                    sh.Write(CapPlans[i].DatePlan.Date);
                    sh.Write("\t");
                    sh.Write(CapPlans[i].NetValuePfFix);
                    sh.Write("\t");
                    sh.Write(CapPlans[i].NetValuePf);
                    sh.Write("\t");
                    sh.Write(CapPlans[i].NetValuePfFix - CapPlans[i].NetValuePf);
                    sh.Write("\t");
                    sh.WriteLine();

                }

            }

            sh.Close();

        }


        public static void creatNote(string route, string pathWriter)
        {
            string r = PathWriter + route;
            FileStream fk;
            fk = new FileStream(r, FileMode.Create, FileAccess.Write);
            StreamWriter stream2 = new StreamWriter(fk);
            stream2.Close();
        }
    }
}
