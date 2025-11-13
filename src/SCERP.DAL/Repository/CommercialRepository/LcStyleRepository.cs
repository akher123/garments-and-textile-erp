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
    public class LcStyleRepository : Repository<COMMLcStyle>, ILcStyleRepository
    {
        private readonly string _companyId;

        public LcStyleRepository(SCERPDBContext context)
            : base(context)
        {
            this._companyId = PortalContext.CurrentUser.CompId;
        }

        public COMMLcStyle GetLcStyleById(int? id)
        {
            return Context.COMMLcStyles.FirstOrDefault(x => x.LcStyleId == id);
        }

        public int? GetLcIdByLcNo(string lcNo)
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

        public List<COMMLcStyle> GetAllLcStyles()
        {
            return Context.COMMLcStyles.Where(x => x.IsActive == true).OrderBy(y => y.LcStyleId).ToList();
        }

        public List<COMMLcStyle> GetLcStyleBySearchKey(int searchByCountry, string searchByLcStyle)
        {
            List<COMMLcStyle> lcStyles = null;
            lcStyles = Context.COMMLcStyles.Where(x => x.IsActive).ToList();
            return lcStyles;
        }

        public List<COMMLcStyle> GetLcStyleByLcId(int lcId)
        {
            List<COMMLcStyle> lcStyles = null;
            lcStyles = Context.COMMLcStyles.Where(p => p.IsActive && p.LcRefId == lcId).ToList();
            return lcStyles;
        }

        public List<VwCommLcStyle> GetAllLcStylesByPaging(int startPage, int pageSize, out int totalRecords, COMMLcStyle lcStyle)
        {
            int lcId = lcStyle.LcRefId ?? 0;
            string orderNo = lcStyle.OrderNo ?? "";

            List<VwCommLcStyle> result = Context.Database.SqlQuery<VwCommLcStyle>("SPCommLcStyleInfo @LcId, @OrderNo", new SqlParameter("LcId", lcId), new SqlParameter("OrderNo", orderNo)).ToList();
            totalRecords = result.Count();

            return result.ToList();
        }

        public List<VwCommLcStyle> GetLcStyleEditByLcId(COMMLcStyle lcStyle)
        {
            int lcId = lcStyle.LcRefId ?? 0;
            string orderNo = "";

            List<VwCommLcStyle> result = Context.Database.SqlQuery<VwCommLcStyle>("SPCommLcStyleInfo @LcId, @OrderNo", new SqlParameter("LcId", lcId), new SqlParameter("OrderNo", orderNo)).ToList();

            return result.ToList();
        }
    }
}