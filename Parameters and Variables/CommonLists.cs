using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPSO.CMP.Log;

namespace IPSO.CMP.CommonFunctions.ParameterClasses
{
    public class CommonLists
    {

        //****************************Logger***************************
        //*********
        //این آدرس بعدا باید برای همه خطوط پر شود یا در هر خط  یا در همین جا
        public FileLogger fileLogger = new FileLogger();
        public FileLogger fileLoggerRoll = new FileLogger();
        public FileLogger fileLoggerBeforAlgorithm = new FileLogger();
        public FileLogger FileLoggerSelectCoil = new FileLogger();
        



        //****************************object***************************
        public Solution bestSolution = new Solution();
        public Solution currSolution = new Solution();
        public Solution bigSolution = new Solution();

        //**************list of class***************
        public List<Coil> Coils = new List<Coil>();
        public List<CoilRelease> CoilReleases = new List<CoilRelease>();
        public List<CoilRelease> CoilReleasesOtherSt = new List<CoilRelease>();

        public List<CapPlan> CapPlans = new List<CapPlan>();
        public List<CapPlan> CapPlansCurr = new List<CapPlan>();
        public List<CapPlanUpDate> CapPlanUpDates = new List<CapPlanUpDate>();
        public List<DBDCampPlan> DBDCampPlans = new List<DBDCampPlan>();

        public List<EquipGroupFailureTime> EquipGroupFailureTimes = new List<EquipGroupFailureTime>();

        public List<GroupDef> GroupDefs = new List<GroupDef>();
        public List<GroupDef> GroupDefsZone = new List<GroupDef>();

        public List<JumpBetweenCrown> JumpBetweenCrowns = new List<JumpBetweenCrown>();

        public List<MaxValueGroup> MaxValueGroups = new List<MaxValueGroup>();

        public List<ProgEfraz> ProgEfrazes = new List<ProgEfraz>();

        public List<Roll> RollsWork = new List<Roll>();

        public List<Roll> RollsBack = new List<Roll>();

        public List<ReleaseSched> ReleaseScheds = new List<ReleaseSched>();

        public List<Sarfasl> Sarfasls = new List<Sarfasl>();

        public List<Scheduling> Schedulings = new List<Scheduling>();

        public List<Scheduling> SchedulingsOtherSt = new List<Scheduling>();

        public List<Setup> Setups = new List<Setup>();

        public List<ShiftWork> ShiftWorks = new List<ShiftWork>();

        public List<Solution> SolutionsOutputPlan = new List<Solution>();

        public List<StationStop> StationStops = new List<StationStop>();

        public List<TksJump> TksJumps = new List<TksJump>();

        public List<WidthJump> WidthJumps = new List<WidthJump>();
        
        public List<ChangeRoll> ChangeRolls = new List<ChangeRoll>();

        public List<Zone> Zones = new List<Zone>();

        public List<Priority> Prioritys = new List<Priority>();


        //vaez
        //ذخیره جواب های ساخته شده به ازای هر جواب فیکس شده
        public List<Solution> SolutionsLocal= new List<Solution>();

       
           

        //public List<Zone> Zones = new List<Zone>();

        //****************************   List of other station    **********************
        //لیست توقفات خط دیگر
        public List<StationStop> StationStopsOtherSt = new List<StationStop>();

        //لیست برنامه های خط دیگر
        public List<ReleaseSched> ReleaseSchedsOtherStation = new List<ReleaseSched>();
        //******************************************************************************
        // مواقع مورد نیاز نیو شود که نیاز به  خالی کردن لیست ها نباشد
        public List<Coil> CoilsAvailProg = new List<Coil>();
        public List<Coil> CoilsMain = new List<Coil>(); 
        public List<Coil> CoilsMainCopy = new List<Coil>(); 
        public List<Coil> CoilsTemDelete = new List<Coil>();
        public List<Coil> CoilsCapDay = new List<Coil>();
        public List<Coil> CoilsRestart = new List<Coil>();// حذف کلاف ها که برنامه با مینیمم تعداد کلاف تولید نکرده اند

        public List<int> lstAvailEquipGroupFailureTime = new List<int>();
        public List<int> lstAvailMaxValueGroup = new List<int>();

        public List<int> lstAvailProg = new List<int>();//idAfraz OF class ProgAFraz
        public List<int> lstAvailSarfasl = new List<int>();
        public List<int> lstReadAllGroupDef = new List<int>();

        public List<int> lstNotAvailLenForCoil = new List<int>();
        public List<int> lstNotAvailLenForCoilRelease = new List<int>();
        public List<int> lstNotAvailLenForCoilReleaseOtherSt = new List<int>();

        public List<Coil> lstFailedCoilForPlan = new List<Coil>();
        // خطوطی که داری غلتک پشتیبان هستند

        public static List<int> lstStationBackRoll = new List<int>();
        // خطوطی که دارای غلتک کاری هستند
        public static List<int> lstStationWorkRoll = new List<int>();


        //******************************************************************************
    

    }
}
