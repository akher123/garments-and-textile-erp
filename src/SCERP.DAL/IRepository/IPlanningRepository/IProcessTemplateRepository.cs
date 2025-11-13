using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Planning;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IPlanningRepository
{
    public interface IProcessTemplateRepository : IRepository<PLAN_ProcessTemplate>
    {
        PLAN_ProcessTemplate GetProcessTemplateById(int? id);
        List<PLAN_ProcessTemplate> GetAllProcessTemplate();
        List<PLAN_ProcessTemplate> GetAllProcessTemplateByPaging(int startPage, int pageSize, out int totalRecords, PLAN_ProcessTemplate planProcessTemplate);
        List<PLAN_ProcessTemplate> GetProcessTemplateBySearchKey(string styleId, int processId);
        List<PLAN_ProcessTemplate> GetProcessTemplateByProcessId(int? processId);
        List<OM_Style> GetAllStyles();
        List<PLAN_Process> GetAllProcesses();
        List<PLAN_ResponsiblePerson> GetAllResponsiblePersons();
        List<OM_Buyer> GetAllBuyers();
        List<OM_BuyerOrder> GetAllOrders();
    }
}
