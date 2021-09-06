using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.CommonFunctions.ParameterClasses;
using IPSO.Functions;
using IPSO.CMP.Log;

namespace SKPScheduling
{
    public class SarfaslSKP : SarfaslL2
    {
        //SkP1
        public override void insertAvailSarfasl(List<int> lstAvailSarfasl, List<Coil> Coils, List<Roll> RollsBack, List<Sarfasl> Sarfasls,
           SarfaslL2 sarfaslLine, List<Scheduling> Schedulings, List<WidthJump> WidthJumps, List<GroupDef> GroupDefs,
           List<JumpBetweenCrown> JumpBetweenCrowns, FileLogger fileLogger)
        {
            lstAvailSarfasl.Clear();

            if (Coils.FindIndex(a => InnerParameter.lstPfAvail.Contains(a.PfId) == true) != -1)
            {
                List<int> sarfaalLocAvail = Coils.Where(b => b.FlagPlan == 1).SelectMany(a => a.LstSarfaslGroup).Distinct().ToList();

                foreach (var item in Sarfasls)
                {
                    if (sarfaalLocAvail.Contains(item.IndexSarfasl))
                        lstAvailSarfasl.Add(item.IndexSarfasl);
                }

                lstAvailSarfasl = lstAvailSarfasl.Distinct().ToList();
                lstAvailSarfasl = lstAvailSarfasl.OrderBy(a => a).ToList();
            }
            else
                InnerParameter.lstChekFinishCapplan.Add(true);

        }
    
    }
}
