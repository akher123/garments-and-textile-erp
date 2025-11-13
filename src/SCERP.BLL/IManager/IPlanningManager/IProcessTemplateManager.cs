using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Linq;
using SCERP.Model.Planning;

namespace SCERP.BLL.IManager.IPlanningManager
{
    public interface IProcessTemplateManager
    {
        List<PLAN_ProcessTemplate> GetAllProcessTemplateByPaging(int startPage, int pageSize, out int totalRecords, PLAN_ProcessTemplate processTemplate);

        List<PLAN_ProcessTemplate> GetAllProcessTemplate();

        PLAN_ProcessTemplate GetProcessTemplateById(int? id);

        int SaveProcessTemplate(PLAN_ProcessTemplate processTemplate);

        int EditProcessTemplate(PLAN_ProcessTemplate processTemplate);

        int DeleteProcessTemplate(PLAN_ProcessTemplate processTemplate);

        bool CheckExistingProcessTemplate(PLAN_ProcessTemplate processTemplate);

        List<PLAN_ProcessTemplate> GetProcessTemplateBySearchKey(string styleId, int processId);

        List<PLAN_ProcessTemplate> GetProcessTemplateByProcessId(int? processId);

        List<OM_Style> GetAllStyles();

        List<PLAN_Process> GetAllProcesses();

        List<PLAN_ResponsiblePerson> GetAllResponsiblePersons();

        List<OM_Buyer> GetAllBuyers();

        List<OM_BuyerOrder> GetAllOrders();
    }
}
