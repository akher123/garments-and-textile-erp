using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model.CommonModel;

namespace SCERP.DAL.IRepository.ICommonRepository
{
   public interface ISqlReportParameterRepository:IRepository<SqlReportParameter>
    {
       List<Dropdown> GetDropDownListbySqlQuery(string querystring);
    }
}
