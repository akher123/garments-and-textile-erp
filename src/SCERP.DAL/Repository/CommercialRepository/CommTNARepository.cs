using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Model.CommercialModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.DAL.Repository.CommercialRepository
{
    public class CommTNARepository : Repository<CommTNA>, ICommTNARepository
    {

        private readonly string _companyId;
        public CommTNARepository(SCERPDBContext context) : base(context)
        {
            this._companyId = PortalContext.CurrentUser.CompId;
        }

        public int UpdateTna(string compId, int commTnaRowId, string key, string value)
        {
            string sqlQuery = String.Format("update CommTNA SET {0}='{1}'  where CommTnaRowId='{2}' and CompId='{3}'", key, value, commTnaRowId, compId);
            return Context.Database.ExecuteSqlCommand(sqlQuery);
        }
    }
}
