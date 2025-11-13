using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.CommonModel;

namespace SCERP.Web.Areas.Common.Models.ViewModel
{
    public class PartyViewModel:Party
    {
        public List<Party> Parties { get; set; }
        public PartyViewModel()
        {
            Parties=new List<Party>();
        }
    }
}