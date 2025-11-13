using System.Collections.Generic;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.BLL.IManager.IInventoryManager
{
   public interface IGoodsReceivingNoteManager
    {
       string GetNewGoodsReceivingNoteNumber();
       bool CheckGoodsReceivingNote(int qualityCertificateId);
       int GoodsReceivingNote(Inventory_GoodsReceivingNote grn);
       List<VGoodsReceivingNote> GetGoodsReceivingNoteByPaging(out int totalRecords, VGoodsReceivingNote model);
       Inventory_GoodsReceivingNote GetGoodsReceivingNoteById(int goodsReceivingNotesId);
       ResponsModel SendToStoreLager(Inventory_GoodsReceivingNote goodsReceivingNote);
       int DeleteGoodsReceivingNote(int goodsReceivingNotesId);
       int ApprovedGrnSave
           (int goodsReceivingNotesId);
    }
}
