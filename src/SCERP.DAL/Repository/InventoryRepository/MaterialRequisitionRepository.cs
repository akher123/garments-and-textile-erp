using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Data.Entity;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class MaterialRequisitionRepository :Repository<Inventory_MaterialRequisition>, IMaterialRequisitionRepository
    {
        public MaterialRequisitionRepository(SCERPDBContext context) : base(context)
        {

        }

        public IQueryable<VMaterialRequisition> GetMaterialRequisitions(Expression<Func<VMaterialRequisition, bool>> predicate)
        {
            return Context.VMaterialRequisitions.Where(predicate);
        }

        public VMaterialRequisition GetVMaterialRequisitionById(int materialRequisitionId)
        {
            return Context.VMaterialRequisitions.FirstOrDefault(x => x.MaterialRequisitionId == materialRequisitionId);
        }

        public List<Inventory_MaterialRequisitionDetail> GetMaterialRequisitionDetails(int materialRequisitionId)
        {
            return
                Context.Inventory_MaterialRequisitionDetail.Where(x => x.MaterialRequisitionId == materialRequisitionId&&x.IsActive).ToList();
        }

        public Inventory_MaterialRequisitionDetail GetMaterialRequisitionDetailById(int materialRequisitionDetailId)
        {
           return Context.Inventory_MaterialRequisitionDetail.Find(materialRequisitionDetailId);
        }

        public bool IsExistMaterialRequisitionNoteNo(VMaterialRequisition model)
        {
            return
                Context.VMaterialRequisitions.Any(
                    x =>
                        x.RequisitionNoteNo.ToLower().Equals(model.RequisitionNoteNo.ToLower())&&
                        x.MaterialRequisitionId != model.MaterialRequisitionId && x.BranchId != model.BranchId);
        }
    }
}
