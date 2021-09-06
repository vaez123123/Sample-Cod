using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPSO.CMP.CommonFunctions.ParameterClasses
{
    public class Scheduling
    {
        public int Id { get; set; }// idAfraz - indexListSetup and IndexListShutdown
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        /// <summary>
        /// 1=PLAN, 2=ALG, 3=SHUTDOWN, 4=INACTIVE, 5=SETUP, 6=finishCapPlan, 7 ,8 , -1=virtual
        /// </summary>
        public int TypId { get; set; }//نشان دهنده اینکه برنامه است یا شاتدان یا این اکتیو


        //ALL
        public static void addLstScheduling(int typeId, int indxSeq, TimeSpan setupLoc, List<ReleaseSched> ReleaseScheds,
            List<Solution> SolutionsOutputPlan, List<Scheduling> Schedulings, List<ShiftWork> ShiftWorks)
        {
            Scheduling a = new Scheduling();

            if (typeId == 1)//plan
            {
                if (setupLoc != TimeSpan.Zero)
                {
                    a.Id = Parameter.IdSetup;
                    a.Start = Status.CurrTime;
                    a.End = Status.CurrTime + setupLoc;
                    a.TypId = 5;

                    Schedulings.Add(a);
                }

                a = new Scheduling();
                a.Id = ReleaseScheds[indxSeq].IdEfraz;
                a.Start = ReleaseScheds[indxSeq].StartTimeSelectProg;
                a.End = ReleaseScheds[indxSeq].EndTimeSelectProg;
                a.TypId = typeId;
            }

            else if (typeId == 2)//alg
            {
                if (setupLoc != TimeSpan.Zero)
                {
                    a.Id = Parameter.IdSetup;
                    a.Start = Status.CurrTime;
                    a.End = Status.CurrTime + setupLoc;
                    a.TypId = 5;

                    Schedulings.Add(a);
                }

                a = new Scheduling();

                a.Id = SolutionsOutputPlan.Last().IdEfraz;
                a.Start = SolutionsOutputPlan.Last().StartTimeSelectProg;
                a.End = SolutionsOutputPlan.Last().EndTimeSelectProg;
                a.TypId = typeId;
            }

            else if (typeId == 3)
            {
                a.Id = Parameter.IdShutDown;
                a.Start = Status.CurrTime;
                a.End = Status.CurrTime + setupLoc;
                a.TypId = typeId;
            }
            else if (typeId == 4)
            {
                a.Id = Parameter.IdInactive;
                a.Start = Status.CurrTime;
                a.End = Status.CurrTime + setupLoc;
                a.TypId = typeId;
            }
            else if (typeId == 6)
            {
                a.Id = Parameter.IdFinishCapDay;
                a.Start = Status.CurrTime;
                a.End = Status.CurrTime + setupLoc;
                a.TypId = typeId;
            }

           else if (typeId == 7)
            {
                a.Id = SolutionsOutputPlan[SolutionsOutputPlan.Count - 1].IdEfraz;
                a.Start = Status.CurrTime;
                a.End = Status.CurrTime + setupLoc;
                a.TypId = typeId;
            }
            else if (typeId == 8)
            {
                 a.Id = SolutionsOutputPlan[SolutionsOutputPlan.Count- 2].IdEfraz;
                a.Start = SolutionsOutputPlan[SolutionsOutputPlan.Count- 2].StartTimeSelectProg;
                a.End = SolutionsOutputPlan[SolutionsOutputPlan.Count- 2].EndTimeSelectProg;
                a.TypId = typeId;
            }

            Schedulings.Add(a);
            

            Status.CurrTime = Schedulings.Last().End;
            ShiftWork.calcuCurrShift(Status.CurrTime, ShiftWorks);
        }

    }
}
