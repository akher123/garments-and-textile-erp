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
    public class DepreciationChartManager : BaseManager, IDepreciationChartManager
    {

        private IDepreciationChartRepository DepreciationChartRepository = null;

        public DepreciationChartManager(SCERPDBContext context)
        {
            this.DepreciationChartRepository = new DepreciationChartRepository(context);
        }

        public List<Acc_DepreciationChart> GetAllDepreciationCharts(int page, int records, string sort)
        {
            return DepreciationChartRepository.GetAllDepreciation(page, records, sort);
        }

        public Acc_DepreciationChart GetDepreciationChartById(int? id)
        {
            return DepreciationChartRepository.GetDepreciationById(id);
        }

        public int SaveDepreciationChart(Acc_DepreciationChart DepreciationChart)
        {
            DepreciationChart.IsActive = true;

            int savedDepreciationChart = 0;

            try
            {
                if (
                    DepreciationChartRepository.Exists(
                        p =>
                            p.ControlCode == DepreciationChart.ControlCode && DepreciationChart.Id == 0 &&
                            p.IsActive == true))

                    return 2;

                savedDepreciationChart = DepreciationChartRepository.Save(DepreciationChart);
            }
            
            catch (Exception ex)
            {
                savedDepreciationChart = 0;
            }

            return savedDepreciationChart;
        }

        public void DeleteDepreciationChart(Acc_DepreciationChart DepreciationChart)
        {
            DepreciationChart.IsActive = false;
            DepreciationChartRepository.Edit(DepreciationChart);
        }

        public IQueryable<Acc_ControlAccounts> GetAllControlAccounts()
        {
            return DepreciationChartRepository.GetAllControlAccounts();
        }

        public string GetControlName(decimal ControlCode)
        {
            return DepreciationChartRepository.GetControlName(ControlCode);
        }
    }
}
