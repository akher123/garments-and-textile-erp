using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{


    public class BuyOrdShipRepository : Repository<OM_BuyOrdShip>, IBuyOrdShipRepository
    {
        public BuyOrdShipRepository(SCERPDBContext context)
            : base(context)
        {

        }


        public string GetNewOrderShipRefId(string compId)
        {
            string sqlQuery =
                String.Format(@"Select  substring(MAX(OrderShipRefId),4,8 ) from OM_BuyOrdShip where CompId='{0}'",
                    compId);
            string issueReceiveNo =
                Context.Database.SqlQuery<string>(sqlQuery)
                    .SingleOrDefault() ?? "0";
            int maxNumericValue = Convert.ToInt32(issueReceiveNo);
            var irNo = "SH/" + GetRefNumber(maxNumericValue, 5);
            return irNo;
        }



        private string GetRefNumber(int maxNumericValue, int length)
        {
            var refNumber = Convert.ToString(maxNumericValue + 1);
            while (refNumber.Length != length)
            {
                refNumber = "0" + refNumber;
            }
            return refNumber;
        }

        public DataTable UpdateTempAssort(OM_BuyOrdShip model)
        {

            var sqlTempAssortDelete = String.Format(@"DELETE FROM OM_TempAssort WHERE CompId='{0}' ", model.CompId);
            Context.Database.ExecuteSqlCommand(sqlTempAssortDelete);
            var sqlTempAssortInsert =
                String.Format(@"INSERT INTO OM_TempAssort (CompId, ColorRow, ColorRefId, C1, C2, C3, C4, C5, C6, C7, C8, C9, C10, C11, C12, C13, C14, C15, C16, C17, C18, C19, C20, C21, C22, C23, C24, C25, Total) SELECT CompId,ColorRow, ColorRefId, 0 AS C1, 0 AS C2, 0 AS C3, 0 AS C4, 0 AS C5, 0 AS C6, 0 AS C7, 0 AS C8, 0 AS C9, 0 AS C10, 0 AS C11, 0 AS C12, 0 AS C13, 0 AS C14, 0 AS C15, 0 AS C16, 0 AS C17, 0 AS C18, 0 AS C19, 0 AS C20, 0 AS C21, 0 AS C22, 0 AS C23, 0 AS C24, 0 AS C25, 0 AS Total FROM OM_BuyOrdStyleColor WHERE (CompID = '{0}') AND (OrderStyleRefId = '{1}')", model.CompId, model.OrderStyleRefId);
            Context.Database.ExecuteSqlCommand(sqlTempAssortInsert);
            string sqlShipDetail = "";
            List<VBuyOrdShipDetail> shipDetails;
            if (!String.IsNullOrEmpty(model.OrderShipRefId) && !String.IsNullOrEmpty(model.OrderStyleRefId))
            {
                 shipDetails = Context.VBuyOrdShipDetails.Where(x => x.CompId == model.CompId && x.OrderShipRefId == model.OrderShipRefId).GroupBy(x => new
                {
                    x.ColorRefId,
                    x.SizeRow
                }).ToList().Select(y => new VBuyOrdShipDetail()
                {
                    ColorRefId = y.First().ColorRefId,
                    SizeRow = y.First().SizeRow,
                    Quantity = y.Sum(x => x.Quantity),
                    QuantityP = y.Sum(x => x.QuantityP),
                }).ToList();
            }
            else
            {
                 shipDetails = Context.VBuyOrdShipDetails.Where(x => x.CompId == model.CompId && x.OrderStyleRefId == model.OrderStyleRefId).GroupBy(x => new
                {
                    x.ColorRefId,
                    x.SizeRow
                }).ToList().Select(y => new VBuyOrdShipDetail()
                {
                    ColorRefId = y.First().ColorRefId,
                    SizeRow = y.First().SizeRow,
                    Quantity = y.Sum(x => x.Quantity),
                    QuantityP = y.Sum(x => x.QuantityP),
                }).ToList();

            }
           
     


            foreach (var detail in shipDetails)
            {
                var updateTemAssort = String.Format(@"Update OM_TempAssort set c{0}={1} where CompId='{2}' and ColorRefId='{3}'", detail.SizeRow, detail.Quantity, model.CompId, detail.ColorRefId);
                Context.Database.ExecuteSqlCommand(updateTemAssort);
            }
            var sqlQueryTotal =
                String.Format(
                    @"UPDATE OM_TempAssort SET Total=C1+C2+C3+C4+C5+C6+C7+C8+C9+C10+C11+C12+C13+C14+C15+C16+C17+C18+C19+C20+C21+C22+C23+C24+C25 WHERE CompId='{0}'",
                    model.CompId);
            Context.Database.ExecuteSqlCommand(sqlQueryTotal);
            var tempAssor = ExecuteQuery(String.Format(@"Select (Select top 1 ColorName  From OM_Color where CompId=OM_TempAssort.CompId and OM_TempAssort.ColorRefId=OM_Color.ColorRefId) as Color ,* From OM_TempAssort where CompId='{0}'", model.CompId));
            return tempAssor;
        }

        public List<VBuyOrdStyleSize> GetBuyOrdStyleSize(OM_BuyOrdShip model)
        {
            var ordStyleSizeQuery =
            String.Format(@"SELECT OM_BuyOrdStyleSize.SizeRow, OM_BuyOrdStyleSize.SizeRefId, OM_Size.SizeName FROM OM_BuyOrdStyleSize INNER JOIN OM_Size ON OM_BuyOrdStyleSize.CompId = OM_Size.CompId AND OM_BuyOrdStyleSize.SizeRefId = OM_Size.SizeRefId WHERE OM_BuyOrdStyleSize.CompID = '{0}' AND (OM_BuyOrdStyleSize.OrderStyleRefId= '{1}') order by OM_BuyOrdStyleSize.SizeRow", model.CompId, model.OrderStyleRefId);
            var ordStyleSizeList = Context.Database.SqlQuery<VBuyOrdStyleSize>(ordStyleSizeQuery).ToList();
            return ordStyleSizeList;
        }

        public string GetNewLotNo(string compId, string orderStyleRefId)
        {
            var sqlQuery = String.Format("SELECT RIGHT('00'+ CAST( ISNULL(MAX(LotNo),0)+1 as varchar(2) ),2) as LotNo FROM OM_BuyOrdShip WHERE CompId='{0}' and OrderStyleRefId='{1}'", compId,orderStyleRefId);
            return Context.Database.SqlQuery<string>(sqlQuery).FirstOrDefault();
        }

        public DataTable UpdatedShipmentTotalTempAssort(OM_BuyOrdShip model)
        {

            var sqlTempAssortDelete = String.Format(@"DELETE FROM OM_TempAssort WHERE CompId='{0}' ", model.CompId);
            Context.Database.ExecuteSqlCommand(sqlTempAssortDelete);
            var sqlTempAssortInsert =
                String.Format(@"INSERT INTO OM_TempAssort (CompId, ColorRow, ColorRefId, C1, C2, C3, C4, C5, C6, C7, C8, C9, C10, C11, C12, C13, C14, C15, C16, C17, C18, C19, C20, C21, C22, C23, C24, C25, Total) SELECT CompId,ColorRow, ColorRefId, 0 AS C1, 0 AS C2, 0 AS C3, 0 AS C4, 0 AS C5, 0 AS C6, 0 AS C7, 0 AS C8, 0 AS C9, 0 AS C10, 0 AS C11, 0 AS C12, 0 AS C13, 0 AS C14, 0 AS C15, 0 AS C16, 0 AS C17, 0 AS C18, 0 AS C19, 0 AS C20, 0 AS C21, 0 AS C22, 0 AS C23, 0 AS C24, 0 AS C25, 0 AS Total FROM OM_BuyOrdStyleColor WHERE (CompID = '{0}') AND (OrderStyleRefId = '{1}')", model.CompId, model.OrderStyleRefId);
            Context.Database.ExecuteSqlCommand(sqlTempAssortInsert);
            string sqlShipDetail = "";
            List<VBuyOrdShipDetail> shipDetails;
            if (!String.IsNullOrEmpty(model.OrderShipRefId) && !String.IsNullOrEmpty(model.OrderStyleRefId))
            {
                shipDetails = Context.VBuyOrdShipDetails.Where(x => x.CompId == model.CompId && x.OrderShipRefId == model.OrderShipRefId).GroupBy(x => new
                {
                    x.ColorRefId,
                    x.SizeRow
                }).ToList().Select(y => new VBuyOrdShipDetail()
                {
                    ColorRefId = y.First().ColorRefId,
                    SizeRow = y.First().SizeRow,
                    Quantity = y.Sum(x => x.ShQty),
                    QuantityP = y.Sum(x => x.ShQty),
                }).ToList();
            }
            else
            {
                shipDetails = Context.VBuyOrdShipDetails.Where(x => x.CompId == model.CompId && x.OrderStyleRefId == model.OrderStyleRefId).GroupBy(x => new
                {
                    x.ColorRefId,
                    x.SizeRow
                }).ToList().Select(y => new VBuyOrdShipDetail()
                {
                    ColorRefId = y.First().ColorRefId,
                    SizeRow = y.First().SizeRow,
                    Quantity = y.Sum(x => x.ShQty),
                    QuantityP = y.Sum(x => x.ShQty),
                }).ToList();

            }




            foreach (var detail in shipDetails)
            {
                var updateTemAssort = String.Format(@"Update OM_TempAssort set c{0}={1} where CompId='{2}' and ColorRefId='{3}'", detail.SizeRow, detail.Quantity, model.CompId, detail.ColorRefId);
                Context.Database.ExecuteSqlCommand(updateTemAssort);
            }
            var sqlQueryTotal =
                String.Format(
                    @"UPDATE OM_TempAssort SET Total=C1+C2+C3+C4+C5+C6+C7+C8+C9+C10+C11+C12+C13+C14+C15+C16+C17+C18+C19+C20+C21+C22+C23+C24+C25 WHERE CompId='{0}'",
                    model.CompId);
            Context.Database.ExecuteSqlCommand(sqlQueryTotal);
            var tempAssor = ExecuteQuery(String.Format(@"Select (Select top 1 ColorName  From OM_Color where CompId=OM_TempAssort.CompId and OM_TempAssort.ColorRefId=OM_Color.ColorRefId) as Color ,* From OM_TempAssort where CompId='{0}'", model.CompId));
            return tempAssor;
        }

        public IQueryable<VwBuyOrdShip> GetBuyOrdShips(string orderStyleRefId, string compId)
        {
          return  Context.Database.SqlQuery<VwBuyOrdShip>(String.Format("SELECT * FROM VwBuyOrdShip WHERE OrderStyleRefId='{0}' AND CompId='{1}'",orderStyleRefId,compId)).AsQueryable();
        }
    }
}