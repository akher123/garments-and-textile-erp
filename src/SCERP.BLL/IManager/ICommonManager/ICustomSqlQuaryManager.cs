using System.Collections.Generic;
using System.Data;
using SCERP.Common;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.IManager.ICommonManager
{
   public interface ICustomSqlQuaryManager
   {
       DataTable GetSqlQueryResult(string sqlQuery);
       int SaveCustomSqlQuary(CustomSqlQuary sqlQuary);
       List<CustomSqlQuary> GetCustomSqlQueres();
       CustomSqlQuary GetCustomSqlQuary(int customSqlQuaryId);
       int EditustomSqlQuary(CustomSqlQuary model);
       List<string> GetAllTableNames();
       string GetNewSqlQuaryRefId();

       List<VUserReport> GetUserReport(string userName);


       List<SqlReportParameter> GetSqlReportParametersByReportId(int customSqlQuaryId);
       List<Dropdown> GetDropDownListbySqlQuery(string querystring);
   }
}
