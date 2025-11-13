using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IAccountingManager
{
    public interface IDepreciationChartManager
    {
        List<Acc_DepreciationChart> GetAllDepreciationCharts(int page, int records, string sort);

        Acc_DepreciationChart GetDepreciationChartById(int? id);

        int SaveDepreciationChart(Acc_DepreciationChart DepreciationChart);

        void DeleteDepreciationChart(Acc_DepreciationChart DepreciationChart);

        IQueryable<Acc_ControlAccounts> GetAllControlAccounts();

        string GetControlName(decimal ControlCode);
    }
}
