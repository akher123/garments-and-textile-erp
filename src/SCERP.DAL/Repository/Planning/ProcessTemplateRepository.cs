using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.Model;
using SCERP.Model.Planning;
using SCERP.Common;

namespace SCERP.DAL.Repository.Planning
{
    public class ProcessTemplateRepository : Repository<PLAN_ProcessTemplate>, IProcessTemplateRepository
    {
        public ProcessTemplateRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public PLAN_ProcessTemplate GetProcessTemplateById(int? id)
        {
            return Context.PLAN_ProcessTemplate.Include("PLAN_Process").FirstOrDefault(x => x.Id == id);
        }

        public List<PLAN_ProcessTemplate> GetAllProcessTemplateByPaging(int startPage, int pageSize, out int totalRecords, PLAN_ProcessTemplate planProcessTemplate)
        {
            IQueryable<PLAN_ProcessTemplate> planProcessTemplates;
            var searchByProcessId = planProcessTemplate.ProcessId;
            searchByProcessId = 0;
            var searchByStylerefId = planProcessTemplate.StylerefId;

            planProcessTemplates = Context.PLAN_ProcessTemplate.Include("PLAN_Process").Include("PLAN_ResponsiblePerson").Where(x => x.IsActive == true && ((x.StylerefId.Replace(" ", "").ToLower().Contains(searchByStylerefId.Replace(" ", "").ToLower())) ||
                                                                                                                                                            String.IsNullOrEmpty(searchByStylerefId)) && ((x.ProcessId == searchByProcessId || searchByProcessId == 0)));
            totalRecords = planProcessTemplates.Count();

            switch (planProcessTemplate.sort)
            {
                case "Id":

                    switch (planProcessTemplate.sortdir)
                    {
                        case "DESC":
                            planProcessTemplates = planProcessTemplates
                                .OrderByDescending(r => r.Id).ThenBy(x => x.Id)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            planProcessTemplates = planProcessTemplates
                                .OrderBy(r => r.Id).ThenBy(x => x.Id)
                                .Skip(startPage*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    planProcessTemplates = planProcessTemplates
                        .OrderBy(r => r.Id).ThenBy(x => x.Id)
                        .Skip(startPage*pageSize)
                        .Take(pageSize);
                    break;
            }
            return planProcessTemplates.ToList();
        }

        public List<PLAN_ProcessTemplate> GetAllProcessTemplate()
        {
            return Context.PLAN_ProcessTemplate.Where(x => x.IsActive == true).OrderBy(y => y.Id).ToList();
        }

        public List<PLAN_ProcessTemplate> GetProcessTemplateBySearchKey(string styleId, int processId)
        {
            List<PLAN_ProcessTemplate> planProcessTemplates = null;

            planProcessTemplates = Context.PLAN_ProcessTemplate.Where(
                x =>
                    x.IsActive == true
                    && ((x.StylerefId.Replace(" ", "").ToLower().Contains(styleId.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(styleId))
                    && (x.ProcessId == processId || processId == 0)).ToList();

            return planProcessTemplates;
        }

        public List<PLAN_ProcessTemplate> GetProcessTemplateByProcessId(int? processId)
        {
            List<PLAN_ProcessTemplate> planProcessTemplates;
            planProcessTemplates = Context.PLAN_ProcessTemplate.Where(x => x.ProcessId == processId && x.IsActive == true).OrderBy(r => r.Id).ToList();
            return planProcessTemplates;
        }

        public List<OM_Style> GetAllStyles()
        {
            List<OM_Style> styles;
            styles = Context.OM_Style.OrderBy(r => r.StyleId).ToList();
            return styles;
        }

        public List<PLAN_Process> GetAllProcesses()
        {
            List<PLAN_Process> processes;
            processes = Context.PLAN_Process.OrderBy(r => r.ProcessId).ToList();
            return processes;
        }

        public List<PLAN_ResponsiblePerson> GetAllResponsiblePersons()
        {
            List<PLAN_ResponsiblePerson> responsible;
            responsible = Context.PLAN_ResponsiblePerson.OrderBy(r => r.Id).ToList();
            return responsible;
        }

        public List<OM_Buyer> GetAllBuyers()
        {
            List<OM_Buyer> buyers;
            buyers = Context.OM_Buyer.OrderBy(r => r.BuyerId).ToList();
            return buyers;
        }

        public List<OM_BuyerOrder> GetAllOrders()
        {
            List<OM_BuyerOrder> buyerOrders;
            buyerOrders = Context.OM_BuyerOrder.OrderBy(r => r.BuyerOrderId).ToList();
            return buyerOrders;
        }
    }
}