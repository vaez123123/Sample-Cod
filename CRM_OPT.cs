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



    }
}
