using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class MaterialReceivedManager : IMaterialReceivedManager
    {
        private readonly IMaterialReceivedRepository _materialReceivedRepository;
        private readonly IMateraialReceivedDetailRepository _materaialReceivedDetailRepository;

        public MaterialReceivedManager(IMaterialReceivedRepository materialReceivedRepository, IMateraialReceivedDetailRepository materaialReceivedDetailRepository)
        {
            _materialReceivedRepository = materialReceivedRepository;
            _materaialReceivedDetailRepository = materaialReceivedDetailRepository;
        }

        public List<Inventory_MaterialReceived> GetMaterialReceivedByPaging(DateTime? fromDate, DateTime? toDate, string searchString, string registerType, string compId, int pageIndex,
           string sort, string sortdir, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var materialReceivedList = _materialReceivedRepository.Filter(x => x.CompId == compId && (x.RegisterType.Trim().Contains(registerType) || String.IsNullOrEmpty(registerType)) &&
            ((x.ChallanNo.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString))
             ||(x.MaterialReceivedRefId.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString))
              || (x.BuyerName.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString))
              || (x.SupplierName.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString))
           && ((x.ReceivedDate >= fromDate || fromDate == null) && (x.ReceivedDate <= toDate || x.ReceivedDate == null))));
            totalRecords = materialReceivedList.Count();
            switch (sort)
            {
                case "BuyerName":
                    switch (sortdir)
                    {
                        case "DESC":
                            materialReceivedList = materialReceivedList
                                 .OrderByDescending(r => r.MaterialReceivedId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            materialReceivedList = materialReceivedList
                                 .OrderBy(r => r.MaterialReceivedId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    materialReceivedList = materialReceivedList
                        .OrderByDescending(r => r.MaterialReceivedId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return materialReceivedList.ToList();
        }

        public string GetMaterialReceivedRefId()
        {
            var maxRefId = _materialReceivedRepository.Filter(x=>x.CompId==PortalContext.CurrentUser.CompId).Max(x => x.MaterialReceivedRefId) ?? "0";
            return maxRefId.IncrementOne().PadZero(8);
        }

        public Inventory_MaterialReceived GetMaterialReceivedId(long materialReceivedId, string compId)
        {
            return _materialReceivedRepository.FindOne(x => x.CompId == compId && x.MaterialReceivedId == materialReceivedId);
        }

        public bool IsMaterialReceivedExist(Inventory_MaterialReceived model)
        {
            //return
            //    _materialReceivedRepository.Exists(
            //        x =>
            //            x.CompId == PortalContext.CurrentUser.CompId && x.ChallanNo == model.ChallanNo &&
            //            x.MaterialReceivedId != model.MaterialReceivedId); //This not applicatble for other 
            return false;
        }

        public int EditMaterialReceived(Inventory_MaterialReceived model)
        {
            int edited = 0;
            using (var transaction = new TransactionScope())
            {
               
                Inventory_MaterialReceived materialReceived = _materialReceivedRepository.FindOne(x => x.CompId == model.CompId && x.MaterialReceivedId == model.MaterialReceivedId);
                materialReceived.GRN = model.GRN;
                materialReceived.GEN = model.GEN;
                materialReceived.ChallanNo = model.ChallanNo;
                materialReceived.ChallanDate = model.ChallanDate;
                materialReceived.SupplierName = model.SupplierName;
                materialReceived.BuyerName = model.BuyerName;
                materialReceived.StyleNo = model.StyleNo;
                materialReceived.Article = model.Article;
                materialReceived.LCNo = model.LCNo;
                materialReceived.ReceivedDate = model.ReceivedDate;
                materialReceived.BillStatus = model.BillStatus;
                materialReceived.Remarks = model.Remarks;
                edited = _materialReceivedRepository.Edit(materialReceived);
                var detail = model.Inventory_MaterialReceivedDetail.Select(x =>
                {
                    x.MaterialReceivedId = materialReceived.MaterialReceivedId;
                    return x;
                });
                    _materaialReceivedDetailRepository.Delete(x => x.MaterialReceivedId == model.MaterialReceivedId);
                    edited += _materaialReceivedDetailRepository.SaveList(detail.ToList()); 

                transaction.Complete();
            }
            return edited;
        }

        public int SaveMaterialReceived(Inventory_MaterialReceived model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
            model.MaterialReceivedRefId = GetMaterialReceivedRefId();
            return _materialReceivedRepository.Save(model);
        }

        public int DeleteMaterialReceived(long materialReceivedId, string compId)
        {
            int deleted = 0;
            using (var transaction = new TransactionScope())
            {
                deleted += _materaialReceivedDetailRepository.Delete(x => x.CompId == compId && x.MaterialReceivedId == materialReceivedId);
                deleted += _materialReceivedRepository.Delete(x => x.CompId == compId && x.MaterialReceivedId == materialReceivedId);
                transaction.Complete();
            }
            return deleted;
        }

        public DataTable GetMaterialReceivedDataTable(DateTime? fromDate, DateTime? toDate, string challanNo,string registerType, string compId)
        {
            return _materialReceivedRepository.GetMaterialReceivedDataTable(fromDate, toDate, challanNo,registerType, compId);

        }

        public DataTable GetReceivedYarnByStyle(string orderStyleRefId)
        {
            string sql = @"
                        select 
                        ISNULL((select top(1) ItemName from Inventory_Item where ItemId=POD.ItemId),'Total :') AS Yarn,
                        (select ColorName from OM_Color where ColorRefId=POD.FColorRefId and CompId=POD.CompId) AS Color,
                        (select ColorName from OM_Color where ColorRefId=POD.ColorRefId and CompId=POD.CompId) AS LotNo,
                        (select SizeName from OM_Size where SizeRefId=POD.SizeRefId and CompId=POD.CompId) AS [Count]
                        ,SUM(POD.ReceivedQty-RejectedQty) AS Quantity
                         from Inventory_MaterialReceiveAgainstPo AS PO
                        inner join Inventory_MaterialReceiveAgainstPoDetail AS POD ON PO.MaterialReceiveAgstPoId=POD.MaterialReceiveAgstPoId
                        WHERE POD.OrderStyleRefId='{0}' and PO.RType='P'  AND PO.StoreId=1
                        GROUP BY GROUPING SETS ((POD.CompId,POD.SizeRefId,POD.ColorRefId,POD.FColorRefId,POD.ItemId), ())";
            sql = String.Format(sql, orderStyleRefId);
            return _materialReceivedRepository.ExecuteQuery(sql);
        }
    }
}
