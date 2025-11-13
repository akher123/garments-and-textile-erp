using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.DAL.Repository.CommonRepository;
using SCERP.Model;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.Manager.CommonManager
{
    public class CustomSqlQuaryManager : ICustomSqlQuaryManager
    {
        private readonly ICustomSqlQuaryRepository _customSqlQuaryRepository;
        private ISqlReportParameterRepository _reportParameterRepository;
        public CustomSqlQuaryManager(SCERPDBContext context)
        {
            _customSqlQuaryRepository=new CustomSqlQuaryRepository(context);
            _reportParameterRepository=new SqlReportParameterRepository(context);
        }

        public DataTable GetSqlQueryResult(string sqlQuery)
        {
            return _customSqlQuaryRepository.ExecuteQuery(sqlQuery);
          
        }

        public int SaveCustomSqlQuary(CustomSqlQuary sqlQuary)
        {
            sqlQuary.CreatedBy = PortalContext.CurrentUser.UserId;
            sqlQuary.CreatedDate = DateTime.Now;
            sqlQuary.IsActive = true;
            return _customSqlQuaryRepository.Save(sqlQuary);
        }

        public List<CustomSqlQuary> GetCustomSqlQueres()
        {
          return  _customSqlQuaryRepository.Filter(x => x.IsActive).OrderByDescending(x => x.CustomSqlQuaryId).ToList();
        }

        public CustomSqlQuary GetCustomSqlQuary(int customSqlQuaryId)
        {

            var coustomSqlReport= _customSqlQuaryRepository.FindOne(x => x.CustomSqlQuaryId == customSqlQuaryId);
            coustomSqlReport.SqlReportParameter =
                _reportParameterRepository.Filter(x => x.CustomSqlQuaryId == customSqlQuaryId).ToList();
            return coustomSqlReport;
        }

        public int EditustomSqlQuary(CustomSqlQuary model)
        {
            int effectedRows = 0;
            using (var transaction=new TransactionScope())
            {
                var customSqlQuary = _customSqlQuaryRepository.FindOne(x => x.CustomSqlQuaryId == model.CustomSqlQuaryId);
                customSqlQuary.SqlQuary = model.SqlQuary;
                customSqlQuary.EditedBy = PortalContext.CurrentUser.UserId;
                customSqlQuary.EditedDate = DateTime.Now;
                customSqlQuary.SqlQuaryRefId = model.SqlQuaryRefId;
                customSqlQuary.Name = model.Name;
                customSqlQuary.Description = model.Description;
                _customSqlQuaryRepository.Edit(customSqlQuary);
                effectedRows+=_reportParameterRepository.Delete(x => x.CustomSqlQuaryId == model.CustomSqlQuaryId);
                effectedRows+= _reportParameterRepository.SaveList(model.SqlReportParameter.ToList());
                transaction.Complete();
            }


            return effectedRows;

        }

        public List<string> GetAllTableNames()
        {
          var datatable=  _customSqlQuaryRepository.ExecuteQuery("SELECT Name FROM sys.tables ORDER BY Name").Select();

          var tableNames = datatable.AsEnumerable()
                           .Select(r => r.Field<string>("Name"))
                           .ToList();
            return tableNames;
        }

        public string GetNewSqlQuaryRefId()
        {
            var refId = _customSqlQuaryRepository.Filter(x => x.IsActive).Max(x => x.SqlQuaryRefId.Substring(3,8))??"0";
            var maxNumericRefId = Convert.ToInt32(refId);
            var sqlRefId = "SQL" + GetRefNumber(maxNumericRefId, 5);
            return sqlRefId;
        }

        public List<VUserReport> GetUserReport(string userName)
        {
            return _customSqlQuaryRepository.GetUserReport(userName);
        }

        public List<SqlReportParameter> GetSqlReportParametersByReportId(int customSqlQuaryId)
        {
            return _reportParameterRepository.Filter(x => x.CustomSqlQuaryId == customSqlQuaryId).ToList();
        }

        public List<Dropdown> GetDropDownListbySqlQuery(string querystring)
        {
           return _reportParameterRepository.GetDropDownListbySqlQuery(querystring);
        }

        public string GetRefNumber(int refId, int length)
        {
            var refNumber = Convert.ToString(refId+1);
            while (refNumber.Length != length)
            {
                refNumber = "0"+refNumber;
            }
            return refNumber;
        }
    }
}
