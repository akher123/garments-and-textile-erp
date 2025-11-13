using SCERP.Common;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Model.CommercialModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.DAL.Repository.CommercialRepository
{
    public class CashBbLcRepository : Repository<CommCashBbLcInfo>, ICashBbLcRepository
    {
        private readonly string _companyId;
        public CashBbLcRepository(SCERPDBContext context) : base(context)
        {
            this._companyId = PortalContext.CurrentUser.CompId;
        }

        public List<CommCashBbLcInfo> GetAllCashBbLcInfos()
        {
            throw new NotImplementedException();
        }

        public List<CommCashBbLcInfo> GetAllCashBbLcInfosByPaging(int startPage, int pageSize, out int totalRecords, CommCashBbLcInfo cashBbLcInfo)
        {
            throw new NotImplementedException();
        }

        public CommCashBbLcInfo GetCashBbLcIdByBbLcNo(string bbLcNo)
        {
            throw new NotImplementedException();
        }

        public CommCashBbLcInfo GetCashBbLcInfoById(int? id)
        {
            return Context.CommCashBbLcInfos.FirstOrDefault(x => x.BbLcId == id);
            
        }

        public List<CommCashBbLcInfo> GetCashBbLcInfoBySearchKey(int searchByCountry, string searchByCashBbLcInfo)
        {
            throw new NotImplementedException();
        }
    }
}
