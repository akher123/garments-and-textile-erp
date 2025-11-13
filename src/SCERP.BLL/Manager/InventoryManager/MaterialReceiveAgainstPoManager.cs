using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class MaterialReceiveAgainstPoManager : IMaterialReceiveAgainstPoManager
    {
        private readonly IMaterialReceiveAgainstPoRepository _materialReceiveAgainstPoRepository;
        private readonly IMaterialReceiveAgainstPoDetailRepository _materialReceiveAgainstPoDetailRepository;
        private readonly IStockRegisterRepository _stockRegisterRepository;
        public MaterialReceiveAgainstPoManager(IStockRegisterRepository stockRegisterRepository, IMaterialReceiveAgainstPoRepository materialReceiveAgainstPoRepository, IMaterialReceiveAgainstPoDetailRepository materialReceiveAgainstPoDetailRepository)
        {
            _materialReceiveAgainstPoRepository = materialReceiveAgainstPoRepository;
            _materialReceiveAgainstPoDetailRepository = materialReceiveAgainstPoDetailRepository;
            _stockRegisterRepository = stockRegisterRepository;
        }

        public List<Inventory_MaterialReceiveAgainstPo> GetReceiveAgainstPoByPaging(int pageIndex, string sort, string sortdir, DateTime? fromDate, DateTime? toDate,
            string searchString, out int totalRecords, int storeId,  string[] rtypes)
        {
            var pageSize = AppConfig.PageSize;
            var compId = PortalContext.CurrentUser.CompId;
            var materialList = _materialReceiveAgainstPoRepository.GetWithInclude(x => x.CompId == compId && x.StoreId == storeId&& rtypes.Contains(x.RType)
                && ((x.MRRNo.Trim().ToLower().Contains(searchString.Trim().ToLower()) || String.IsNullOrEmpty(searchString))
                || (x.InvoiceNo.Trim().ToLower().Contains(searchString.Trim().ToLower()) || String.IsNullOrEmpty(searchString))
                || (x.PoNo.Trim().ToLower().Contains(searchString.Trim().ToLower()) || String.IsNullOrEmpty(searchString))
                || (x.RefNo.Trim().ToLower().Contains(searchString.Trim().ToLower()) || String.IsNullOrEmpty(searchString))
                || (x.Mrc_SupplierCompany.CompanyName.Trim().ToLower().Contains(searchString.Trim().ToLower()) || String.IsNullOrEmpty(searchString)))
               && ((x.MRRDate >= fromDate || fromDate == null) && (x.MRRDate <= toDate || toDate == null)), "Mrc_SupplierCompany");
            totalRecords = materialList.Count();
            materialList = materialList.OrderByDescending(r => r.MaterialReceiveAgstPoId).Skip(pageIndex * pageSize).Take(pageSize);
            return materialList.ToList();
        }

        public Inventory_MaterialReceiveAgainstPo GetReceiveAgainstPoByid(long materialReceiveAgstPoId)
        {
            var compId = PortalContext.CurrentUser.CompId;
            return _materialReceiveAgainstPoRepository.FindOne(
                x => x.CompId == compId && x.MaterialReceiveAgstPoId == materialReceiveAgstPoId);
        }
        public string GetNewRcvRefId()
        {
            string compId = PortalContext.CurrentUser.CompId;
            var refNo = _materialReceiveAgainstPoRepository.Filter(x => x.CompId == compId&&x.StoreId==(int)StoreType.Yarn).Max(x => x.RefNo.Substring(2)) ?? "0";
            string newRefNo = refNo.IncrementOne().PadZero(10);
            return newRefNo;
        }

        public Dictionary<string, VwMaterialReceiveAgainstPoDetail> GetDictionary(long lecevedAgainstPoId)
        {
            List<VwMaterialReceiveAgainstPoDetail> details = _materialReceiveAgainstPoRepository.VwMaterialReceiveAgainstPoDetail(lecevedAgainstPoId);
            return details.ToDictionary(x => Convert.ToString(x.MaterialReceiveAgstPoDetailId), x => x);
        }

        public int SaveReceiveAgainstPo(Inventory_MaterialReceiveAgainstPo receiveAgainstPo)
        {
            int saveIndex = 0;
            using (var transaction = new TransactionScope())
            {
                string compId = PortalContext.CurrentUser.CompId;
                var receivedObj = _materialReceiveAgainstPoRepository.FindOne(x => x.MaterialReceiveAgstPoId == receiveAgainstPo.MaterialReceiveAgstPoId && x.CompId == compId) ?? new Inventory_MaterialReceiveAgainstPo();
                receivedObj.CompId = compId;
                receivedObj.VoucherNo = receiveAgainstPo.VoucherNo;
                receivedObj.OrderStyleRefId = receiveAgainstPo.OrderStyleRefId;
                receivedObj.MaterialReceiveAgstPoId = receiveAgainstPo.MaterialReceiveAgstPoId;
                receivedObj.SupplierId = receiveAgainstPo.SupplierId;
                receivedObj.MRRNo = receiveAgainstPo.MRRNo;
                receivedObj.MRRDate = receiveAgainstPo.MRRDate;
                receivedObj.InvoiceNo = receiveAgainstPo.InvoiceNo;
                receivedObj.InvoiceDate = receiveAgainstPo.InvoiceDate;
                receivedObj.ReceiveRegDate = receiveAgainstPo.ReceiveRegDate;
                receivedObj.ReceiveRegNo = receiveAgainstPo.ReceiveRegNo;
                receivedObj.GateEntryNo = receiveAgainstPo.GateEntryNo;
                receivedObj.GateEntryDate = receiveAgainstPo.GateEntryDate;
                receivedObj.PoNo = receiveAgainstPo.PoNo;
                receivedObj.PoDate = receiveAgainstPo.PoDate;
                receivedObj.StoreId = receiveAgainstPo.StoreId;
                receivedObj.RType = receiveAgainstPo.RType;
                receivedObj.BuyerId = receiveAgainstPo.BuyerId;
                receivedObj.OrderNo = receiveAgainstPo.OrderNo;
                receivedObj.StyleNo = receiveAgainstPo.StyleNo;
                receivedObj.LcNo = receiveAgainstPo.LcNo;
                receivedObj.Remarks = receiveAgainstPo.Remarks;
                receivedObj.EmployeeId = PortalContext.CurrentUser.UserId;
                receivedObj.QcStatus = receiveAgainstPo.QcStatus;
                receivedObj.GrnStatus = receiveAgainstPo.GrnStatus;
                receivedObj.TotalAmount = receiveAgainstPo.Inventory_MaterialReceiveAgainstPoDetail.Sum(x => (x.ReceivedQty - x.RejectedQty.GetValueOrDefault()) * x.ReceivedRate);
                receivedObj.Inventory_MaterialReceiveAgainstPoDetail = receiveAgainstPo.Inventory_MaterialReceiveAgainstPoDetail;
                if (receivedObj.MaterialReceiveAgstPoId > 0 &&
                    receivedObj.Inventory_MaterialReceiveAgainstPoDetail.Any())
                {
                   // receivedObj.RefNo = receiveAgainstPo.RefNo;
                    saveIndex += _materialReceiveAgainstPoDetailRepository.Delete(x => x.CompId == compId && x.MaterialReceiveAgstPoId == receivedObj.MaterialReceiveAgstPoId);
                }
                else
                {
                    receivedObj.RefNo = receiveAgainstPo.RefNo;

                }
                saveIndex += _materialReceiveAgainstPoRepository.Save(receivedObj);
                transaction.Complete();
            }
            return saveIndex;
        }

        public int DeteteReceiveAgainstPo(long materialReceiveAgstPoId, int receiveActionType)
        {
            string compId = PortalContext.CurrentUser.CompId;
            int delteIndex = 0;
            var receivedAnstPo = _materialReceiveAgainstPoRepository.FindOne(x => x.CompId == compId && x.MaterialReceiveAgstPoId == materialReceiveAgstPoId);
            if (receivedAnstPo == null)
            {
                throw new Exception("Delete Failed ");
            }
            else
            {
                delteIndex += _materialReceiveAgainstPoDetailRepository.Delete(x => x.CompId == compId && x.MaterialReceiveAgstPoId == materialReceiveAgstPoId);
                delteIndex += _stockRegisterRepository.Delete(x => x.CompId == compId && x.SourceId == materialReceiveAgstPoId && x.ActionType == receiveActionType);
                delteIndex += _materialReceiveAgainstPoRepository.DeleteOne(receivedAnstPo);
            }
            return delteIndex;
        }

        public List<VwMaterialReceiveAgainstPo> GetMrrSummaryReport(DateTime? fromDate, DateTime? toDate, string searchString)
        {
            var compId = PortalContext.CurrentUser.CompId;
            return _materialReceiveAgainstPoRepository.GetMrrSummaryReport(compId, fromDate, toDate, searchString);
        }

        public List<VwMaterialReceiveAgainstPoDetail> GetVwMaterialReceiveAgainstPoDetail(long materialReceiveAgstPoId)
        {

            return _materialReceiveAgainstPoRepository.VwMaterialReceiveAgainstPoDetail(materialReceiveAgstPoId);
        }

        public int UpdateQc(Inventory_MaterialReceiveAgainstPo receiveAgainstPo)
        {
            int saveIndex = 0;
            using (var transaction = new TransactionScope())
            {
                string compId = PortalContext.CurrentUser.CompId;
                Inventory_MaterialReceiveAgainstPo receivedObj = _materialReceiveAgainstPoRepository.FindOne(x => x.MaterialReceiveAgstPoId == receiveAgainstPo.MaterialReceiveAgstPoId && x.CompId == compId) ?? new Inventory_MaterialReceiveAgainstPo();
                receivedObj.QcDate = receiveAgainstPo.QcDate;
                receivedObj.QcStatus = receiveAgainstPo.QcStatus;
                receivedObj.QcRemarks = receiveAgainstPo.QcRemarks;
                receivedObj.TotalAmount = receiveAgainstPo.Inventory_MaterialReceiveAgainstPoDetail.Sum(x => (x.ReceivedQty - x.RejectedQty.GetValueOrDefault()) * x.ReceivedRate);
                saveIndex += _materialReceiveAgainstPoRepository.Edit(receivedObj);
                foreach (var detail in receiveAgainstPo.Inventory_MaterialReceiveAgainstPoDetail)
                {
                    var receivedDetailObj = _materialReceiveAgainstPoDetailRepository.FindOne(
                   x => x.CompId == compId && x.MaterialReceiveAgstPoId == receiveAgainstPo.MaterialReceiveAgstPoId && x.MaterialReceiveAgstPoDetailId == detail.MaterialReceiveAgstPoDetailId);
                    receivedDetailObj.RejectedQty = detail.RejectedQty;
                    receivedDetailObj.DiscountQty = detail.DiscountQty;
                    saveIndex += _materialReceiveAgainstPoDetailRepository.Edit(receivedDetailObj);
                }
                transaction.Complete();
            }
            return saveIndex;
        }

        public int UpdateGrn(Inventory_MaterialReceiveAgainstPo receiveAgainstPo, int receiveActionType)
        {
            int saveIndex = 0;
            string compId = PortalContext.CurrentUser.CompId;
            Inventory_MaterialReceiveAgainstPo receivedObj = _materialReceiveAgainstPoRepository.FindOne(x => x.MaterialReceiveAgstPoId == receiveAgainstPo.MaterialReceiveAgstPoId && x.CompId == compId) ?? new Inventory_MaterialReceiveAgainstPo();
            receivedObj.GrnDate = receiveAgainstPo.GrnDate;
            receivedObj.GrnStatus = receiveAgainstPo.GrnStatus;
            receivedObj.GrnRemarks = receiveAgainstPo.GrnRemarks;

            using (var transaction = new TransactionScope())
            {
                saveIndex += _materialReceiveAgainstPoRepository.Edit(receivedObj);
                saveIndex += _stockRegisterRepository.Delete(x => x.CompId == compId && x.SourceId == receiveAgainstPo.MaterialReceiveAgstPoId && x.ActionType == receiveActionType);
                saveIndex += receiveAgainstPo.Inventory_MaterialReceiveAgainstPoDetail.Sum(detail => _stockRegisterRepository.Save(new Inventory_StockRegister()
                {
                    CompId = compId,
                    ItemId = detail.ItemId,
                    TransactionDate = receiveAgainstPo.GrnDate.GetValueOrDefault(),
                    TransactionType = (int)StoreLedgerTransactionType.Receive,
                    StoreId = receivedObj.StoreId,
                    Rate = detail.ReceivedRate,
                    Quantity = (detail.ReceivedQty - detail.RejectedQty.GetValueOrDefault()),
                    ColorRefId = detail.ColorRefId,
                    SizeRefId = detail.SizeRefId,
                    SourceId = receiveAgainstPo.MaterialReceiveAgstPoId,
                    ActionType = receiveActionType
                }));
                transaction.Complete();
            }
            return saveIndex;
        }

        public string GetNewAcessRcvRefId()
        {
            int accessories = (int)StoreType.Acessories;
            string compId = PortalContext.CurrentUser.CompId;
            var refNo = _materialReceiveAgainstPoRepository.Filter(x => x.CompId == compId && x.StoreId == accessories).Max(x => x.RefNo.Substring(2)) ?? "0";
            string newRefNo = refNo.IncrementOne().PadZero(8);
            return "AR" + newRefNo;
        }

        public DataTable GetAccessoriesStatusDataTable(string modelOrderStyleRefId, string compId)
        {
            return _materialReceiveAgainstPoRepository.GetAccessoriesStatusDataTable(modelOrderStyleRefId, compId);
        }

        public DataTable GetAccessoriesRcvDetailStatus(DateTime? fromDate, DateTime? toDate, string compId)
        {
            return _materialReceiveAgainstPoRepository.GetAccessoriesRcvDetailStatus(fromDate, toDate, compId);
        }
    }
}
