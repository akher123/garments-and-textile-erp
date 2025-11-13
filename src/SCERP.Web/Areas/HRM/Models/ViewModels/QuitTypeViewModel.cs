using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class QuitTypeViewModel : QuitType
    {
        public List<QuitType> QuitTypes { get; set; }
        public string SearchByQuitType { get; set; }
        [Remote("CheckQuitTypeExist", "QuitType", AdditionalFields ="QuitTypeId", ErrorMessage = @"Exist this type")]
        public override string Type { get; set; }

        public QuitTypeViewModel()
        {
            QuitTypes=new List<QuitType>();
        }
    }
}