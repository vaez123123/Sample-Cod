using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using IPSO.CMP.CommonFunctions.ParameterClasses;
//using System.Data.OracleClient;
using System.Data;
using IPSO.CMP.DataBaseMODEL;
using System.Collections;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using IPSO.CMP.Log;

namespace IPSO.CMP.CommonFunctions.Functions
{
    public class WriterFunc
    {
        private static string comment;
        public static string PathWriterCommon;

        public void writeOutputs(CommonLists Lst, int flgWriter, string PathWriter)
        {
            writeOutputDataBaseNew(Lst); // write in database

            if (flgWriter == 1)// write in text file   
                writeOutputNotes(Lst, flgWriter, PathWriter);
        }

        protected virtual void writeOutputNotes(CommonLists Lst, int flgWriter, string PathWriter)
        {
        }


        #region write in Database

        private static void writeOutputDataBaseNew(CommonLists Lst)
        {
            MODELDataBase DataBase = new MODELDataBase();
            DataTable dt = new DataTable();
            string CommandText = "";
            int maxIdDraft;
            for (int i = 0; i < Lst.SolutionsOutputPlan.Count; i++)
            {
                string comment1 = string.Format(@" select  cmp.cmp_opt_draft_sch_headers_seq.nextval from dual", RunInformation.RunId, RunInformation.NumStation);

                object maxObj = new object();
                DataBase.ExecuteScalar(ref maxObj, comment1, CommandType.Text);
                maxIdDraft = Convert.ToInt32(maxObj);

                //insert Comment
                CommandText = sqlWriteTableCMP_OPT_DRAFT_SCH_HEADERS();


                System.Data.OracleClient.OracleParameter[] InstParam = new System.Data.OracleClient.OracleParameter[11];

                InstParam[0] = new System.Data.OracleClient.OracleParameter("PGTYP_BAS_PROGRAM_TYPE_DEF_ID", System.Data.OracleClient.OracleType.Number);
                InstParam[0].Value = Lst.ProgEfrazes.Find(b => b.IdEfraz == Lst.SolutionsOutputPlan[i].IdEfraz).CodProgMis;

                InstParam[1] = new System.Data.OracleClient.OracleParameter("RUNTH_OPT_RUN_TIME_HIST_ID", System.Data.OracleClient.OracleType.Number);
                InstParam[1].Value = RunInformation.RunId;

                InstParam[2] = new System.Data.OracleClient.OracleParameter("VAL_COST_SEQUENCE_DFSCH", System.Data.OracleClient.OracleType.Number);
                InstParam[2].Value = Lst.SolutionsOutputPlan[i].TotalObj;

                InstParam[3] = new System.Data.OracleClient.OracleParameter("QTY_PRODUCT_IN_PROG_DFSCH", System.Data.OracleClient.OracleType.Number);
                InstParam[3].Value = Lst.SolutionsOutputPlan[i].LstSeqCoil.Count;

                InstParam[4] = new System.Data.OracleClient.OracleParameter("WEI_PRODUCT_IN_PROG_DFSCH", System.Data.OracleClient.OracleType.Number);
                InstParam[4].Value = Lst.SolutionsOutputPlan[i].WeiProg;

                InstParam[5] = new System.Data.OracleClient.OracleParameter("LTH_KM_COIL_IN_PROG_DFSCH", System.Data.OracleClient.OracleType.Number);
                InstParam[5].Value = Lst.SolutionsOutputPlan[i].LenProg;

                InstParam[6] = new System.Data.OracleClient.OracleParameter("OPT_DRAFT_SCH_HEADER_ID", System.Data.OracleClient.OracleType.Number);
                InstParam[6].Value = maxIdDraft;

                InstParam[7] = new System.Data.OracleClient.OracleParameter("DAT_START_PROG_DFSCH", System.Data.OracleClient.OracleType.DateTime);
                InstParam[7].Value = Lst.SolutionsOutputPlan[i].StartTimeSelectProg;

                InstParam[8] = new System.Data.OracleClient.OracleParameter("DAT_END_PROG_DFSCH", System.Data.OracleClient.OracleType.DateTime);
                InstParam[8].Value = Lst.SolutionsOutputPlan[i].EndTimeSelectProg;

                InstParam[9] = new System.Data.OracleClient.OracleParameter("SEQ_PROCESS_PROG_DFSCH", System.Data.OracleClient.OracleType.Number);
                InstParam[9].Value = i + 1;

                InstParam[10] = new System.Data.OracleClient.OracleParameter("COD_PROGRAM_DFSCH", System.Data.OracleClient.OracleType.Number);
                if (RunInformation.NumStation == Station.TanStationId)
                {
                    if (Lst.SolutionsOutputPlan[i].IndexSarfasl == Lst.Sarfasls.Last().IndexSarfasl)
                        InstParam[10].Value = 1;
                    else
                        InstParam[10].Value = 0;
                }
                else
                    InstParam[10].Value = -1;



                DataBase.ExecuteCommand(CommandText, CommandType.Text, InstParam);

                #region insert in table : CMP_OPT_DRAFT_SCH_LINES
                OracleConnection con = new OracleConnection(MODELDataBase.ConncetionString);

                con.Open();

                ArrayList IdSnapshot = new ArrayList();
                ArrayList MaxIdDraft = new ArrayList();
                ArrayList Sequence = new ArrayList();
                for (int j = 0; j < Lst.SolutionsOutputPlan[i].LstSeqCoil.Count; j++)
                {

                    MaxIdDraft.Add(maxIdDraft);
                    IdSnapshot.Add(Lst.Coils[Lst.SolutionsOutputPlan[i].LstSeqCoil[j]].IdSnapshot);
                    Sequence.Add(j + 1);

                }
                OracleParameter idSnapshot = new OracleParameter();
                idSnapshot.OracleDbType = OracleDbType.Double;
                idSnapshot.Value = IdSnapshot.ToArray(typeof(int)) as int[];

                OracleParameter idDraftMax = new OracleParameter();
                idDraftMax.OracleDbType = OracleDbType.Int32;
                idDraftMax.Value = MaxIdDraft.ToArray(typeof(int)) as int[];

                OracleParameter sequenceCoil = new OracleParameter();
                sequenceCoil.OracleDbType = OracleDbType.Int32;
                sequenceCoil.Value = Sequence.ToArray(typeof(int)) as int[];


                int maxRecord = IdSnapshot.Count;
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = "begin apps.App_Cmp_Opt_Model_Pkg.Cmp_insert_coil_program_prc(:idSnapshot,:idDraftMax,:sequenceCoil); end;";
                cmd.ArrayBindCount = maxRecord;
                cmd.Parameters.Add(idSnapshot);
                cmd.Parameters.Add(idDraftMax);
                cmd.Parameters.Add(sequenceCoil);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "commit";
                cmd.ExecuteNonQuery();
                con.Close();
                #endregion

                #region insert in table : CMP_OPT_DRF_SCH_HEAD_COSTS


                for (int cc = 0; cc < 17; cc++)
                {
                    string CommandText2 = "";

                    //insert Comment
                    CommandText2 = sqlWriteTableCMP_OPT_DRF_SCH_HEAD_COSTS();


                    System.Data.OracleClient.OracleParameter[] InstParam2 = new System.Data.OracleClient.OracleParameter[3];

                    InstParam2[0] = new System.Data.OracleClient.OracleParameter("DFSCH_OPT_DRAFT_SCH_HEADER_ID", System.Data.OracleClient.OracleType.Number);
                    InstParam2[0].Value = maxIdDraft;

                    InstParam2[1] = new System.Data.OracleClient.OracleParameter("LKP_COST_DETAIL_PROG_DRSCS", System.Data.OracleClient.OracleType.VarChar);
                    InstParam2[2] = new System.Data.OracleClient.OracleParameter("VAL_COST_DETAIL_PROG_DRSCS", System.Data.OracleClient.OracleType.Number);

                    #region switch
                    switch (cc)
                    {
                        case 0:
                            {
                                InstParam2[1].Value = "CMP_TOTAL_COST";
                                InstParam2[2].Value = Lst.SolutionsOutputPlan[i].TotalObj;
                                break;
                            }

                        case 1:
                            {
                                InstParam2[1].Value = "CMP_CAPPLAN_COST";
                                InstParam2[2].Value = Lst.SolutionsOutputPlan[i].ObjNormalCapPlan ; 
                                break;
                            }

                        case 2:
                            {
                                InstParam2[1].Value = "CMP_CONTINUE_PATTERN_COST";
                                InstParam2[2].Value = Lst.SolutionsOutputPlan[i].ObjNormalContinuePattern;
                                break;
                            }
                        case 3:
                            {
                                InstParam2[1].Value = "CMP_COUNTCOIL_COST";
                                InstParam2[2].Value =  Lst.SolutionsOutputPlan[i].ObjNormalCountProg; 
                                break;
                            }

                        case 4:
                            {
                                InstParam2[1].Value = "CMP_DATLAST_COST";
                                InstParam2[2].Value =  Lst.SolutionsOutputPlan[i].ObjNormalDatlast;  
                                break;
                            }

                        case 5:
                            {
                                InstParam2[1].Value = "CMP_DURABILITY_COST";
                                InstParam2[2].Value = Lst.SolutionsOutputPlan[i].ObjNormalDurability;  
                                break;
                            }
                        case 6:
                            {
                                InstParam2[1].Value = "CMP_LEVELSTORGE_COST";
                                InstParam2[2].Value =  Lst.SolutionsOutputPlan[i].ObjNormalLevelStorge;  
                                break;
                            }
                        case 7:
                            {
                                InstParam2[1].Value = "CMP_LENPROG_COST";
                                InstParam2[2].Value = Lst.SolutionsOutputPlan[i].ObjNormalLenProg;
                                break;
                            }
                        case 8:
                            {
                                InstParam2[1].Value = "CMP_PRIORITY_COST";
                                InstParam2[2].Value = Lst.SolutionsOutputPlan[i].ObjNormalPrior;
                                break;
                            }

                        case 9:
                            {
                                InstParam2[1].Value = "CMP_PRIOR_MIS_PROG_COST";
                                InstParam2[2].Value = Lst.SolutionsOutputPlan[i].ObjNormalPriorGroupTypeMis;
                                break;
                            }

                        case 10:
                            {
                                InstParam2[1].Value = "CMP_PRIOR_STATION_COST";
                                InstParam2[2].Value = Lst.SolutionsOutputPlan[i].ObjNormalPriorStation;
                                break;
                            }

                        case 11:
                            {
                                InstParam2[1].Value = "CMP_SARFASL_COST";
                                InstParam2[2].Value = Lst.SolutionsOutputPlan[i].ObjNormalSarfasl;
                                break;
                            }

                        case 12:
                            {
                                InstParam2[1].Value = "CMP_START_CAMP_COST";
                                InstParam2[2].Value = Lst.SolutionsOutputPlan[i].ObjNormalStartCamp;
                                break;
                            }

                        case 13:
                            {
                                InstParam2[1].Value = "CMP_SETUP_COST";
                                InstParam2[2].Value = Lst.SolutionsOutputPlan[i].ObjNormalSetupCost; 
                                break;
                            }
                        case 14:
                            {
                                InstParam2[1].Value = "CMP_WEIPROG_COST";
                                InstParam2[2].Value = Lst.SolutionsOutputPlan[i].ObjNormalWeiProg;
                                break;
                            }
                        case 15:
                            {
                                InstParam2[1].Value = "CMP_WORKROL_LEN_COST";
                                InstParam2[2].Value = Lst.SolutionsOutputPlan[i].ObjNormalLenWorkRoll;
                                break;
                            }
                        case 16:
                            {
                                InstParam2[1].Value = "CMP_WORKROL_WEI_COST";
                                InstParam2[2].Value = Lst.SolutionsOutputPlan[i].ObjNormalWeiWorkRoll;
                                break;
                            }
                          
                                

                    }
                    #endregion switch

                    DataBase.ExecuteCommand(CommandText2, CommandType.Text, InstParam2);
                }
                #endregion
            }
        }

        private static string sqlWriteTableCMP_OPT_DRAFT_SCH_HEADERS()
        {
            comment = @"insert into cmp.CMP_OPT_DRAFT_SCH_HEADERS(OPT_DRAFT_SCH_HEADER_ID,
                                                                  PGTYP_BAS_PROGRAM_TYPE_DEF_ID,
                                                                  RUNTH_OPT_RUN_TIME_HIST_ID,
                                                                  VAL_COST_SEQUENCE_DFSCH,
                                                                   QTY_PRODUCT_IN_PROG_DFSCH,
                                                                    WEI_PRODUCT_IN_PROG_DFSCH,
                                                                    LTH_KM_COIL_IN_PROG_DFSCH,
                                                                    DAT_START_PROG_DFSCH,
                                                                    DAT_END_PROG_DFSCH,
                                                                    SEQ_PROCESS_PROG_DFSCH,
                                                                    COD_PROGRAM_DFSCH
                                                                    )

                            values( :OPT_DRAFT_SCH_HEADER_ID,
                                    :PGTYP_BAS_PROGRAM_TYPE_DEF_ID,
                                    :RUNTH_OPT_RUN_TIME_HIST_ID,
                                    :VAL_COST_SEQUENCE_DFSCH,
                                    :QTY_PRODUCT_IN_PROG_DFSCH,
                                    :WEI_PRODUCT_IN_PROG_DFSCH,
                                    :LTH_KM_COIL_IN_PROG_DFSCH,
                                    :DAT_START_PROG_DFSCH,
                                    :DAT_END_PROG_DFSCH,
                                    :SEQ_PROCESS_PROG_DFSCH,
                                    :COD_PROGRAM_DFSCH)";
            return comment;
        }

        private static string sqlWriteTableCMP_OPT_DRF_SCH_HEAD_COSTS()
        {
            comment = @"insert into cmp.cmp_opt_drf_sch_head_costs(OPT_DRF_SCH_HEAD_COST_ID,
                                                                                 DFSCH_OPT_DRAFT_SCH_HEADER_ID,
                                                                                 LKP_COST_DETAIL_PROG_DRSCS,
                                                                                 VAL_COST_DETAIL_PROG_DRSCS)
                                             values(cmp.cmp_opt_drf_sch_head_costs_seq.nextval,
                                                    :DFSCH_OPT_DRAFT_SCH_HEADER_ID,
                                                     :LKP_COST_DETAIL_PROG_DRSCS,
                                                      :VAL_COST_DETAIL_PROG_DRSCS)";
            return comment;
        }

   

        //test shavad
        public static void writeOutputQsNew(CommonLists Lst)
        {

            MODELDataBase DataBase = new MODELDataBase();


            OracleConnection con = new OracleConnection(MODELDataBase.ConncetionString);

            con.Open();
            ArrayList CodProgMis = new ArrayList();
            ArrayList NumStation = new ArrayList();
            ArrayList RunIdMas = new ArrayList();
            ArrayList EndTimeSelectProg = new ArrayList();
            ArrayList StartTimeSelectProg = new ArrayList();
            ArrayList SeqProcess = new ArrayList();


            for (int i = 0; i < Lst.SolutionsOutputPlan.Count; i++)
            {

                CodProgMis.Add(Lst.ProgEfrazes.Find(b => b.IdEfraz == Lst.SolutionsOutputPlan[i].IdEfraz).CodProgMis);
                NumStation.Add(RunInformation.NumStation);
                RunIdMas.Add(RunInformation.RunIdMas);
                EndTimeSelectProg.Add(Lst.SolutionsOutputPlan[i].EndTimeSelectProg);
                StartTimeSelectProg.Add(Lst.SolutionsOutputPlan[i].EndTimeSelectProg);
                SeqProcess.Add(i + 1);

                #region
                string comment = string.Format(@" select  mas_qs_solutions_seq.nextval from dual", RunInformation.RunId, RunInformation.NumStation);
                object maxObj = new object();
                DataBase.ExecuteScalar(ref maxObj, comment, CommandType.Text);
                int maxIdDraft = Convert.ToInt32(maxObj);
                DataTable dt = new DataTable();

                for (int j = 0; j < Lst.SolutionsOutputPlan[i].LstSeqCoil.Count; j++)
                {
                    string CommandText1 = "";
                    CommandText1 = string.Format(@"update  mas_initial_mus t
                                                   set t.statn_bas_station_id = {0},
                                                   t.SEQ_SEQUENCE_INITL = {1},
                                                   t.QSSOL_QS_SOLUTION_ID ={2},
                                                   t.DAT_START_TIME_INITL={3},
                                                   t.dat_finish_time_initl={4},
                                                   t.FLG_SCHEDULED_INITL={5}
   
                                                   where t.initial_mu_id ={6}", RunInformation.NumStation, j + 1, maxIdDraft
                                                                            , Lst.Coils[Lst.SolutionsOutputPlan[i].LstSeqCoil[j]].StartProduceTime
                                                                           , Lst.Coils[Lst.SolutionsOutputPlan[i].LstSeqCoil[j]].EndProduceTime
                                                                            , 1, Lst.Coils[Lst.SolutionsOutputPlan[i].LstSeqCoil[j]].IdSnapshot);

                }
                #endregion

            }


            OracleParameter codProgMis = new OracleParameter();
            codProgMis.OracleDbType = OracleDbType.Double;
            codProgMis.Value = CodProgMis.ToArray(typeof(int)) as int[];

            OracleParameter numStation = new OracleParameter();
            numStation.OracleDbType = OracleDbType.Int32;
            numStation.Value = NumStation.ToArray(typeof(int)) as int[];

            OracleParameter runIdMas = new OracleParameter();
            runIdMas.OracleDbType = OracleDbType.Int32;
            runIdMas.Value = RunIdMas.ToArray(typeof(int)) as int[];

            OracleParameter endTimeSelectProg = new OracleParameter();
            endTimeSelectProg.OracleDbType = OracleDbType.Date;
            endTimeSelectProg.Value = EndTimeSelectProg.ToArray(typeof(DateTime)) as DateTime[];

            OracleParameter startTimeSelectProg = new OracleParameter();
            startTimeSelectProg.OracleDbType = OracleDbType.Date;
            startTimeSelectProg.Value = StartTimeSelectProg.ToArray(typeof(DateTime)) as DateTime[];

            OracleParameter seqProcess = new OracleParameter();
            seqProcess.OracleDbType = OracleDbType.Date;
            seqProcess.Value = SeqProcess.ToArray(typeof(int)) as int[];


            int maxRecord = CodProgMis.Count;
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "begin apps.App_Cmp_Opt_Model_Pkg.Cmp_insert_program_QS_prc(:codProgMis,:numStation,:runIdMas,:endTimeSelectProg,:startTimeSelectProg,:seqProcess); end;   ";
            cmd.ArrayBindCount = maxRecord;
            cmd.Parameters.Add(codProgMis);
            cmd.Parameters.Add(numStation);
            cmd.Parameters.Add(runIdMas);
            cmd.Parameters.Add(endTimeSelectProg);
            cmd.Parameters.Add(startTimeSelectProg);
            cmd.Parameters.Add(seqProcess);

            cmd.ExecuteNonQuery();
            cmd.CommandText = "commit";
            cmd.ExecuteNonQuery();
            con.Close();




        }//end of programs

     
        private static void writeTimeRelease(CommonLists Lst)
        {
            #region insert in table : cmp_opt_release_schedules

            OracleConnection con = new OracleConnection(MODELDataBase.ConncetionString);
            con.Open();

            ArrayList NumProg = new ArrayList();
            ArrayList StartTime = new ArrayList();
            ArrayList EndTime = new ArrayList();

            for (int i = 0; i < Lst.ReleaseScheds.Count; i++)
            {
                for (int j = 0; j < Lst.ReleaseScheds[i].LstSeqCoil.Count; j++)
                {
                    NumProg.Add(Lst.ReleaseScheds[i].NumSched);
                    StartTime.Add(Lst.ReleaseScheds[i].StartTimeSelectProg);
                    EndTime.Add(Lst.ReleaseScheds[i].EndTimeSelectProg);
                }
            }
            OracleParameter numProg = new OracleParameter();
            numProg.OracleDbType = OracleDbType.Int32;
            numProg.Value = NumProg.ToArray(typeof(int));

            OracleParameter startTime = new OracleParameter();
            startTime.OracleDbType = OracleDbType.Date;
            startTime.Value = StartTime.ToArray(typeof(DateTime));

            OracleParameter endTime = new OracleParameter();
            endTime.OracleDbType = OracleDbType.Date;
            endTime.Value = EndTime.ToArray(typeof(DateTime));


            int maxRecord = NumProg.Count;
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "begin apps.App_Cmp_Opt_Model_Pkg.Cmp_update_release_program_prc(); end;";
            cmd.ArrayBindCount = maxRecord;
            cmd.Parameters.Add(NumProg);
            cmd.Parameters.Add(StartTime);
            cmd.Parameters.Add(EndTime);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "commit";
            cmd.ExecuteNonQuery();
            con.Close();
            #endregion



        }



        #endregion write in Database

        #region write notepads
        public static void writerCapProg(int number, string name, string pathWriter, int flgWriter, List<CapPlanUpDate> CapPlanUpDates)
        {
            if (flgWriter == 1)
            {
                int z = -1;
                double wei = 0;
                string route;
                string exten = ".txt";

                route = pathWriter + name + exten;
                FileStream fk2;

                fk2 = new FileStream(route, FileMode.Append, FileAccess.Write);
                StreamWriter stream2 = new StreamWriter(fk2);

                
                foreach (var i in CapPlanUpDates)
                {
                    int Pf = i.PfId;
                    if (z != Pf)
                    {
                        z = Pf;
                        wei = i.RespondProgPf;
                        if (wei > 0)
                        {
                            stream2.Write(Convert.ToString(number) + "\t" + Convert.ToString(Pf) + "\t" + Convert.ToString(wei));
                            stream2.WriteLine();
                        }
                    }
                }
                //stream2.WriteLine();
                stream2.Close();
            }
        }

        protected static void creatNote(string route, string pathWriter, int flgWriter)
        {
            if (flgWriter == 1)
            {
                string exten = ".txt";
                string r = pathWriter + route + exten;
                FileStream fk = new FileStream(r, FileMode.Create, FileAccess.Write);
                StreamWriter stream2 = new StreamWriter(fk);
                stream2.Close();
            }
        }

        public static void writeFileLogger(FileLogger fileLogger, string name, string pathWriter, int flgWriter)
        {
            if (flgWriter == 1)
            {
                string route;
                string exten = ".txt";
                route = pathWriter + name + exten;
                FileStream fk2;
                string[] ss = new string[1000];

                ss = fileLogger.LogComment[0].Split(',');
                          
                fk2 = new FileStream(route, FileMode.Append, FileAccess.Write);

                StreamWriter stream2 = new StreamWriter(fk2);

                //int count = 0;
                for (int i = 0; i < fileLogger.LogComment.Count; i++)
                {
                   // ss = fileLogger.LogComment[i].Split(',');
                    
                   // double g=ss.LongCount(c=>c!="");
                    
                    //for (int k = 0; k <g ; k++)
                    //{
                        //if (ss[k] != null && ss[k] != "*" && ss[k] !="\n")
                        //{
                        //    count++;
                        //    stream2.Write(count + ":");
                        //}

                        //stream2.Write(ss[k]);

                    stream2.Write(fileLogger.LogComment[i]);
                        stream2.WriteLine();
                   // }
                  
                }
                stream2.Close();
            }
            
        }
   

           #endregion write notepads

    }
}
