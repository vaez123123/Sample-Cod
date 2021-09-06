using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.CommonFunctions.ParameterClasses;

namespace TandemScheduling
{
    public class ObjectiveSkp1
    {
        //  objective functions
        //ALL
        public void calcuTotalObj(Solution currSolution, int idEfrazLocal, int idSarfaslLocal, ref int doubleChekWorkRoll, List<Roll> RollsWork)
        {

            calcuobjPriorMisGroup(idEfrazLocal, idSarfaslLocal);

            calcuObjRoll(ref doubleChekWorkRoll,currSolution,RollsWork);// بین صفر و یک

           // calcuObjChangSarfasl();

           
            Solution.calcuTotalObjCommon(currSolution);



        }
          //TEM-SKP1
        public void calcuObjRoll(  ref int  doubleChekWorkRoll, Solution currSolution, List< Roll> RollsWork)
        {

            if (doubleChekWorkRoll == 1)
            {
                int chek = chekChangeWorkRoll(RollsWork, ref doubleChekWorkRoll,currSolution);
                currSolution.ChekChangeWorkRoll = chek;
                // not sarfasl

                if (chek == 30 || chek == 31)
                {

                    if (RollsWork.Last().LenOpt != 0)
                    {
                        double dif = RollsWork.Last().LenOpt - RollsWork.Last().LenDB - RollsWork.Last().LenRelease -
                           RollsWork.Last().CurrentTotalFixLen - currSolution.LenProg;

                        if (dif > 0)
                            currSolution.ObjLenWorkRoll = (dif / RollsWork.Last().LenOpt);

                        else
                            currSolution.ObjLenWorkRoll = 0;
                    }

                    if (RollsWork.Last().WeiOpt != 0)
                    {
                        double dif = RollsWork.Last().WeiOpt - RollsWork.Last().WeiDB - RollsWork.Last().WeiRelease -
                           RollsWork.Last().CurrentTotalFixWei - currSolution.WeiProg;
                        if (dif > 0)
                            currSolution.ObjWeiWorkRoll = (dif / (RollsWork.Last().WeiOpt));

                        else
                            currSolution.ObjWeiWorkRoll = 0;
                    }
                }
                // sarfasl
                else
                {

                    if (RollsWork.Last().LenOpt != 0)
                    {
                        double objNew = 0;
                        if (RollsWork.Last().LenOpt - InnerParameter.lenTotal > 0)
                        {
                            objNew = ((RollsWork.Last().LenOpt - InnerParameter.lenTotal) / RollsWork.Last().LenOpt);
                        }

                        else
                            objNew = 0;


                        double obj = 0;
                        double dif = RollsWork.Last().LenOpt - RollsWork.Last().LenDB - RollsWork.Last().LenRelease -
                                       RollsWork.Last().CurrentTotalFixLen;

                        if (dif > 0)
                            obj = (dif / RollsWork.Last().LenOpt);

                        else
                            obj = 0;

                        currSolution.ObjLenWorkRoll = obj * DataBase.CoefObjWorkRoll + objNew * DataBase.CoefObjWorkRollNew;


                    }
                    if (RollsWork.Last().WeiOpt != 0)
                    {
                        double objNew = 0;
                        if (RollsWork.Last().WeiOpt -InnerParameter.weiTotal > 0)

                            objNew = ((RollsWork.Last().WeiOpt - InnerParameter.weiTotal) / (RollsWork.Last().WeiOpt));

                        else
                            objNew = 0;

                        double obj = 0;

                        double dif = RollsWork.Last().WeiOpt - RollsWork.Last().WeiDB - RollsWork.Last().WeiRelease -
                           RollsWork.Last().CurrentTotalFixWei;

                        if (dif > 0)
                            obj = (dif / (RollsWork.Last().WeiOpt));

                        else
                            obj = 0;

                        currSolution.ObjWeiWorkRoll = obj * DataBase.CoefObjWorkRoll + objNew * DataBase.CoefObjWorkRollNew; ;
                    }
                }
            }

                //
            else
            {
                int chek = chekChangeWorkRoll(RollsWork,ref doubleChekWorkRoll,currSolution);
                currSolution.ChekChangeWorkRoll = chek;


                if (RollsWork.Last().LenOpt != 0)
                {
                    double objNew = 0;
                    if (RollsWork.Last().LenOpt - InnerParameter.lenTotal > 0)
                    {
                        objNew = ((RollsWork.Last().LenOpt -InnerParameter.lenTotal) / RollsWork.Last().LenOpt);
                    }

                    else
                        objNew = 0;


                    double obj = 0;
                    double dif = RollsWork.Last().LenOpt - RollsWork.Last().LenDB - RollsWork.Last().LenRelease -
                                   RollsWork.Last().CurrentTotalFixLen;

                    if (dif > 0)
                        obj = (dif / RollsWork.Last().LenOpt);

                    else
                        obj = 0;

                    currSolution.ObjLenWorkRoll = obj * DataBase.CoefObjWorkRoll + objNew * DataBase.CoefObjWorkRollNew;


                }
                if (RollsWork.Last().WeiOpt != 0)
                {
                    double objNew = 0;
                    if (RollsWork.Last().WeiOpt -InnerParameter.weiTotal > 0)

                        objNew = ((RollsWork.Last().WeiOpt - InnerParameter.weiTotal) / (RollsWork.Last().WeiOpt));

                    else
                        objNew = 0;

                    double obj = 0;

                    double dif = RollsWork.Last().WeiOpt - RollsWork.Last().WeiDB - RollsWork.Last().WeiRelease -
                       RollsWork.Last().CurrentTotalFixWei;

                    if (dif > 0)
                        obj = (dif / (RollsWork.Last().WeiOpt));

                    else
                        obj = 0;

                    currSolution.ObjWeiWorkRoll = obj * DataBase.CoefObjWorkRoll + objNew * DataBase.CoefObjWorkRollNew; ;
                }

            }




        }

         //TEM-SKP1
        public int chekChangeWorkRoll(List<Roll> lstRollLocal,ref int  doubleChekWorkRoll,Solution currSolution)
        {
            
            #region if (lstRollLocal.Last().weiOpt != 0)

            if (lstRollLocal.Last().WeiOpt != 0)
            {
                // Changing roll because of sarfasl

                if (Status.IndexSarfasl != -1 && Status.IndexSarfasl !=currSolution.IndexSarfasl)
                {
                    // Changing roll because of capacity

                    if ((lstRollLocal.Last().WeiOpt) * (1-lstRollLocal.Last().LowerPerc) < (//lstRollLocal.Last().weiDB
                        //  + lstRollLocal.Last().currentTotalFixWei
                        // + lstRollLocal.Last().weiRelease
                                                           +currSolution.WeiProg))
                        return 10;

                    else
                        return 11;
                }


                else
                {
                    if (doubleChekWorkRoll == 1)
                    {
                        //    
                        // Changing roll because of capacity

                        if ((lstRollLocal.Last().WeiOpt) * (1 - lstRollLocal.Last().LowerPerc) < (lstRollLocal.Last().WeiDB
                                                                + lstRollLocal.Last().CurrentTotalFixWei
                                                                + lstRollLocal.Last().WeiRelease
                                                                + currSolution.WeiProg))

                            return 30;


                        else
                            return 31;

                    }

                    //(doubleChekWorkRoll == 2)
                    else
                    {
                          
                        //(doubleChekWorkRoll == 2)
                       

                        if ((lstRollLocal.Last().WeiOpt) * (1 - lstRollLocal.Last().LowerPerc) < (//lstRollLocal.Last().weiDB
                            //  + lstRollLocal.Last().currentTotalFixWei
                            // + lstRollLocal.Last().weiRelease
                                                               +currSolution.WeiProg))
                            return 20;


                        else
                            return 21;


                    }

                }



            }
            #endregion


            #region if (lstRollLocal.Last().lenOpt != 0)

            else
            {

                // Changing roll because of sarfasl

                if (Status.IndexSarfasl != -1 && Status.IndexSarfasl != currSolution.IndexSarfasl)
                {
                    //Changing roll because of capacity

                    if ((lstRollLocal.Last().LenOpt) * (1 - lstRollLocal.Last().LowerPerc) < (//lstRollLocal.Last().lenDB
                        //  + lstRollLocal.Last().currentTotalFixLen
                        // + lstRollLocal.Last().lenRelease
                                                           +currSolution.LenProg))
                        return 10;

                    else
                        return 11;
                }
                else
                {
                    if (doubleChekWorkRoll == 1)
                    {
                        //    Changing roll because of capacity


                        if ((lstRollLocal.Last().LenOpt) * (1 - lstRollLocal.Last().LowerPerc) < (lstRollLocal.Last().LenDB
                                                                + lstRollLocal.Last().CurrentTotalFixLen
                                                                + lstRollLocal.Last().LenRelease
                                                                + currSolution.LenProg))

                            return 30;


                        else
                            return 31;

                    }

                    //(doubleChekWorkRoll == 2)
                    else
                    {
                       
                        if ((lstRollLocal.Last().LenOpt) * (1 - lstRollLocal.Last().LowerPerc) < (//lstRollLocal.Last().lenDB
                            //  + lstRollLocal.Last().currentTotalFixLen
                            // + lstRollLocal.Last().lenRelease
                                                          +currSolution.LenProg))
                            return 20;


                        else
                            return 21;


                    }

                }




            }
            #endregion


        }

    }
}
