using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.Production;

namespace SCERP.Web.Areas.Production.Models
{
    public class GroupSubProcessModel : ProSearchModel<GroupSubProcessModel>
    {
        public List<PROD_GroupSubProcess> GroupSubProcesses { get; set; }
        public PROD_GroupSubProcess GroupSubProcess { get; set; }
        public GroupSubProcessModel()
        {
            GroupSubProcess=new PROD_GroupSubProcess();
            GroupSubProcesses=new List<PROD_GroupSubProcess>();
        }

    }
}