using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.Model;

namespace SCERP.BLL.IManager.IInventoryManager
{
  public  interface  IMaterialIssueRequisitionManager
    {
      List<VMaterialIssueRequisition> GetMaterialIssueRequisitionByPaging(VMaterialIssueRequisition model, out int totalRecords);
      ResponsModel SaveMaterialIssueRequisition(Inventory_MaterialIssueRequisition materialIssueRequisition);

      VMaterialIssueRequisition GetVMaterialIssueRequisitionById(int materialIssueRequisitionId);
      List<Inventory_MaterialIssueRequisitionDetail> GetMaterialIssueRequisitionDetails(int materialIssueRequisitionId);
      ResponsModel EditMaterialIssueRequisition(Inventory_MaterialIssueRequisition materialIssueRequisition);
      int DeleteMaterialIssueRequisition(int? materialIssueRequisitionId);

      int DeleteMaterialIssueRequisitionDetail(int? materialIssueRequisitionDetailId);
      bool CheckModifiedByStore(int materialIssueRequisitionId);
    }
}
