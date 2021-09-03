using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.CommonFunctions.ParameterClasses;

namespace IPSO.CMP.CommonFunctions.Functions
{
   public class Improvment
    {
       public static void updateAfterUnexpectedInsertCoil(int flgCondition, int selectLoc, List<Coil> Coils, List<Solution> SolutionsOutputPlan,
           List<ReleaseSched> ReleaseScheds, List<CapPlanUpDate> CapPlanUpDates, List<CoilRelease> CoilReleases, string PathWriter, List<StationStop> StationStops,
           List<Scheduling> Schedulings, List<ShiftWork> ShiftWorks, List<CapPlan> CapPlans, List<MaxValueGroup> MaxValueGroups, List<int> lstAvailMaxValueGroup,
           Solution currSolution, List<Setup> Setups)
       {
           if (flgCondition == 0) // insert b avale barname fe'eli
           {
               InnerParameter.weiTotal += Coils[selectLoc].Weight;
               InnerParameter.lenTotal += Coils[selectLoc].Len;
               Solution.sumWeiLenProg(selectLoc, SolutionsOutputPlan[SolutionsOutputPlan.Count() - 1], Coils);
               //??????
               //General.updateMaxValueGroupCurr(selectLoc, MaxValueGroups, lstAvailMaxValueGroup, Coils);
           }
           else// == 1 ya 2 insert be entehaye barname ghabli ya be onvane yek barname jadid beine 2 barname
           {
               int seq = -10;
               if (flgCondition == 1)
                   seq = -3;
               else //flgCondition == 2
                   seq = -4;

               //??????
               //General.updateMaxValueGroupCurr(selectLoc, MaxValueGroups, lstAvailMaxValueGroup, Coils);

               Solution.sumWeiLenProg(selectLoc,SolutionsOutputPlan[SolutionsOutputPlan.Count() - 2], Coils);

               TimeFunc.chekTime(seq, SolutionsOutputPlan, ReleaseScheds, StationStops, Schedulings, ShiftWorks, CapPlans, MaxValueGroups,
                Coils,CoilReleases,lstAvailMaxValueGroup,currSolution,Setups);

               CapPlanFunc.calcuPffForPlans(seq, SolutionsOutputPlan, ReleaseScheds, PathWriter,CapPlanUpDates, Coils, CoilReleases);
           }
       }

    }
}
