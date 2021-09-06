using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPSO.CMP.CommonFunctions.ParameterClasses
{
   /// <summary>
    /// tan = insert  max wid in first index list 
    /// Pic = insert  max tks in first index list 
   /// </summary>
 
    public class Sarfasl
    {
        public int IndexSarfasl;
        public int IdSarfasl { get; set; }//DB  کد سر فصل
        public List<int> LstCoilSarfasl = new List<int>();//modelindexCoil

        public List<int> LstGroupSarfasl = new List<int>();
     

        public Sarfasl()
        { }

        public  static void assignSarfaslGroup(List<Sarfasl> Sarfasls, List<Coil> Coils, List<GroupDef> GroupDefs)
        {
            foreach (var i in Sarfasls)
            {
                List<GroupDef> lstGroupLoc = GroupDefs.Where(a => i.LstGroupSarfasl.Contains(a.IdGroup)).ToList();

                foreach (var gr in lstGroupLoc)
                {
                    i.LstCoilSarfasl.AddRange(gr.LstCoilGroup);
                }

                i.LstCoilSarfasl = i.LstCoilSarfasl.Distinct().ToList();

                foreach (int item in i.LstCoilSarfasl)
                {
                    Coils[item].LstSarfaslGroup.Add(i.IndexSarfasl);
                }
            }
        }
    }

        
}
