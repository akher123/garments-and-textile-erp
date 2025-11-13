using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.Model;
using SCERP.Model.CommonModel;

namespace SCERP.DAL.Repository.CommonRepository
{
    public class CustomSqlQuaryRepository :Repository<CustomSqlQuary>, ICustomSqlQuaryRepository
    {

        public CustomSqlQuaryRepository(SCERPDBContext context) : base(context)
        {
        }


        public List<VUserReport> GetUserReport(string userName)
        {
            return Context.VUserReports.Where(x => x.IsEnable && x.UserName == userName).ToList();
        }
    }
}
