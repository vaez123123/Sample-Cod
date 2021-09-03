using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;
using System.Data.OracleClient;
using IPSO.CMP.CommonFunctions.ParameterClasses;


namespace SkinPass1Scheduling
{
    public partial class SkinPass1Model
    {
        public SKINPASS1DataBase WorkData = new SKINPASS1DataBase();
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        # region READ DATA

        public void readDataBase()
        {

            // readorder();
            //num_order = OrderList.Count;

        }

       


        //ALL
        

        # endregion


        # region WRITE DATA


        public void writeOutput()
        {

            SKINPASS1DataBase DataBase = new SKINPASS1DataBase();
            string CommandText = "";
            CommandText = @"insert into CMP_OPT_DRAFT_SCH_HEADERS(OPT_DRAFT_SCH_HEADER_ID,
                                                                  PGTYP_BAS_PROGRAM_TYPE_DEF_ID,
                                                                  RUNTH_OPT_RUN_TIME_HIST_ID,
                                                                  VAL_COST_SEQUENCE_DFSCH,
                                                                   QTY_PRODUCT_IN_PROG_DFSCH,
                                                                    WEI_PRODUCT_IN_PROG_DFSCH,
                                                                    LTH_KM_COIL_IN_PROG_DFSCH)

                            values(CMP_OPT_DRAFT_SCH_HEADERS_seq.nextval,
                                    :PGTYP_BAS_PROGRAM_TYPE_DEF_ID,
                                    :RUNTH_OPT_RUN_TIME_HIST_ID,
                                    :VAL_COST_SEQUENCE_DFSCH,
                                    :QTY_PRODUCT_IN_PROG_DFSCH,
                                    :WEI_PRODUCT_IN_PROG_DFSCH,
                                    :LTH_KM_COIL_IN_PROG_DFSCH)";





            for (int i = 0; i < SolutionsOutputPlan.Count; i++)
            {
                int maxIdDraft = -1;
                OracleParameter[] InstParam = new OracleParameter[6];

                InstParam[0] = new OracleParameter("PGTYP_BAS_PROGRAM_TYPE_DEF_ID", OracleType.Number);
                InstParam[0].Value = ProgEfrazes.Find(b => b.idAfraz == SolutionsOutputPlan[i].IdEfraz).codProgMis;

                InstParam[1] = new OracleParameter("RUNTH_OPT_RUN_TIME_HIST_ID", OracleType.Number);
                InstParam[1].Value = DataBase.runId;

                InstParam[2] = new OracleParameter("VAL_COST_SEQUENCE_DFSCH", OracleType.Number);
                InstParam[2].Value = SolutionsOutputPlan[i].TotalObj;

                InstParam[3] = new OracleParameter("QTY_PRODUCT_IN_PROG_DFSCH", OracleType.Number);
                InstParam[3].Value = SolutionsOutputPlan[i].LstSeqCoil.Count;

                InstParam[4] = new OracleParameter("WEI_PRODUCT_IN_PROG_DFSCH", OracleType.Number);
                InstParam[4].Value = SolutionsOutputPlan[i].WeiProg;

                InstParam[5] = new OracleParameter("LTH_KM_COIL_IN_PROG_DFSCH", OracleType.Number);
                InstParam[5].Value = SolutionsOutputPlan[i].LenProg;

                DataBase.ExecuteCommand(CommandText, CommandType.Text, InstParam);

                #region

                dt = new DataTable();

                string comment = string.Format(@" select max(t.opt_draft_sch_header_id) from cmp.cmp_opt_draft_sch_headers t");
                object maxObj = new object();
                DataBase.ExecuteScalar(ref maxObj, comment, CommandType.Text);
                maxIdDraft = Convert.ToInt32(maxObj);


                #endregion

                for (int j = 0; j < SolutionsOutputPlan[i].LstSeqCoil.Count; j++)
                {

                    string CommandText1 = "";
                    CommandText1 = @"insert into CMP_OPT_DRAFT_SCH_LINES(OPT_DRAFT_SCH_LINE_ID,
                                                                         DFSCH_OPT_DRAFT_SCH_HEADER_ID,
                                                                         SNAMU_OPT_SNAPSHOT_MU_ID,
                                                                         SEQ_PRODUCT_DRSCL)
                                     values(CMP_OPT_DRAFT_SCH_HEADERS_seq.nextval,
                                            :DFSCH_OPT_DRAFT_SCH_HEADER_ID,
                                             :SNAMU_OPT_SNAPSHOT_MU_ID,
                                              :SEQ_PRODUCT_DRSCL)";


                    OracleParameter[] InstParam1 = new OracleParameter[3];

                    InstParam1[0] = new OracleParameter("DFSCH_OPT_DRAFT_SCH_HEADER_ID", OracleType.Number);
                    InstParam1[0].Value = maxIdDraft;

                    InstParam1[1] = new OracleParameter("SNAMU_OPT_SNAPSHOT_MU_ID", OracleType.Double);

                    InstParam1[1].Value = Coils[SolutionsOutputPlan[i].LstSeqCoil[j]].idSnapshot;

                    InstParam1[2] = new OracleParameter("SEQ_PRODUCT_DRSCL", OracleType.Number);
                    InstParam1[2].Value = j;

                    DataBase.ExecuteCommand(CommandText1, CommandType.Text, InstParam1);



                }


            }



        }



        //public static void Insert_ndc_out()
        //{
        //    int m = 0, n = 0;
        //    DateTime time = DateTime.Now;// Use current time
        //    PersianCalendar pc = new PersianCalendar();
        //    int year = pc.GetYear(DateTime.Now);
        //    int month = pc.GetMonth(DateTime.Now);
        //    int day = pc.GetDayOfMonth(DateTime.Now);
        //    int hour = pc.GetHour(DateTime.Now);
        //    int minit = pc.GetMinute(DateTime.Now);
        //    int sec = pc.GetSecond(DateTime.Now);
        //    double x = year * Math.Pow(10, 10) + month * Math.Pow(10, 8) + day * Math.Pow(10, 6) + hour * Math.Pow(10, 4) + minit * 100 + sec;
        //    DateTime current_time = new DateTime(year, month, day, hour, minit, sec);
        //    string gg = String.Format("{0:0.##}", x);
        //    string CommandText = "";
        //    TANDEMDataBase DataBase = new TANDEMDataBase();
        //    CommandText = "delete from opl_ndc_out";
        //    DataBase.ExecuteCommand(CommandText, CommandType.Text);
        //    CommandText = "insert into opl_ndc_out(order_id, station_id, ndc, user_run, date_run)" +
        //   " values(:order_id, :station_id, :ndc, :user_run, :date_run)";
        //    for (int i = 0; i < num_order; i++)
        //    {
        //        n = OrderList[i].n;
        //        for (int s = 0; s < num_station - 1; s++)
        //            for (int j = 1; j < num_station; j++)
        //                if (B[n, s, j] == 1)
        //                {
        //                    for (int k = 0; k < num_station; k++)
        //                        if (StationList[k].model_index == j)
        //                        {
        //                            m = StationList[k].station_id;
        //                            break;
        //                        }
        //                    OracleParameter[] InstParam = new OracleParameter[5];
        //                    InstParam[0] = new OracleParameter("order_id", OracleType.Number);
        //                    InstParam[0].Value = OrderList[i].order_id;
        //                    InstParam[1] = new OracleParameter("station_id", OracleType.Number);
        //                    InstParam[1].Value = m;
        //                    InstParam[2] = new OracleParameter("ndc", OracleType.Double);
        //                    if (OrderList[i].netdemand[j] > 1)
        //                        InstParam[2].Value = OrderList[i].netdemand[j];
        //                    else
        //                        InstParam[2].Value = 0;
        //                    InstParam[3] = new OracleParameter("user_run", OracleType.NVarChar);
        //                    InstParam[3].Value = "kavosh";
        //                    InstParam[4] = new OracleParameter("date_run", OracleType.NVarChar);
        //                    InstParam[4].Value = gg; ;
        //                    DataBase.ExecuteCommand(CommandText, CommandType.Text, InstParam);
        //                }//end if 

        //    }//end for i
        //    CommandText = "update ipso_bas_initial  set  edit_order_id = :edit_order_id, EDIT_ORDER_USER = :EDIT_ORDER_USER," +
        //           "EDIT_ORDER_DATE = :EDIT_ORDER_DATE, EDIT_ORDER_MODULE = :EDIT_ORDER_MODULE where MU_ID=:MU_ID";
        //    OracleParameter[] UPDParam = new OracleParameter[5];
        //    for (int i = 0; i < num_mu; i++)
        //        if (MuList[i].flag_edit == 1)
        //        {
        //            UPDParam[0] = new OracleParameter("EDIT_ORDER_ID", OracleType.Number);
        //            UPDParam[0].Value = MuList[i].order_id;
        //            UPDParam[1] = new OracleParameter("EDIT_ORDER_USER", OracleType.NVarChar);
        //            UPDParam[1].Value = "kavosh";
        //            UPDParam[2] = new OracleParameter("EDIT_ORDER_DATE", OracleType.Number);
        //            UPDParam[2].Value = gg;
        //            UPDParam[3] = new OracleParameter("EDIT_ORDER_MODULE", OracleType.NVarChar);
        //            UPDParam[3].Value = "NDC";
        //            UPDParam[4] = new OracleParameter("MU_ID", OracleType.Number);
        //            UPDParam[4].Value = MuList[i].mu_id;
        //            DataBase.ExecuteCommand(CommandText, CommandType.Text, UPDParam);
        //        }
        //}


        # endregion




    }
}
