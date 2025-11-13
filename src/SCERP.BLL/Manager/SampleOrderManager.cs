using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;
using SCERP.Model.UserRightManagementModel;

namespace SCERP.BLL.Manager
{
   public class SampleOrderManager:ISampleOrderManager
    {
        private readonly ISampleOrderRepository _sampleOrderRepository;
        private readonly IRepository<UserMerchandiser> _userMerchandiserRepository;
        private readonly IRepository<OM_Merchandiser> _merchandiserRepository;
        public SampleOrderManager(ISampleOrderRepository sampleOrderRepository, IRepository<UserMerchandiser> userMerchandiserRepository, IRepository<OM_Merchandiser> merchandiserRepository)
        {
            _sampleOrderRepository = sampleOrderRepository;
            _userMerchandiserRepository = userMerchandiserRepository;
            _merchandiserRepository = merchandiserRepository;
        }


       public List<OM_SampleOrder> GetSampleOrder(int pageIndex, string sort, string sortdir, string searchString, out int totalRecords)
        {
           

            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            Guid? userId = PortalContext.CurrentUser.UserId;
            List<string> merList = _userMerchandiserRepository.Filter(x => x.EmployeeId == userId).Select(x => x.MerchandiserRefId).ToList();
         //   List<int> merchandiserIds=   _merchandiserRepository.Filter(x => merList.Contains(x.EmpId)).Select(x => x.MerchandiserId).ToList();
            var sampleOrders = _sampleOrderRepository.GetWithInclude(x => x.OrderNo.Contains(searchString) || String.IsNullOrEmpty(searchString) || x.ArticleNo.Contains(searchString) || String.IsNullOrEmpty(searchString), "OM_Buyer", "OM_Merchandiser")
                .Where(x => merList.Contains(x.OM_Merchandiser.EmpId));
            totalRecords = sampleOrders.Count();
            switch (sort)
            {
                case "OrderNo":
                    switch (sortdir)
                    {
                        case "DESC":
                            sampleOrders = sampleOrders
                                .OrderByDescending(r => r.OrderNo)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            sampleOrders = sampleOrders
                                .OrderBy(r => r.RefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    sampleOrders = sampleOrders
                        .OrderByDescending(r => r.RefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return sampleOrders.ToList();
        }

        public List<OM_SampleOrder> GetAllSampleOrder()
        {
            return _sampleOrderRepository.All().OrderBy(x => x.OrderDate).ToList();
        }

        public OM_SampleOrder GetSampleOrderById(int sampleOrderId)
        {
            return _sampleOrderRepository.GetWithInclude(x => x.SampleOrderId == sampleOrderId,"OM_Buyer").First();
        }

        public int EditSampleOrder(OM_SampleOrder model)
        {
             OM_SampleOrder sampleOrder=  _sampleOrderRepository.FindOne(x => x.SampleOrderId == model.SampleOrderId);
            sampleOrder.EditedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
            sampleOrder.EditedDate = DateTime.Now;
            sampleOrder.BuyerId = model.BuyerId;
            sampleOrder.MerchandiserId = model.MerchandiserId;
            sampleOrder.OrderDate = model.OrderDate;
            sampleOrder.OrderNo = model.OrderNo;
            sampleOrder.OrderQty = model.OrderQty;
            sampleOrder.ArticleNo = model.ArticleNo;
            sampleOrder.Agent = model.Agent;
            sampleOrder.Remarks = model.Remarks;
            sampleOrder.Season = model.Season;
            return _sampleOrderRepository.Edit(sampleOrder);
        }

        public int SaveSampleOrder(OM_SampleOrder sampleOrder)
        {
            sampleOrder.CompId = PortalContext.CurrentUser.CompId;
            sampleOrder.CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
            sampleOrder.CreatedDate = DateTime.Now;
            sampleOrder.IsApproved = false;
            sampleOrder.RefId = GetNewRefId(sampleOrder.CompId);
            return _sampleOrderRepository.Save(sampleOrder);
        }

        public int DeleteSampleOrder(OM_SampleOrder sampleOrder)
        {
            return _sampleOrderRepository.DeleteOne(sampleOrder);
        }

       public string GetNewRefId(string compId)
       {
           var maxRefId = _sampleOrderRepository.Filter(x=>x.CompId==compId).Max(x => x.RefId);
           return maxRefId.IncrementOne().PadZero(6);
       }

        public OM_SampleOrder GetSampleOrderByRefId(string searchString)
        {
            return _sampleOrderRepository.GetWithInclude(x => x.RefId == searchString, "OM_Buyer").First();
        }
    }
}
