using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IInventoryRepository
{
   public interface IMaterialRequisitionRepository:IRepository<Inventory_MaterialRequisition>
   {
       IQueryable<VMaterialRequisition> GetMaterialRequisitions(Expression<Func<VMaterialRequisition, bool>> predicate);
       VMaterialRequisition GetVMaterialRequisitionById(int materialRequisitionId);
       List<Inventory_MaterialRequisitionDetail> GetMaterialRequisitionDetails(int materialRequisitionId);
       Inventory_MaterialRequisitionDetail GetMaterialRequisitionDetailById(int materialRequisitionDetailId);
       bool IsExistMaterialRequisitionNoteNo(VMaterialRequisition model);
   }
}
