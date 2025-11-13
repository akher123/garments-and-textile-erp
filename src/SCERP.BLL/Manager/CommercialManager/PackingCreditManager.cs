using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.DAL.IRepository;
using SCERP.Model.CommercialModel;

namespace SCERP.BLL.Manager.CommercialManager
{
    public class PackingCreditManager : IPackingCreditManager
    {
        private IRepository<CommPackingCredit> _packingCreditRepository;

        public PackingCreditManager(IRepository<CommPackingCredit> packingCreditRepository)
        {
            _packingCreditRepository = packingCreditRepository;
        }

        public List<CommPackingCredit> GetPakingCredits(int lcId)
        {
            return _packingCreditRepository.Filter(x => x.IsAcive && x.LcId == lcId).ToList();
        }

        public CommPackingCredit GetPakingCreditById(int packingCreditId)
        {
            return _packingCreditRepository.FindOne(x => x.IsAcive && x.PackingCreditId == packingCreditId);
        }

        public int SavePackingCredit(CommPackingCredit packingCredit)
        {
            return _packingCreditRepository.Save(packingCredit);
        }

        public int EditPackingCredit(CommPackingCredit model)
        {
            CommPackingCredit commPacking= _packingCreditRepository.FindOne(x => x.IsAcive && x.PackingCreditId == model.PackingCreditId);
            commPacking.CreditDate = model.CreditDate;
            commPacking.Amount = model.Amount;
            commPacking.UsdAmount = model.UsdAmount;
            commPacking.EditedBy = model.EditedBy;
            commPacking.EditedDate = model.EditedDate;
            return _packingCreditRepository.Edit(commPacking);
        }

        public int DeletePackingCredit(int packingCreditId)
        {
            CommPackingCredit commPacking = _packingCreditRepository.FindOne(x => x.IsAcive && x.PackingCreditId == packingCreditId);
            commPacking.IsAcive =false;
            return _packingCreditRepository.Edit(commPacking);
        }
    }
}
