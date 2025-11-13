using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
    public interface IGoodsReceivingNoteRepository:IRepository<Inventory_GoodsReceivingNote>
    {
        string GetNewGoodsReceivingNoteNumber();
        IQueryable<VGoodsReceivingNote> GetGoodsReceivingNotes(Expression<Func<VGoodsReceivingNote, bool>> predicate);
    }
}
