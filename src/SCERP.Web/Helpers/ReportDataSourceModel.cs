using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;


namespace SCERP.Web.Helpers
{
    public class ReportDataSourceModel
    {
        public ReportDataSourceModel()
        {
            DataSource = new object();
          
        }
        /// <summary>
        /// Content allo data source 
        /// </summary>
        public object DataSource { get; set; }
        /// <summary>
        /// RDLC Report path , EXP :~/Areas/Merchandising/Report/RDLC/BuyerOrderList.rdlc
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// RDLC Report data Set name, EXP:BuyerOrderDS
        /// </summary>
        public string DataSetName { get; set; }

        public ReportParameter[] ReportParameters { get; set; }
      
    }
}