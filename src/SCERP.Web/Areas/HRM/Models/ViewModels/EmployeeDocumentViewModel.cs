using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class EmployeeDocumentViewModel:EmployeeDocument
    {
        public List<EmployeeDocument> EmployeeDocuments { get; set; }
    }
}