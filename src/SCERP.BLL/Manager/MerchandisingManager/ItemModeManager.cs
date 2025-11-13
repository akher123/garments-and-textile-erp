using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.Repository.MerchandisingRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class ItemModeManager : IItemModeManager
    {
        private readonly IItemModeRepository _itemModeRepository;
        public ItemModeManager(IItemModeRepository itemModeRepository)
        {
            _itemModeRepository = itemModeRepository;
        }

        public List<OM_ItemMode> GetItemModes()
        {
            return _itemModeRepository.All().ToList();
        }

        public List<OM_ItemMode> GetItemModePaging(OM_ItemMode model, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public OM_ItemMode GetItemModeById(long styleId)
        {
            throw new NotImplementedException();
        }

      
        public int EditItemMode(OM_ItemMode model)
        {
            throw new NotImplementedException();
        }

        public int SaveItemMode(OM_ItemMode model)
        {
            throw new NotImplementedException();
        }

        public int DeleteItemMode(string p)
        {
            throw new NotImplementedException();
        }
    }
}
