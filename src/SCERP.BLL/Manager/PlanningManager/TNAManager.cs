using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using System.Web.UI.WebControls;
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
    public class TNAManager : BaseManager, ITNAManager
    {
        private readonly ITNARepository _tnaRepository = null;
        private readonly ITNAHorizontalRepository _tnaHorizontalRepository = null;
        private readonly ITNATemplateRepository _tnaTemplateRepository = null;

        public TNAManager(ITNARepository tnaRepository
            , ITNAHorizontalRepository tnaHorizontalRepository
            , ITNATemplateRepository tnaTemplateRepository)
        {
            _tnaRepository = tnaRepository;
            _tnaHorizontalRepository = tnaHorizontalRepository;
            _tnaTemplateRepository = tnaTemplateRepository;
        }

        public List<PLAN_TNAReport> GetAllTnaByPaging(int startPage, int pageSize, out int totalRecords, string buyerRefId, string orderNo, string orderStyleRefId)
        
        {
            List<PLAN_TNAReport> planTna = null;
            var userId = PortalContext.CurrentUser.UserId.GetValueOrDefault();
            planTna = _tnaRepository.GetAllTnaByPaging(startPage, pageSize, out totalRecords, buyerRefId,orderNo,orderStyleRefId,userId).ToList();
            return planTna;
        }

        public List<PLAN_TNAReport> GetAllTnaResponsibleByPaging(int startPage, int pageSize, out int totalRecords, PLAN_TNA Tna)
        {
            List<PLAN_TNAReport> planTna = null;
            planTna = _tnaRepository.GetAllTnaResponsibleByPaging(startPage, pageSize, out totalRecords, Tna).ToList();
            return planTna;
        }

        public List<PLAN_TNA> GetAllTna()
        {
            List<PLAN_TNA> tnaPlan = null;
            tnaPlan = _tnaRepository.Filter(x => x.IsActive).OrderBy(x => x.Id).ToList();
            return tnaPlan;
        }

        public PLAN_TNA GetTnaById(int? id)
        {
            PLAN_TNA tna = null;
            tna = _tnaRepository.GetTnaById(id);
            return tna;
        }

        public bool CheckExistingTna(PLAN_TNA Tna)
        {
            bool isExist = false;

            isExist =
                _tnaRepository.Exists(
                    x =>
                        x.IsActive == true &&
                        x.Id != Tna.Id &&
                        x.ActivityId == Tna.ActivityId &&
                        x.OrderStyleRefId.Replace(" ", "").ToLower().Equals(Tna.OrderStyleRefId.Replace(" ", "").ToLower()));

            return isExist;
        }

        public int SaveTna(PLAN_TNA Tna)
        {
            var savedTna = 0;
            Tna.CreatedDate = DateTime.Now;
            Tna.CreatedBy = PortalContext.CurrentUser.UserId;
            Tna.IsActive = true;
            savedTna = _tnaRepository.Save(Tna);
            return savedTna;
        }

        public int EditTna(PLAN_TNA Tna)
        {
            var editedTna = 0;
            Tna.EditedDate = DateTime.Now;
            Tna.IsActive = true;
            Tna.EditedBy = PortalContext.CurrentUser.UserId;
            editedTna = _tnaRepository.Edit(Tna);
            return editedTna;
        }

        public int DeleteTna(PLAN_TNA Tna)
        {
            var deletedTna = 0;
            Tna.EditedDate = DateTime.Now;
            Tna.EditedBy = PortalContext.CurrentUser.UserId;
            Tna.IsActive = false;
            deletedTna = _tnaRepository.Edit(Tna);
            return deletedTna;
        }

        public List<PLAN_TNA> GetTnaBySearchKey(string styleId, int activityId)
        {
            var plan = new List<PLAN_TNA>();
            plan = _tnaRepository.GetTnaBySearchKey(styleId, activityId);
            return plan;
        }

        public List<PLAN_TNA> GetTnaByActivityId(int? activityId)
        {
            List<PLAN_TNA> tna = null;
            tna = _tnaRepository.GetTnaByActivityId(activityId);
            return tna;
        }

        public List<OM_Style> GetAllStyles()
        {
            List<OM_Style> styles = null;
            styles = _tnaRepository.GetAllStyles();
            return styles;
        }

        public List<PLAN_Activity> GetAllActivities()
        {
            List<PLAN_Activity> activities = null;
            activities = _tnaRepository.GetAllActivities();
            return activities;
        }

        public List<PLAN_ResponsiblePerson> GetAllResponsiblePersons()
        {
            List<PLAN_ResponsiblePerson> responsible = null;
            responsible = _tnaRepository.GetAllResponsiblePersons();
            return responsible;
        }

        public List<OM_Buyer> GetAllBuyers()
        {
            List<OM_Buyer> buyers = null;
            buyers = _tnaRepository.GetAllBuyers();
            return buyers;
        }

        public List<OM_BuyerOrder> GetAllOrders()
        {
            List<OM_BuyerOrder> buyerOrders = null;
            buyerOrders = _tnaRepository.GetAllOrders();
            return buyerOrders;
        }

        public List<OM_Season> GetAllSeasons()
        {
            List<OM_Season> seasons = null;
            seasons = _tnaRepository.GetAllSeasons();
            return seasons;
        }

        public List<OM_Merchandiser> GetAllMerchandiser()
        {
            List<OM_Merchandiser> merchandisers = null;
            merchandisers = _tnaRepository.GetAllMerchandiser();
            return merchandisers;
        }

        public PLAN_Activity GetActivityById(int activityId)
        {
            return _tnaRepository.GetActivityById(activityId);
        }

        public string GetOrderStyleRefIdByStyleName(string styleName)
        {
            return _tnaRepository.GetOrderStyleRefIdByStyleName(styleName);
        }

        public string GetStyleNameByOrderStyleRefId(string orderStyleRefId)
        {
            return _tnaRepository.GetStyleNameByOrderStyleRefId(orderStyleRefId);
        }

        public PLAN_TNA GetTnaByRefId(string buyerOrderRef, int? activityId)
        {
            return _tnaRepository.GetTnaByRefId(buyerOrderRef, activityId);
        }

        public List<string> GetStyleNames()
        {
            return _tnaRepository.GetStyleNames();
        }

        public List<string> GetResponsibles()
        {
            return _tnaRepository.GetResponsibles();
        }
        public List<OM_BuyOrdStyle> GetAllStyleNames()
        {
            return _tnaRepository.GetAllStyleNames();
        }

        public List<PLAN_Activity> GetAllTnaTemplateByPaging(int startPage, int pageSize, out int totalRecords, PLAN_Activity planTna)
        {
            return _tnaRepository.GetAllTnaTemplateByPaging(startPage, pageSize, out totalRecords, planTna);
        }

        public string ProcessTNAToTemplate(int templateId, string styleRefNo)
        {
            var tna = new PLAN_TNA();
            var templates = _tnaRepository.GetTnaTemplatesById(templateId);
            DateTime orderDateTime = _tnaRepository.GetOrderConfirmDate(styleRefNo);

            foreach (var t in templates)
            {
                var result = 0;
                tna = _tnaRepository.GetTnaByRefId(styleRefNo, t.ActivityId) ?? new PLAN_TNA();
                tna.ActivityId = t.ActivityId;
                tna.OrderStyleRefId = styleRefNo;
                tna.PlannedStartDate = t.FromLeadTime == null ? (DateTime?) null : orderDateTime.AddDays(Convert.ToDouble(t.FromLeadTime));
                tna.PlannedEndDate = t.ToLeadTime == null ? (DateTime?) null : orderDateTime.AddDays(Convert.ToDouble(t.ToLeadTime));
                tna.CreatedDate = DateTime.Now;
                tna.EditedDate = DateTime.Now;
                tna.CreatedBy = PortalContext.CurrentUser.UserId;
                tna.EditedBy = PortalContext.CurrentUser.UserId;
                tna.CompId = PortalContext.CurrentUser.CompId;
                tna.IsActive = true;

                var exist = _tnaHorizontalRepository.Exists(p => p.OrderStyleRefId == tna.OrderStyleRefId && p.ActivityId == tna.ActivityId && p.IsActive);
                result = exist ? _tnaHorizontalRepository.Edit(tna) : _tnaHorizontalRepository.Save(tna);

                if (result > 0)
                {
                    _tnaHorizontalRepository.SaveTnaHorizontal(tna);
                }
            }
            return "Success";
        }

        public string SaveTNATemplate(List<string> values)
        {
            foreach (var t in values)
            {
                var activityId = t.Split('-').ElementAt(0).Trim();
                var fromLeadTime = t.Split('-').ElementAt(1).Trim();
                var toLeadTime = t.Split('-').ElementAt(2).Trim();

                PLAN_TNA_Template planTnaTemplate = _tnaRepository.GetTnaTemplateById(1, (activityId == "" ? (int?) null : Convert.ToInt32(activityId))) ?? new PLAN_TNA_Template();

                planTnaTemplate.TemplateId = 1;
                planTnaTemplate.ActivityId = (activityId == "" ? (int?) null : Convert.ToInt32(activityId));
                planTnaTemplate.FromLeadTime = (fromLeadTime == "" ? (int?) null : Convert.ToInt32(fromLeadTime));
                planTnaTemplate.ToLeadTime = (toLeadTime == "" ? (int?) null : Convert.ToInt32(toLeadTime));
                planTnaTemplate.IsActive = true;

                var exist = _tnaTemplateRepository.Exists(p => p.ActivityId == planTnaTemplate.ActivityId && p.IsActive);
                var result = exist ? _tnaTemplateRepository.Edit(planTnaTemplate) : _tnaTemplateRepository.Save(planTnaTemplate);
            }
            return "Data Saved Successfully !";
        }

        public List<PLAN_StyleUF> GetStyleUf()
        {
            return _tnaRepository.GetStyleUf();
        }

        public List<PLAN_TNAReport> GetTnaMailData()
        {
            return _tnaRepository.GetTnaMailData();
        }
    }
}
