using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPSO.CMP.CommonFunctions.ParameterClasses
{
    // in class felan baraye pic ast
    public class GroupDef// insert  min tks in first index list 
    {
        public int IndexGroup { get; set; }
        public int IdGroup { get; set; }//DB  
        public int PriorityGroup { get; set; }//DB
        public int WidFrom { get; set; }//DB
        public int WidTo { get; set; }//DB
        public double TksFrom { get; set; }//DB
        public double TksTo { get; set; }//DB
        public double TksOutTo { get; set; }//DB
        public double TksOutFrom { get; set; }//DB
        public List<string> LstgroupMisProg = new List<string>();
        public List<int> LstCoilGroup = new List<int>();//modelindexCoil

        //Tem
        public int CodSurfaceRoughness;

        //Galvan
        public int ProductFamily;

        public virtual void assignGroupDef(List<GroupDef> GroupDefs, List<Coil> Coils)
        {
            foreach (var gr in GroupDefs)
            {
                List<Coil> lstLocCoil = new List<Coil>();
                lstLocCoil = Coils.Where(a => a.Width <= gr.WidTo && a.Width >= gr.WidFrom
                                                        && a.Tks <= gr.TksTo && a.Tks >= gr.TksFrom
                                                        && a.TksOutput <= gr.TksOutTo && a.TksOutput >= gr.TksOutFrom
                                                        && gr.LstgroupMisProg.Contains(a.MisProg.ToString())
                                                        ).ToList();

                foreach (var j in lstLocCoil)
                {
                    j.LstGroupDef.Add(gr.IdGroup);

                    gr.LstCoilGroup.Add(j.ModelIndexCoil);
                }
            }
        }

       
    }

}
