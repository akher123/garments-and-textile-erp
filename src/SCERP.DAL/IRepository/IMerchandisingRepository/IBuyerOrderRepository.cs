using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using SCERP.Model;
using SCERP.Model.InventoryModel;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.IRepository.IMerchandisingRepository
{
    public interface IBuyerOrderRepository:IRepository<OM_BuyerOrder>
    {
        IQueryable<VBuyerOrder> GetBuyerOrderViews(Expression<Func<VBuyerOrder, bool>> predicate);
        string GetNewRefNo(string compId);
        DataTable GetMerchaiserWiseOrderDataTable(DateTime? fromDate, DateTime? toDate);

    }
}
