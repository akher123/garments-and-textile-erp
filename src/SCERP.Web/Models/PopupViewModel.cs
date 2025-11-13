using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SCERP.Web.Models
{
    public class PopupViewModel
    {
        public int Id { get; set; }
  
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string PopupViewModalId { get; set; }

    }
}