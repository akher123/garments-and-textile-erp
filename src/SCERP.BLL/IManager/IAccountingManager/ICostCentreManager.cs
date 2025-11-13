using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.AccountingModel;

namespace SCERP.BLL.IManager.IAccountingManager
{
    public interface ICostCentreManager
    {
        List<Acc_CostCentre> GetAllCostCentres(int page, int records, string sort);

        Acc_CostCentre GetCostCentreById(int? id);

        int SaveCostCentre(Acc_CostCentre aCostCentre);

        void DeleteCostCentre(Acc_CostCentre CostCentre);

        IQueryable<Acc_CompanySector> GetAllCompanySector();



        /*************** CostCentre Multilayer ***************/

        List<Acc_CostCentreMultiLayer> GetAllCostCentreMultiLayers();

        int GetMaxItemId(int parentId);

        int SaveCostCentre(Acc_CostCentreMultiLayer costCentre);

        int EditControlAccount(Acc_CostCentreMultiLayer model);

        int DeleteCostCentre(int id);

        Acc_CostCentreMultiLayer GetMultiLayerById(int id);

        Acc_CostCentreMultiLayer GetNewCostCentreById(int? id);
    }
}
