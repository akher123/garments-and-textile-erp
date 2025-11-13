using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;


namespace SCERP.DAL.Repository.InventoryRepository
{
    public class GoodsReceivingNoteRepository : Repository<Inventory_GoodsReceivingNote>, IGoodsReceivingNoteRepository
    {
        public GoodsReceivingNoteRepository(SCERPDBContext context)
            : base(context)
        {
        }

        //public string GetNewGoodsReceivingNoteNumber()
        //{
        //    string maxGroupCode = Context.Database.SqlQuery<string>(
        //  "select  RIGHT ('000000'+ CAST (isnull(max(GRNNumber),0)+1 AS varchar), 6) as GRNNumber from Inventory_GoodsReceivingNote").SingleOrDefault();
        //    return Convert.ToString(maxGroupCode);
        //}


        public string GetNewGoodsReceivingNoteNumber()
        {
            var reqNo = Context.Database.SqlQuery<string>(
                    "Select  substring(MAX(GRNNumber),4,8 )from Inventory_GoodsReceivingNote")
                    .SingleOrDefault() ?? "0";
            var maxNumericValue = Convert.ToInt32(reqNo);
            var irNo = "GRN" + GetRefNumber(maxNumericValue, 5);
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
        public IQueryable<VGoodsReceivingNote> GetGoodsReceivingNotes(Expression<Func<VGoodsReceivingNote, bool>> predicate)
        {
          return Context.VGoodsReceivingNotes.Where(predicate);
        }
    }
}
