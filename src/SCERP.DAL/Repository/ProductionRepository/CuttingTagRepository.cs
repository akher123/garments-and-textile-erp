using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model;
using SCERP.Model.CommonModel;
using SCERP.Model.Production;

namespace SCERP.DAL.Repository.ProductionRepository
{
    public class CuttingTagRepository : Repository<PROD_CuttingTag>, ICuttingTagRepository
    {
        public CuttingTagRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public IQueryable<VwCuttingTag> GetVwCuttingTagByCuttingSequenceId(string compId, long cuttingSequenceId)
        {
            string sqlQuery = String.Format(@"SELECT CT.CuttingTagId,CT.CuttingSequenceId,CT.CompId,CT.ComponentRefId,CT.IsSolid,CT.IsPrint,CT.IsEmbroidery,OMC.ComponentId,OMC.ComponentName FROM PROD_CuttingTag AS CT INNER JOIN OM_Component AS OMC
                                              ON CT.ComponentRefId=OMC.ComponentRefId AND CT.CompId=OMC.CompId where CT.CompId='{0}' and CT.CuttingSequenceId='{1}'", compId, cuttingSequenceId);
            return Context.Database.SqlQuery<VwCuttingTag>(sqlQuery).AsQueryable();
        }
        public IQueryable<VwCuttingTag> GetVwCuttingTagSupplierByCuttingTagId(string compId, long cuttingTagId)
        {
            string sqlQuery = String.Format(@"SELECT CTS.CuttingTagSupplierId,CTS.Rate,CTS.DeductionAllowance,CTS.EmblishmentStatus,P.PartyId,P.Name AS PartyName,CT.CuttingTagId,CT.CuttingSequenceId,CT.CompId,CT.ComponentRefId,
            CT.IsSolid,CT.IsPrint,CT.IsEmbroidery,OMC.ComponentId,OMC.ComponentName
            FROM PROD_CuttingTag AS CT 
            INNER JOIN OM_Component AS OMC
            ON CT.ComponentRefId=OMC.ComponentRefId AND CT.CompId=OMC.CompId
            INNER JOIN PROD_CuttingTagSupplier AS CTS
            ON CT.CuttingTagId=CTS.CuttingTagId AND CT.CompId=CTS.CompId
            INNER JOIN Party AS P
            ON P.PartyId=CTS.PartyId where CTS.CompId='{0}' AND CTS.CuttingTagId='{1}'", compId, cuttingTagId);
            return Context.Database.SqlQuery<VwCuttingTag>(sqlQuery).AsQueryable();
        }

        public object GetTagBySequence(string componentRefId, string orderStyleRefId, string compId)
        {
            var taglist = (from ctg in Context.PROD_CuttingTag
                join csq in Context.PROD_CuttingSequence on ctg.CuttingSequenceId equals csq.CuttingSequenceId
                join com in Context.OM_Component on new{ctg.ComponentRefId,ctg.CompId} equals new{ com.ComponentRefId,com.CompId}
                where csq.ComponentRefId == componentRefId && csq.OrderStyleRefId == orderStyleRefId && ctg.CompId == compId && ctg.IsPrint 
                select new {com.ComponentRefId, com.ComponentName}).Distinct().ToList();
            return taglist;
        }

        public List<SpPrintEmbroiderySummary> GetPrintEmbroideryBalance(string cuttingBatchRefId,string buyerRefId,string orderNo,string orderStyleRefId,string colorRefId)
        {
            var parObjects = new[]
            {
                new SqlParameter("@CuttingBatchRefId",cuttingBatchRefId),
                new SqlParameter("@BuyerRefId",buyerRefId),
                new SqlParameter("@OrderNo",orderNo),
                new SqlParameter("@OrderStyleRefId",orderStyleRefId),
                new SqlParameter("@ColorRefId",colorRefId),
                new SqlParameter("@CompId", PortalContext.CurrentUser.CompId)
            };
            return Context.Database.SqlQuery<SpPrintEmbroiderySummary>("SpPrintEmbroiderySummary @CuttingBatchRefId,@BuyerRefId,@OrderNo,@OrderStyleRefId,@ColorRefId,@CompId", parObjects).ToList();
        }
    }
}
