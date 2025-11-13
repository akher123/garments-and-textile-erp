using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class CuttFabRejectManager : ICuttFabRejectManager
    {
        private readonly ICuttFabRejectRepository _cuttingFabricRejectRepository;
        public CuttFabRejectManager(ICuttFabRejectRepository cuttingFabricRejectRepository)
        {
            _cuttingFabricRejectRepository = cuttingFabricRejectRepository;
        }
        public List<VwCuttFabReject> GetCuttFabRejectsByPaging(string searchString, int pageIndex, out int totalRecord)
        {
            string compId = PortalContext.CurrentUser.CompId;
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            IQueryable<VwCuttFabReject> cuttFabricRejects =
                _cuttingFabricRejectRepository.GetCuttFabRejects(x => x.CompId == compId &&
                                                                      ((x.BatchNo.Contains(searchString) ||
                                                                        string.IsNullOrEmpty(searchString)) ||
                                                                       (x.StyleName.Contains(searchString) ||
                                                                        string.IsNullOrEmpty(searchString))));
            totalRecord = cuttFabricRejects.Count();
            cuttFabricRejects = cuttFabricRejects.OrderByDescending(
                    x => x.EntryDate)
                .Skip(index * pageSize)
                .Take(pageSize);
            return cuttFabricRejects.ToList();
        }

        public PROD_CuttFabReject GetCuttFabRejectById(int cuttFabRejectId)
        {
            return _cuttingFabricRejectRepository.FindOne(x => x.CuttFabRejectId == cuttFabRejectId);
        }

        public int SaveFabReject(PROD_CuttFabReject cuttFabReject)
        {
            return _cuttingFabricRejectRepository.Save(cuttFabReject);
        }
        public VwCuttFabReject GetVwCuttFabRejectById(int cuttFabRejectId)
        {
            return _cuttingFabricRejectRepository.GetCuttFabRejects(x => x.CuttFabRejectId == cuttFabRejectId)
                .FirstOrDefault();
        }

        public int DeleteCuttFabReject(int cuttFabRejectId)
        {
          return  _cuttingFabricRejectRepository.Delete(x => x.CuttFabRejectId == cuttFabRejectId);
        }
    }
}
