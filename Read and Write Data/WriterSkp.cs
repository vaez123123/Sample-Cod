using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using IPSO.CMP.CommonFunctions.ParameterClasses;
using SKPScheduling;
using IPSO.CMP.CommonFunctions.Functions;
using IPSO.CMP.Log;

namespace SKPScheduling
{
    public  class WriterSKP : WriterFunc
    {
        public static int ffff; 
        public static string PathWriter;
        public static int flgWriter = -1;
        public static DataTable dt = new DataTable();
        public static void creatNotes(string PathWriter)
        {
            if (flgWriter == 1)
            {
                //creat Folder
                bool folderExists = Directory.Exists(PathWriter);
                if (!folderExists)
                    Directory.CreateDirectory(PathWriter);

                creatNote("TimeData.txt", PathWriter, flgWriter);
                creatNote("capPlanProg.txt", PathWriter, flgWriter);
                creatNote("Sarfasl.txt", PathWriter, flgWriter);
                creatNote("SarfaslLoc.txt", PathWriter, flgWriter);

                creatNote("Main.txt", PathWriter, flgWriter);
                creatNote("Time.txt", PathWriter, flgWriter);
                creatNote("local.txt", PathWriter, flgWriter);
                creatNote("capPlanRelease.txt", PathWriter, flgWriter);
                creatNote("select.txt", PathWriter, flgWriter);
                creatNote("Release.txt", PathWriter, flgWriter);
            }
        }

        protected override void writeOutputNotes(CommonLists Lst, int flgWriter, string PathWriter)
        {
            string coilmain = "coilmain";
            string workRoll = "workRoll.txt";
            string backRoll = "backRoll.txt";
            string capPlan = "capPlan";
            string snapShot = "snapShot";
            string scheduling = "scheduling";

            if (flgWriter == 1)
            {      
                write(coilmain, Lst);
                writerworkRoll(Lst.RollsWork, workRoll);
                writerworkRoll(Lst.RollsBack, backRoll);
                writeCapPlan(capPlan, Lst);
                writeSnapShot(snapShot, Lst.Coils);
                writeScheduling(scheduling, Lst);
                writerRelease("Release.txt", true, Lst);
                string rankAllCoil = "rankAllCoil";
                writeRankAll(rankAllCoil, Lst);
            }
        }

        public static void write(string name, CommonLists Lst)
        {
            if (flgWriter == 1)
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
                  
                }

                sh.Close();
            }

        }


        public static void writeSnapShot(string name, List<Coil> Coils)
        {
            if (flgWriter == 1)
            {
                string fullnameoutput;
                string exten = ".txt";
                fullnameoutput = PathWriter + name + exten;

                // fullnameoutput = @"C:\Documents and Settings\p.vaez\Desktop\Output\SKP1\" + DataBase.runId2 + "\\" + name + exten;
                FileStream fk = new FileStream(fullnameoutput, FileMode.Create, FileAccess.Write);

                StreamWriter sh = new StreamWriter(fk);
                int i = 0;
                

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
                    sh.Write(Coils[i].LenKmOutput);
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

        }


        public static void writerBestProg(Solution progLoc, string route, bool breakline, List<Coil> Coils, CommonLists Lst)
        {
            if (flgWriter == 1)
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
                  
                    stream2.WriteLine();
                }

                stream2.Close();
            }
        }

        public static void writercurrProg(Solution progLoc, string route, bool breakline, List<Coil> Coils, CommonLists Lst)
        {
            if (flgWriter == 1)
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
                    
                    stream2.WriteLine();
                }

                stream2.Close();
            }
        }

        public static void writerworkRoll(List<Roll> lstWorkRolllocal, string route)
        {
            if (flgWriter == 1)
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
        }


        public static void rankAll(string name, CommonLists Lst)
        {
            if (flgWriter == 1)
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

        }

        public static void writeCapPlan(string capPlan, CommonLists Lst)
        {
            if (flgWriter == 1)
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

        }

        public static void writeCapPlanpa(string capPlan, List<CapPlan> CapPlans)
        {
            if (flgWriter == 1)
            {
                string fullnameoutput;

                string exten = ".txt";

                fullnameoutput = PathWriter + capPlan + exten;

                //  fullnameoutput = @"C:\Documents and Settings\p.vaez\Desktop\Output\SKP1\" + DataBase.runId2 + "\\" + capPlan + exten;

                FileStream fk = new FileStream(fullnameoutput, FileMode.Create, FileAccess.Write);

                StreamWriter sh = new StreamWriter(fk);
                int i = 0;

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
        }


       
        public static void writeScheduling(string name, CommonLists Lst)
        {
            if (flgWriter == 1)
            {
                string fullnameoutput;
                string exten = ".txt";

                fullnameoutput = PathWriter + name + exten;

                FileStream fk = new FileStream(fullnameoutput, FileMode.Create, FileAccess.Write);

                StreamWriter sh = new StreamWriter(fk);

                int counterr = 1;
                foreach (var item in Lst.Schedulings)
                {
                    sh.Write(counterr + "\t" + item.Id + "\t" + item.TypId + "\t" + item.Start + "\t" + item.End);
                    sh.WriteLine();
                    counterr++;
                }
                sh.Close();
            }
        }
        public static void writerRelease(string route, bool breakline, CommonLists Lst)
        {
            if (flgWriter == 1)
            {

                string route2 = PathWriter + route;
                FileStream fk2;

                fk2 = new FileStream(route2, FileMode.Append, FileAccess.Write);
                StreamWriter stream2 = new StreamWriter(fk2);



                if (breakline == false)
                    stream2.WriteLine();
                else
                {

                    for (int i = 0; i < Lst.ReleaseScheds.Count; i++)
                    {

                        var tksMaxx = (from c in Lst.CoilReleases.AsEnumerable()
                                       where Lst.ReleaseScheds[i].LstSeqCoil.Contains(c.CoilIndex)
                                       select c.Tks).Max();
                        var tksMinn = (from c in Lst.CoilReleases.AsEnumerable()
                                       where Lst.ReleaseScheds[i].LstSeqCoil.Contains(c.CoilIndex)
                                       select c.Tks).Min();

                        var tksOutMaxx = (from c in Lst.CoilReleases.AsEnumerable()
                                          where Lst.ReleaseScheds[i].LstSeqCoil.Contains(c.CoilIndex)
                                          select c.TksOutput).Max();

                        var tksOutMinn = (from c in Lst.CoilReleases.AsEnumerable()
                                          where Lst.ReleaseScheds[i].LstSeqCoil.Contains(c.CoilIndex)
                                          select c.TksOutput).Min();

                        var widMaxx = (from c in Lst.CoilReleases.AsEnumerable()
                                       where Lst.ReleaseScheds[i].LstSeqCoil.Contains(c.CoilIndex)
                                       select c.Width).Max();

                        var widMinn = (from c in Lst.CoilReleases.AsEnumerable()
                                       where Lst.ReleaseScheds[i].LstSeqCoil.Contains(c.CoilIndex)
                                       select c.Width).Min();

                        double tksMin = tksMinn;
                        double tksMax = tksMaxx;
                        double tksOutMax = tksOutMaxx;
                        double tksOutMin = tksOutMinn;
                        double maxWid = widMaxx;
                        double minWid = widMinn;

                        stream2.Write(Convert.ToString(Lst.ReleaseScheds[i].LstSeqCoil.Count)


                           + "\t" + Convert.ToString(Lst.ReleaseScheds[i].IdEfraz)


                              + "\t" + Convert.ToString(Lst.ProgEfrazes.Find(c => c.IdEfraz == Lst.ReleaseScheds[i].IdEfraz).ProgMis)
                                 + "\t" + Convert.ToString(Lst.ReleaseScheds[i].LstIndexSarfasl.Last())

                              + "\t" + Convert.ToString(maxWid)
                              + "\t" + Convert.ToString(minWid)

                               + "\t" + Convert.ToString(tksMax)
                                + "\t" + Convert.ToString(tksMin)


                               + "\t" + Convert.ToString(tksOutMax)
                             + "\t" + Convert.ToString(tksOutMin)


                              + "\t" + Convert.ToString(InnerParameter.weiTotal)
                               + "\t" + Convert.ToString(InnerParameter.weiOpt)


                                + "\t" + Convert.ToString(Lst.ReleaseScheds[i].LenSched)
                             + "\t" + Convert.ToString(Lst.ReleaseScheds[i].WeiSched)

                                          + "\t" + Convert.ToString(Lst.ReleaseScheds[i].StartTimeSelectProg)
                                       + "\t" + Convert.ToString(Lst.ReleaseScheds[i].EndTimeSelectProg)

                            );
                        stream2.Write("\t");
                        for (int k = 0; k < Lst.ReleaseScheds[i].LstWorkRoll.Count; k++)
                        {
                            stream2.Write(Lst.ReleaseScheds[i].LstWorkRoll[k]);
                        }

                        stream2.WriteLine();

                    }
                }

                stream2.Close();
            }
        }

        public static void writeRankAll(string name, CommonLists Lst)
        {
            if (flgWriter == 1)
            {
                string fullnameoutput;
                string exten = ".txt";

                fullnameoutput = PathWriter + name + exten;
                FileStream fk = new FileStream(fullnameoutput, FileMode.Create, FileAccess.Write);

                StreamWriter sh = new StreamWriter(fk);



                for (int i = 0; i < Lst.Coils.Count; i++)
                {
                    sh.Write(Lst.Coils[i].ModelIndexCoil + "\t" +
                    Lst.Coils[i].IdSnapshot + "\t" +

                       Lst.Coils[i].RankTotal + "\t"
                    );


                    sh.WriteLine();
                }
                sh.Close();
            }
        }


    }
}
