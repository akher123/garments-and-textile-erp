using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model.InventoryModel;

namespace SCERP.BLL.Manager.InventoryManager
{
    public class FabricReturnManager : IFabricReturnManager
    {
        private readonly IFabricReturnRepository _fabricReturnRepository;
        public FabricReturnManager(IFabricReturnRepository fabricReturnRepository)
        {
            _fabricReturnRepository = fabricReturnRepository;
        }

        public List<Inventory_FabricReturn> GetFabricReturnByProgramId(long programId)
        {
            return _fabricReturnRepository.GetWithInclude(x => x.ProgramId == programId, "PLAN_Program").ToList();
        }

        public Inventory_FabricReturn GetFabricReturnById(long fabricReturnId)
        {
            return _fabricReturnRepository.FindOne(x => x.FabricReturnId == fabricReturnId);
        }

        public int EditFabricReturn(Inventory_FabricReturn fabricReturn)
        {
            var fabricRetn = _fabricReturnRepository.FindOne(x => x.FabricReturnId == fabricReturn.FabricReturnId);
            fabricRetn.ReturnDate = fabricReturn.ReturnDate;
            fabricRetn.Remarks = fabricReturn.Remarks;
            fabricRetn.ReturnChallanNo = fabricReturn.ReturnChallanNo;
            fabricRetn.FabQty = fabricReturn.FabQty;
            fabricRetn.QtyInPcs = fabricReturn.QtyInPcs;
            fabricRetn.ReturnYarnQty = fabricReturn.ReturnYarnQty;
            fabricRetn.WstYarnQty = fabricReturn.WstYarnQty;
            fabricRetn.ReceivedBy = fabricReturn.ReceivedBy;
            fabricRetn.ProgramDetailId = fabricReturn.ProgramDetailId;
            return _fabricReturnRepository.Edit(fabricRetn);
        }

        public int SaveFabricReturn(Inventory_FabricReturn fabricReturn)
        {
            return _fabricReturnRepository.Save(fabricReturn);
        }

        public int DeleteFabricById(long fabricReturnId)
        {
            return _fabricReturnRepository.Delete(x => x.FabricReturnId == fabricReturnId);
        }
    }
}
