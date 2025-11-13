using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IPayrollManager;
using SCERP.BLL.Manager.AccountingManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.DAL.Repository.AccountingRepository;
using SCERP.DAL.Repository.PayrollRepository;
using SCERP.Model;
using System.Globalization;


namespace SCERP.BLL.Manager.PayrollManager
{
    public class SalaryMappingManager : BaseManager, ISalaryMappingManager
    {
        protected readonly ISalaryMappingRepository SalaryMappingRepository = null;

        public SalaryMappingManager(SCERPDBContext context)
        {
            SalaryMappingRepository = new SalaryMappingRepository(context);
        }

        public List<SalaryHead> GetAllSalaryHead()
        {
            List<SalaryHead> salaryHead = null;

            try
            {
                salaryHead = SalaryMappingRepository.GetAllSalaryHead();
            }
            catch (Exception exception)
            {
                salaryHead = null;
                Errorlog.WriteLog(exception);
            }
            return salaryHead;
        }

        public string SaveSalaryMapping(List<string> lt)
        {
            try
            {
                List<SalaryHead> sh = SalaryMappingRepository.GetAllSalaryHead();
                int? sectorId = Convert.ToInt32(lt[0]);
                int? costCentre = Convert.ToInt32(lt[1]);

                SalaryMappingRepository.Delete(p => p.SectorId == sectorId && p.CostCentreId == costCentre);

                for (int i = 2; i < 14; i++)
                {
                    var sm = new Acc_SalaryMapping
                    {
                        SectorId = Convert.ToInt32(lt[0]),
                        CostCentreId = Convert.ToInt32(lt[1]),
                    };

                    if (string.IsNullOrEmpty(lt[i]))
                        continue;

                    var glCode = Convert.ToDecimal(lt[i].Substring(0, 10));
                    sm.SalaryHeadId = sh[i - 2].Id;
                    sm.GLID = SalaryMappingRepository.GetAccountId(glCode);
                    sm.IsActive = true;
                    sm.CDT = DateTime.Now;
                    sm.CreatedBy = new Guid("698130ec-de49-42df-8f63-0386d60fd5e8");
                    sm.EDT = DateTime.Now;
                    sm.EditedBy = new Guid("698130ec-de49-42df-8f63-0386d60fd5e8");

                    SalaryMappingRepository.Save(sm);
                }
                return "Data saved Successfully !";
            }
            catch (Exception ex)
            {
                Errorlog.WriteLog(ex);
                return "Error has occured !";
            }
        }

        public List<string> GetSalaryMapping(int? sectorId, int? costCentreId)
        {
            List<string> lt = new List<string>();

            try
            {
                var temp =
                    SalaryMappingRepository.All()
                        .Where(p => p.IsActive == true && p.SectorId == sectorId && p.CostCentreId == costCentreId);

                for (int i = 0; i < 12; i++)
                {
                    lt.Add("");
                }

                foreach (var t in temp)
                {
                    lt.Insert(t.SalaryHeadId.Value - 1, SalaryMappingRepository.GetAccountNamesById(t.GLID.Value));
                }
                return lt;
            }
            catch (Exception ex)
            {
                return lt;
            }
        }
    }
}
