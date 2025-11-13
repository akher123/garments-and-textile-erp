using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.Repository.MerchandisingRepository;
using SCERP.Model.Custom;
using SCERP.DAL;


namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class MrcReportManager : BaseManager, IMrcReportManager
    {
        protected readonly IMrcReportRepository _mrcReportRepository = null;

        public MrcReportManager(IMrcReportRepository mrcReportRepository)
        {
            _mrcReportRepository = mrcReportRepository;
        }

        public List<SpecSheetModel> GetSpecSheetDetail(int id)
        {
            return _mrcReportRepository.GetSpecSheetDetail(id);
        }

        public List<SpecSheetModel> GetSpecSheetList(int? buyerId, string styleNo, string jobNo, DateTime? fromDate, DateTime? toDate)
        {
            return _mrcReportRepository.GetSpecSheetList(buyerId, styleNo, jobNo, fromDate, toDate);
        }
    }
}
