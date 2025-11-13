using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.Model;
using SCERP.Model.Planning;
using SCERP.Common;
using System.Data;

namespace SCERP.DAL.Repository.Planning
{
    public class TNARepository : Repository<PLAN_TNA>, ITNARepository
    {
        private readonly SCERPDBContext _context;
        private readonly string _companyId;

        public TNARepository(SCERPDBContext context)
            : base(context)
        {
            this._context = context;
            this._companyId = PortalContext.CurrentUser.CompId;
        }

        public PLAN_TNA GetTnaById(int? id)
        {
            return Context.PLAN_TNA.Include("PLAN_Activity").FirstOrDefault(x => x.Id == id);
        }

        public List<PLAN_TNAReport> GetAllTnaByPaging(int startPage, int pageSize, out int totalRecords, string buyerRefId, string orderNo, string orderStyleRefId,Guid userId)
        {
            object[] parms = {
                new SqlParameter("BuyerRefId", buyerRefId),
                new SqlParameter("OrderNo", orderNo),
                new SqlParameter("OrderStyleRefId", orderStyleRefId),
                new SqlParameter("CompanyId", _companyId),
                new SqlParameter("UserId", userId)
            };
            var tnaReports = _context.Database.SqlQuery<PLAN_TNAReport>("SPPlanTNAReport @BuyerRefId,@OrderNo,  @OrderStyleRefId, @CompanyId,@UserId", parms).ToList();
            totalRecords = tnaReports.Count();
                    tnaReports = tnaReports
                        .OrderBy(r => r.OrderStyleRefId).ThenBy(x => x.ActivityId)
                        .Skip(startPage*pageSize)
                        .Take(pageSize).ToList();
            return tnaReports.ToList();
        }

        public List<PLAN_TNAReport> GetAllTnaResponsibleByPaging(int startPage, int pageSize, out int totalRecords, PLAN_TNA planTna)
        {
            var personName = planTna.OrderStyleRefId;

            List<PLAN_TNAReport> tnaReports;
            tnaReports = _context.Database.SqlQuery<PLAN_TNAReport>("SPPlanTNAResponsible @PersonName, @CompanyId", new SqlParameter("PersonName", personName), new SqlParameter("CompanyId", _companyId)).ToList();

            totalRecords = tnaReports.Count();

            switch (planTna.sort)
            {
                case "Id":

                    switch (planTna.sortdir)
                    {
                        case "DESC":
                            tnaReports = tnaReports
                                .OrderByDescending(r => r.Id).ThenBy(x => x.Id)
                                .Skip(startPage * pageSize)
                                .Take(pageSize).ToList();
                            break;
                        default:
                            tnaReports = tnaReports
                                .OrderBy(r => r.Id).ThenBy(x => x.Id)
                                .Skip(startPage * pageSize)
                                .Take(pageSize).ToList();
                            break;
                    }
                    break;

                default:
                    tnaReports = tnaReports
                        .OrderBy(r => r.OrderStyleRefId).ThenBy(x => x.ActivityId)
                        .Skip(startPage * pageSize)
                        .Take(pageSize).ToList();
                    break;
            }
            return tnaReports.ToList();
        }

        public List<PLAN_TNA> GetAllTna()
        {
            return Context.PLAN_TNA.Where(x => x.IsActive == true).OrderBy(y => y.Id).ToList();
        }

        public List<PLAN_TNA> GetTnaBySearchKey(string styleId, int activityId)
        {
            List<PLAN_TNA> planTnAs = null;

            planTnAs = Context.PLAN_TNA.Where(
                x =>
                    x.IsActive == true
                    && ((x.OrderStyleRefId.Replace(" ", "").ToLower().Contains(styleId.Replace(" ", "").ToLower())) || String.IsNullOrEmpty(styleId))
                    && (x.ActivityId == activityId || activityId == 0)).ToList();

            return planTnAs;
        }

        public List<PLAN_TNA> GetTnaByActivityId(int? activityId)
        {
            List<PLAN_TNA> planTnAs;
            planTnAs = Context.PLAN_TNA.Where(x => x.ActivityId == activityId && x.IsActive == true).OrderBy(r => r.Id).ToList();
            return planTnAs;
        }

        public List<OM_Style> GetAllStyles()
        {
            List<OM_Style> styles;
            styles = Context.OM_Style.OrderBy(r => r.StyleId).ToList();
            return styles;
        }

        public List<PLAN_Activity> GetAllActivities()
        {
            List<PLAN_Activity> activities;
            activities = Context.PLAN_Activity.Where(p => p.IsActive && p.ActivityMode != "N" && p.CompId == _companyId).OrderBy(r => r.Id).ToList();
            return activities;
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
            buyers = Context.OM_Buyer.Where(p => p.CompId == _companyId).OrderBy(r => r.BuyerName).ToList();
            return buyers;
        }

        public List<OM_BuyerOrder> GetAllOrders()
        {

            List<OM_BuyerOrder> buyerOrders;
            buyerOrders = Context.OM_BuyerOrder.Where(p => p.CompId == _companyId).OrderBy(r => r.BuyerOrderId).ToList();
            return buyerOrders;
        }

        public List<OM_Season> GetAllSeasons()
        {
            List<OM_Season> seasons;
            seasons = Context.OM_Season.Where(p => p.CompId == _companyId).OrderBy(r => r.SeasonRefId).ToList();
            return seasons;
        }

        public List<OM_Merchandiser> GetAllMerchandiser()
        {
            List<OM_Merchandiser> merchandisers;
            merchandisers = Context.OM_Merchandiser.Where(p => p.CompId == _companyId).OrderBy(r => r.EmpName).ToList();
            return merchandisers;
        }

        public PLAN_Activity GetActivityById(int activityId)
        {
            PLAN_Activity activity;
            activity = Context.PLAN_Activity.FirstOrDefault(p => p.Id == activityId && p.IsActive);
            return activity;
        }

        public PLAN_TNA GetTnaByRefId(string buyerOrderRef, int? activityId)
        {
            PLAN_TNA tna;
            tna = Context.PLAN_TNA.FirstOrDefault(p => p.ActivityId == activityId && p.OrderStyleRefId == buyerOrderRef && p.IsActive);
            return tna;
        }

        public List<string> GetStyleNames()
        {
            var style = from p in Context.OM_Style
                        where p.CompID == _companyId
                        select p;

            return style.Select(t => t.StyleName.Trim()).ToList();
        }

        public List<string> GetResponsibles()
        {
            var style = from p in Context.PLAN_ResponsiblePerson
                        where p.IsActive
                        select p;

            return style.Select(t => t.ResponsiblePersonName.Trim()).ToList();
        }

        public List<OM_BuyOrdStyle> GetAllStyleNames()
        {
            var lt = new List<OM_BuyOrdStyle>();
            var item = new OM_BuyOrdStyle();

            var style = from p in Context.OM_BuyOrdStyle
                        join q in Context.OM_Style on p.StyleRefId equals q.StylerefId
                        where p.CompId == _companyId
                        orderby p.StyleentDate descending
                        select new
                        {
                            OrderStyleRefId = p.OrderStyleRefId,
                            StyleName = q.StyleName
                        };

            foreach (var t in style)
            {
                item = new OM_BuyOrdStyle();
                item.OrderStyleRefId = t.OrderStyleRefId;
                item.OrderNo = t.OrderStyleRefId + " - " + t.StyleName;
                lt.Add(item);
            }
            return lt.ToList();
        }

        public string GetOrderStyleRefIdByStyleName(string styleName)
        {
            var orderStyleRefId = "";

            var buyOrdStyle = (from p in Context.OM_Style
                               join q in Context.OM_BuyOrdStyle on p.StylerefId equals q.StyleRefId
                               where p.StyleName.ToLower() == styleName.ToLower()
                               select q).FirstOrDefault();

            if (buyOrdStyle != null)
            {
                orderStyleRefId = buyOrdStyle.OrderStyleRefId;
            }
            return orderStyleRefId;
        }

        public string GetStyleNameByOrderStyleRefId(string orderStyleRefId)
        {
            var styleName = "";

            var style = (from p in Context.OM_Style
                         join q in Context.OM_BuyOrdStyle on p.StylerefId equals q.StyleRefId
                         where q.OrderStyleRefId.Trim().ToLower() == orderStyleRefId.Trim().ToLower()
                         select p).FirstOrDefault();

            if (style != null)
            {
                styleName = style.StyleName;
            }
            return styleName;
        }

        public List<PLAN_TNAReport> GetTnaMailData()
        {
            List<PLAN_TNAReport> tnaReports;
            tnaReports = _context.Database.SqlQuery<PLAN_TNAReport>("SPPlanTNAMail @CompanyId", new SqlParameter("CompanyId", _companyId)).ToList();
            return tnaReports;
        }

        /**************************************** TNA Template ********************************************/

        public List<PLAN_Activity> GetAllTnaTemplateByPaging(int startPage, int pageSize, out int totalRecords, PLAN_Activity planTna)
        {
            List<PLAN_Activity> planActivities;
            var TemplateId = 1;

            planActivities = Context.Database.SqlQuery<PLAN_Activity>("SPPlanGetTemplate @TemplateId", new SqlParameter("TemplateId", TemplateId)).ToList();

            totalRecords = planActivities.Count();

            switch (planTna.sort)
            {
                case "Id":

                    switch (planTna.sortdir)
                    {
                        case "DESC":
                            planActivities = planActivities
                                .OrderByDescending(r => r.Id).ThenBy(x => x.Id)
                                .Skip(startPage * pageSize)
                                .Take(pageSize).ToList();
                            break;

                        default:
                            planActivities = planActivities
                                .OrderBy(r => r.Id).ThenBy(x => x.Id)
                                .Skip(startPage * pageSize)
                                .Take(pageSize).ToList();
                            break;
                    }
                    break;

                default:
                    planActivities = planActivities
                        .OrderBy(r => r.SerialId).ThenBy(x => x.SerialId)
                        .Skip(startPage * pageSize)
                        .Take(pageSize).ToList();
                    break;
            }
            return planActivities.ToList();
        }

        public List<PLAN_TNA_Template> GetTnaTemplatesById(int templateId)
        {
            return Context.PLAN_TNA_Template.Where(p => p.TemplateId == templateId).ToList();
        }

        public PLAN_TNA_Template GetTnaTemplateById(int templateId, int? activityId)
        {
            return Context.PLAN_TNA_Template.FirstOrDefault(p => p.ActivityId == activityId && p.TemplateId == templateId && p.IsActive);
        }

        public DateTime GetOrderConfirmDate(string styleRefId)
        {
            var date = (from p in Context.OM_BuyOrdStyle
                        join q in Context.OM_BuyerOrder on p.OrderNo equals q.OrderNo into pq
                        from g in pq.DefaultIfEmpty()
                        where p.OrderStyleRefId == styleRefId
                        select new { temp = g.OrderDate }).SingleOrDefault();

            if (date != null) return date.temp.Value;
            return DateTime.Now;
        }

        public List<PLAN_StyleUF> GetStyleUf()
        {
            return Context.PLAN_StyleUF.Where(p => p.OTypeID == "10").ToList();
        }
    }
}