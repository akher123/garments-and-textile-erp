using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.DAL.Repository.Planning;
using SCERP.Model;
using System.Linq;
using SCERP.Model.Planning;

namespace SCERP.BLL.Manager.PlanningManager
{
    public class ProcessTemplateManager : BaseManager, IProcessTemplateManager
    {
        private readonly IProcessTemplateRepository _processTemplateRepository = null;

        public ProcessTemplateManager(IProcessTemplateRepository processTemplateRepository)
        {
            _processTemplateRepository = processTemplateRepository;
        }

        public List<PLAN_ProcessTemplate> GetAllProcessTemplateByPaging(int startPage, int pageSize, out int totalRecords, PLAN_ProcessTemplate ProcessTemplate)
        {
            List<PLAN_ProcessTemplate> planProcessTemplate = null;
            planProcessTemplate = _processTemplateRepository.GetAllProcessTemplateByPaging(startPage, pageSize, out totalRecords, ProcessTemplate).ToList();
            return planProcessTemplate;
        }

        public List<PLAN_ProcessTemplate> GetAllProcessTemplate()
        {
            List<PLAN_ProcessTemplate> processTemplatePlan = null;
            processTemplatePlan = _processTemplateRepository.Filter(x => x.IsActive).OrderBy(x => x.Id).ToList();
            return processTemplatePlan;
        }

        public PLAN_ProcessTemplate GetProcessTemplateById(int? id)
        {
            PLAN_ProcessTemplate processTemplate = null;
            processTemplate = _processTemplateRepository.GetProcessTemplateById(id);
            return processTemplate;
        }

        public bool CheckExistingProcessTemplate(PLAN_ProcessTemplate ProcessTemplate)
        {
            bool isExist = false;

            isExist =
                _processTemplateRepository.Exists(
                    x =>
                        x.IsActive == true &&
                        x.Id != ProcessTemplate.Id &&
                        x.ProcessId == ProcessTemplate.ProcessId &&
                        x.StylerefId.Replace(" ", "").ToLower().Equals(ProcessTemplate.StylerefId.Replace(" ", "").ToLower()));
            return isExist;
        }

        public int SaveProcessTemplate(PLAN_ProcessTemplate processTemplate)
        {
            var savedProcessTemplate = 0;
            processTemplate.CreatedDate = DateTime.Now;
            processTemplate.CreatedBy = PortalContext.CurrentUser.UserId;
            processTemplate.IsActive = true;
            savedProcessTemplate = _processTemplateRepository.Save(processTemplate);
            return savedProcessTemplate;
        }

        public int EditProcessTemplate(PLAN_ProcessTemplate processTemplate)
        {
            var editedProcessTemplate = 0;
            processTemplate.EditedDate = DateTime.Now;
            processTemplate.EditedBy = PortalContext.CurrentUser.UserId;
            editedProcessTemplate = _processTemplateRepository.Edit(processTemplate);
            return editedProcessTemplate;
        }

        public int DeleteProcessTemplate(PLAN_ProcessTemplate processTemplate)
        {
            var deletedProcessTemplate = 0;
            processTemplate.EditedDate = DateTime.Now;
            processTemplate.EditedBy = PortalContext.CurrentUser.UserId;
            processTemplate.IsActive = false;
            deletedProcessTemplate = _processTemplateRepository.Edit(processTemplate);
            return deletedProcessTemplate;
        }

        public List<PLAN_ProcessTemplate> GetProcessTemplateBySearchKey(string styleId, int processId)
        {
            var plan = new List<PLAN_ProcessTemplate>();
            plan = _processTemplateRepository.GetProcessTemplateBySearchKey(styleId, processId);
            return plan;
        }

        public List<PLAN_ProcessTemplate> GetProcessTemplateByProcessId(int? processId)
        {
            List<PLAN_ProcessTemplate> processTemplate = null;
            processTemplate = _processTemplateRepository.GetProcessTemplateByProcessId(processId);
            return processTemplate;
        }

        public List<OM_Style> GetAllStyles()
        {
            List<OM_Style> styles = null;
            styles = _processTemplateRepository.GetAllStyles();
            return styles;
        }

        public List<PLAN_Process> GetAllProcesses()
        {
            List<PLAN_Process> processes = null;
            processes = _processTemplateRepository.GetAllProcesses();
            return processes;
        }

        public List<PLAN_ResponsiblePerson> GetAllResponsiblePersons()
        {
            List<PLAN_ResponsiblePerson> responsible = null;
            responsible = _processTemplateRepository.GetAllResponsiblePersons();
            return responsible;
        }

        public List<OM_Buyer> GetAllBuyers()
        {
            List<OM_Buyer> buyers = null;
            buyers = _processTemplateRepository.GetAllBuyers();
            return buyers;
        }

        public List<OM_BuyerOrder> GetAllOrders()
        {
            List<OM_BuyerOrder> buyerOrders = null;
            buyerOrders = _processTemplateRepository.GetAllOrders();
            return buyerOrders;
        }
    }
}
