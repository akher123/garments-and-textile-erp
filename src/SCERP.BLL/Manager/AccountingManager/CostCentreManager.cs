using System;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.DAL.Repository.AccountingRepository;
using System.Linq;
using SCERP.Model;
using System.Collections.Generic;
using SCERP.Model.AccountingModel;
using System.Transactions;

namespace SCERP.BLL.Manager.AccountingManager
{
    public class CostCentreManager : BaseManager, ICostCentreManager
    {

        private ICostCentreRepository CostCentreRepository = null;
        private ICostCentreMultiLayerRepository costCentreMultiLayerRepository = null;

        public CostCentreManager(SCERPDBContext context)
        {
            this.CostCentreRepository = new CostCentreRepository(context);
            costCentreMultiLayerRepository = new CostCentreMultiLayerRepository(context);
        }

        public List<Acc_CostCentre> GetAllCostCentres(int page, int records, string sort)
        {
            return CostCentreRepository.GetAllCostCentres(page, records, sort);
        }

        public Acc_CostCentre GetCostCentreById(int? id)
        {
            return CostCentreRepository.GetCostCentreById(id);
        }

        public Acc_CostCentreMultiLayer GetNewCostCentreById(int? id)
        {
            return CostCentreRepository.GetNewCostCentreById(id);
        }

        public int SaveCostCentre(Acc_CostCentre aCostCentre)
        {
            aCostCentre.IsActive = true;

            int savedCostCentre = 0;

            try
            {
                if (
                    CostCentreRepository.Exists(
                        p =>
                            p.CostCentreName == aCostCentre.CostCentreName && aCostCentre.Id == 0 &&
                            p.IsActive == true))
                    return 2;

                else if (
                    CostCentreRepository.Exists(
                        p =>
                            p.CostCentreCode == aCostCentre.CostCentreCode && aCostCentre.Id == 0 &&
                            p.IsActive == true))
                    return 3;


                savedCostCentre = CostCentreRepository.Save(aCostCentre);
            }
            catch (Exception ex)
            {
                savedCostCentre = 0;
            }

            return savedCostCentre;
        }

        public void DeleteCostCentre(Acc_CostCentre CostCentre)
        {
            CostCentre.IsActive = false;
            CostCentreRepository.Edit(CostCentre);
        }

        public IQueryable<Acc_CompanySector> GetAllCompanySector()
        {
            return CostCentreRepository.GetAllCompanySector();
        }




        /*************** CostCentre Multilayer ***************/

        public List<Acc_CostCentreMultiLayer> GetAllCostCentreMultiLayers()
        {
            return costCentreMultiLayerRepository.GetAllCostCentreMultiLayers();
        }

        public int GetMaxItemId(int parentId)
        {
            return costCentreMultiLayerRepository.GetMaxItemId(parentId);
        }
         
        public int SaveCostCentre(Acc_CostCentreMultiLayer costCentre)
        {
            int index = 0;

            try
            {
                using (var transaction = new TransactionScope())
                {
                    index += costCentreMultiLayerRepository.Save(costCentre);
                    transaction.Complete();
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                index = 0;
            }
            return index;
        }

        public int EditControlAccount(Acc_CostCentreMultiLayer model)
        {
            int edit = 0;

            try
            {
                var accControlAccounts = costCentreMultiLayerRepository.FindOne(x => x.Id == model.Id);
                accControlAccounts.ItemId = model.ItemId;
                accControlAccounts.ItemLevel = model.ItemLevel;
                accControlAccounts.ItemName = model.ItemName;
                accControlAccounts.ParentId = model.ParentId;
                accControlAccounts.SortOrder = model.SortOrder;
                accControlAccounts.IsActive = model.IsActive;

                edit = costCentreMultiLayerRepository.Edit(accControlAccounts);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                edit = 0;
            }

            return edit;
        }

        public int DeleteCostCentre(int id)
        {
            int edit = 0;

            try
            {
                if (!costCentreMultiLayerRepository.ExistCostCentre(id))
                {
                    Acc_CostCentreMultiLayer costCentre = costCentreMultiLayerRepository.FindOne(x => x.Id == id);
                    costCentre.IsActive = false;
                    edit = costCentreMultiLayerRepository.Edit(costCentre);
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                edit = 0;
            }
            return edit;
        }

        public Acc_CostCentreMultiLayer GetMultiLayerById(int id)
        {
            return costCentreMultiLayerRepository.GetMultiLayerById(id);
        }
    }
}
