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
    public class OpeningBalaceManager : BaseManager, IOpeningBalaceManager
    {

        private IOpeningBalanceRepository  OpeningBalaceRepository = null;

        public OpeningBalaceManager(SCERPDBContext context)
        {
            this.OpeningBalaceRepository = new OpeningBalanceRepository(context);
        }

        public List<Acc_OpeningClosing> GetAllOpeningBalaces(int page, int records, string sort, ref List<decimal> totalAmount, int? FpId, int? SectorId, string GlId)
        {
            return OpeningBalaceRepository.GetAllOpeningBalaces(page, records, sort, ref totalAmount, FpId, SectorId,
                GlId);
        }

        public Acc_OpeningClosing GetOpeningBalaceById(long? id)
        {
            return OpeningBalaceRepository.GetOpeningBalaceById(id);
        }
          
        public int SaveOpeningBalance(Acc_OpeningClosing openingBalance)
        {
            openingBalance.IsActive = true;

            int savedOpeningBalance = 0;

            try
            {
                if (openingBalance.Id == 0)
                {
                    if (
                        OpeningBalaceRepository.Exists(
                            p =>
                                p.SectorId == openingBalance.SectorId && p.FpId == openingBalance.FpId &&
                                p.GlId == openingBalance.GlId && p.IsActive == true))

                        return 2;
                }
                else
                {
                    if (
                        OpeningBalaceRepository.Exists(
                            p =>
                                p.SectorId == openingBalance.SectorId && p.FpId == openingBalance.FpId &&
                                p.GlId == openingBalance.GlId && p.Id != openingBalance.Id && p.IsActive == true))

                        return 2;
                }

                savedOpeningBalance = OpeningBalaceRepository.Save(openingBalance);
            }
            catch (Exception ex)
            {
                savedOpeningBalance = 0;
            }

            return savedOpeningBalance;
        }

        public void DeleteOpeningBalace(Acc_OpeningClosing openingBalace)
        {
            openingBalace.IsActive = false;
            OpeningBalaceRepository.Edit(openingBalace);
        }

        public IQueryable<Acc_FinancialPeriod> GetFinancialPeriod()
        {
            return OpeningBalaceRepository.GetFinancialPeriod();
        }

        public IQueryable<Acc_CompanySector> GetSector()
        {
            return OpeningBalaceRepository.GetSector();
        }

        public IQueryable<Acc_GLAccounts> GetGLAccounts()
        {
            return OpeningBalaceRepository.GetGLAccounts();
        }

        public List<string> GetAccountName()
        {
            return OpeningBalaceRepository.GetAccountNames();
        }

        public string GetAccountNamesById(int Id)
        {            
            return OpeningBalaceRepository.GetAccountNamesById(Id);
        }

        public int Save(Acc_OpeningClosing open, string accountHead)
        {
            return OpeningBalaceRepository.Save(open, accountHead);
        }
    }
}
