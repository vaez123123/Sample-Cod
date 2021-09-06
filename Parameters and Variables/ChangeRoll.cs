using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPSO.CMP.CommonFunctions.ParameterClasses
{
   public class ChangeRoll
    {
        public int IndexSarfaslFrom;
        public int IndexSarfaslTo;
        public int IdMisProgFrom;
        public int IdMisProgTo;
        public int FlagIncreaseWid;// یک نشان دهنده افزایش عرضی
        public int FlagChangeRoll;//یک نیاز به تغییر غلتک دارد
        public int FlagMinCamp;//برای برنامه های حساس باید از مینیم عرض کمپین شروع شوند 
        public static bool changeRoll = true; // true change  fals no change
       //Tan
        public enum WorkRollEnum
        {
            NotDecreaseIncreaseNewBeforRoll = 60,
            NotDecreaseIncreaseNotChangeRoll = 6,

            NotSarfaslNewbeforRoll = 50,
            NotSarfaslChangeRoll = 5,

            NotProgNewbeforRoll = 40,
            NotProgChangeRoll = 4,

            NotDataNewbeforRoll = 30,
            NotDataNotChangeRoll = 3,

            DecreaseIncreaseNewBeforRoll = 20,
            DecreaseIncreaseChangeRoll = 2,

            IncreaseNewBeforRoll = 10,
            IncreaseNotChangeRoll = 1,

            AfterSolutionIncreaseChangeNewRoll = 100,
            AfterSolutionIncreaseChangeRoll = 101,
            AfterSolutionIncreaseNotChangeNewRoll = 200,
            AfterSolutionIncreaseNotChangeRoll = 201,

            AfterSolutionDecreaseChangeNewRoll = 300,
            AfterSolutionDecreaseChangeRoll = 301,
            AfterSolutionDecreaseNotChangeNewRoll = 400,
            AfterSolutionDecreaseNotChangeRoll = 401,

            NewRollbutUseOldRoll = 1000,
            OldRoll = 1001
        };
      
        public ChangeRoll()
        { }

        public ChangeRoll(int indexSarfaslFrom, int indexSarfaslTo, int idMisProgFrom, int idMisProgTo, int flagIncreaseWid, int flagChangeRoll, int flagMinCamp)
        {
            this.IndexSarfaslFrom = indexSarfaslFrom;
            this.IndexSarfaslTo = indexSarfaslTo;
            this.IdMisProgFrom = idMisProgFrom;
            this.IdMisProgTo = idMisProgTo;
            this.FlagChangeRoll = flagChangeRoll;
            this.FlagIncreaseWid = flagIncreaseWid;
            this.FlagMinCamp = flagMinCamp;
          

        }
    }
}
