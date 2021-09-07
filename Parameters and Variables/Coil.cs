using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPSO.CMP.CommonFunctions.ParameterClasses
{
    public class Coil
    {

        #region
        public string message;

        //********************************coil****************************

        public int IdEfraz { get; set; }// idAfraz OF class ProgAFraz
        public int IdSnapshot { get; private set; }//DB
        public string IdCoil { get; private set; }//DB
        public string IdOrder { get; private set; }//DB     
        public int Width { get; private set; }//DB
        public double Tks { get; private set; }//DB

        public double Weight { get; private set; }//DB
        public double Lth{ get; private set; }//DB
        public double LenKmInput { get; private set; }//DB
        public double LenKmOutput { get; private set; }//DB
        public int LevelStorge { get; private set; }//DB   
        public DateTime AvailTime { get; private set; }//DB  

        //Auto
        public int FlagPlan { get; set; }//DB 1= ghabel barname rizi  -1  in plan
        public int ModelIndexCoil { get; private set; }//AUTO~INDEX COIL LIST 

        //**********************Order***********************

        public int Priority { get; private set; }//DB
       
        public int PfId { get; private set; }//DB
        //Pic
        public int CodQuality { get; private set; }//DB  =cod 4 raghami keifiat
        //Pic
        public int FlgPhosphorus { get; private set; } //DB  //1 = phosphor darad ,  0 = no phosphor


        //Tem
        public int CodReduction { get; set; }//DB  value : 0,1
        //Tem-Tin 
        public int CodSurfaceroughness { get; set; }//DB  
        //Tem
        public string RoughnessCategories { get; set; }//DB
        ////Tem
        //public string Surfaceroughness { get; set; }//DB
        ////Tem
        //public int CodRoughnessCategories { get; set; }//DB
        ////Tem
        //public string Reduction { get; set; }//DB
        //Tin-Gal-Col
        public int CodCoat { get; set; }//DB// code pusheshe sath //  
        //Gal-Col
        public int CodCoatBottom { get; set; }//DB

        //color
        //public TypeProduct
      
        //*************************OrderStation***************************
        //Pic
        public int WidthOutput { get; private set; }//DB
        //Tan - SRM
        public double TksOutput { get; private set; } //DB
        public double Pdw { get; private set; }//DB
        public DateTime DatLast { get; private set; }//DB
        public DateTime DatLastSh { get; private set; }//DB // tarikh be shamsi
        public string MisProg { get; private set; }//DB
        //Pic
        public int Trim { get; private set; }//DB
        //Pic
        public int Oil { get; private set; }//DB ??
        public int CodMisProg { get; private set; }//DB
        // MET
        public TimeSpan DuProcessTime { get; private set; }//MET
        public DateTime EndProduceTime { get; set; }//MET
        public DateTime StartProduceTime { get; set; }//MET
        public double RankDatLast { get; set; }//MET
        public double RankPriority { get; set; }//MET
        public double RankLevelStorge { get; set; }//MET
        public double RankCapPlan { get; set; }//MET
        public double RankTotal { get; set; }//MET
        public double RankDurability { get; set; }//MET
        //Pic
        public double RankSameWidGroup { get; set; }//MET
        //Pic
        public double RankFix { get; set; }//MET

        public List<int> LstEquipGroupFailureTime = new List<int>();//          
        public List<int> LstSarfaslGroup = new List<int>();// index of class sarfasl
        public List<int> LstMaxValueGroup = new List<int>();
        public List<int> LstGroupDef = new List<int>();
        //Pic-SRM
        public List<int> LstGroupDefZone = new List<int>();
        //Pic-SRM
        public List<int> LstZone = new List<int>();


        public Coil(int idEfraz, int modelIndexCoil, string idCoil, int idSnapshot, int flagPlan, string idOrder, int width,double tks,
                double weight, double lth,double lenKmInput, double lenKmOutput, int levelStorge, DateTime availTime, int pfId, int codQuality,
                int flgPhosphorus, int CodReduction,int CodSurfaceroughness, string RoughnessCategories, int CodCoat, int CodCoatBottom,
                int priority, int widthOutput, double tksOutput, double pdw, DateTime datLast, DateTime datLastSh,
                string misProg, int codMisProg, int trim, int oil, TimeSpan duProcessTime
            )
        {
            this.IdEfraz = idEfraz;
            this.ModelIndexCoil = modelIndexCoil;
            this.IdCoil = idCoil;
            this.FlagPlan = flagPlan;
            this.IdOrder = idOrder;
            this.Width = width;
            this.Tks = tks;
            this.Weight = weight;
            this.Lth = lth;
            this.LenKmInput = lenKmInput;
            this.LenKmOutput = lenKmOutput;
            this.LevelStorge = levelStorge;
            this.AvailTime = availTime;
            this.PfId = pfId;
            this.CodQuality = codQuality;
            this.FlgPhosphorus = flgPhosphorus;

            this.CodReduction = CodReduction;
            this.CodSurfaceroughness = CodSurfaceroughness;
            this.RoughnessCategories = RoughnessCategories;
            this.CodCoat = CodCoat;
            this.CodCoatBottom = CodCoatBottom;

            this.Priority = priority;
            this.WidthOutput = widthOutput;
            this.TksOutput = tksOutput;

            this.Pdw = pdw;
            this.DatLast = datLast;
            this.DatLastSh = datLastSh;
            this.MisProg = misProg;
            this.CodMisProg = codMisProg;
            this.Trim = trim;
            this.Oil = oil;

            this.DuProcessTime = duProcessTime;
            this.IdSnapshot = idSnapshot;
        }



        public Coil() { }

        #endregion

        #region Rank
        public static void calcuRankDatLast(int modelIndexCoil, List<Coil> Coils)
        {
            if (DataBase.MaxDatLast.Date != DataBase.MinDatLast.Date)
            {
                double difDat = ((Status.CurrTime.Date - Coils[modelIndexCoil].DatLast.Date).Days);

                if (difDat > DataBase.BoundDatlast)
                    Coils[modelIndexCoil].RankDatLast = 1;
                else
                {
                    double difDat1 = ((DataBase.MaxDatLast.Date - Coils[modelIndexCoil].DatLast.Date).Days);
                    Coils[modelIndexCoil].RankDatLast = Math.Round(difDat1 / ((DataBase.MaxDatLast.Date - DataBase.MinDatLast.Date).Days), 4);
                }
            }

            else
                Coils[modelIndexCoil].RankDatLast = 0;
        }

        public static void calcuRankDurability(int modelIndexCoil, List<Coil> Coils)
        {
            if (DataBase.MaxAvailTime.Date != DataBase.MinAvailTime.Date)
            {
                double difDur = ((Status.CurrTime.Date - Coils[modelIndexCoil].AvailTime.Date).Days);

                if (difDur > DataBase.BoundDurability)
                    Coils[modelIndexCoil].RankDurability = 1;
                else
                {
                    double difDur1 = ((DataBase.MaxAvailTime.Date - Coils[modelIndexCoil].AvailTime.Date).Days);
                    Coils[modelIndexCoil].RankDurability = Math.Round(difDur1 / ((DataBase.MaxAvailTime.Date - DataBase.MinAvailTime.Date).Days), 4);
                }
            }

            else
                Coils[modelIndexCoil].RankDurability = 0;
        }

        public static void calcuRankPriority(int modelIndexCoil, List<Coil> Coils)
        {
            if (DataBase.MaxPriority != 0)
                Coils[modelIndexCoil].RankPriority = Coils[modelIndexCoil].Priority / DataBase.MaxPriority;
            else
                Coils[modelIndexCoil].RankPriority = 0;
        }

        public static void calcuRankLevelStorge(int modelIndexCoil, List<Coil> Coils)
        {
            if (DataBase.MaxLevelStorge != 0)

                Coils[modelIndexCoil].RankLevelStorge = Coils[modelIndexCoil].LevelStorge / DataBase.MaxLevelStorge;
            else
                Coils[modelIndexCoil].RankLevelStorge = 0;
        }

        public static void calcuTotalRank(int modelIndexCoilLocal, List<Coil> Coils)
        {
            calcuRankDatLast(modelIndexCoilLocal, Coils);
            calcuRankDurability(modelIndexCoilLocal, Coils);
            calcuRankPriority(modelIndexCoilLocal, Coils);
            if(RunInformation.FlgOptSim == 0)
            calcuRankLevelStorge(modelIndexCoilLocal, Coils);

            double weightRank = WeightRank.DatLastCoef + WeightRank.LevelStorgeCoef + WeightRank.PriorityCoef + WeightRank.DurabilityCoef;

           Coils[modelIndexCoilLocal].RankTotal= Coils[modelIndexCoilLocal].RankFix =
                                                (WeightRank.DatLastCoef * Coils[modelIndexCoilLocal].RankDatLast) +
                                                (WeightRank.LevelStorgeCoef * Coils[modelIndexCoilLocal].RankLevelStorge) +
                                                 (WeightRank.PriorityCoef * Coils[modelIndexCoilLocal].RankPriority) +
                                                   (WeightRank.DurabilityCoef * Coils[modelIndexCoilLocal].RankDurability)
                                                   ;

           Coils[modelIndexCoilLocal].RankTotal = Coils[modelIndexCoilLocal].RankFix = 
               Math.Round(Coils[modelIndexCoilLocal].RankFix / weightRank, 4) * 100;

             
        }

        #endregion Rank

       
 

        public static void flgCoilClass(Solution bestSolution, List<Coil> Coils)
        {
            foreach (int i in bestSolution.LstSeqCoil)
                Coils[i].FlagPlan = -1;
        }

        public static void chekAvailSarfaslForCoils(List<Coil> Coils)
        {
            foreach (var item in Coils)
            {
                if (item.LstSarfaslGroup.Count() == 0)
                {
                    //error message = Sarfasl is Null and coil is not avail for planning
                    item.FlagPlan = -1;
                }
            }
        }

        //vorudi baraye coilhaye asli : flgTypeCoil=0 va lstNotAvailLenForCoilLocal = lstNotAvailLenForCoil
        //vorudi baraye coil Release : flgTypeCoil= 1 va lstNotAvailLenForCoilLocal = lstNotAvailLenForCoilRelease
        public static void chekCoilWithLenOutZero(List<Coil> Coils, List<CoilRelease> CoilReleases, int flgTypeCoil,
            List<int> lstNotAvailLenForCoilLocal)
        {
            if (flgTypeCoil == 0)// baraye list coilhaye asli
            {
                foreach (Coil c in Coils)
                {
                    if (c.LenKmOutput - c.LenKmInput < 0)
                        lstNotAvailLenForCoilLocal.Add(c.ModelIndexCoil);
                }
            }
            else if (flgTypeCoil == 1)// baraye list coilhaye release
            {
                foreach (CoilRelease c in CoilReleases)
                {
                    if (c.LenKmOutput - c.Len < 0)
                        lstNotAvailLenForCoilLocal.Add(c.CoilIndex);
                }
            }
        }
    }

}
