using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.Model;
using SCERP.Model.UserRightManagementModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class EmbWorkOrderManager : IEmbWorkOrderManager
    {
        private readonly IRepository<OM_EmbWorkOrder> _embWorkOrderRepository;
        private readonly IRepository<OM_EmbWorkOrderDetail> _embWorkOrderDetailRepository;
        private readonly IRepository<UserMerchandiser> _userMerchandiserRepository;
        public EmbWorkOrderManager(IRepository<OM_EmbWorkOrder> embWorkOrderRepository, IRepository<OM_EmbWorkOrderDetail> embWorkOrderDetailRepository, IRepository<UserMerchandiser> userMerchandiserRepository)
        {
            _embWorkOrderRepository = embWorkOrderRepository;
            _embWorkOrderDetailRepository = embWorkOrderDetailRepository;
            _userMerchandiserRepository = userMerchandiserRepository;
        }

        public List<OM_EmbWorkOrder> GetEmbWorkOrders(int pageIndex, string sort, string sortdir, out int totalRecords, string searchString, long partyId)
        {
            Guid? userId = PortalContext.CurrentUser.UserId;
            List<string> merList = _userMerchandiserRepository.Filter(x => x.EmployeeId == userId).Select(x => x.MerchandiserRefId).ToList();
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var workOrderList = _embWorkOrderRepository.GetWithInclude(x => merList.Contains(x.MerchandiserRefId)&&(x.PartyId==partyId||partyId==0)
                    && ((x.RefId.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString))
                    || (x.Attention.Contains(searchString) || String.IsNullOrEmpty(searchString))),"Party");
            totalRecords = workOrderList.Count();
            switch (sort)
            {
                case "RefId":
                    switch (sortdir)
                    {
                        case "DESC":
                            workOrderList = workOrderList
                                 .OrderByDescending(r => r.RefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            workOrderList = workOrderList
                                 .OrderBy(r => r.RefId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    workOrderList = workOrderList
                        .OrderByDescending(r => r.RefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return workOrderList.ToList();
        }

        public string GetNewRefId(string compId)
        {
            var maxRefId = _embWorkOrderRepository.Filter(x=>x.CompId==compId).Max(x => x.RefId);
            return maxRefId.IncrementOne().PadZero(7);
        }

        public OM_EmbWorkOrder GetEmbWorkOrderById(int embWorkOrderId)
        {
            return _embWorkOrderRepository.FindOne(x => x.EmbWorkOrderId == embWorkOrderId);
        }

        public int SaveEmbWorkOrder(OM_EmbWorkOrder embWorkOrder)
        {
            return _embWorkOrderRepository.Save(embWorkOrder);
        }

        public int DeleteEmbWorkOrder(int embWorkOrderId)
        {
            using (var transaction=new TransactionScope())
            {
                _embWorkOrderDetailRepository.Delete(x => x.EmbWorkOrderId == embWorkOrderId);
                OM_EmbWorkOrder embWorkOrder = _embWorkOrderRepository.FindOne(x => x.EmbWorkOrderId == embWorkOrderId);
               
                int deleted= _embWorkOrderRepository.DeleteOne(embWorkOrder);
                transaction.Complete();
                return deleted;
            }
        
        }

        public IEnumerable GetEmbWorkOrderDetails(int workOrderId)
        {
            string sp =string.Format( "exec spGetEmbWorkOrderDetails @EmbWorkOrderId={0}",workOrderId);
            DataTable dataTable = _embWorkOrderDetailRepository.ExecuteQuery(sp);
            return dataTable.Todynamic().ToList();
        }

        public OM_EmbWorkOrderDetail GetEmbWorkOrderDetailById(int embWorkOrderDetailId)
        {
            return _embWorkOrderDetailRepository.FindOne(x => x.EmbWorkOrderDetailId == embWorkOrderDetailId);
        }

        public int SaveEmbWorkOrderDetail(OM_EmbWorkOrderDetail embWorkOrderDetail)
        {
            return _embWorkOrderDetailRepository.Save(embWorkOrderDetail);
        }

        public int DeleteEmbWorkOrderDetail(int embWorkOrderDetailId)
        {
            OM_EmbWorkOrderDetail embWorkOrderDetail = _embWorkOrderDetailRepository.FindOne(x => x.EmbWorkOrderDetailId == embWorkOrderDetailId);
            return _embWorkOrderDetailRepository.DeleteOne(embWorkOrderDetail);
        }
    }
}
