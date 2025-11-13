using System;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.DAL;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.DAL.Repository.AccountingRepository;
using System.Linq;
using SCERP.Model;
using System.Collections.Generic;

namespace SCERP.BLL.Manager.AccountingManager
{
    public class CompanySectorManager : BaseManager, ICompanySectorManager
    {

        private ICompanySectorRepository companySectorRepository = null;

        public CompanySectorManager(SCERPDBContext context)
        {
            this.companySectorRepository = new CompanySectorRepository(context);
        }

        public List<Acc_CompanySector> GetAllCompanySectors(int page, int records, string sort)
        {
            return companySectorRepository.GetAllCompanySectors(page, records, sort);
        }

        public Acc_CompanySector GetCompanySectorById(int? id)
        {
            return companySectorRepository.GetCompanySectorById(id);
        }

        public int SaveCompanySector(Acc_CompanySector aCompanySector)
        {
            aCompanySector.IsActive = true;

            int savedCompanySector = 0;

            try
            {

                if (
                    companySectorRepository.Exists(
                        p =>
                            p.SectorCode == aCompanySector.SectorCode && aCompanySector.Id == 0 &&
                            p.IsActive == true))
                    return 2;

                else if (
                    companySectorRepository.Exists(
                        p =>
                            p.SectorName == aCompanySector.SectorName && aCompanySector.Id == 0 &&
                            p.IsActive == true))
                    return 3;

                savedCompanySector = companySectorRepository.Save(aCompanySector);
            }
            catch (Exception ex)
            {
                savedCompanySector = 0;
            }

            return savedCompanySector;
        }

        public void DeleteCompanySector(Acc_CompanySector CompanySector)
        {
            CompanySector.IsActive = false;
            companySectorRepository.Edit(CompanySector);
        }
    }
}
