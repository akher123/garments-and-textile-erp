using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.BLL.IManager.IInventoryManager
{
   public interface IMaterialRequisitionManager
    {
       List<VMaterialRequisition> GetMaterialRequisitionByPaging(VMaterialRequisition model, out int totalRecords);
       ResponsModel SaveMaterialRequisition(Inventory_MaterialRequisition materialRequisition);
       VMaterialRequisition GetVMaterialRequisitionById(int materialRequisitionId);
       List<Inventory_MaterialRequisitionDetail> GetMaterialRequisitionDetails(int materialRequisitionId);
       ResponsModel EditMaterialRequisition(Inventory_MaterialRequisition materialRequisition);
       bool IsExistMaterialRequisitionNoteNo(VMaterialRequisition model);
       bool CheckModifiedByStore(int materialRequisitionId);
       int DeleteMaterialRequisitionDetail(int? materialRequisitionDetailId);
    }
}
