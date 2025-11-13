using System;
using System.Collections.Generic;
using System.Linq;
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
    public class InventoryGroupManager : IInventoryGroupManager
    {
        private readonly IInventoryGroupRepository _inventoryGroupRepository = null;
        private readonly ResponsModel _responsModel;
        public InventoryGroupManager(SCERPDBContext context)
        {
            _inventoryGroupRepository = new InventoryGroupRepository(context);
            _responsModel=new ResponsModel();
        }

        public string GetMaxGroupCode()
        {
            return _inventoryGroupRepository.GetMaxGroupCode();
        }

        

        public Inventory_Group GetGroupById(int groupId)
        {
            return _inventoryGroupRepository.FindOne(x => x.GroupId == groupId&&x.IsActive);
        }

        public ResponsModel EditGroup(Inventory_Group group)
        {
            _responsModel.Message = "";
            int edit = 0;
            bool isGroupExist =
                _inventoryGroupRepository.Exists(
                    x =>
                        x.GroupId != group.GroupId &&
                        x.GroupName.Replace(" ", "").ToLower().Equals(group.GroupName.Replace(" ", "").ToLower()));
            if (isGroupExist)
            {
                _responsModel.Message += String.Format("{0}" + " Group name,",
                    group.GroupName);

            }
            else
            {
                group.IsActive = true;
                
                group.EditedBy = PortalContext.CurrentUser.UserId;
                group.EditedDate = DateTime.Now;
              edit = _inventoryGroupRepository.Edit(group);
            }

         
            if (edit>0)
            {
                _responsModel.Status = true; 
            }
          
           
            return _responsModel;
          
        }

        public List<Inventory_Group> AutocompliteGroup(string groupName)
        {
            return _inventoryGroupRepository.AutocompliteGroup(groupName);
        }

        public List<Inventory_Group> GetGroups()
        {
            return _inventoryGroupRepository.Filter(x => x.IsActive).ToList();
        }

        public ResponsModel SaveGroup(Inventory_Group group)
        {
            _responsModel.Message = "";
            var save = 0;
            var isGroupExist =
                _inventoryGroupRepository.Exists(
                    x =>x.GroupName.Replace(" ", "").ToLower().Equals(group.GroupName.Replace(" ", "").ToLower()));
            if (isGroupExist)
            {
                _responsModel.Message += String.Format("{0}" + " Group name already exist",
                    group.GroupName);
            }
            else
            {
                  
                group.IsActive = true;
                group.CreatedBy = PortalContext.CurrentUser.UserId;
                group.CreatedDate = DateTime.Now;
                save = _inventoryGroupRepository.Save(group);
            }
            if (save > 0)
            {
                _responsModel.Status = true;
            }


            return _responsModel;
        }

        public List<Inventory_Group> GetGroups(string groupCode)
        {
            return _inventoryGroupRepository.Filter(x => x.IsActive&&x.GroupCode==groupCode).ToList();
        }
    }
}
