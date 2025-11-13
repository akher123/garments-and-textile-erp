using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Model.CommercialModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.DAL.Repository.CommercialRepository
{
    public class LCBBLCInfoDataRepository : Repository<CommLCBBLCInfoData>, ILCBBLCInfoDataRepository
    {
        public LCBBLCInfoDataRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public int UpdateTna(string compId, int tnaRowId, string key, string value)
        {
            string sqlQuery = String.Format("update CommLCBBLCInfoData SET {0}='{1}' , EditedDate='{4}' , EditedBy='{5}' where TnaRowId='{2}' and CompId='{3}'", key, value, tnaRowId, compId, DateTime.Now, PortalContext.CurrentUser.UserId);
            return Context.Database.ExecuteSqlCommand(sqlQuery);
        }
    }
}
