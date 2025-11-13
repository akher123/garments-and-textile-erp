using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Planning;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IPlanningRepository
{
    public interface ITNARepository : IRepository<PLAN_TNA>
    {
        PLAN_TNA GetTnaById(int? id);
        List<PLAN_TNA> GetAllTna();
        List<PLAN_TNAReport> GetAllTnaByPaging(int startPage, int pageSize, out int totalRecords, string buyerRefId, string orderNo, string orderStyleRefId, Guid userId);
        List<PLAN_TNAReport> GetAllTnaResponsibleByPaging(int startPage, int pageSize, out int totalRecords, PLAN_TNA planTna);
        List<PLAN_TNA> GetTnaBySearchKey(string styleId, int activityId);
        List<PLAN_TNA> GetTnaByActivityId(int? activityId);
        List<OM_Style> GetAllStyles();
        List<PLAN_Activity> GetAllActivities();
        List<PLAN_ResponsiblePerson> GetAllResponsiblePersons();
        List<OM_Buyer> GetAllBuyers();
        List<OM_BuyerOrder> GetAllOrders();
        List<OM_Season> GetAllSeasons();
        List<OM_Merchandiser> GetAllMerchandiser();
        PLAN_Activity GetActivityById(int activityId);
        PLAN_TNA GetTnaByRefId(string buyerOrderRef, int? activityId);
        List<string> GetStyleNames();
        List<string> GetResponsibles();
        List<OM_BuyOrdStyle> GetAllStyleNames();
        List<PLAN_Activity> GetAllTnaTemplateByPaging(int startPage, int pageSize, out int totalRecords, PLAN_Activity planTna);
        List<PLAN_TNA_Template> GetTnaTemplatesById(int templateId);
        DateTime GetOrderConfirmDate(string styleRefId);
        PLAN_TNA_Template GetTnaTemplateById(int templateId, int? activityId);
        List<PLAN_StyleUF> GetStyleUf();
        string GetOrderStyleRefIdByStyleName(string styleName);
        string GetStyleNameByOrderStyleRefId(string orderStyleRefId);
        List<PLAN_TNAReport> GetTnaMailData();
    }
}
