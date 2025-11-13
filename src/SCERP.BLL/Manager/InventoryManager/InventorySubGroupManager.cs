using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.DAL.Repository.InventoryRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class InventorySubGroupManager : IInventorySubGroupManager
    {
        private readonly IInventorySubGroupRepository _inventorySubGroupRepository = null;
        private readonly ResponsModel _responsModel;
        public InventorySubGroupManager(SCERPDBContext context)
        {
            _inventorySubGroupRepository=new InventorySubGroupRepository(context);
            _responsModel=new ResponsModel();
        }

        public Inventory_SubGroup GetSubGroupById(int subGroupId)
        {
            return _inventorySubGroupRepository.FindOne(x => x.IsActive && x.SubGroupId == subGroupId);
        }

        public ResponsModel EditSubGroup(Inventory_SubGroup subGroup)
        {
            var edit = 0;
            _responsModel.Message = "";
            bool isSubGroupExist =
                _inventorySubGroupRepository.Exists(
                    x =>
                        x.SubGroupId != subGroup.SubGroupId && x.GroupId != subGroup.GroupId  &&
                        x.SubGroupName.Replace(" ", "").ToLower().Equals(subGroup.SubGroupName.Replace(" ", "").ToLower()));
            if (isSubGroupExist)
            {
                _responsModel.Message += String.Format("{0}" + " Group name,",
                    subGroup.SubGroupName);
            }
            else
            {
              
                subGroup.IsActive = true;
                subGroup.EditedBy = PortalContext.CurrentUser.UserId;
                subGroup.EditedDate = DateTime.Now;
                edit = _inventorySubGroupRepository.Edit(subGroup);
            }
           
            if (edit > 0)
            {
                _responsModel.Status = true;
            }
            return _responsModel;
       
        }

        public ResponsModel SaveSubGroup(Inventory_SubGroup subGroup)
        {

            var edit = 0;
            _responsModel.Message = "";
            var isSubGroupExist =
                _inventorySubGroupRepository.Exists(
                    x => x.GroupId == subGroup.GroupId &&
                        x.SubGroupName.Replace(" ", "").ToLower()==subGroup.SubGroupName.Replace(" ", "").ToLower());
            if (isSubGroupExist)
            {
                _responsModel.Message += String.Format("{0}" + " Group name already exist .",
                    subGroup.SubGroupName);
            }
            else
            {

                subGroup.IsActive = true;
                subGroup.CreatedBy = PortalContext.CurrentUser.UserId;
                subGroup.CreatedDate = DateTime.Now;
                edit =  _inventorySubGroupRepository.Save(subGroup);
            }

            if (edit > 0)
            {
                _responsModel.Status = true;
            }
            return _responsModel;

        
      
        }

        public List<Inventory_SubGroup> AutocompliteSubGroup(string subGroupName, int groupId, int branchId)
        {
            var subGroups= _inventorySubGroupRepository.AutocompliteSubGroup(subGroupName, groupId, branchId);
            return subGroups;
        }

        public List<Inventory_SubGroup> GetSubGroupByBranchId(int? groupId)
        {
          return  _inventorySubGroupRepository.Filter(x => x.IsActive && x.GroupId == groupId).ToList();
        }

        public List<Inventory_SubGroup> GetSubGroupByGroupId(int groupId)
        {
            return _inventorySubGroupRepository.Filter(x => x.IsActive && x.GroupId == groupId).ToList();
        }
    }
}
