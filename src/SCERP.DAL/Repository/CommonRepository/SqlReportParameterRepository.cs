using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.Model.CommonModel;

namespace SCERP.DAL.Repository.CommonRepository
{
    public class SqlReportParameterRepository :Repository<SqlReportParameter>, ISqlReportParameterRepository
    {
        public SqlReportParameterRepository(SCERPDBContext context) : base(context)
        {
        }

        public List<Dropdown> GetDropDownListbySqlQuery(string querystring)
        {
           //return Context.Database.SqlQuery<Dropdown>(querystring).ToList();
            return ExecuteQuery(querystring).ToList<Dropdown>();
        }
    }
}
