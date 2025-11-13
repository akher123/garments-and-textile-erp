using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommercialModel;

namespace SCERP.BLL.IManager.ICommonManager
{
    public interface IPackingCreditManager
    {
        List<CommPackingCredit> GetPakingCredits(int lcId);
        CommPackingCredit GetPakingCreditById(int packingCreditId);
        int SavePackingCredit(CommPackingCredit packingCredit);
        int EditPackingCredit(CommPackingCredit packingCredit);
        int DeletePackingCredit(int packingCreditId);
    }
}
