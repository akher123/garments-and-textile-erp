using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using SCERP.Common;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Model;
using SCERP.Model.CommercialModel;

namespace SCERP.DAL.Repository.CommercialRepository
{
    public class BbLcPurchaseRepository : Repository<CommBbLcPurchaseCommon>, IBbLcPurchaseRepository
    {
        private readonly string _companyId;

        public BbLcPurchaseRepository(SCERPDBContext context)
            : base(context)
        {
            this._companyId = PortalContext.CurrentUser.CompId;
        }

        public CommBbLcPurchaseCommon GetBbLcPurchaseById(int? id)
        {
            return Context.CommBbLcPurchaseCommons.FirstOrDefault(x => x.BbLcPurchaseId == id);
        }

        public int? GetBbLcIdByLcNo(string lcNo)
        {
            var lcId = Context.COMMLcInfos.FirstOrDefault(p => p.LcNo.Trim().ToLower() == lcNo.Trim().ToLower());
            if (lcId != null)
                return lcId.LcId;
            else
                return 0;
        }

        public string GetOrderNoByOrderRefNo(string orderRefNo)
        {
            var orderNo = Context.OM_BuyerOrder.FirstOrDefault(p => p.RefNo.Trim().ToLower() == orderRefNo.Trim().ToLower());
            if (orderNo != null)
                return orderNo.OrderNo;
            else
                return "";
        }

        public List<OM_BuyOrdStyle> GetStylesByOrderNo(string orderNo)
        {
            return Context.OM_BuyOrdStyle.Where(p => p.OrderNo == orderNo).ToList();
        }

        public List<CommBbLcPurchaseCommon> GetAllBbLcPurchases()
        {
            return Context.CommBbLcPurchaseCommons.Where(x => x.IsActive == true).OrderBy(y => y.BbLcPurchaseId).ToList();
        }

        public List<CommBbLcPurchaseCommon> GetBbLcPurchaseBySearchKey(int searchByCountry, string searchByLcStyle)
        {
            List<CommBbLcPurchaseCommon> lcStyles = null;
            lcStyles = Context.CommBbLcPurchaseCommons.Where(x => x.IsActive).ToList();
            return lcStyles;
        }

        public List<CommBbLcPurchaseCommon> GetBbLcPurchaseByBbLcId(int lcId)
        {
            List<CommBbLcPurchaseCommon> lcStyles = null;
            lcStyles = Context.CommBbLcPurchaseCommons.Where(p => p.IsActive && p.BbLcPurchaseId == lcId).ToList();
            return lcStyles;
        }

        public List<VwBbLcPurchaseCommon> GetAllBbLcPurchasesByPaging(int startPage, int pageSize, out int totalRecords, CommBbLcPurchaseCommon bbLcPurchase)
        {
            string bbLcNo = bbLcPurchase.BbLcNo ?? "";
            string purchaseOrderNo = bbLcPurchase.PurchaseOrderNo ?? "";
            List<VwBbLcPurchaseCommon> result = Context.Database.SqlQuery<VwBbLcPurchaseCommon>("SPCommBbLcPurchaseCommonInfo @CompId, @BbLcNo, @PurchaseOrderNo", new SqlParameter("CompId", _companyId), new SqlParameter("BbLcNo", bbLcNo), new SqlParameter("PurchaseOrderNo", purchaseOrderNo)).ToList();
            totalRecords = result.Count();
            return result.ToList();
        }

        public List<VwCommBbLcPurchase> GetBbLcPurchaseEditByBbLcId(CommBbLcPurchaseCommon lcStyle)
        {
            int lcId = lcStyle.BbLcRefId ?? 0;
            string orderNo = "";
            List<VwCommBbLcPurchase> result = Context.Database.SqlQuery<VwCommBbLcPurchase>("SPCommBbLcPurchaseInfo @LcId, @OrderNo", new SqlParameter("LcId", lcId), new SqlParameter("OrderNo", orderNo)).ToList();
            return result.ToList();
        }
    }
}
