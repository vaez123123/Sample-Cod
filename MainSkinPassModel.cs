using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.CommonFunctions.ParameterClasses;
using IPSO.CMP.CommonFunctions.Functions;
using IPSO.ParameterClasses;
using IPSO.CMP.Tan_Skp_Tem_Function.ConstraintSequence;
using IPSO.CMP.Log;
using IPSO.Functions;


namespace SKPScheduling
{
    public class MainSkinPassModel
    {
        
        private FunctionSKP functionSKP = new FunctionSKP();
        private SarfaslSKP sarfaslSKP = new SarfaslSKP();

        private ObjectiveSkp objectiveSkp = new ObjectiveSkp();

        private ReleaseSKP releaseSKP = new ReleaseSKP();
        private GroupDefSKP groupDefSKP = new GroupDefSKP();
        private SequenceSKP sequenceSKP = new SequenceSKP();
        private RollSKP rollSKP = new RollSKP();

        public void runSkinPass1Model(CommonLists Lst)
        {
           functionSKP.chekStatBeforAlgorithm(Lst,releaseSKP,functionSKP,WriterSKP.PathWriter,WriterSKP.flgWriter);
            do
            {

                // For each sarfasl and efraz
                MainL2.doForSarfaslEfraz(Lst, rollSKP, functionSKP, sequenceSKP, objectiveSkp, sarfaslSKP);

                if (RunInformation.flgStopAlgorithm == -1)
                    break;

                #region  When no program is created
                int chekCountinue = 0;

                chekCountinue = MainL2.chekNotProg(Lst);


                if (chekCountinue == 1)

                    continue;
                else if (chekCountinue == 0)

                    break;
                #endregion


              MainL2.fixSolutionAndUpdate(Lst, rollSKP,functionSKP);

                if (RunInformation.flgStopAlgorithm == -1)
                    break;

                InnerParameter.finiTimeAlgorithm = Status.CurrTime;
            } while ((Lst.SolutionsOutputPlan.Count < RunInformation.CountProg


            || ((InnerParameter.finiTimeAlgorithm - InnerParameter.starTimeAlgorithm).TotalMinutes) < RunInformation.Hours.TotalMinutes)


            && RunInformation.flgStopAlgorithm != -1);

            CapPlanUpDate.chekCoilWithoutSarfasl(Lst.CapPlanUpDates, Lst.Coils);



        }

    }
}
