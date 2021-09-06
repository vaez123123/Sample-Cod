using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.CommonFunctions.ParameterClasses;

namespace IPSO.CMP.CommonFunctions.Functions
{
    public class CapPlanFunc
    {

        // Different types of capacity changes
        public static void calcuPffForPlans(int seq, List<Solution> SolutionsOutputPlan, List<ReleaseSched> ReleaseScheds, string PathWriter,
            List<CapPlanUpDate> CapPlanUpDates, List<Coil> Coils, List<CoilRelease> CoilReleases)
        {

            int counter;
            if (seq == -1)
            {
                CapPlanUpDate.calcuRespondValueobj(seq, SolutionsOutputPlan[SolutionsOutputPlan.Count - 1], Coils, CapPlanUpDates);
                counter = SolutionsOutputPlan.Count;
                WriterFunc.writerCapProg(counter, "capPlanProg", PathWriter, CapPlanUpDates);
                
            }
            else if(seq == -3)
            {
                CapPlanUpDate.calcuRespondValueobj(seq, SolutionsOutputPlan[SolutionsOutputPlan.Count - 2], Coils, CapPlanUpDates);
                counter = SolutionsOutputPlan.Count - 1;
                WriterFunc.writerCapProg(counter, "capPlanProg", PathWriter, CapPlanUpDates);
            }
            else if (seq == -4)
            {
                CapPlanUpDate.calcuRespondValueobj(seq, SolutionsOutputPlan[SolutionsOutputPlan.Count - 2], Coils, CapPlanUpDates);
                counter = SolutionsOutputPlan.Count - 1;
                WriterFunc.writerCapProg(counter, "capPlanProg", PathWriter, CapPlanUpDates);
            }

            else
            {
                CapPlanUpDate.calcuRespondValueobjRelease(ReleaseScheds[seq], CoilReleases, CapPlanUpDates);
                counter = -seq;
                WriterFunc.writerCapProg(counter, "capPlanRelease", PathWriter, CapPlanUpDates);
            }
            CapPlanUpDate.resetRespondProg(CapPlanUpDates);

        }


        // Calculate the maximum amount of capacity
        public static int chekMaxCapPlan(int selectCoil, List<CapPlan> CapPlansCurr, List<Coil> Coils)
        {
            int pfLocal = Coils[selectCoil].PfId;
            double weiLocal = Coils[selectCoil].Weight;

            int indx = CapPlansCurr.FindIndex(a => a.NetValuePf >= weiLocal && a.PfId == pfLocal);
            if (indx != -1)
            {
                double maxVal = CapPlansCurr.Find(i => i.DatePlan.Date == Status.CurrTime.Date && i.PfId == pfLocal).MaxValueRespond;
                if (maxVal - weiLocal >= 0)
                    return 1;
            }

            return -1;
        }

        //Update capacity 
        public static void updateCapCurr(int select, List<CapPlan> CapPlansCurr, List<Coil> Coils)
        {
            double weiLocal = Coils[select].Weight;
            do
            {
                int indx = CapPlansCurr.FindIndex(i => i.PfId == Coils[select].PfId && i.NetValuePf > 0 &&
                    i.DatePlan.Date == CapPlansCurr.Where(j => j.PfId == Coils[select].PfId && j.NetValuePf > 0).Min(a => a.DatePlan.Date));

                if (indx != -1)
                {
                    if (weiLocal > CapPlansCurr[indx].NetValuePf)
                    {
                        weiLocal -= CapPlansCurr[indx].NetValuePf;
                        CapPlansCurr[indx].NetValuePf = 0;
                    }
                    else
                    {
                        CapPlansCurr[indx].NetValuePf -= weiLocal;
                        weiLocal = 0;
                        break;
                    }
                }
                else
                    break;
            } while (weiLocal > 0);

            CapPlan.updateMaxValueCapPlan(CapPlansCurr, select, Coils);
        }



    }
}
