using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IPSO.CMP.CommonFunctions.ParameterClasses;

using MasQSCommonFunctions;
using IPSO.CMP.DataBaseMODEL;

namespace IPSO.CMP.CommonFunctions.Functions
{
    public class ReaderFunc
    {
        public MODELDataBase WorkData = new MODELDataBase();
        DataTable dt = new DataTable();
        MasQSCommonFunctions.ClsQSDatas qs = new MasQSCommonFunctions.ClsQSDatas();


        public string readCommonData(CommonLists Lst,string pathWriter, int counterCoil, int counterCoilRelease, int counterCoilReleaseOtherSt
           , string widAtt, string TksAtt, string TksOutAtt, string Prog, string SurfaceRoughAtt, string trim, string oil
           , string typ_Product_Family_GAL)
        {
            if (RunInformation.FlgOptSim == 1)
            {
                qs.masRunId = RunInformation.RunIdMas;
                qs.crmRunId = RunInformation.RunId;
                qs.stationId = RunInformation.NumStation;
                qs.numStand = RunInformation.NumStand;
                qs.readPeriod();
            }            

            readParameter(widAtt, TksAtt, TksOutAtt, Prog, SurfaceRoughAtt, trim, oil, typ_Product_Family_GAL);
           
            readCoils(Lst.Coils, counterCoil, Lst.SolutionsOutputPlan);
            LogForCommon.printWarnings(Lst, pathWriter, "readCoils");
            if (RunInformation.flgStopAlgorithm == -1)
                return InnerParameter.errorMessage;
            readReleaseCoil(Lst.ReleaseScheds, Lst.CoilReleases, counterCoilRelease, RunInformation.NumStation);              
            readReleaseCoil(Lst.ReleaseSchedsOtherStation, Lst.CoilReleasesOtherSt, counterCoilReleaseOtherSt, RunInformation.NumStationOther);
            LogForCommon.printWarnings(Lst, pathWriter, "readReleaseCoil");
            readProgEfraz(Lst.ProgEfrazes, Lst.Coils, Lst.CoilReleases, Lst.CoilReleasesOtherSt);
            LogForCommon.printWarnings(Lst, pathWriter, "readProgEfraz");            
            if (RunInformation.flgStopAlgorithm == -1)
                return InnerParameter.errorMessage;
            readMaxValue();
            readMaxSetup();
            readCapPlan(ref Lst.CapPlans, Lst.Coils, ref Lst.CapPlansCurr, Lst.CapPlanUpDates, Lst.DBDCampPlans);
            LogForCommon.printWarnings(Lst, pathWriter, "readCapPlan");
            if (RunInformation.flgStopAlgorithm == -1)
                return InnerParameter.errorMessage;
            readShift(Lst.ShiftWorks);
            readSetup(Lst.ProgEfrazes, Lst.Setups);
            readTksJump(Lst.TksJumps);
            LogForCommon.printWarnings(Lst, pathWriter, "readTksJump");
            readWidJump(Lst.WidthJumps);
            LogForCommon.printWarnings(Lst, pathWriter, "readWidJump");
            readRoll(Lst.RollsWork, Lst.RollsBack, Lst.Coils);
            LogForCommon.printWarnings(Lst, pathWriter, "readRoll");
            readWeightObjective();
            readMaxValueGroup(Lst.MaxValueGroups);
           // readEquipGroupFailureTime(EquipGroupFailureTimes);
            readSarfasl(Lst.Sarfasls, ref Lst.lstReadAllGroupDef);
            LogForCommon.printWarnings(Lst, pathWriter, "readSarfasl");
            if (RunInformation.flgStopAlgorithm == -1)
                return InnerParameter.errorMessage;
            readGroupDef(Lst.GroupDefs, Lst.ProgEfrazes,Lst.lstReadAllGroupDef);
            LogForCommon.printWarnings(Lst, pathWriter, "readGroupDef");
            if (RunInformation.flgStopAlgorithm == -1)
                return InnerParameter.errorMessage;            
            readJumpBetweenCrown(Lst.Sarfasls, Lst.JumpBetweenCrowns);
            LogForCommon.printWarnings(Lst, pathWriter, "readJumpBetweenCrown");
            return InnerParameter.errorMessage;
        }


        public void readCommonDataTemp(CommonLists Lst, int counterCoil, int counterCoilRelease, int counterCoilReleaseOtherSt
        , string widAtt, string TksAtt, string TksOutAtt, string Prog, string SurfaceRoughAtt, string trim, string oil
        , string typ_Product_Family_GAL)
        {
            if (RunInformation.FlgOptSim == 1)
            {
                qs.masRunId = RunInformation.RunIdMas;
                qs.crmRunId = RunInformation.RunId;
                qs.stationId = RunInformation.NumStation;
                qs.numStand = RunInformation.NumStand;
                qs.readPeriod();
            }
            
            readParameter(widAtt, TksAtt, TksOutAtt, Prog, SurfaceRoughAtt, trim, oil, typ_Product_Family_GAL);

            TimeParameter.timereadCoil.Start();
            readCoils(Lst.Coils, counterCoil, Lst.SolutionsOutputPlan);
            TimeParameter.timereadCoil.Stop();

            TimeParameter.timereadReleaseCoil.Start();
            readReleaseCoil(Lst.ReleaseScheds, Lst.CoilReleases, counterCoilRelease, RunInformation.NumStation);
            TimeParameter.timereadReleaseCoil.Stop();

            TimeParameter.timereadReleaseCoilOtherSt.Start();
            readReleaseCoil(Lst.ReleaseSchedsOtherStation, Lst.CoilReleasesOtherSt, counterCoilReleaseOtherSt, RunInformation.NumStationOther);
            TimeParameter.timereadReleaseCoilOtherSt.Stop();

            TimeParameter.timereadProgAFraz.Start();
            readProgEfraz(Lst.ProgEfrazes, Lst.Coils, Lst.CoilReleases, Lst.CoilReleasesOtherSt);
            TimeParameter.timereadProgAFraz.Stop();

            TimeParameter.timereadMaxValue.Start();
            readMaxValue();
            TimeParameter.timereadMaxValue.Stop();

            TimeParameter.timereadMaxSetup.Start();
            readMaxSetup();
            TimeParameter.timereadMaxSetup.Stop();

            TimeParameter.timereadCapPlan.Start();
            readCapPlan(ref Lst.CapPlans, Lst.Coils, ref Lst.CapPlansCurr, Lst.CapPlanUpDates, Lst.DBDCampPlans);
            TimeParameter.timereadCapPlan.Stop();

            TimeParameter.timereadShift.Start();
            readShift(Lst.ShiftWorks);
            TimeParameter.timereadShift.Stop();

            TimeParameter.timereadSetup.Start();
            readSetup(Lst.ProgEfrazes, Lst.Setups);
            TimeParameter.timereadSetup.Stop();

            TimeParameter.timereadTksJump.Start();
            readTksJump(Lst.TksJumps);
            TimeParameter.timereadTksJump.Stop();

            TimeParameter.timereadJumpWid.Start();
            readWidJump(Lst.WidthJumps);
            TimeParameter.timereadJumpWid.Stop();

            TimeParameter.timereadRoll.Start();
            readRoll(Lst.RollsWork, Lst.RollsBack, Lst.Coils);
            TimeParameter.timereadRoll.Stop();

            TimeParameter.timereadWeightObjective.Start();
            readWeightObjective();
            TimeParameter.timereadWeightObjective.Start();

            TimeParameter.timereadMaxValueGroup.Start();
            readMaxValueGroup(Lst.MaxValueGroups);
            TimeParameter.timereadMaxValueGroup.Stop();

            //readEquipGroupFailureTime(EquipGroupFailureTimes);

            TimeParameter.timereadGroupDef.Start();
            readGroupDefTem(Lst.GroupDefs, Lst.ProgEfrazes);
            TimeParameter.timereadGroupDef.Stop();

            TimeParameter.timereadSarfasl.Start();
            readSarfaslTem(Lst.Sarfasls);
            TimeParameter.timereadSarfasl.Stop();

            TimeParameter.timereadJumpBetweenCrown.Start();
            readJumpBetweenCrown(Lst.Sarfasls, Lst.JumpBetweenCrowns);
            TimeParameter.timereadJumpBetweenCrown.Stop();

        }

    


        private void readParameter(string widAtt, string TksAtt, string TksOutAtt, string Prog, string SurfaceRoughAtt, string trim, string oil, string typ_Product_Family_GAL)
        {
            Parameter.InitializeLogicals(1, 2, 3, 4, 5);
            Parameter.InitiliazeAttributes("", widAtt, TksAtt, TksOutAtt, Prog, SurfaceRoughAtt, trim, oil, typ_Product_Family_GAL);
            Parameter.InitiliazeOtherVariables(0.01, 30 * 1000, 99997, 99998, 99999, 100000, -100, 1, Status.CurrTime.AddDays(300),
               Status.CurrTime, Status.CurrTime, -1, 1001, 1300, 30, 300);
            Parameter.InitiliazeCoeficientDatabase();
        }

        private void readCoils(List<Coil> Coils, int counterCoil, List<Solution> SolutionsOutputPlan)
        {

            string comment = "";
            dt = new DataTable();

            if (RunInformation.FlgOptSim == 1)

                comment = qs.readCoils();
            else

                #region
                comment = string.Format(@"select c.NUM_PROD_SNAMU,
                                                   c.opt_snapshot_mu_id,
                                                   c.Cod_Order_Snamu,
                                                   c.Wid_Prod_Snamu,
                                                   c.Tks_Prod_Snamu,
                                                   c.Qty_Fal_Snamu,
                                                   c.WEI_PROD_SNAMU,
                                                   c.LTH_PROD_SNAMU,
                                                   substr(c.POS_POSITION_PROD_SNAMU, 10, 1)  position,
                                                   c.DAT_AVAILABLE_TIME_SNAMU,

                                                   o.COD_URG_ORINF,
                                                   o.DAT_DLV_ORDER_ORINF,
                                                   o.COD_PRODUCT_FAMILY_ITEM_ORINF,
                                                   o.cod_int_qual_orinf,
                                                   (select t.flg_phpsphorus_qtydt from mas.mas_bas_quality_datas t where t.statn_bas_station_id = {0}
                                                   and t.cod_quality_qtydt = o.cod_int_qual_orinf) as flg_phpsphorus_qtydt,
                                                    o.COD_REDUCTION_ORDER_ORINF,
                                                    o.cod_surface_roghness_orinf,
                                                    o.Cod_Roughness_Categories_Orinf,
                                                    o.COD_COAT_ORINF,
                                                    o.COD_COAT_BUT_ORINF,

                                                   os.cod_sepration_ostif,
                                                   os.WID_OUT_OSTIF,
                                                   os.TKS_OUT_OSTIF,
                                                   os.QTY_PDW_OSTIF,
                                                   os.TYP_PROG_OSTIF,
                                                   (select tt.bas_program_type_def_id from mas.mas_bas_program_type_defs tt 
                                                   where tt.nam_program_type_pgtyp =  os.TYP_PROG_OSTIF and tt.statn_bas_station_id = {0}) as cod_prog_mis,

                                                   os.num_tri_ostif,
                                                   os.typ_oil_ostif,

                                                  (/*select to_char(Max(Dat_Calde),'yyyy/mm/dd','NLS_CALENDAR=persian') 
                                                            from  AAC.AAC_CALENDAR_DETAILS ACD

                                                            Where ACD.CALNR_LKP_COD_CAL_CALNR = 'A'
                                                            And to_char(Dat_Calde, 'yyyy', 'NLS_CALENDAR=persian') 
                                                            || LPAD((Num_Week_Year_Calde), 2, '0') = os.NUM_DAT_LAST_ORDER_OSTIF */
                                                  select  max(cal.Dat_Calde)
                                                  from cmp.cmp_opt_order_station_infos os, apps.aac_lmp_CALENDAR_viw   CAL
                                                  Where  cal.Num_Week_Year_Ppc_Calde = os.num_dat_last_order_ostif
                                                  ) as NUM_DAT_LASTsh_ORDER_OSTIF,
                
                                                   nvl((select  max(cal.Dat_Calde)
                                                   from cmp.cmp_opt_order_station_infos os, apps.aac_lmp_CALENDAR_viw   CAL
                                                   Where  cal.Num_Week_Year_Ppc_Calde = os.num_dat_last_order_ostif),o.dat_dlv_order_orinf
                                                   ) as NUM_DAT_LAST_ORDER_OSTIF

                                              from cmp.cmp_opt_snapshot_mus  c, cmp.cmp_opt_order_infos o, cmp.cmp_opt_order_station_infos os

                                             where c.ORINF_OPT_ORDER_INFO_ID = o.opt_order_info_id
                                               and o.opt_order_info_id = os.Orinf_Opt_Order_Info_Id
                                               and (c.statn_bas_station_id_uses = {0} or c.statn_bas_station_id_uses_a = {0} or
                                                    c.statn_bas_station_id_uses_b = {0} or c.statn_bas_station_id_uses_c = {0} or
                                                    c.statn_bas_station_id_uses_d = {0})
                                               and os.Statn_Bas_Station_Id ={0}
                                               and c.runth_opt_run_time_hist_id = {1}
                                               and o.RUNTH_OPT_RUN_TIME_HIST_ID = {1}
                                               and os.RUNTH_OPT_RUN_TIME_HIST_ID = {1}
                                               and c.TYP_PROGRAM_TYPE_SNAMU=1                                                
                                               and (nvl(o.Cod_Suspend_Ship_Orinf,-1) != 1)
                                               and (nvl(o.Cod_Suspend_Prod_Orinf, -1) != 1)
                                               and (nvl(os.FLG_SUSPEND_PRO_OSTIF,-1) != 1)
                                                
                                               order by c.wid_prod_snamu desc , c.cod_order_snamu desc, c.opt_snapshot_mu_id desc",
                                                                             RunInformation.NumStation,RunInformation.RunId);
                #endregion

            dt = WorkData.GetDataTable(comment, CommandType.Text);

            foreach (DataRow dr in dt.Rows)
                addCoil(dr, Coils, ref counterCoil);

            RunInformation.chekFlagStopAlgorithm(Coils, SolutionsOutputPlan);
        }

        private void readReleaseCoil(List<ReleaseSched> ReleaseSchedsLoc, List<CoilRelease> CoilReleases, int counterCoilRelease, int NumStation)
        {
            string comment = "";
            dt = new DataTable();

            if (RunInformation.FlgOptSim == 1)
                comment = qs.readReleaseCoil();
            else
                comment = string.Format(@"select distinct (c.NUM_PROD_SNAMU),
                                                   c.opt_snapshot_mu_id,
                                                   c.Cod_Order_Snamu,
                                                   c.Wid_Prod_Snamu,
                                                   c.Tks_Prod_Snamu,
                                                   c.Qty_Fal_Snamu,
                                                   c.WEI_PROD_SNAMU,
                                                   c.LTH_PROD_SNAMU,
                                                   c.seq_prod_snamu,
                                                 
                                                   os.cod_sepration_ostif,
                                                   os.WID_OUT_OSTIF,
                                                   os.TKS_OUT_OSTIF,
                                                   os.QTY_PDW_OSTIF,
                                                   os.TYP_PROG_OSTIF,
                                                   (select tt.bas_program_type_def_id from mas.mas_bas_program_type_defs tt 
                                                   where tt.nam_program_type_pgtyp =  os.TYP_PROG_OSTIF and tt.statn_bas_station_id = {0}) as cod_prog_mis,

                                                   os.num_tri_ostif,
                                                   os.typ_oil_ostif,
                                                   o.cod_surface_roghness_orinf,

                                                   o.COD_PRODUCT_FAMILY_ITEM_ORINF,
                                                   o.cod_int_qual_orinf,
                                                   (select t.flg_phpsphorus_qtydt from mas.mas_bas_quality_datas t where t.statn_bas_station_id = {0}
                                                   and t.cod_quality_qtydt = o.cod_int_qual_orinf) as flg_phpsphorus_qtydt,                                                     

                                                   nvl(t.SEQ_PLAN_RELSC,0) as SEQ_PLAN_RELSC,                                                    
                                                   nvl(t.STA_RELEAS_PLAN_RELSC,1) as STA_RELEAS_PLAN_RELSC,
                                                   nvl(t.WEI_PLAN_RELSC,0) as WEI_PLAN_RELSC,
                                                   nvl(t.LTH_PLAN_RELSC,0) as LTH_PLAN_RELSC,
                                                   nvl(t.NUM_PROG_TYPE_RELSC,0)as NUM_PROG_TYPE_RELSC
                                                    
                                                from cmp.cmp_opt_release_schedules t, cmp.cmp_Opt_Snapshot_Mus c,
                                                     cmp.cmp_opt_order_infos  o, cmp.cmp_opt_order_station_infos  os

                                                where t.statn_bas_station_id = {0}
                                                and (c.statn_bas_station_id_uses = {0} or c.statn_bas_station_id_uses_a = {0} or
                                                    c.statn_bas_station_id_uses_b = {0} or c.statn_bas_station_id_uses_c = {0} or
                                                    c.statn_bas_station_id_uses_d = {0})
                                                and os.statn_bas_station_id = {0}
                                                and t.runth_opt_run_time_hist_id = {1}
                                                and c.runth_opt_run_time_hist_id = t.runth_opt_run_time_hist_id
                                                and o.runth_opt_run_time_hist_id = t.runth_opt_run_time_hist_id
                                                and os.runth_opt_run_time_hist_id = t.runth_opt_run_time_hist_id
                                                and t.opt_release_schedule_id = c.relsc_opt_release_schedule_id
                                                and o.cod_order_orinf = c.cod_order_snamu
                                                and os.cod_order_ostif = c.cod_order_snamu
                                                and c.TYP_PROGRAM_TYPE_SNAMU = 2

                                                order by seq_plan_relsc, c.seq_prod_snamu "
                                                      , NumStation, RunInformation.RunId);

            dt = WorkData.GetDataTable(comment, CommandType.Text);

            int z = -1;

            foreach (DataRow dr in dt.Rows)
            {
                if (z == int.Parse(dr["seq_plan_relsc"].ToString()))
                {
                    ReleaseSchedsLoc.Last().LstSeqCoil.Add(counterCoilRelease);
                    addCoilRelease(dr, CoilReleases, ref counterCoilRelease);
                }
                else
                {
                    z = int.Parse(dr["seq_plan_relsc"].ToString());

                    addRelease(dr, ReleaseSchedsLoc);

                    ReleaseSchedsLoc.Last().LstSeqCoil.Add(counterCoilRelease);
                    addCoilRelease(dr, CoilReleases, ref counterCoilRelease);

                }
            }
        }

        private void readProgEfraz(List<ProgEfraz> ProgEfrazes, List<Coil> Coils, List<CoilRelease> CoilReleases, List<CoilRelease> CoilReleasesOtherSt)
        {
            string comment = "";
            dt = new DataTable();

            if (RunInformation.FlgOptSim == 1)
                comment = qs.readProgEfraz();
            else
                comment = string.Format(@"select t.cod_seprate_prog_sptyp,
                                               w.nam_program_type_pgtyp,
                                               t.nam_latin_sptyp,
                                              t.val_attribute_sptyp,
                                               t.typ_rel_op_sptyp,
                                                nvl(w.val_max_length_pgtyp,0) as val_max_length_pgtyp,
                                                nvl(w.val_max_weight_pgtyp,0) as val_max_weight_pgtyp,
                                                nvl(w.val_max_number_mu_pgtyp,0) as val_max_number_mu_pgtyp,
                                                nvl(w.val_min_number_mu_pgtyp,0) as val_min_number_mu_pgtyp,
                                               nvl(w.flg_nigth_shift_pgtyp, 0) as flg_night,
                                               nvl(w.val_max_num_wid_change_pgtyp, 0) as max_change_wid,
                                                nvl(t.PGTYP_BAS_PROGRAM_TYPE_DEF_ID,0) as PGTYP_BAS_PROGRAM_TYPE_DEF_ID,
                                               t.VAL_ATTRIBUTE2_SPTYP
                                          from mas.mas_sepration_types t,
                                            apps.mas_cmp_program_type_def_viw w
                                         where t.STATN_BAS_STATION_ID ={0}
                                           and t.pgtyp_bas_program_type_def_id= w.program_id
                                 order by t.PGTYP_BAS_PROGRAM_TYPE_DEF_ID,
                                          t.cod_seprate_prog_sptyp,
                                          t.grpdt_sop_group_data_def_id", RunInformation.NumStation);

            dt = WorkData.GetDataTable(comment, CommandType.Text);

            int z = -1;

            foreach (DataRow dr in dt.Rows)
            {
                if (z == int.Parse(dr["cod_seprate_prog_sptyp"].ToString()))
                {
                    if (int.Parse(dr["typ_rel_op_sptyp"].ToString()) == Parameter.Equal)
                    {
                        string Efrazlocal = dr["nam_latin_sptyp"].ToString().ToUpper();

                        if (Efrazlocal == Parameter.TrimAttribute)
                        {
                            ProgEfrazes.Last().Trim.Add(Convert.ToInt32(dr["val_attribute_sptyp"]));
                        }

                        else if (Efrazlocal == Parameter.OilAttribute)
                        {
                            ProgEfrazes.Last().Oil = Convert.ToInt32(dr["val_attribute_sptyp"]);
                        }
                    }
                    
                }
                else
                {
                    z = int.Parse(dr["cod_seprate_prog_sptyp"].ToString());

                    ProgEfraz b = new ProgEfraz();
                    b.IdEfraz = Convert.ToInt32(dr["cod_seprate_prog_sptyp"]);
                    b.ProgMis = dr["nam_program_type_pgtyp"].ToString().ToUpper();
                    b.CodProgMis = Convert.ToInt32(dr["PGTYP_BAS_PROGRAM_TYPE_DEF_ID"]);
                    b.CountOpt = Convert.ToInt32(dr["val_max_number_mu_pgtyp"]);
                    b.CountMin = Convert.ToInt32(dr["val_min_number_mu_pgtyp"]);
                    b.LenOpt = Convert.ToInt32(dr["val_max_length_pgtyp"]);
                    b.WeiOpt = Convert.ToInt32(dr["val_max_weight_pgtyp"]);
                    b.FlgNightPlan = Convert.ToInt32(dr["flg_night"]);
                    b.MaxNumChangeWid = Convert.ToInt16(dr["max_change_wid"]);

                    ProgEfrazes.Add(b);

                    int rel = Convert.ToInt32(dr["typ_rel_op_sptyp"].Equals(DBNull.Value) ? "0" : dr["typ_rel_op_sptyp"]);

                    if (rel == Parameter.Equal)
                    {
                        string Efrazlocal = dr["nam_latin_sptyp"].ToString().ToUpper();

                        if (Efrazlocal == Parameter.TrimAttribute)
                        {
                            ProgEfrazes.Last().Trim.Add(Convert.ToInt32(dr["val_attribute_sptyp"]));
                        }

                        else if (Efrazlocal == Parameter.OilAttribute)
                        {
                            ProgEfrazes.Last().Oil = Convert.ToInt32(dr["val_attribute_sptyp"]);
                        }
                    }
                    
               

                }

            }//foreach dt

            //ProgEfrazes = ProgEfrazes.Distinct().ToList();

            List<int> IdEfrazLoc = Coils.Select(a => a.IdEfraz).Distinct().ToList();

            IdEfrazLoc.AddRange(CoilReleases.Select(a => a.IdEfraz).Distinct());

            IdEfrazLoc.AddRange(CoilReleasesOtherSt.Select(a => a.IdEfraz).Distinct());

            ProgEfrazes.RemoveAll(a => !IdEfrazLoc.Contains(a.IdEfraz));

               ProgEfrazes = ProgEfrazes.OrderBy(a => a.IdEfraz).ToList();


        }

        private void readTksJump(List<TksJump> TksJumps)
        {
            string comment = "";
            dt = new DataTable();

            float z = -1;
            float x = -1;

            if (RunInformation.FlgOptSim == 1)
                comment = qs.readTksJump();


            else
                comment = string.Format(@"select t.tks_coil_befor_from_tkjum,
                                               t.tks_coil_befor_to_tkjum,
                                               t.tks_max_jump_tkjum,
                                               t.tks_pcn_max_jump_tkjum,
                                               t.tks_limit_jump_tkjum ,
                                               t.flg_in_out_tkjum,
                                               t.pgtyp_bas_program_type_def_id,
                                               (select p.nam_program_type_pgtyp from mas.mas_bas_program_type_defs p
                                                where p.bas_program_type_def_id = t.pgtyp_bas_program_type_def_id) as mis_prog
                                          from mas.mas_bas_tks_jump_consts t
                                           where t.STATN_BAS_STATION_ID={0}
                                          order by t.tks_coil_befor_from_tkjum, t.tks_coil_befor_to_tkjum", RunInformation.NumStation);

            dt = WorkData.GetDataTable(comment, CommandType.Text);


            foreach (DataRow dr in dt.Rows)
            {
                addTksJump(dr, TksJumps);


                float tksFrom = TksJumps.Last().TksFrom;
                float tksTo = TksJumps.Last().TksTo;

                z = tksFrom;
                x = tksTo;

                int flg = TksJumps.Last().FlgInputOutput;
                if (flg == 0)// inputTks
                {
                    if (dr["tks_max_jump_tkjum"] != DBNull.Value)
                    {
                        float valJumInput = float.Parse(dr["tks_max_jump_tkjum"].ToString());
                        TksJumps.Last().FlgInputOutput = flg;
                        TksJumps.Last().LimitedJumpInput = float.Parse(dr["tks_limit_jump_tkjum"].Equals(DBNull.Value) ? "0"
                                                            : dr["tks_limit_jump_tkjum"].ToString());
                        TksJumps.Last().ValueJumpInput = valJumInput;
                        TksJumps.Last().PercentJumpInput = -1;
                    }


                    else 
                    {
                        float perJumInput = float.Parse(dr["tks_pcn_max_jump_tkjum"].ToString());
                        TksJumps.Last().FlgInputOutput = flg;
                        TksJumps.Last().PercentJumpInput = perJumInput;
                        TksJumps.Last().LimitedJumpInput = float.Parse(dr["tks_limit_jump_tkjum"].Equals(DBNull.Value) ? "0"
                                                            : dr["tks_limit_jump_tkjum"].ToString());
                        TksJumps.Last().ValueJumpInput = -1;
                    }
                }// if inputTks


                else if (flg == 1) // outputTks
                {
                    if (dr["tks_max_jump_tkjum"] != DBNull.Value)  
                    {
                        float valJumpOutput = float.Parse(dr["tks_max_jump_tkjum"].ToString());
                        TksJumps.Last().FlgInputOutput = flg;
                        TksJumps.Last().ValueJumpOutput = valJumpOutput;
                        TksJumps.Last().PercentJumpOutput = -1;
                    }

                    else 
                    {
                        float perJumOutput = float.Parse(dr["tks_pcn_max_jump_tkjum"].ToString());
                        TksJumps.Last().FlgInputOutput = flg;
                        TksJumps.Last().PercentJumpOutput = perJumOutput;
                        TksJumps.Last().ValueJumpOutput = -1;
                    }
                }//else // outputTks

                else
                {
                    //message error : meghdare flgInputOutput be dorosti vared nashode ya fielde marbute barabare null ast
                }
            }//foreach

        }

        private void readWidJump(List<WidthJump> WidthJumps)
        {
            DataTable dtt = new DataTable();
            string comment = string.Format(@"select j.pgtyp_bas_program_type_def_id,
                                                       j.qty_wid_asc_mjpcn,
                                                       j.qty_wid_des_mjpcn,
                                                       j.flg_in_out_mjpcn,
                                                       (select m.nam_program_type_pgtyp from  mas.mas_bas_program_type_defs m 
                                                        where m.statn_bas_station_id = {0}
                                                       and m.bas_program_type_def_id = j.pgtyp_bas_program_type_def_id) as namProg
                                                  from mas.mas_bas_jump_wid_consts j
                                                 where j.statn_bas_station_id = {0}                                                
                                                 order by j.pgtyp_bas_program_type_def_id", RunInformation.NumStation);

            dtt = WorkData.GetDataTable(comment, CommandType.Text);

            foreach (DataRow dr in dtt.Rows)
            {
                WidthJump widJump = new WidthJump();
                widJump.CodMis = Convert.ToInt32(dr["pgtyp_bas_program_type_def_id"].Equals(DBNull.Value) ? Parameter.CodMisNull.ToString() :
                                                 dr["pgtyp_bas_program_type_def_id"].ToString());
                widJump.MisProg = dr["namProg"].ToString();
                widJump.FlgInputOutput = Convert.ToInt32(dr["flg_in_out_mjpcn"].Equals(DBNull.Value) ? "-1" : dr["flg_in_out_mjpcn"].ToString());

                if (widJump.FlgInputOutput == 0)// jump in plan  
                {
                    widJump.MaxWidDecrease = Convert.ToInt32(dr["qty_wid_des_mjpcn"].Equals(DBNull.Value) ?
                                                            DataBase.MaxJumpWid.ToString() : dr["qty_wid_des_mjpcn"].ToString());
                    widJump.MaxWidIncrease = Convert.ToInt32(dr["qty_wid_asc_mjpcn"].Equals(DBNull.Value) ?
                                                            DataBase.MaxJumpWid.ToString() : dr["qty_wid_asc_mjpcn"].ToString());
                }
                else if (widJump.FlgInputOutput == 1)// jump between plan  
                {
                    widJump.MaxWidDecrease = Convert.ToInt32(dr["qty_wid_des_mjpcn"].Equals(DBNull.Value) ?
                                                            DataBase.MaxJumpWid.ToString() : dr["qty_wid_des_mjpcn"].ToString());
                    widJump.MaxWidIncrease = Convert.ToInt32(dr["qty_wid_asc_mjpcn"].Equals(DBNull.Value) ?
                                                            DataBase.MaxJumpWid.ToString() : dr["qty_wid_asc_mjpcn"].ToString());
                }
                else
                {
                    widJump.MaxWidDecrease = Convert.ToInt32(dr["qty_wid_des_mjpcn"].Equals(DBNull.Value) ?
                                                            DataBase.MaxJumpWid.ToString() : dr["qty_wid_des_mjpcn"].ToString());
                    widJump.MaxWidIncrease = Convert.ToInt32(dr["qty_wid_asc_mjpcn"].Equals(DBNull.Value) ?
                                                            DataBase.MaxJumpWid.ToString() : dr["qty_wid_asc_mjpcn"].ToString());

                    widJump.FlgInputOutput = 0;
                    WidthJumps.Add(widJump);
                    widJump.FlgInputOutput = 1;
                }

                WidthJumps.Add(widJump);
            }
        }

        private void readMaxValue()
        {
            string comment = "";
            dt = new DataTable();

            if (RunInformation.FlgOptSim == 1)
                comment = qs.readMaxValue();

            else
                comment = string.Format(@"select max( nvl(NUM_DAT_LAST_ORDER_OSTIF, dat_dlv_order_orinf)) AS maxDatLast,
                                                   min(nvl(NUM_DAT_LAST_ORDER_OSTIF , dat_dlv_order_orinf)) as minDatLast ,
                                                   nvl(max(wid_prod_snamu), 9999) as widMax,
                                                   nvl(min(wid_prod_snamu),0) as widMin,
                                                   nvl(max(tks_prod_snamu),10) as tksMax,
                                                   nvl(min(tks_prod_snamu),0) as tksMin,
                                                   nvl(min(tks_out_ostif),0) as tksOutMin,
                                                   nvl(max(tks_out_ostif),0) as tksOutMax,
                                                   nvl(max(prio),0) as priority,
                                                   min(DatAvailTime) as minAvailTime,
                                                   max(DatAvailTime) as maxAvailTime,
                                                   nvl(max(position), 1) as position
                                              from (select nvl((select  max(cal.Dat_Calde)
                                                   from cmp.cmp_opt_order_station_infos os, apps.aac_lmp_CALENDAR_viw   CAL
                                                   Where  cal.Num_Week_Year_Ppc_Calde = os.num_dat_last_order_ostif),o.dat_dlv_order_orinf) as NUM_DAT_LAST_ORDER_OSTIF,
                                                           o.COD_URG_ORINF prio,
                                                           c.DAT_AVAILABLE_TIME_SNAMU as DatAvailTime,
                                                           substr(c.POS_POSITION_PROD_SNAMU, 10, 1) position,
                                                           c.wid_prod_snamu,
                                                           os.tks_out_ostif,
                                                           c.tks_prod_snamu,
                                                           o.dat_dlv_order_orinf
        
                                                      from cmp.cmp_opt_snapshot_mus    c,
                                                           cmp.cmp_opt_order_infos         o,
                                                           cmp.cmp_opt_order_station_infos os
        
                                                     where c.ORINF_OPT_ORDER_INFO_ID = o.opt_order_info_id
                                                       and o.opt_order_info_id = os.Orinf_Opt_Order_Info_Id
                                                       and (c.statn_bas_station_id_uses = {0} 
                                                       or c.statn_bas_station_id_uses_a = {0} 
                                                       or c.statn_bas_station_id_uses_b = {0} 
                                                       or c.statn_bas_station_id_uses_c = {0}
                                                       or c.statn_bas_station_id_uses_d = {0})
                                                       and os.Statn_Bas_Station_Id = {0}
                                                       and c.runth_opt_run_time_hist_id = {1}
                                                       and o.RUNTH_OPT_RUN_TIME_HIST_ID = {1}
                                                       and os.RUNTH_OPT_RUN_TIME_HIST_ID = {1}
                                                       and c.TYP_PROGRAM_TYPE_SNAMU = 1
                                                       and (nvl(o.Cod_Suspend_Ship_Orinf,-1) != 1)
                                                       and (nvl(o.Cod_Suspend_Prod_Orinf, -1) != 1)
                                                       and (nvl(os.FLG_SUSPEND_PRO_OSTIF,-1) != 1))
                                            ", RunInformation.NumStation,
                                             RunInformation.RunId
                                             , CommandType.Text);

            dt = WorkData.GetDataTable(comment, CommandType.Text);

            foreach (DataRow dr in dt.Rows)
            {
                DataBase.MaxDatLast = DateTime.Parse(dr["maxDatLast"].ToString());
                DataBase.MinDatLast = DateTime.Parse(dr["minDatLast"].ToString());
                DataBase.MinAvailTime = DateTime.Parse(dr["minAvailTime"].Equals(DBNull.Value) ? Parameter.MinAvailTimeNull.ToString()
                                                         : dr["minAvailTime"].ToString());
                DataBase.MaxAvailTime = DateTime.Parse(dr["maxAvailTime"].Equals(DBNull.Value) ? Parameter.MaxAvailTimeNull.ToString()
                                                         : dr["maxAvailTime"].ToString());
                DataBase.MaxPriority = int.Parse(dr["priority"].ToString());
                DataBase.MaxLevelStorge = double.Parse(dr["position"].ToString());
                DataBase.WidMaxStorge = int.Parse(dr["widMax"].ToString());
                DataBase.WidMinStorge = int.Parse(dr["widMin"].ToString());
                DataBase.TksMaxStorge = double.Parse(dr["tksMax"].ToString());
                DataBase.TksMinStorge = double.Parse(dr["tksMin"].ToString());
                DataBase.MaxJumpTks = DataBase.TksMaxStorge - DataBase.TksMinStorge;
                DataBase.MaxJumpWid = DataBase.WidMaxStorge - DataBase.WidMinStorge;
                DataBase.TksOutMax = double.Parse(dr["tksOutMax"].ToString());
                DataBase.TksOutMin = double.Parse(dr["tksOutMin"].ToString());
                DataBase.MaxJumpTksOut = DataBase.TksOutMax - DataBase.TksOutMin;
                //DataBase.MaxJumpWidOut = DataBase.WidOutMax - DataBase.WidOutMin;


            }
        }

   
  
        private void readShift(List<ShiftWork> ShiftWorks)
        {
            ShiftWork shift = new ShiftWork();
            shift.IndexClass = 0;//auto number
            shift.StartShift = DateTime.MinValue + TimeSpan.FromHours(6);//DB
            shift.LenShift = TimeSpan.FromHours(12);//DB
            shift.EndShift = shift.StartShift + shift.LenShift;//DB           

            shift.FlgNightShift = 0;
            
         

            shift.FlgShutDown = 1;
            
 

            ShiftWorks.Add(shift);

            ShiftWork shift1 = new ShiftWork();
            shift1.IndexClass = 0;//auto number
            shift1.StartShift = DateTime.MinValue + TimeSpan.FromHours(18);//DB
            shift1.LenShift = TimeSpan.FromHours(12);//DB
            shift1.EndShift = shift1.StartShift + shift1.LenShift;//DB           

            shift1.FlgNightShift = 1;
            
   

            shift1.FlgShutDown = 0;
            
 

            ShiftWorks.Add(shift1);
        }

      
        private void readEquipGroupFailureTime(List<EquipGroupFailureTime> EquipGroupFailureTimes)
        {

            string comment = "";
            dt = new DataTable();

            if (RunInformation.FlgOptSim == 1)
                comment = qs.readEquipGroupFailureTime();

            else
                comment = string.Format(@"select * from apps.mas_cmp_equip_group_viw t
                                            where t.statn_bas_station_id={0}" + RunInformation.NumStation);
            dt = WorkData.GetDataTable(comment, CommandType.Text);

            int z = -1;
            foreach (DataRow dr in dt.Rows)
            {
              
                if (z == int.Parse(dr["order_group_id"].ToString()))
                {
                       string Orderlocal = dr[""].ToString().ToUpper();
                    EquipGroupFailureTimes.Last().LstOrderEquip.Add(Orderlocal);

                }
                else
                {
                    addEquipGroupFailureTime(dr, EquipGroupFailureTimes);
                }
            }
        }

        private void readStationStops(List<StationStop> StationStops)
        {

            string comment = "";
            dt = new DataTable();

            if (RunInformation.FlgOptSim == 1)
                comment = qs.readStationStops();

            else
                comment = string.Format(@"select t.COD_REPAIR_REPTM,
                                           t.date_repair,
                                           t.DUR_REPAIR_REPTM
                                      from apps.mas_cmp_repair_time_viw t
                                    where t.statn_bas_station_id = {0}" + RunInformation.NumStation);
            dt = WorkData.GetDataTable(comment, CommandType.Text);

            foreach (DataRow dr in dt.Rows)
                addStationStops(dr, StationStops);

        }

        private void readRoll(List<Roll> RollsWork, List<Roll> RollsBack, List<Coil> Coils)
        {

            string comment = "";
            dt = new DataTable();

            if (RunInformation.FlgOptSim == 1)

                comment = qs.readRoll();

            else
                comment = string.Format(@" Select distinct (b.cod_roll_type_rolba),
                                                                b.wei_optimal_roll_rolba * 1000 as wei_optimal_roll_rolba,
                                                                b.lth_optimal_roll_rolba  ,
                                                                b.int_dur_optimal_roll_rolba,
                                                                g.wei_actual_roll_rolac*1000 as wei_actual_roll_rolac,
                                                                g.lth_actual_roll_rolac ,
                                                                b.pcn_pos_tol_rolba,
                                                                b.pcn_neg_tol_rolba,
                                                                g.wid_min_camp_rolac,
                                                                g.dat_entr_roll_rolac
                                                  From (select t.wei_actual_roll_rolac,
                                                               t.lth_actual_roll_rolac,
                                                               t.cod_roll_uplow_rolac,
                                                               t.dat_entr_roll_rolac,
                                                               t.cod_roll_type_rolac,
                                                                t.wid_min_camp_rolac
                                                          from cmp.cmp_opt_rolls_actuals t
                                                         where t.runth_opt_run_time_hist_id = {0}
                                                           and nvl(t.num_stand_rolac,0) = {1}
                                                           and t.statn_bas_station_id = {2}
                                                           and dat_entr_roll_rolac in (select min(dat_entr_roll_rolac)
                                                                  from cmp.cmp_opt_rolls_actuals ac
                                                                 where ac.statn_bas_station_id = {2})) g right join
                                                       mas.mas_bas_rolls_bases b 
                                                       on b.cod_roll_type_rolba = g.cod_roll_type_rolac
                                                 Where b.statn_bas_station_id = {2}
                                                   and nvl(b.num_stand_rolba,0) = {1} ",
                                               RunInformation.RunId
                                               , RunInformation.NumStand, RunInformation.NumStation);

            dt = WorkData.GetDataTable(comment, CommandType.Text);

            foreach (DataRow dr in dt.Rows)
            {
                RunInformation.MinWidCamp = int.Parse(dr["wid_min_camp_rolac"].Equals(DBNull.Value) ? Coils.Max(c => c.Width).ToString() : dr["wid_min_camp_rolac"].ToString());

                Roll b = new Roll();
                b.DatRollEnter = DateTime.Parse(dr["dat_entr_roll_rolac"].Equals(DBNull.Value) ? Status.CurrTime.ToString() : dr["dat_entr_roll_rolac"].ToString());
                b.WeiOpt = double.Parse(dr["wei_optimal_roll_rolba"].Equals(DBNull.Value) ? "0" : dr["wei_optimal_roll_rolba"].ToString());
                b.WeiDB = double.Parse(dr["wei_actual_roll_rolac"].Equals(DBNull.Value) ? "0" : dr["wei_actual_roll_rolac"].ToString());
                b.LenOpt = double.Parse(dr["lth_optimal_roll_rolba"].Equals(DBNull.Value) ? "0" : dr["lth_optimal_roll_rolba"].ToString());
                b.LenDB = double.Parse(dr["lth_actual_roll_rolac"].Equals(DBNull.Value) ? "0" : dr["lth_actual_roll_rolac"].ToString());
                TimeSpan loc = TimeSpan.Parse(dr["int_dur_optimal_roll_rolba"].Equals(DBNull.Value) ? "0" : dr["int_dur_optimal_roll_rolba"].ToString());
                b.DatOpt = loc.Days;
                b.DatDB = (b.DatRollEnter - Status.CurrTime).Days;
                b.UpperPerc = double.Parse(dr["pcn_pos_tol_rolba"].Equals(DBNull.Value) ? "0" : dr["pcn_pos_tol_rolba"].ToString());
                b.LowerPerc = double.Parse(dr["pcn_neg_tol_rolba"].Equals(DBNull.Value) ? "0" : dr["pcn_neg_tol_rolba"].ToString());


                if (b.WeiDB == 0 && b.LenDB == 0 && b.DatDB == 0)
                    b.FirstPlan = false;
                else
                    b.FirstPlan = true;

                if (dr["cod_roll_type_rolba"].ToString().ToUpper() == "W")
                    RollsWork.Add(b);

                else
                    RollsBack.Add(b);

            }

        }

        private void readWeightObjective()
        {

            string comment = "";
            dt = new DataTable();

            if (RunInformation.FlgOptSim == 1)
                comment = qs.readWeightObjective();
            else
                comment = string.Format(@"select t.val_multi_value_cofal, t.lkp_multi_cofal
                                      from cmp.cmp_opt_coefficient_algs t
                                      where t.statn_bas_station_id ={0}
                                        and  t.runth_opt_run_time_hist_id = {1}", RunInformation.NumStation, RunInformation.RunIdParameter);


            dt = WorkData.GetDataTable(comment, CommandType.Text);

            foreach (DataRow dr in dt.Rows)
            {
                string lookup = dr["lkp_multi_cofal"].ToString().ToUpper();

                #region Objective
                if (Parameter.CMP_DATLAST_OBJ == lookup)

                    WeightObjective.DatLastCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());

                else if (Parameter.CMP_PRIORITY_OBJ == lookup)
                    WeightObjective.PriorityCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());


                else if (Parameter.CMP_DURABILITY_OBJ == lookup)
                    WeightObjective.DurabilityCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());


                else if (Parameter.CMP_CAPPLAN_OBJ == lookup)
                    WeightObjective.CapPlanCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());


                else if (Parameter.CMP_SETUP_OBJ == lookup)
                    WeightObjective.SetupCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());


                else if (Parameter.CMP_LENPROG_OBJ == lookup)
                    WeightObjective.LenProgCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());

                else if (Parameter.CMP_WEIPROG_OBJ == lookup)
                    WeightObjective.WeiProgCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());


                else if (Parameter.CMP_SARFASL_OBJ == lookup)
                    WeightObjective.SarfaslCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());


                else if (Parameter.CMP_LEVELSTORGE_OBJ == lookup)
                    WeightObjective.LevelStorgeCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());

                else if (Parameter.CMP_COUNTCOIL_OBJ == lookup)
                    WeightObjective.CountProgCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());

                else if (Parameter.CMP_CONTINUE_PATTERN_OBJ == lookup)
                    WeightObjective.ContinuePatternCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());

                else if (Parameter.CMP_START_CAMP_OBJ == lookup)
                    WeightObjective.StartCampCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());

                else if (Parameter.CMP_CONTINUE_TYPEPROG_OBJ == lookup)
                    WeightObjective.continueTypeProgCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());

                else if (Parameter.CMP_PRIOR_STATION_OBJ == lookup)
                    WeightObjective.PriorStationCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());

                else if (Parameter.CMP_MIS_PROG_PRIO == lookup)
                    WeightObjective.PriorGroupTypeMisCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());

                else if (Parameter.CMP_WORKROL_WEI_OBJ == lookup)
                    WeightObjective.WorkRollWeiCoef = float.Parse(dr["val_multi_value_cofal"].ToString());

                else if (Parameter.CMP_WORKROL_LEN_OBJ == lookup)
                    WeightObjective.WorkRollLenCoef = float.Parse(dr["val_multi_value_cofal"].ToString());

                #endregion


                #region  Rank
                else if (Parameter.CMP_DATLAST_RANK == lookup)
                    WeightRank.DatLastCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());

                else if (Parameter.CMP_PRIORITY_RANK == lookup)
                    WeightRank.PriorityCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());

                else if (Parameter.CMP_DURABILITY_RANK == lookup)
                    WeightRank.DurabilityCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());

                else if (Parameter.CMP_LEVELSTORGE_RANK == lookup)
                    WeightRank.LevelStorgeCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());

                else if (Parameter.CMP_SAMEWID_RANK == lookup)
                    WeightRank.SameWidGroupCoef = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "0" : dr["val_multi_value_cofal"].ToString());

                #endregion

                #region constraint

                else if (Parameter.PIC_MAX_NUM_CHANGE_WID == lookup)
                    Parameter.MaxNumChangeWidTrim = int.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "-1" : dr["val_multi_value_cofal"].ToString());

                else if (Parameter.PIC_MAX_PERC_CHANGE_WID == lookup)
                    Parameter.MaxPercChangeWidTrim = float.Parse(dr["val_multi_value_cofal"].Equals(DBNull.Value) ? "-1" : dr["val_multi_value_cofal"].ToString());


                #endregion


            }
        }

        private void readGroupDef(List<GroupDef> GroupDefs, List<ProgEfraz> ProgEfrazes, List<int> lstReadAllGroupDef)
        {

            int i = 0;

            dt = new DataTable();
            string strArr = "";
            foreach(int gd in lstReadAllGroupDef)
            {
                if (strArr != "")
                {
                    strArr += ",";
                }
                strArr += gd;
            }
            string s = string.Format(@"select gd1.sop_order_group_def_id,
                            gd1.ogdef_sop_order_group_def_id,
                            sgr.sop_order_group_id,
                            nvl(sgr.num_priority_orgrp, 1) as num_priority_orgrp,
                            gd1.typ_rel_op_ogdef,
                            nvl((select va.val_att_grval  from lmp.lmp_sop_group_vals va   where va.sop_group_val_id = gd1.grval_sop_group_val_id),-1) as val_value1_ogdef,
                            nvl((select va.val_att_grval  from lmp.lmp_sop_group_vals va     where va.sop_group_val_id = gd1.grval_sop_group_val_id_used) , -1) as val_value2_ogdef,
                            (select d.nam_latin_grpdt  from lmp.lmp_sop_group_data_defs d  where d.sop_group_data_def_id = gd1.grpdt_sop_group_data_def_id) as nam_latin_grpdt
                            from lmp.lmp_sop_order_group_defs  gd1,
                            (select gd.sop_order_group_def_id,
                            b.num_priority_orgrp,
                            b.sop_order_group_id
                                from lmp.lmp_sop_order_group_defs  gd,   lmp.lmp_sop_order_group_types gt,  lmp.lmp_sop_order_groups  b 
                            where b.sop_order_group_id in ({5})
                            and gt.sop_order_group_type_id = b.OGTYP_SOP_ORDER_GROUP_TYPE_ID
                            and b.SOP_ORDER_GROUP_ID = gd.ORGRP_SOP_ORDER_GROUP_ID
                           -- and b.STATN_BAS_STATION_ID = {0}
                            ) sgr

                            where gd1.OGDEF_SOP_ORDER_GROUP_DEF_ID =  sgr.sop_order_group_def_id
                            order by sgr.num_priority_orgrp,sop_order_group_id, ogdef_sop_order_group_def_id, grpdt_sop_group_data_def_id,
                                    decode(typ_rel_op_ogdef, {1}, 1,{2}, 2, {3}, 3, {4}, 4), val_value1_ogdef desc, val_value2_ogdef desc"
                , RunInformation.NumStation, Parameter.Equal, Parameter.LargerOrEqual, Parameter.Between, Parameter.SmallerOrEqual, strArr);



            dt = WorkData.GetDataTable(s, CommandType.Text);

            int z = -1;


            foreach (DataRow dr in dt.Rows)
            {
                int groupLoc = Convert.ToInt32(dr["sop_order_group_id"]);
                string att = dr["nam_latin_grpdt"].ToString().ToUpper();
                int rel = Convert.ToInt32(dr["typ_rel_op_ogdef"]);


                if (z != groupLoc)
                {
                    z = groupLoc;

                    GroupDef b = new GroupDef();
                    b.IdGroup = groupLoc;
                    b.IndexGroup = i;
                    b.PriorityGroup = Convert.ToInt32(dr["num_priority_orgrp"]);


                    if (att == Parameter.WidthAttribute)
                    {
                        if (rel == Parameter.LargerOrEqual)
                        {
                            b.WidFrom = Convert.ToInt32(dr["val_value1_ogdef"]);
                            b.WidTo = DataBase.WidMaxStorge;
                        }
                        else if (rel == Parameter.Between)
                        {
                            b.WidFrom = Convert.ToInt32(dr["val_value1_ogdef"]);
                            b.WidTo = Convert.ToInt32(dr["val_value2_ogdef"]);
                        }
                        else if (rel == Parameter.SmallerOrEqual)
                        {
                            b.WidFrom = DataBase.WidMinStorge;
                            b.WidTo = Convert.ToInt32(dr["val_value1_ogdef"]);
                        }
                        else if (rel == Parameter.Equal)
                        {
                            b.WidFrom = Convert.ToInt32(dr["val_value1_ogdef"]);
                            b.WidTo = Convert.ToInt32(dr["val_value1_ogdef"]);
                        }
                    }

                    else if (att == Parameter.TksAttribute)
                    {
                        if (rel == Parameter.LargerOrEqual)
                        {
                            b.TksFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            b.TksTo = DataBase.TksMaxStorge;
                        }
                        else if (rel == Parameter.Between)
                        {
                            b.TksFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            b.TksTo = double.Parse(dr["val_value2_ogdef"].ToString());
                        }
                        else if (rel == Parameter.SmallerOrEqual)
                        {
                            b.TksFrom = DataBase.TksMinStorge;
                            b.TksTo = double.Parse(dr["val_value1_ogdef"].ToString());
                        }
                        else if (rel == Parameter.Equal)
                        {
                            b.TksFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            b.TksTo = double.Parse(dr["val_value1_ogdef"].ToString());
                        }
                    }
                    else if (att == Parameter.TksOutAttribute)
                    {
                        if (rel == Parameter.LargerOrEqual)
                        {
                            b.TksOutFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            b.TksOutTo = DataBase.TksMaxStorge;
                        }
                        else if (rel == Parameter.Between)
                        {
                            b.TksOutFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            b.TksOutTo = double.Parse(dr["val_value2_ogdef"].ToString());
                        }
                        else if (rel == Parameter.SmallerOrEqual)
                        {
                            b.TksOutFrom = DataBase.TksMinStorge;
                            b.TksOutTo = double.Parse(dr["val_value1_ogdef"].ToString());
                        }
                        else if (rel == Parameter.Equal)
                        {
                            b.TksOutFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            b.TksOutTo = double.Parse(dr["val_value1_ogdef"].ToString());
                        }
                    }
                    else if (att == Parameter.ProgMisAttribute)
                    {
                        b.LstgroupMisProg.Add(dr["val_value1_ogdef"].ToString());
                    }

                    GroupDefs.Add(b);
                    i++;
                }

                else
                {
                    if (att == Parameter.WidthAttribute)
                    {
                        if (rel == Parameter.LargerOrEqual)
                        {
                            GroupDefs.Last().WidFrom = Convert.ToInt32(dr["val_value1_ogdef"]);
                            GroupDefs.Last().WidTo = DataBase.WidMaxStorge;
                        }
                        else if (rel == Parameter.Between)
                        {
                            GroupDefs.Last().WidFrom = Convert.ToInt32(dr["val_value1_ogdef"]);
                            GroupDefs.Last().WidTo = Convert.ToInt32(dr["val_value2_ogdef"]);
                        }
                        else if (rel == Parameter.SmallerOrEqual)
                        {
                            GroupDefs.Last().WidFrom = DataBase.WidMinStorge;
                            GroupDefs.Last().WidTo = Convert.ToInt32(dr["val_value1_ogdef"]);
                        }
                        else if (rel == Parameter.Equal)
                        {
                            GroupDefs.Last().WidFrom = Convert.ToInt32(dr["val_value1_ogdef"]);
                            GroupDefs.Last().WidTo = Convert.ToInt32(dr["val_value1_ogdef"]);
                        }

                    }

                    else if (att == Parameter.TksAttribute)
                    {
                        if (rel == Parameter.LargerOrEqual)
                        {
                            GroupDefs.Last().TksFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            GroupDefs.Last().TksTo = DataBase.TksMaxStorge;
                        }
                        else if (rel == Parameter.Between)
                        {
                            GroupDefs.Last().TksFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            GroupDefs.Last().TksTo = double.Parse(dr["val_value2_ogdef"].ToString());
                        }
                        else if (rel == Parameter.SmallerOrEqual)
                        {
                            GroupDefs.Last().TksFrom = DataBase.TksMinStorge;
                            GroupDefs.Last().TksTo = double.Parse(dr["val_value1_ogdef"].ToString());
                        }
                        else if (rel == Parameter.Equal)
                        {
                            GroupDefs.Last().TksFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            GroupDefs.Last().TksTo = double.Parse(dr["val_value1_ogdef"].ToString());
                        }
                    }
                    else if (att == Parameter.TksOutAttribute)
                    {
                        if (rel == Parameter.LargerOrEqual)
                        {
                            GroupDefs.Last().TksOutFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            GroupDefs.Last().TksOutTo = DataBase.TksMaxStorge;
                        }
                        else if (rel == Parameter.Between)
                        {
                            GroupDefs.Last().TksOutFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            GroupDefs.Last().TksOutTo = double.Parse(dr["val_value2_ogdef"].ToString());
                        }
                        else if (rel == Parameter.SmallerOrEqual)
                        {
                            GroupDefs.Last().TksOutFrom = DataBase.TksMinStorge;
                            GroupDefs.Last().TksOutTo = double.Parse(dr["val_value1_ogdef"].ToString());
                        }
                        else if (rel == Parameter.Equal)
                        {
                            GroupDefs.Last().TksOutFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            GroupDefs.Last().TksOutTo = double.Parse(dr["val_value1_ogdef"].ToString());
                        }
                    }
                    else if (att == Parameter.ProgMisAttribute)
                    {
                        GroupDefs.Last().LstgroupMisProg.Add(dr["val_value1_ogdef"].ToString());
                    }
                }
            }

            foreach (var item in GroupDefs)
            {
                if (item.WidFrom == 0)
                    item.WidFrom = DataBase.WidMinStorge;
                if (item.WidTo == 0)
                    item.WidTo = DataBase.WidMaxStorge;
                if (item.TksFrom == 0)
                    item.TksFrom = DataBase.TksMinStorge;
                if (item.TksTo == 0)
                    item.TksTo = DataBase.TksMaxStorge;
                if (item.TksOutFrom == 0)
                    item.TksOutFrom = DataBase.TksOutMin;
                if (item.TksOutTo == 0)
                    item.TksOutTo = DataBase.TksOutMax;

                if (item.LstgroupMisProg.Count() == 0)
                {
                    foreach (var j in ProgEfrazes)
                    {
                        if (item.LstgroupMisProg.Contains(j.ProgMis) == false)
                            item.LstgroupMisProg.Add(j.ProgMis);
                    }
                }
            }


        }

        private void readGroupDefTem(List<GroupDef> GroupDefs, List<ProgEfraz> ProgEfrazes)
        {
            

            int i = 0;
            dt = new DataTable();
            string s = string.Format(@"select --gd1.sop_order_group_def_id,
                            --gd1.ogdef_sop_order_group_def_id,
                            sgr.sop_order_group_id,
                            nvl(sgr.num_priority_orgrp, 1) as num_priority_orgrp,
                            gd1.typ_rel_op_ogdef,
                            nvl((select va.val_att_grval  from lmp_sop_group_vals_temp va   where va.sop_group_val_id = gd1.grval_sop_group_val_id),-1) as val_value1_ogdef,
                            nvl((select va.val_att_grval  from  lmp_sop_group_vals_temp va   where va.sop_group_val_id = gd1.grval_sop_group_valuses_as_sec) , -1) as val_value2_ogdef,
                            (select d.nam_latin_grpdt  from lmp_sop_group_data_defs_temp d  where d.sop_group_data_def_id = gd1.grpdt_sop_group_data_def_id) as nam_latin_grpdt
                            from lmp_sop_order_group_defs_temp  gd1,
                            (select gd.sop_order_group_def_id,
                            b.num_priority_orgrp,
                            b.sop_order_group_id
                                from lmp_sop_order_group_defs_temp  gd,lmp_sop_order_group_types_temp gt, lmp_sop_order_groups_temp  b 
                            where gt.cod_group_type_ogtyp = 3101
                            and gt.sop_order_group_type_id = b.OGTYP_SOP_ORDER_GROUP_TYPE_ID
                            and b.SOP_ORDER_GROUP_ID = gd.ORGRP_SOP_ORDER_GROUP_ID
                            and b.STATN_BAS_STATION_ID = {0}) sgr

                            where gd1.OGDEF_SOP_ORDER_GROUP_DEF_ID =  sgr.sop_order_group_def_id
                            order by sgr.num_priority_orgrp,sop_order_group_id, ogdef_sop_order_group_def_id, grpdt_sop_group_data_def_id,
                                    decode(typ_rel_op_ogdef, {1}, 1,{2}, 2, {3}, 3, {4}, 4),   val_value1_ogdef desc,  val_value2_ogdef desc"
                , RunInformation.NumStation, Parameter.Equal, Parameter.LargerOrEqual, Parameter.Between, Parameter.SmallerOrEqual);

            dt = WorkData.GetDataTable(s, CommandType.Text);
            int z = -1;
            foreach (DataRow dr in dt.Rows)
            {
                int groupLoc = Convert.ToInt32(dr["sop_order_group_id"]);
                string att = dr["nam_latin_grpdt"].ToString().ToUpper();
                int rel = Convert.ToInt32(dr["typ_rel_op_ogdef"]);

                if (z != groupLoc)
                {
                    z = groupLoc;

                    GroupDef b = new GroupDef();
                    b.IdGroup = groupLoc;
                    b.IndexGroup = i;
                    b.PriorityGroup = Convert.ToInt32(dr["num_priority_orgrp"]);

                    if (att == Parameter.WidthAttribute)
                    {
                        if (rel == Parameter.LargerOrEqual)
                        {
                            b.WidFrom = Convert.ToInt32(dr["val_value1_ogdef"]);
                            b.WidTo = DataBase.WidMaxStorge;
                        }
                        else if (rel == Parameter.Between)
                        {
                            b.WidFrom = Convert.ToInt32(dr["val_value1_ogdef"]);
                            b.WidTo = Convert.ToInt32(dr["val_value2_ogdef"]);
                        }
                        else if (rel == Parameter.SmallerOrEqual)
                        {
                            b.WidFrom = DataBase.WidMinStorge;
                            b.WidTo = Convert.ToInt32(dr["val_value1_ogdef"]);
                        }
                        else if (rel == Parameter.Equal)
                        {
                            b.WidFrom = Convert.ToInt32(dr["val_value1_ogdef"]);
                            b.WidTo = Convert.ToInt32(dr["val_value1_ogdef"]);
                        }
                    }

                    else if (att == Parameter.TksAttribute)
                    {
                        if (rel == Parameter.LargerOrEqual)
                        {
                            b.TksFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            b.TksTo = DataBase.TksMaxStorge;
                        }
                        else if (rel == Parameter.Between)
                        {
                            b.TksFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            b.TksTo = double.Parse(dr["val_value2_ogdef"].ToString());
                        }
                        else if (rel == Parameter.SmallerOrEqual)
                        {
                            b.TksFrom = DataBase.TksMinStorge;
                            b.TksTo = double.Parse(dr["val_value1_ogdef"].ToString());
                        }
                        else if (rel == Parameter.Equal)
                        {
                            b.TksFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            b.TksTo = double.Parse(dr["val_value1_ogdef"].ToString());
                        }
                    }
                    else if (att == Parameter.TksOutAttribute)
                    {
                        if (rel == Parameter.LargerOrEqual)
                        {
                            b.TksOutFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            b.TksOutTo = DataBase.TksMaxStorge;
                        }
                        else if (rel == Parameter.Between)
                        {
                            b.TksOutFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            b.TksOutTo = double.Parse(dr["val_value2_ogdef"].ToString());
                        }
                        else if (rel == Parameter.SmallerOrEqual)
                        {
                            b.TksOutFrom = DataBase.TksMinStorge;
                            b.TksOutTo = double.Parse(dr["val_value1_ogdef"].ToString());
                        }
                        else if (rel == Parameter.Equal)
                        {
                            b.TksOutFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            b.TksOutTo = double.Parse(dr["val_value1_ogdef"].ToString());
                        }
                    }
                    else if (att == Parameter.ProgMisAttribute)
                    {
                        b.LstgroupMisProg.Add(dr["val_value1_ogdef"].ToString());
                    }
                    else if (att == Parameter.SurfaceRoughAttribute)
                    {
                        b.CodSurfaceRoughness = int.Parse(dr["val_value1_ogdef"].ToString());
                    }
                    else if (att == Parameter.TYP_Product_Family_GAL)
                    {
                        b.ProductFamily = int.Parse(dr["val_value1_ogdef"].ToString());
                    }



                    GroupDefs.Add(b);
                    i++;
                }

                else
                {
                    if (att == Parameter.WidthAttribute)
                    {
                        if (rel == Parameter.LargerOrEqual)
                        {
                            GroupDefs.Last().WidFrom = Convert.ToInt32(dr["val_value1_ogdef"]);
                            GroupDefs.Last().WidTo = DataBase.WidMaxStorge;
                        }
                        else if (rel == Parameter.Between)
                        {
                            GroupDefs.Last().WidFrom = Convert.ToInt32(dr["val_value1_ogdef"]);
                            GroupDefs.Last().WidTo = Convert.ToInt32(dr["val_value2_ogdef"]);
                        }
                        else if (rel == Parameter.SmallerOrEqual)
                        {
                            GroupDefs.Last().WidFrom = DataBase.WidMinStorge;
                            GroupDefs.Last().WidTo = Convert.ToInt32(dr["val_value1_ogdef"]);
                        }
                        else if (rel == Parameter.Equal)
                        {
                            GroupDefs.Last().WidFrom = Convert.ToInt32(dr["val_value1_ogdef"]);
                            GroupDefs.Last().WidTo = Convert.ToInt32(dr["val_value1_ogdef"]);
                        }

                    }

                    else if (att == Parameter.TksAttribute)
                    {
                        if (rel == Parameter.LargerOrEqual)
                        {
                            GroupDefs.Last().TksFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            GroupDefs.Last().TksTo = DataBase.TksMaxStorge;
                        }
                        else if (rel == Parameter.Between)
                        {
                            GroupDefs.Last().TksFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            GroupDefs.Last().TksTo = double.Parse(dr["val_value2_ogdef"].ToString());
                        }
                        else if (rel == Parameter.SmallerOrEqual)
                        {
                            GroupDefs.Last().TksFrom = DataBase.TksMinStorge;
                            GroupDefs.Last().TksTo = double.Parse(dr["val_value1_ogdef"].ToString());
                        }
                        else if (rel == Parameter.Equal)
                        {
                            GroupDefs.Last().TksFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            GroupDefs.Last().TksTo = double.Parse(dr["val_value1_ogdef"].ToString());
                        }
                    }
                    else if (att == Parameter.TksOutAttribute)
                    {
                        if (rel == Parameter.LargerOrEqual)
                        {
                            GroupDefs.Last().TksOutFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            GroupDefs.Last().TksOutTo = DataBase.TksMaxStorge;
                        }
                        else if (rel == Parameter.Between)
                        {
                            GroupDefs.Last().TksOutFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            GroupDefs.Last().TksOutTo = double.Parse(dr["val_value2_ogdef"].ToString());
                        }
                        else if (rel == Parameter.SmallerOrEqual)
                        {
                            GroupDefs.Last().TksOutFrom = DataBase.TksMinStorge;
                            GroupDefs.Last().TksOutTo = double.Parse(dr["val_value1_ogdef"].ToString());
                        }
                        else if (rel == Parameter.Equal)
                        {
                            GroupDefs.Last().TksOutFrom = double.Parse(dr["val_value1_ogdef"].ToString());
                            GroupDefs.Last().TksOutTo = double.Parse(dr["val_value1_ogdef"].ToString());
                        }
                    }
                    else if (att == Parameter.ProgMisAttribute)
                    {
                        GroupDefs.Last().LstgroupMisProg.Add(dr["val_value1_ogdef"].ToString());
                    }
                    else if (att == Parameter.SurfaceRoughAttribute)
                    {
                        GroupDefs.Last().CodSurfaceRoughness = int.Parse(dr["val_value1_ogdef"].ToString());
                    }
                    else if (att == Parameter.TYP_Product_Family_GAL)
                    {
                        GroupDefs.Last().ProductFamily = int.Parse(dr["val_value1_ogdef"].ToString());
                    }


                }
            }

            foreach (var item in GroupDefs)
            {
                if (item.WidFrom == 0)
                    item.WidFrom = DataBase.WidMinStorge;
                if (item.WidTo == 0)
                    item.WidTo = DataBase.WidMaxStorge;
                if (item.TksFrom == 0)
                    item.TksFrom = DataBase.TksMinStorge;
                if (item.TksTo == 0)
                    item.TksTo = DataBase.TksMaxStorge;
                if (item.TksOutFrom == 0)
                    item.TksOutFrom = DataBase.TksOutMin;
                if (item.TksOutTo == 0)
                    item.TksOutTo = DataBase.TksOutMax;

                if (item.LstgroupMisProg.Count() == 0)
                {
                    foreach (var j in ProgEfrazes)
                    {
                        if (item.LstgroupMisProg.Contains(j.ProgMis) == false)
                            item.LstgroupMisProg.Add(j.ProgMis);
                    }
                }
            }


        }

        private void readSarfasl(List<Sarfasl> Sarfasls,  ref List<int> lstReadAllGroupDef)
        {
            dt = new DataTable();
            string s = string.Format(@"select def.bas_camp_define_id,
                                                       def.nam_camp_cmpdf,      
                                                       ass.cmpdf_bas_camp_define_id,
                                                       ass.orgrp_sop_order_group_id
                                                  from lmp.lmp_bas_camp_defines def,
                                                       lmp.lmp_bas_camp_assigns ass
                                                where def.statn_bas_station_id = {0}
                                                and ass.cmpdf_bas_camp_define_id = def.bas_camp_define_id
                                                order by ass.cmpdf_bas_camp_define_id ", RunInformation.NumStation);

            dt = WorkData.GetDataTable(s, CommandType.Text);
            int z = -1;
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                int sarfaslLoc = Convert.ToInt32(dr["cmpdf_bas_camp_define_id"]);

                if (z != sarfaslLoc)
                {
                   
                    z = sarfaslLoc;

                    Sarfasl b = new Sarfasl();
                    b.IdSarfasl = sarfaslLoc;
                    b.IndexSarfasl = i;

                    b.LstGroupSarfasl.Add(Convert.ToInt32(dr["orgrp_sop_order_group_id"].ToString()));
                    lstReadAllGroupDef.Add(Convert.ToInt32(dr["orgrp_sop_order_group_id"].ToString()));
                    
                    Sarfasls.Add(b);
                    i++;
                }

                else
                {
                    Sarfasls.Last().LstGroupSarfasl.Add(Convert.ToInt32(dr["orgrp_sop_order_group_id"].ToString()));
                    lstReadAllGroupDef.Add(Convert.ToInt32(dr["orgrp_sop_order_group_id"].ToString()));
                }
                
            }
            lstReadAllGroupDef = lstReadAllGroupDef.Distinct().ToList();

        }

        private void readSarfaslTem(List<Sarfasl> Sarfasls)
        {
            dt = new DataTable();
            string s = string.Format(@"select def.bas_camp_define_id,
                                               def.nam_camp_cmpdf,      
                                               ass.cmpdf_bas_camp_define_id,
                                               ass.orgrp_sop_order_group_id
                                          from lmp_bas_camp_defines_temp def,
                                               lmp_bas_camp_assigns_temp ass
                                        where def.statn_bas_station_id = {0}
                                        and ass.cmpdf_bas_camp_define_id = def.bas_camp_define_id
                                        order by ass.cmpdf_bas_camp_define_id ", RunInformation.NumStation);

            dt = WorkData.GetDataTable(s, CommandType.Text);
            int z = -1;
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                int sarfaslLoc = Convert.ToInt32(dr["cmpdf_bas_camp_define_id"]);

                if (z != sarfaslLoc)
                {
                    z = sarfaslLoc;

                    Sarfasl b = new Sarfasl();
                    b.IdSarfasl = sarfaslLoc;
                    b.IndexSarfasl = i;

                    b.LstGroupSarfasl.Add(Convert.ToInt32(dr["orgrp_sop_order_group_id"].ToString()));

                    Sarfasls.Add(b);
                    i++;
                }

                else
                {
                    Sarfasls.Last().LstGroupSarfasl.Add(Convert.ToInt32(dr["orgrp_sop_order_group_id"].ToString()));
                }
            }


        }

        private void readJumpBetweenCrown(List<Sarfasl> Sarfasls, List<JumpBetweenCrown> JumpBetweenCrowns)
        {
            dt = new DataTable();
            string command = string.Format(@"select t.cmpdf_bas_camp_define_idfrom,
                                                       t.cmpdf_bas_camp_define_id,
                                                       t.lth_permit_from_jwgro,
                                                       t.lth_permit_to_jwgro,
                                                       t.flg_permit_jump_jwgro,
                                                       t.val_cost_group_typ_jwgro,
                                                       s.statn_bas_station_id
                                                  from mas.mas_bas_jump_wid_groups t, lmp.lmp_bas_camp_defines s
                                                 where s.statn_bas_station_id = {0}
                                                   and s.bas_camp_define_id = t.cmpdf_bas_camp_define_idfrom
                                                    and t.cod_typ_form_jwgro != {1}", RunInformation.NumStation, 3);


            dt = WorkData.GetDataTable(command, CommandType.Text);

            foreach (DataRow dr in dt.Rows)
            {
                int indexFromlocal;
                int indexTolocal;
                indexFromlocal = Sarfasls.FindIndex(c => c.IdSarfasl == Convert.ToInt32(dr["cmpdf_bas_camp_define_idfrom"]));
                indexTolocal = Sarfasls.FindIndex(c => c.IdSarfasl == Convert.ToInt32(dr["cmpdf_bas_camp_define_id"]));

                JumpBetweenCrown b = new JumpBetweenCrown();
                b.IndexSarfaslFrom = indexFromlocal;
                b.IndexSarfaslTo = indexTolocal;
                b.LenFrom = Convert.ToInt32(dr["lth_permit_from_jwgro"].Equals(DBNull.Value) ? "-1" : dr["lth_permit_from_jwgro"].ToString());
                b.LenTo = Convert.ToInt32(dr["lth_permit_to_jwgro"].Equals(DBNull.Value) ? "-1" : dr["lth_permit_to_jwgro"].ToString());
                b.FlqPossibleJump = Convert.ToInt32(dr["flg_permit_jump_jwgro"]);

                b.FlgPossibleWithChangeRoll = Convert.ToInt32(dr["flg_permit_jump_jwgro"]);
                b.CostBetweenSarfasl = Convert.ToInt32(dr["val_cost_group_typ_jwgro"].Equals(DBNull.Value) ? "0" : dr["val_cost_group_typ_jwgro"].ToString());

                JumpBetweenCrowns.Add(b);
            }

        }

        public void addCoil(DataRow dr, List<Coil> Coils, ref int counterCoil)
        {
            Coil b = new Coil(int.Parse(dr["cod_sepration_ostif"].Equals(DBNull.Value) ? Parameter.CodEfrazNull.ToString() : dr["cod_sepration_ostif"].ToString()),
                counterCoil++,

                dr["NUM_PROD_SNAMU"].Equals(DBNull.Value) ? "0" : dr["NUM_PROD_SNAMU"].ToString(),

                int.Parse(dr["opt_snapshot_mu_id"].Equals(DBNull.Value) ? "0" : dr["opt_snapshot_mu_id"].ToString()),

                1, dr["Cod_Order_Snamu"].ToString(),

                int.Parse(dr["Wid_Prod_Snamu"].Equals(DBNull.Value) ? "0" : dr["Wid_Prod_Snamu"].ToString()),

                Math.Round((double.Parse(dr["Tks_Prod_Snamu"].Equals(DBNull.Value) ? "0" : dr["Tks_Prod_Snamu"].ToString())), 2),

               double.Parse(dr["WEI_PROD_SNAMU"].Equals(DBNull.Value) ? "0" : dr["WEI_PROD_SNAMU"].ToString()),

                double.Parse(dr["LTH_PROD_SNAMU"].Equals(DBNull.Value) ? "0" : dr["LTH_PROD_SNAMU"].ToString()),

                int.Parse(dr["position"].Equals(DBNull.Value) ? "1" : dr["position"].ToString()),

               //DateTime.Parse(dr["DAT_STORAGE_INTER_SNAMU"].Equals(DBNull.Value) ? currStat.CurrTime.ToString() : dr["DAT_STORAGE_INTER_SNAMU"].ToString()),

                DateTime.Parse(dr["DAT_AVAILABLE_TIME_SNAMU"].Equals(DBNull.Value) ? Status.CurrTime.ToString() : dr["DAT_AVAILABLE_TIME_SNAMU"].ToString()),

                int.Parse(dr["COD_PRODUCT_FAMILY_ITEM_ORINF"].Equals(DBNull.Value) ? "0" : dr["COD_PRODUCT_FAMILY_ITEM_ORINF"].ToString()),

                int.Parse(dr["cod_int_qual_orinf"].Equals(DBNull.Value) ? "0" : dr["cod_int_qual_orinf"].ToString()),

                int.Parse(dr["flg_phpsphorus_qtydt"].Equals(DBNull.Value) ? "0" : dr["flg_phpsphorus_qtydt"].ToString()),

                int.Parse(dr["COD_REDUCTION_ORDER_ORINF"].Equals(DBNull.Value) ? "0" : dr["COD_REDUCTION_ORDER_ORINF"].ToString()),
                 int.Parse(dr["cod_surface_roghness_orinf"].Equals(DBNull.Value) ? "0" : dr["cod_surface_roghness_orinf"].ToString()),
                 dr["Cod_Roughness_Categories_Orinf"].Equals(DBNull.Value) ? "0" : dr["Cod_Roughness_Categories_Orinf"].ToString(),

                int.Parse(dr["COD_COAT_ORINF"].Equals(DBNull.Value) ? "0" : dr["COD_COAT_ORINF"].ToString()),
                int.Parse(dr["COD_COAT_BUT_ORINF"].Equals(DBNull.Value) ? "0" : dr["COD_COAT_BUT_ORINF"].ToString()),


                int.Parse(dr["COD_URG_ORINF"].Equals(DBNull.Value) ? "0" : dr["COD_URG_ORINF"].ToString()),

                int.Parse(dr["WID_OUT_OSTIF"].Equals(DBNull.Value) ? "0" : dr["WID_OUT_OSTIF"].ToString()),

                 Math.Round(float.Parse(dr["TKS_OUT_OSTIF"].Equals(DBNull.Value) ? "0" : dr["TKS_OUT_OSTIF"].ToString()), 2),

                 double.Parse(dr["QTY_PDW_OSTIF"].Equals(DBNull.Value) ? Parameter.PdwNull.ToString() : dr["QTY_PDW_OSTIF"].ToString()),

                 DateTime.Parse(dr["NUM_DAT_LAST_ORDER_OSTIF"].Equals(DBNull.Value) ? dr["DAT_DLV_ORDER_ORINF"].ToString() : dr["NUM_DAT_LAST_ORDER_OSTIF"].ToString()),

                 DateTime.Parse(dr["NUM_DAT_LASTsh_ORDER_OSTIF"].Equals(DBNull.Value) ? DateTime.MinValue.ToString() : dr["NUM_DAT_LASTsh_ORDER_OSTIF"].ToString()),

                  dr["TYP_PROG_OSTIF"].ToString(),

                  int.Parse(dr["cod_prog_mis"].Equals(DBNull.Value) ? "0" : dr["cod_prog_mis"].ToString()),

                  int.Parse(dr["num_tri_ostif"].Equals(DBNull.Value) ? "0" : dr["num_tri_ostif"].ToString()),

                  int.Parse(dr["typ_oil_ostif"].Equals(DBNull.Value) ? "0" : dr["typ_oil_ostif"].ToString()),

                  TimeSpan.FromHours(double.Parse(dr["WEI_PROD_SNAMU"].Equals(DBNull.Value) ? "0"
                  : dr["WEI_PROD_SNAMU"].ToString()) / (double.Parse(dr["QTY_PDW_OSTIF"].Equals(DBNull.Value) ? Parameter.PdwNull.ToString() : dr["QTY_PDW_OSTIF"].ToString()) * 1000)
                  ));



            Coils.Add(b);
        }

        public void addCoilRelease(DataRow dr, List<CoilRelease> CoilReleases, ref int counterCoilRelease)
        {

            CoilRelease b = new CoilRelease(
                int.Parse(dr["cod_sepration_ostif"].Equals(DBNull.Value) ? Parameter.CodEfrazNull.ToString() : dr["cod_sepration_ostif"].ToString()),

                counterCoilRelease++,

                double.Parse(dr["NUM_PROD_SNAMU"].Equals(DBNull.Value) ? "0" : dr["NUM_PROD_SNAMU"].ToString()),

                double.Parse(dr["opt_snapshot_mu_id"].Equals(DBNull.Value) ? "0" : dr["opt_snapshot_mu_id"].ToString()),

                1, dr["Cod_Order_Snamu"].ToString(),

                int.Parse(dr["Wid_Prod_Snamu"].Equals(DBNull.Value) ? "0" : dr["Wid_Prod_Snamu"].ToString()),

                Math.Round((double.Parse(dr["Tks_Prod_Snamu"].Equals(DBNull.Value) ? "0" : dr["Tks_Prod_Snamu"].ToString())), 3),

                double.Parse(dr["WEI_PROD_SNAMU"].Equals(DBNull.Value) ? "0" : dr["WEI_PROD_SNAMU"].ToString()),

                double.Parse(dr["LTH_PROD_SNAMU"].Equals(DBNull.Value) ? "0" : dr["LTH_PROD_SNAMU"].ToString()),

                int.Parse(dr["COD_PRODUCT_FAMILY_ITEM_ORINF"].Equals(DBNull.Value) ? "0" : dr["COD_PRODUCT_FAMILY_ITEM_ORINF"].ToString()),

                int.Parse(dr["cod_int_qual_orinf"].Equals(DBNull.Value) ? "0" : dr["cod_int_qual_orinf"].ToString()),

                int.Parse(dr["flg_phpsphorus_qtydt"].Equals(DBNull.Value) ? "0" : dr["flg_phpsphorus_qtydt"].ToString()),

                //int.Parse(dr["WID_INP_OSTIF"].Equals(DBNull.Value) ? "0" : dr["WID_INP_OSTIF"].ToString()),

                int.Parse(dr["WID_OUT_OSTIF"].Equals(DBNull.Value) ? "0" : dr["WID_OUT_OSTIF"].ToString()),

                //Math.Round(float.Parse(dr["TKS_INP_OSTIF"].Equals(DBNull.Value) ? "0" : dr["TKS_INP_OSTIF"].ToString()), 3),

                 Math.Round(float.Parse(dr["TKS_OUT_OSTIF"].Equals(DBNull.Value) ? "0" : dr["TKS_OUT_OSTIF"].ToString()), 3),

                 double.Parse(dr["QTY_PDW_OSTIF"].Equals(DBNull.Value) ? Parameter.PdwNull.ToString() : dr["QTY_PDW_OSTIF"].ToString()),

                  dr["TYP_PROG_OSTIF"].ToString(),

                  int.Parse(dr["cod_prog_mis"].Equals(DBNull.Value) ? "0" : dr["cod_prog_mis"].ToString()),

                  int.Parse(dr["num_tri_ostif"].Equals(DBNull.Value) ? "0" : dr["num_tri_ostif"].ToString()),

                  int.Parse(dr["typ_oil_ostif"].Equals(DBNull.Value) ? "0" : dr["typ_oil_ostif"].ToString()),
                  int.Parse(dr["cod_surface_roghness_orinf"].Equals(DBNull.Value) ? "0" : dr["cod_surface_roghness_orinf"].ToString()),

                  TimeSpan.FromHours(double.Parse(dr["WEI_PROD_SNAMU"].Equals(DBNull.Value) ? "0"
                  : dr["WEI_PROD_SNAMU"].ToString()) / (double.Parse(dr["QTY_PDW_OSTIF"].Equals(DBNull.Value) ? Parameter.PdwNull.ToString() : dr["QTY_PDW_OSTIF"].ToString()) * 1000)
                  ));



            CoilReleases.Add(b);
        }

        public void addRelease(DataRow dr, List<ReleaseSched> ReleaseScheds)
        {


            ReleaseSched b = new ReleaseSched(int.Parse(dr["cod_sepration_ostif"].ToString()), int.Parse(dr["NUM_PROG_TYPE_RELSC"].ToString()),
                                                int.Parse(dr["STA_RELEAS_PLAN_RELSC"].ToString()), int.Parse(dr["SEQ_PLAN_RELSC"].ToString()),
                                              double.Parse(dr["LTH_PLAN_RELSC"].ToString()), double.Parse(dr["WEI_PLAN_RELSC"].ToString()),
                                              dr["TYP_PROG_OSTIF"].ToString(),
                                               int.Parse(dr["flg_start_cmp_relsc"].Equals(DBNull.Value) ? "0" : dr["flg_start_cmp_relsc"].ToString()));

            ReleaseScheds.Add(b);
        }

        public void addTksJump(DataRow dr, List<TksJump> TksJumps)
        {
            TksJump b = new TksJump(
                int.Parse(dr["pgtyp_bas_program_type_def_id"].Equals(DBNull.Value) ? "-1" : dr["pgtyp_bas_program_type_def_id"].ToString()),
                dr["mis_prog"].ToString(),
                float.Parse(dr["TKS_COIL_BEFOR_FROM_TKJUM"].ToString()),
                float.Parse(dr["TKS_COIL_BEFOR_TO_TKJUM"].ToString()),
                int.Parse(dr["flg_in_out_tkjum"].Equals(DBNull.Value) ? "-1" : dr["flg_in_out_tkjum"].ToString()));

            TksJumps.Add(b);
        }

        public void addDBDCampPlan(DataRow dr, List<DBDCampPlan> DBDCampPlans)
        {
            DBDCampPlan b = new DBDCampPlan(int.Parse(dr["cod_group_dbd_cappa"].ToString()), DateTime.Parse(dr["DAT_PLAN_CAPPA"].ToString()));
            DBDCampPlans.Add(b);
        }

        public void addCapPlan(DataRow dr, List<CapPlan> CapPlansLoc)
        {
            CapPlan b = new CapPlan(int.Parse(dr["COD_PROD_CAPPA"].ToString()), DateTime.Parse(dr["DAT_PLAN_CAPPA"].ToString()),
                 double.Parse(dr["VAL_NET_CAP_CAPPA"].ToString()), double.Parse(dr["VAL_NET_CAP_CAPPA"].ToString()));

            CapPlansLoc.Add(b);
        }

        public void addSetup(DataRow dr, List<Setup> Setups)
        {
            Setup b = new Setup(int.Parse(dr["PGTYP_BAS_PROGRAM_TYPE_DEF_ID"].ToString()), int.Parse(dr["PGTYP_BAS_PROGRAM_TYPE_DUSES_A"].ToString()),
                double.Parse(dr["VAL_SETUP_COST_SETUT"].Equals(DBNull.Value) ? "0" : dr["VAL_SETUP_COST_SETUT"].ToString()),
                TimeSpan.Parse(dr["DUR_SETUP_TIME_SETUT"].ToString()));

            Setups.Add(b);
        }

        public void addMaxValueGroup(DataRow dr, List<MaxValueGroup> MaxValueGroups)
        {
            MaxValueGroup b = new MaxValueGroup(int.Parse(dr["order_group_id"].ToString()), double.Parse(dr["qty_MAX_daily_ogprd"].ToString()),
                                                    DateTime.Parse(dr["val_day_ogprd"].ToString()));
            MaxValueGroups.Add(b);
        }

        public void addEquipGroupFailureTime(DataRow dr, List<EquipGroupFailureTime> EquipGroupFailureTimes)
        {

            EquipGroupFailureTime b = new EquipGroupFailureTime(int.Parse(dr["order_group_id"].ToString()),
                                                                DateTime.Parse(dr["finish_time"].ToString()));

            EquipGroupFailureTimes.Add(b);
        }

        public void addStationStops(DataRow dr, List<StationStop> StationStops)
        {


            StationStop b = new StationStop(int.Parse(dr["COD_REPAIR_REPTM"].ToString()), DateTime.Parse(dr["order_group_id"].ToString()),
                                                               TimeSpan.Parse(dr["finish_time"].ToString()));

            StationStops.Add(b);

        }

    }
}
