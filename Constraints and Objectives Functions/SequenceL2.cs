using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.CommonFunctions.ParameterClasses;
using IPSO.ParameterClasses;
using IPSO.CMP.CommonFunctions.Functions;

namespace IPSO.CMP.Tan_Skp_Tem_Function.ConstraintSequence
{
    public class SequenceL2
    {

       static double tksCoilAfterInput;
       static double tksCoilAfterOutput;
       static double tksCoilBeforInput;
       static double tksCoilBeforOutput;

       static double maxTks;
       static double maxTksout;
       static double limiteJumpInput;

       static double valueJumpOutput;
       static double valueJumpInput;

       static double tksCoilInput;
       static double tksCoilOutput;

       static double widCoilAfter;
       static double widCoilBefor;
       static double widCoil;


       public  int seqCoilProg(int coilSelection, bool chekDecInc, int idEfrazLocal, Solution currSolution,
            List<ProgEfraz> ProgEfrazes, List<Coil> Coils, List<TksJump> TksJumps)
        {

            int countSize = currSolution.LstSeqCoil.Count();

                if (countSize == 0)
            {
                ////////////////
                if (InnerParameter.RuleBetweenProg == 1)
                {
                    

                    int chek = seqFirst(coilSelection, idEfrazLocal , ProgEfrazes,  Coils,TksJumps);
                    if (chek == 1)
                    {

                        currSolution.LstSeqCoil.Add(coilSelection);


                        return 1;
                    }
                    else
                        return -1;
                }
                else
                {
                    currSolution.LstSeqCoil.Add(coilSelection);

                    return 1;
                }

            }


            else
                {


                    #region //Single member
                    if (countSize == 1)
                {

                     bool widChek = chekJumpWid(coilSelection, -1, currSolution.LstSeqCoil[0], Coils);

                    if (widChek == true)
                    {

                        bool tksChek = chekJumpTks(coilSelection, -1, currSolution.LstSeqCoil[0], Coils, TksJumps);


                        if (tksChek == true)
                        {

                            //////////////
                            if (InnerParameter.RuleBetweenProg == 1)
                            {
                                int chek = seqFirst(coilSelection, idEfrazLocal, ProgEfrazes, Coils,TksJumps);

                                if (chek == 1)
                                {


                                    currSolution.LstSeqCoil.Insert(0, coilSelection);


                                    return 1;
                                }
                                else
                                    return -1;
                            }

                            else
                            {
                                currSolution.LstSeqCoil.Insert(0, coilSelection);


                                return 1;

                            }


                        }
                        else
                        {
                            widChek = chekJumpWid(coilSelection, currSolution.LstSeqCoil[0], -1,Coils);
                            if (widChek == true)
                            {
                                tksChek = chekJumpTks(coilSelection, currSolution.LstSeqCoil[0], -1, Coils, TksJumps);
                                if (tksChek == true)
                                {
                                    currSolution.LstSeqCoil.Insert(1, coilSelection);


                                    return 1;
                                }
                                else
                                    return -1;
                            }
                            else
                                return -1;

                        }


                    }

                     // Compared to previous
                    else//if( widChek==true)
                    {
                        widChek = chekJumpWid(coilSelection, currSolution.LstSeqCoil[0], -1, Coils);
                        if (widChek == true)
                        {
                            bool tksChek = chekJumpTks(coilSelection, -1, currSolution.LstSeqCoil[0], Coils, TksJumps);
                            if (tksChek == true)
                            {
                                currSolution.LstSeqCoil.Insert(1, coilSelection);

                                return 1;
                            }
                            else
                                return -1;
                        }
                        else
                            return -1;


                    }


                }



                #endregion


                    #region //Several members
                    else
                {
                    //  for (int i = currSolution.lstSeqCoil.Count - 1; i < currSolution.lstSeqCoil.Count; i--)
                    for (int i = currSolution.LstSeqCoil.Count - 1; i >= 0; i--)
                    {

                      
                        #region  //  first coil
                        if (i == currSolution.LstSeqCoil.Count - 1)
                        {
                            bool one;
                            one = chekOneTwo(coilSelection, currSolution.LstSeqCoil[i], -1, Coils, TksJumps);
                            if (one == true)
                            {
                                // after i
                                currSolution.LstSeqCoil.Add(coilSelection);


                                return 1;
                            }
                            else
                            {

                                one = chekOneTwo(coilSelection, currSolution.LstSeqCoil[i - 1], currSolution.LstSeqCoil[i],Coils,TksJumps);

                                if (one == true)
                                {
                                // before i
                                    currSolution.LstSeqCoil.Insert(i, coilSelection);


                                    return 1;

                                }
                                //
                                else
                                {
                                    continue;
                                }
                            }


                        }

                        #endregion



                        #region // last


                        else if (i == 0)
                        {

                            bool one;
                            one = chekOneTwo(coilSelection, -1, currSolution.LstSeqCoil[i],Coils, TksJumps);
                            if (one == true)
                            {


                                if (InnerParameter.RuleBetweenProg == 1)
                                {

                                    int chek = seqFirst(coilSelection, idEfrazLocal,ProgEfrazes,Coils,TksJumps);

                                    if (chek == 1)
                                    {

                                        // before i
                                        currSolution.LstSeqCoil.Insert(i, coilSelection);


                                        return 1;
                                    }
                                    else
                                        return -1;
                                }

                                else
                                {
                                    //before i
                                   currSolution.LstSeqCoil.Insert(i, coilSelection);


                                    return 1;

                                }
                                ////////////
                            }

                            else
                                return -1;


                        }
                        #endregion

                        else
                        {
                               if (currSolution.LstSeqCoil[i + 1] != null)
                            {
                                bool one;
                                one = chekOneTwo(coilSelection, currSolution.LstSeqCoil[i], currSolution.LstSeqCoil[i + 1], Coils, TksJumps);

                                if (one == true)
                                {
                                    ///****************************
                                    currSolution.LstSeqCoil.Insert(i + 1, coilSelection);


                                    return 1;
                                }

                                else
                                {
                                     if (currSolution.LstSeqCoil[i - 1] != null)
                                    {
                                        one = chekOneTwo(coilSelection, currSolution.LstSeqCoil[i - 1],currSolution.LstSeqCoil[i], Coils,TksJumps);

                                        if (one == true)
                                        {
                                               currSolution.LstSeqCoil.Insert(i, coilSelection);

                                            return 1;


                                        }
                                        else
                                            continue;
                                    }
                                    else
                                        return -1;

                                }


                            }
                            else
                            {
                                 if (currSolution.LstSeqCoil[i - 1] != null)
                                {
                                    bool one = chekOneTwo(coilSelection, currSolution.LstSeqCoil[i - 1], currSolution.LstSeqCoil[i], Coils,  TksJumps);

                                    if (one == true)
                                    {
                                         currSolution.LstSeqCoil.Insert(i - 1, coilSelection);

                                        return 1;


                                    }
                                    else
                                        continue;
                                }
                                else
                                    return -1;



                            }


                        }



                    }//  for (int i = lstCurrentSeqCoil.Count - 1; i < lstCurrentSeqCoil.Count; i--)

                    return -1;

                }// else

                #endregion



            }



        }
         //ALL
       public static int seqFirst(int coilSelection, int idEfrazLocal, List<ProgEfraz> ProgEfrazes, List<Coil> Coils, List<TksJump> TksJumps)
        {

            string misProgLocal = ProgEfrazes.Find(c => c.IdEfraz == idEfrazLocal).ProgMis;
            //int idEfrazMis = lstProgEFraz.Find(c => c.idEfraz == idEfrazLocal).codProgMis;

            double tksCoilInput = Coils[coilSelection].Tks;
            double tksCoilOutput = Coils[coilSelection].TksOutput;
            double maxTks = Math.Max(Status.LastTks, tksCoilInput);
            // int widJumpLocalAsc;
            // int widJumpLocalDes;

            double maxTksout = Math.Max(Status.LastTksOut, tksCoilOutput);
            double valueJumpOutput;
            double valueJumpInput;
            double limiteJumpInput;


            //widJumpLocalDes = calcuWidJumpDec(idEfrazMis, 1);
            //widJumpLocalAsc = calcuWidJumpAsc(idEfrazMis, 1);

            TimeParameter.Timetks.Start();

            TimeParameter.TimetksIn.Start();
            valueJumpInput = calcuTksJumpInput(maxTks, TanSkpTemParameter.idEfrazMisCurr, TksJumps);
            TimeParameter.TimetksIn.Stop();

            TimeParameter.TimetksOutput.Start();
            valueJumpOutput = calcuTksJumpOutput(maxTksout, TanSkpTemParameter.idEfrazMisCurr, TksJumps);
            TimeParameter.TimetksOutput.Stop();

            TimeParameter.Timetkslimi.Start();
            limiteJumpInput = calcuLimitedJumpTks(maxTks, TanSkpTemParameter.idEfrazMisCurr, TksJumps);
            TimeParameter.Timetkslimi.Stop();
            TimeParameter.Timetks.Stop();
            int WidJumpFinal;

         

            if (TanSkpTemParameter.changeRoll == true && TanSkpTemParameter.lstNotSensitive.Contains(idEfrazLocal) == true)

                WidJumpFinal = InnerParameter.widJumpLocalOutAsc;
            else
                WidJumpFinal = InnerParameter.widJumpLocalOutDes;




            if (Math.Abs(Coils[coilSelection].Width - Status.LastWid) <= WidJumpFinal)
            {

                if (Math.Round(Math.Abs(tksCoilInput - Status.LastTks), 2) <= Math.Round(Math.Min((valueJumpInput * maxTks), limiteJumpInput), 2) &&
              Math.Round(Math.Abs(tksCoilOutput - Status.LastTksOut), 2) <= Math.Round((valueJumpOutput * maxTksout), 2))
                    return 1;

                else
                    return -1;
            }
            else
                return -1;
        }

       public static bool chekJumpWid(int coilSelection, int coilBefor, int coilAfter,  List<Coil> Coils)
        {

             widCoil = Coils[coilSelection].Width;

            // int idEfrazMis = lstProgEFraz.Find(c => c.idEfraz == idEfrazLocal).codProgMis;



            // int widJumpLocal = calcuWidJumpDec(idEfrazMis,0);


            #region if (coilBefor == -1)

            if (coilBefor == -1)
            {

                 widCoilAfter = Coils[coilAfter].Width;


                   if (TanSkpTemParameter.chekDecInc == true)
                {

                    // Sinusoidal
                    if (InnerParameter.flgSeqWidOut == (int)Parameter.EnumStatusJump.Swinging)
                    {

                        if (Math.Abs((widCoil - widCoilAfter)) <= InnerParameter.widJumpLocalInDes)
                            return true;
                        else
                            return false;
                    }


                    // Ascending
                    else if (InnerParameter.flgSeqWidOut == (int)Parameter.EnumStatusJump.Ascending)
                    {
                        // Ascending
                        if ((widCoil - widCoilAfter) <= 0 && Math.Abs((widCoil - widCoilAfter)) <= InnerParameter.widJumpLocalInDes)

                            return true;
                        else
                            return false;
                    }
                    // Descending

                    else
                    {
                        if ((widCoil - widCoilAfter) >= 0 && Math.Abs((widCoil - widCoilAfter)) <= InnerParameter.widJumpLocalInDes)
                            return true;
                        else
                            return false;
                    }

                }
                // sarbarname
                else
                {

                    if ((widCoil - widCoilAfter) <= 0 && Math.Abs((widCoil - widCoilAfter)) <= InnerParameter.widJumpLocalInDes
                        //&& (widCoilAfter-widCoil  ) <= DataBase.widJump
                        )
                        return true;
                    else
                        return false;
                }


            }
#endregion


            #region  if(coilAfter == -1)
            else if (coilAfter == -1)
            {

                 widCoilBefor = Coils[coilBefor].Width;


                if (TanSkpTemParameter.chekDecInc == true)
                {


                    // Sinusoidal
                    if (InnerParameter.flgSeqWidOut == (int)Parameter.EnumStatusJump.Swinging)
                    {

                        if (Math.Abs((widCoilBefor - widCoil)) <= InnerParameter.widJumpLocalInDes)
                            return true;
                        else
                            return false;
                    }

                    // Ascending
                    else if (InnerParameter.flgSeqWidOut == (int)Parameter.EnumStatusJump.Ascending)
                    {

                        if ((widCoilBefor - widCoil) <= 0 && Math.Abs((widCoilBefor - widCoil)) <= InnerParameter.widJumpLocalInDes)
                            return true;
                        else
                            return false;
                    }
                    // Descending
                    else

                    {

                        if ((widCoilBefor - widCoil) >= 0 && Math.Abs((widCoilBefor - widCoil)) <= InnerParameter.widJumpLocalInDes)
                            return true;
                        else
                            return false;
                    }
                }

                    
                else
                {

                    if ((widCoilBefor - widCoil) <= 0 && Math.Abs((widCoilBefor - widCoil)) <= InnerParameter.widJumpLocalInDes
                        // && (widCoil- widCoilBefor) <= DataBase.widJump
                        )
                        return true;
                    else
                        return false;

                }

            }

            #endregion



        }
        //ALL
       public static bool chekJumpTks(int coilSelection, int coilBefor, int coilAfter, List<Coil> Coils, List<TksJump> TksJumps)
        {

            bool chekJumpLoc;
             tksCoilInput = Coils[coilSelection].Tks;
             tksCoilOutput = Coils[coilSelection].TksOutput;
            // string idEfrLocal = lstProgEFraz.Find(c => c.idEfraz == idEfrazLocal).progMis;
            // int idEfrazMis = lstProgEFraz.Find(c => c.idEfraz == idEfrazLocal).codProgMis;


            if (coilBefor == -1)
            {

                tksCoilAfterInput = Coils[coilAfter].Tks;
                tksCoilAfterOutput = Coils[coilAfter].TksOutput;

                 maxTks = Math.Max(tksCoilAfterInput, tksCoilInput);

                 maxTksout = Math.Max(tksCoilAfterOutput, tksCoilOutput);
            
                TimeParameter.Timetks.Start();

                TimeParameter.TimetksOutput.Start();
                valueJumpOutput = calcuTksJumpOutput(maxTksout, TanSkpTemParameter.idEfrazMisCurr, TksJumps);
                TimeParameter.TimetksOutput.Stop();
                TimeParameter.TimetksIn.Start();
                valueJumpInput = calcuTksJumpInput(maxTks, TanSkpTemParameter.idEfrazMisCurr, TksJumps);
                TimeParameter.TimetksIn.Stop();
                TimeParameter.Timetkslimi.Start();
                limiteJumpInput = calcuLimitedJumpTks(maxTks, TanSkpTemParameter.idEfrazMisCurr, TksJumps);
                TimeParameter.Timetkslimi.Stop();

                TimeParameter.Timetks.Stop();



                chekJumpLoc = chekJumpTksIn(-1);
                if (chekJumpLoc == true)
                {
                    chekJumpLoc = chekJumpTksOut(-1);
                    if (chekJumpLoc == true)
                        return true;
                    else
                        return false;
                }
                else
                    return false;

                //if (Math.Round(Math.Abs(tksCoilInput - tksCoilAfterInput), 2) <= Math.Round((Math.Min((valueJumpInput * maxTks), limiteJumpInput)), 2) &&
                //    Math.Round(Math.Abs(tksCoilOutput - tksCoilAfterOutput), 2) <= Math.Round((valueJumpOutput * maxTksout), 2))

                //    return true;
                //else
                //    return false;

              


            }
            else if (coilAfter == -1)
            {

                tksCoilBeforInput = Coils[coilBefor].Tks;
                tksCoilBeforOutput = Coils[coilBefor].TksOutput;

                 maxTks = Math.Max(tksCoilBeforInput, tksCoilInput);
                 maxTksout = Math.Max(tksCoilBeforOutput, tksCoilOutput);
                

                TimeParameter.Timetks.Start();
                TimeParameter.TimetksOutput.Start();
                valueJumpOutput = calcuTksJumpOutput(maxTksout, TanSkpTemParameter.idEfrazMisCurr, TksJumps);
                TimeParameter.TimetksOutput.Stop();
                TimeParameter.TimetksIn.Start();
                valueJumpInput = calcuTksJumpInput(maxTks, TanSkpTemParameter.idEfrazMisCurr, TksJumps);
                TimeParameter.TimetksIn.Stop();
                TimeParameter.Timetkslimi.Start();
                limiteJumpInput = calcuLimitedJumpTks(maxTks, TanSkpTemParameter.idEfrazMisCurr, TksJumps);
                TimeParameter.Timetkslimi.Stop();
                TimeParameter.Timetks.Stop();


                chekJumpLoc = chekJumpTksIn(1);
                if (chekJumpLoc == true)
                {
                    chekJumpLoc = chekJumpTksOut(1);
                    if (chekJumpLoc == true)
                        return true;
                    else
                        return false;
                }
                else
                    return false;

                //if (Math.Round(Math.Abs(tksCoilInput - tksCoilBeforInput), 2) <= Math.Round((Math.Min(valueJumpInput * maxTks, limiteJumpInput)), 2) &&
                //Math.Round(Math.Abs(tksCoilOutput - tksCoilBeforOutput), 2) <= Math.Round((valueJumpOutput * maxTksout), 2))


                //    return true;
                //else
                //    return false;

            }
            else
            {

                 tksCoilAfterInput = Coils[coilAfter].Tks;
                 tksCoilBeforInput = Coils[coilBefor].Tks;
                 tksCoilAfterOutput = Coils[coilAfter].TksOutput;
                 tksCoilBeforOutput = Coils[coilBefor].TksOutput;
                 maxTks = Math.Max(tksCoilAfterInput, tksCoilInput);
                 maxTksout = Math.Max(tksCoilAfterOutput, tksCoilOutput);
             

                TimeParameter.Timetks.Start();
                TimeParameter.TimetksOutput.Start();
                valueJumpOutput = calcuTksJumpOutput(maxTksout, TanSkpTemParameter.idEfrazMisCurr, TksJumps);
                TimeParameter.TimetksOutput.Stop();
                TimeParameter.TimetksIn.Start();
                valueJumpInput = calcuTksJumpInput(maxTks, TanSkpTemParameter.idEfrazMisCurr, TksJumps);
                TimeParameter.TimetksIn.Stop();
                TimeParameter.Timetkslimi.Start();
                limiteJumpInput = calcuLimitedJumpTks(maxTks, TanSkpTemParameter.idEfrazMisCurr, TksJumps);
                TimeParameter.Timetkslimi.Stop();

                TimeParameter.Timetks.Stop();

                chekJumpLoc = chekJumpTksIn(-1);
                if (chekJumpLoc == true)
                {
                    chekJumpLoc = chekJumpTksOut(-1);
                    if (chekJumpLoc == true)
                    {
                    TimeParameter.Timetks.Start();
                    TimeParameter.TimetksOutput.Start();
                    valueJumpOutput = calcuTksJumpOutput(maxTksout, TanSkpTemParameter.idEfrazMisCurr, TksJumps);
                    TimeParameter.TimetksOutput.Stop();
                    TimeParameter.TimetksIn.Start();
                    valueJumpInput = calcuTksJumpInput(maxTks, TanSkpTemParameter.idEfrazMisCurr, TksJumps);
                    TimeParameter.TimetksIn.Stop();
                    TimeParameter.Timetkslimi.Start();
                    limiteJumpInput = calcuLimitedJumpTks(maxTks,TanSkpTemParameter.idEfrazMisCurr, TksJumps);
                    TimeParameter.Timetkslimi.Stop();
                    TimeParameter.Timetks.Stop();


                    chekJumpLoc = chekJumpTksIn(1);
                    if (chekJumpLoc == true)
                    {
                        chekJumpLoc = chekJumpTksOut(1);
                        if (chekJumpLoc == true)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;

                    }
                    else
                        return false;
                }
                else
                    return false;


                //if (Math.Round(Math.Abs(tksCoilInput - tksCoilAfterInput), 2) <= Math.Round((Math.Min(valueJumpInput * maxTks, limiteJumpInput)), 2) &&
                //   Math.Round(Math.Abs(tksCoilOutput - tksCoilAfterOutput), 2) <= Math.Round((valueJumpOutput * maxTksout), 2))
               // {
                    //TimeParameter.Timetks.Start();
                    //TimeParameter.TimetksOutput.Start();
                    //valueJumpOutput = calcuTksJumpOutput(maxTksout, TanSkpTemParameter.idEfrazMisCurr, TksJumps);
                    //TimeParameter.TimetksOutput.Stop();
                    //TimeParameter.TimetksIn.Start();
                    //valueJumpInput = calcuTksJumpInput(maxTks, TanSkpTemParameter.idEfrazMisCurr, TksJumps);
                    //TimeParameter.TimetksIn.Stop();
                    //TimeParameter.Timetkslimi.Start();
                    //limiteJumpInput = calcuLimitedJumpTks(maxTks,TanSkpTemParameter.idEfrazMisCurr, TksJumps);
                    //TimeParameter.Timetkslimi.Stop();
                    //TimeParameter.Timetks.Stop();

                    //if (Math.Round(Math.Abs(tksCoilInput - tksCoilBeforInput), 2) <= Math.Round(Math.Min(valueJumpInput * maxTks, limiteJumpInput), 2) &&
                    //    Math.Round(Math.Abs(tksCoilOutput - tksCoilBeforOutput), 2) <= Math.Round((valueJumpOutput * maxTksout), 2))

                    //    return true;
                    //else
                    //    return false;


                //}
                //else
                //    return false;



            }


        }

       public static bool chekJumpTksIn(int coilBefor)
       {

           #region if (coilBefor == -1)
           if (coilBefor == -1)
           {
               //Sinusoidal
               if (InnerParameter.flgSeqTksIn == (int)Parameter.EnumStatusJump.Swinging)

                   if (Math.Round(Math.Abs(tksCoilInput - tksCoilAfterInput), 2) <= Math.Round((Math.Min((valueJumpInput * maxTks), limiteJumpInput)), 2))
                  

                       return true;
                   else
                       return false;

                   // Ascending
               else if (InnerParameter.flgSeqTksIn == (int)Parameter.EnumStatusJump.Ascending)

                   if (Math.Round(Math.Abs(tksCoilInput - tksCoilAfterInput), 2) <= Math.Round((Math.Min((valueJumpInput * maxTks), limiteJumpInput)), 2)
                       && Math.Round(tksCoilInput - tksCoilAfterInput, 2)<=0)

                       return true;
                   else
                       return false;

                   // Descending
               else

                   if (Math.Round(Math.Abs(tksCoilInput - tksCoilAfterInput), 2) <= Math.Round((Math.Min((valueJumpInput * maxTks), limiteJumpInput)), 2) &&
                         Math.Round(tksCoilInput - tksCoilAfterInput, 2)>=0)

                       return true;
                   else
                       return false;
           }
           #endregion

           #region else if (coilAfter == -1)
           else
           {
               //Sinusoidal
               if (InnerParameter.flgSeqTksIn == (int)Parameter.EnumStatusJump.Swinging)

               if (Math.Round(Math.Abs(tksCoilInput - tksCoilBeforInput), 2) <= Math.Round((Math.Min(valueJumpInput * maxTks, limiteJumpInput)), 2))


                   return true;
               else
                   return false;

               // Ascending

               else if (InnerParameter.flgSeqTksIn == (int)Parameter.EnumStatusJump.Ascending)
                   if (Math.Round(Math.Abs(tksCoilBeforInput - tksCoilInput), 2) <= Math.Round((Math.Min(valueJumpInput * maxTks, limiteJumpInput)), 2) 
                       && Math.Round(tksCoilBeforInput - tksCoilInput, 2) <=0)


                   return true;
               else
                   return false;

               // Descending
               
              else 
                   if (Math.Round(Math.Abs(tksCoilBeforInput - tksCoilInput), 2) <= Math.Round((Math.Min(valueJumpInput * maxTks, limiteJumpInput)), 2) 
                       && Math.Round(tksCoilBeforInput - tksCoilInput, 2)>=0) 
       

                   return true;
               else
                   return false;

           }

           #endregion


       }

       public static bool chekJumpTksOut(int coilBefor)
       {

           #region if (coilBefor == -1)
           if (coilBefor == -1)
           {
               //Sinusoidal
               if (InnerParameter.flgSeqTksOut == (int)Parameter.EnumStatusJump.Swinging)

                   if (Math.Round(Math.Abs(tksCoilOutput - tksCoilAfterOutput), 2) <= Math.Round((valueJumpOutput * maxTksout), 2))

                       return true;
                   else
                       return false;


                          // Ascending
               else if (InnerParameter.flgSeqTksOut == (int)Parameter.EnumStatusJump.Ascending)

                   if (Math.Round(Math.Abs(tksCoilOutput - tksCoilAfterOutput), 2) <= Math.Round((valueJumpOutput * maxTksout), 2)
                       && Math.Round(tksCoilOutput - tksCoilAfterOutput, 2) <= 0)

                       return true;
                   else
                       return false;

               else
                   if (Math.Round(Math.Abs(tksCoilOutput - tksCoilAfterOutput), 2) <= Math.Round((valueJumpOutput * maxTksout), 2)
                       && Math.Round(tksCoilOutput - tksCoilAfterOutput, 2) >= 0)

                       return true;
                   else
                       return false;
           }

           #endregion


           #region else if (coilAfter == -1)
           else
           {


               //Sinusoidal
               if (InnerParameter.flgSeqTksOut == (int)Parameter.EnumStatusJump.Swinging)

                   if (Math.Round(Math.Abs(tksCoilBeforOutput - tksCoilOutput), 2) <= Math.Round((valueJumpOutput * maxTksout), 2))


                       return true;
                   else
                       return false;

               //Ascending
               else if (InnerParameter.flgSeqTksOut == (int)Parameter.EnumStatusJump.Ascending)
                   if (Math.Round(Math.Abs(tksCoilBeforOutput - tksCoilOutput), 2) <= Math.Round((valueJumpOutput * maxTksout), 2)
                       && Math.Round(tksCoilBeforOutput - tksCoilOutput, 2) <= 0)


                       return true;
                   else
                       return false;


                      //Descending
               else

                   if (Math.Round(Math.Abs(tksCoilBeforOutput - tksCoilOutput), 2) <= Math.Round((valueJumpOutput * maxTksout), 2)
                         && Math.Round(tksCoilBeforOutput - tksCoilOutput, 2) >= 0)


                       return true;
                   else
                       return false;


           }
           #endregion
       }

       public static double calcuTksJumpOutput(double maxTksout, int idEfrazMis, List<TksJump> TksJumps)
        {

            var queryout = from tks in TksJumps
                           where tks.TksFrom <= maxTksout
                                                        && tks.TksTo >= maxTksout
                                                        && tks.FlgInputOutput == 1
                                                        && tks.CodMis == idEfrazMis
                           // && a.misProg == misProgLocal

                           select new
                           {

                               percentJumpOutput = tks.PercentJumpOutput,
                               valueJumpOutput = tks.ValueJumpOutput

                           };
            if (queryout.Count() <= 0)
            {
                var queryout2 = from tks in TksJumps
                                where tks.TksFrom <= maxTksout
                                                             && tks.TksTo >= maxTksout
                                                             && tks.FlgInputOutput == 1
                                                             && tks.CodMis == Parameter.CodMisNull
                                // && a.mis
                                select new
                                {

                                    percentJumpOutput = tks.PercentJumpOutput,
                                    valueJumpOutput = tks.ValueJumpOutput

                                };
                if (queryout2.Count() <= 0)
                {
                    return DataBase.MaxJumpTks;
                }
                else
                    if (queryout2.FirstOrDefault().percentJumpOutput != -1)

                        return queryout2.FirstOrDefault().percentJumpOutput;
                    else
                        return
                            queryout2.FirstOrDefault().valueJumpOutput;
            }
            else

                if (queryout.FirstOrDefault().percentJumpOutput != -1)

                    return queryout.FirstOrDefault().percentJumpOutput;
                else

                    return queryout.FirstOrDefault().valueJumpOutput;


        }

       public static bool chekOneTwo(int coilSelection, int coilBefor, int coilAfter, List<Coil> Coils, List<TksJump> TksJumps)
        {
            bool chekWid = true;
            chekWid = chekJumpWid(coilSelection, coilBefor, coilAfter,Coils);
            if (chekWid == true)
            {
                bool chekTks = true;
                chekTks = chekJumpTks(coilSelection, coilBefor, coilAfter, Coils, TksJumps);
                if (chekTks == true)

                    return true;
                else
                    return false;
            }
            else
                return false;


        }
     
       public static double calcuLimitedJumpTks(double maxTks, int idEfrazMis, List<TksJump> TksJumps)
        {

            TimeParameter.TimequeryTks.Start();

            var query = from tks in TksJumps
                        where tks.TksFrom <= maxTks
                              && tks.TksTo >= maxTks
                              && tks.FlgInputOutput == 0
                              && tks.CodMis == idEfrazMis
                        // && tks.misProg == misProgLocal


                        select new
                        {
                            limitedJumpInput = tks.LimitedJumpInput


                        };
            TimeParameter.TimequeryTks.Stop();


            // whereTks.Start();

            //List< TksJump> eee = lstTksJump.Where(a => a.tksFrom <= maxTks && a.tksTo >= maxTks && a.flgInputOutput == 0 && a.codMis == idEfrazMis).ToList();

            // whereTks.Stop();



            int oo = query.Count();
            if (query.Count() <= 0)
            {
                var query2 = from tks in TksJumps
                             where tks.TksFrom <= maxTks
                                && tks.TksTo >= maxTks
                                && tks.FlgInputOutput == 0
                                && tks.CodMis == Parameter.CodMisNull
                             // && tks.misProg == misProgLocal


                             select new
                             {
                                 limitedJumpInput = tks.LimitedJumpInput


                             };
                if (query2.Count() <= 0)
                {
                    return DataBase.MaxJumpTks;
                }
                else
                    return query2.FirstOrDefault().limitedJumpInput;
            }
            else

                return query.FirstOrDefault().limitedJumpInput;
        }
       
       public static double calcuTksJumpInput(double maxTks, int idEfrazMis, List<TksJump> TksJumps)
        {
            var query = from tks in TksJumps
                        where tks.TksFrom <= maxTks
                                                    && tks.TksTo >= maxTks
                                                    && tks.FlgInputOutput == 0
                                                     && tks.CodMis == idEfrazMis
                        // && tks.misProg == misProgLocal


                        select new
                        {
                            valueJumpInput = tks.ValueJumpInput,
                            percentJumpInput = tks.PercentJumpInput,

                        };

            int oo = query.Count();
            if (query.Count() <= 0)
            {
                var query2 = from tks in TksJumps
                             where tks.TksFrom <= maxTks
                                                         && tks.TksTo >= maxTks
                                                         && tks.FlgInputOutput == 0
                                                          && tks.CodMis == Parameter.CodMisNull
                             // && a.misProg == misProgLocal


                             select new
                             {

                                 valueJumpInput = tks.ValueJumpInput,
                                 percentJumpInput = tks.PercentJumpInput,

                             };
                if (query2.Count() <= 0)
                {
                    return DataBase.MaxJumpTks;
                }
                else
                {
                    if (query2.FirstOrDefault().valueJumpInput != -1)
                        return query2.FirstOrDefault().valueJumpInput;
                    else
                        return query2.FirstOrDefault().percentJumpInput;
                }
            }
            else

                if (query.FirstOrDefault().valueJumpInput != -1)
                    return query.FirstOrDefault().valueJumpInput;
                else

                    return query.FirstOrDefault().percentJumpInput;


        }

       public static void selectSameOrder(int modelIndexCoil, int idEfrazLocal, int idSarfaslLocal, DateTime datePlanLoc, int maxCount,CommonLists Lst)
       {
           List<Coil> localCoillist = new List<Coil>();
           localCoillist = Lst.CoilsCapDay.Where(c2 => c2.IdOrder == Lst.Coils[modelIndexCoil].IdOrder
                                                    && c2.Width == Lst.Coils[modelIndexCoil].Width
                                                    && c2.ModelIndexCoil != modelIndexCoil
                                                    && c2.TksOutput == Lst.Coils[modelIndexCoil].TksOutput
                                                    && c2.Tks == Lst.Coils[modelIndexCoil].Tks

               //  && lstCoilRankDelete.Contains(c2) == false
                               ).ToList();


           while (Lst.currSolution.LstSeqCoil.Count < maxCount && localCoillist.Count != 0 &&
               //lenTotal <= lenOpt
               InnerParameter.weiTotal <= InnerParameter.weiOpt)
           {
                 int index = Lst.currSolution.LstSeqCoil.FindIndex(c => c == modelIndexCoil);
                 InnerParameter.chekCap = CapPlanFunc.chekMaxCapPlan(localCoillist[0].ModelIndexCoil, Lst.CapPlansCurr, Lst.Coils);//برنامه ظرفیت برای کلاف چک می شود);

               if (InnerParameter.chekCap == 1)
               {


                   Lst.currSolution.LstSeqCoil.Insert(index + 1, localCoillist[0].ModelIndexCoil);


                   modelIndexCoil = localCoillist[0].ModelIndexCoil;

                   SequenceL2.updateAfterInsertCoil(localCoillist[0].ModelIndexCoil,Lst);



                   localCoillist.RemoveAt(0);
               }
               else
               {
                   Lst.CoilsCapDay.Remove(localCoillist[0]);
                   break;
               }



           }


       }

       public static void updateAfterInsertCoil(int select, CommonLists Lst)
       {


           SequenceFunc.generalInsertAfterCoil(Lst.currSolution, select, Lst.Coils, Lst.MaxValueGroups,
           Lst.lstAvailMaxValueGroup, Lst.CapPlansCurr, Lst.CoilsMain, Lst.CoilsTemDelete,
             Lst.CoilsAvailProg, Lst.CoilsCapDay);

       }


     
    }
}
