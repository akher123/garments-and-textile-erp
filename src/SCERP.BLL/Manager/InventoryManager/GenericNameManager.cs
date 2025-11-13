using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.DAL.Repository.InventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class GenericNameManager : IGenericNameManager
    {
        private readonly IGenericNameRepository _genericNameRepository;
        private readonly string _compId;

        public GenericNameManager(IGenericNameRepository genericNameRepository)
        {
            _genericNameRepository = genericNameRepository;
            _compId = PortalContext.CurrentUser.CompId;
        }

        public List<Inventory_GenericName> GetGenericNameByPaging(int pageIndex, string sort, string sortdir,
            string searchString, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var genericNames = _genericNameRepository.Filter(x => x.CompId == _compId 
                && (x.Name.Trim()
                .Replace(" ", "")
                .ToLower()
                .Contains(searchString.Trim().Replace(" ", "").ToLower())||String.IsNullOrEmpty(searchString)));

            totalRecords = genericNames.Count();
            switch (sort)
            {
                case "Name":
                    switch (sortdir)
                    {
                        case "DESC":
                            genericNames = genericNames
                                .OrderByDescending(r => r.Name)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            genericNames = genericNames
                                .OrderBy(r => r.Name)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    genericNames = genericNames
                        .OrderByDescending(r => r.Name)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return genericNames.ToList();
        }

        public Inventory_GenericName GetGenericNameById(int genericNameId)
        {
            return _genericNameRepository.FindOne(x => x.CompId == _compId && x.GenericNameId == genericNameId);
        }

        public int EditGenericName(Inventory_GenericName genericName)
        {
            bool isExist = IsExistGenericName(genericName);
            int edited = 0;
            if (!isExist)
            {
                var generic =
                    _genericNameRepository.FindOne(
                        x => x.CompId == _compId && x.GenericNameId == genericName.GenericNameId);
                generic.Name = genericName.Name;
                generic.Description=genericName.Description;
             edited=   _genericNameRepository.Edit(generic);

            }
            else
            {
                throw new Exception(message:"Generic Name :"+genericName.Name+" "+"Already Exist");
            }
            return edited;
        }

        public int SaveGenericName(Inventory_GenericName genericName)
        {
            bool isExist = IsExistGenericName(genericName);
            int saveIndex = 0;
            if (!isExist)
            {
                genericName.CompId = _compId;
                saveIndex = _genericNameRepository.Save(genericName);
            }
            else
            {
                throw new Exception(message: "Generic Name :" + genericName.Name + " " + "Already Exist");
            }
            return saveIndex;
        }

        public int DeleteGenericName(int genericNameId)
        {
            var generic =
                 _genericNameRepository.FindOne(
                     x => x.CompId == _compId && x.GenericNameId == genericNameId);
            if (generic!=null)
            {
               return _genericNameRepository.DeleteOne(generic);
            }
            else
            {
                throw new Exception(message: "Generic Name Delete Failed");
            }
        }

        public List<Inventory_GenericName> GetAllGenericNames()
        {
            return _genericNameRepository.Filter(x => x.CompId == _compId).ToList();
        }

        private bool IsExistGenericName(Inventory_GenericName genericName)
        {
           return _genericNameRepository.Exists(  x => x.CompId == _compId && x.GenericNameId != genericName.GenericNameId && x.Name.Trim().Replace(" ", "").ToLower() == genericName.Name.Trim().Replace(" ", "").ToLower());
        }
    }
}
