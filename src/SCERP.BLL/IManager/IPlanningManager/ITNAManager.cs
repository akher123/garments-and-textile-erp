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
    public interface ITNAManager
    {
        List<PLAN_TNAReport> GetAllTnaByPaging(int startPage, int pageSize, out int totalRecords, string buyerRefId,string orderNo,string orderStyleRefId);

        List<PLAN_TNAReport> GetAllTnaResponsibleByPaging(int startPage, int pageSize, out int totalRecords, PLAN_TNA Tna);

        List<PLAN_TNA> GetAllTna();

        PLAN_TNA GetTnaById(int? id);

        int SaveTna(PLAN_TNA Tna);

        int EditTna(PLAN_TNA Tna);

        int DeleteTna(PLAN_TNA Tna);

        bool CheckExistingTna(PLAN_TNA Tna);

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

        string ProcessTNAToTemplate(int templateId, string styleRefNo);

        string SaveTNATemplate(List<string> values);

        List<PLAN_StyleUF> GetStyleUf();

        string GetOrderStyleRefIdByStyleName(string styleName);

        string GetStyleNameByOrderStyleRefId(string orderStyleRefId);

        List<PLAN_TNAReport> GetTnaMailData();
    }
}
