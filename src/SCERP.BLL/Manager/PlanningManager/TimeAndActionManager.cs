using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.Manager.MerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.Model.MerchandisingModel;


namespace SCERP.BLL.Manager.PlanningManager
{
    public class TimeAndActionManager : ITimeAndActionManager
    {

        private ITimeAndActionRepository _tnRepository;
        private readonly IMerchandiserRepository _merchandiserRepository;
        private readonly IRepository<OM_TnaActivity> _tnaActivityRepository;
        private readonly IBuyerTnaTemplateRepository _buyerTnaTemplateRepository;
        public TimeAndActionManager(ITimeAndActionRepository tnRepository, IMerchandiserRepository merchandiserRepository, IRepository<OM_TnaActivity> tnaActivityRepository, IBuyerTnaTemplateRepository buyerTnaTemplateRepository)
        {
            _tnRepository = tnRepository;
            _merchandiserRepository = merchandiserRepository;
            _tnaActivityRepository = tnaActivityRepository;
            _buyerTnaTemplateRepository = buyerTnaTemplateRepository;
        }

        public DataTable GetStyleWiseTna(string orderNo, string orderStyleRefId, string buyerRefId, string compId, string searchKey, string activitySearchKey, DateTime? date)
        {
            string respobsible = PortalContext.CurrentUser.TnaResponsible??"ALL";
            string dat = "";
            string pSql = "";
            if (date != null)
            {
                dat = date.GetValueOrDefault().ToString("yyyy-MM-dd");
            }
            if (!String.IsNullOrEmpty(searchKey))
            {
                searchKey = searchKey.Replace(" ", "").ToLower();
            }
            bool userMarchandiser = _merchandiserRepository.IsMerchandiser(PortalContext.CurrentUser.UserId);
            if (!userMarchandiser)
            {
                pSql = String.Format(" and VOM_BuyOrdStyle.TnaMode='{0}'", "L");
            }
            string sql = String.Format(@"select OM_TNA.TnaRowId, VOM_BuyOrdStyle.BuyerName,VOM_BuyOrdStyle.RefNo as OrderNo,VOM_BuyOrdStyle.StyleName,Quantity,VOM_BuyOrdStyle.Merchandiser,(SELECT      top(1)  MaskId
                FROM  OM_TnaActivity where SlNo=OM_TNA.SerialId) as SerialId,OM_TNA.ActivityName,OM_TNA.PSDate,OM_TNA.PEDate,OM_TNA.Rmks,OM_TNA.XWho,OM_TNA.XWhen,Convert(varchar,OM_TNA.ASDate,103) as ASDate,Convert(varchar,OM_TNA.AEDate,103) as AEDate,OM_TNA.Responsible,OM_TNA.UpdateRemarks from OM_TNA
                        inner join VOM_BuyOrdStyle on OM_TNA.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and OM_TNA.CompId=VOM_BuyOrdStyle.CompId
					    where  (OM_TNA.OrderStyleRefId='{0}' or '{0}'='-1')  and (VOM_BuyOrdStyle.OrderNo='{1}' or '{1}'='-1') and VOM_BuyOrdStyle.BuyerRefId='{2}' and OM_TNA.CompId='{3}' and ( replace(lower(OM_TNA.Responsible),' ','') like '%{4}%' or '{4}'='') and  (OM_TNA.ActivityName like '%{5}%' or '{5}'='') 
                      and VOM_BuyOrdStyle.ActiveStatus=1 and ((dbo.fnDateConvert(OM_TNA.PSDate)>='{6}' or '{6}'='') or (dbo.fnDateConvert(OM_TNA.ASDate)>='{6}' or '{6}'=''))  
                  {7} and (OM_TNA.Responsible='{8}' OR '{8}'='ALL') order by   OM_TNA.SerialId", orderStyleRefId, orderNo, buyerRefId, compId, searchKey, activitySearchKey, dat, pSql, respobsible);
            DataTable tnaTable = _tnRepository.ExecuteQuery(sql);
            return tnaTable;
        }


        public int UpdateTna(string compId, int tnaRowId, string key, string value)
        {
            return _tnRepository.UpdateTna(compId, tnaRowId, key, value);
        }

        public int AddRows(int rowNumber, string orderStyleRefId, string compId)
        {
            List<OM_TNA> teList = new List<OM_TNA>();
            Guid userId = PortalContext.CurrentUser.UserId.GetValueOrDefault();

            for (int i = 1; i <= rowNumber; i++)
            {
                teList.Add(new OM_TNA()
                {
                    SerialId = i,
                    OrderStyleRefId = orderStyleRefId,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    CompId = compId
                });
            }
            return _tnRepository.SaveList(teList);


        }

        public DataTable GetTnAReport(string orderStyleRefId, string compId)
        {
            return _tnRepository.ExecuteQuery(String.Format("exec spTnAReport '{0}','{1}'", compId, orderStyleRefId));
        }

        public bool IsExistTnA(string copyOrderStyleRefId)
        {
            return _tnRepository.Exists(x => x.OrderStyleRefId == copyOrderStyleRefId);
        }

        public bool TnACopyAndPast(string orderStyleRefId, string copyOrderStyleRefId)
        {
            List<OM_TNA> teList = _tnRepository.Filter(x => x.OrderStyleRefId == orderStyleRefId).ToList();
            teList = teList.Select(x =>
            {
                x.OrderStyleRefId = copyOrderStyleRefId;
                x.CreatedDate = DateTime.Now;
                x.CreatedBy = PortalContext.CurrentUser.UserId;
                x.EditedBy = PortalContext.CurrentUser.UserId;
                x.EditedDate = DateTime.Now;
                return x;
            }).ToList();
            return _tnRepository.SaveList(teList) > 0;

        }

        public int RemoveRow(int rowNumber, string orderStyleRefId, string compId)
        {
            List<OM_TNA> teList = _tnRepository.Filter(x => x.OrderStyleRefId == orderStyleRefId && x.SerialId == rowNumber).ToList();

            foreach (OM_TNA tna in teList)
            {
                _tnRepository.Delete(tna);
            }
            return 1;
        }

        public int Delete(string orderStyleRefId, string compId)
        {
            return _tnRepository.Delete(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == compId);
        }

        public bool Exist(string orderStyleRefId, string compId)
        {
            return _tnRepository.Exists(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == compId);
        }

        public DataTable GetHorizontalTna(string orderNo, string orderStyleRefId, string buyerRefId, string compId)
        {

            return _tnRepository.GetHorizontalTna(orderNo, orderStyleRefId, buyerRefId, compId);
        }



        public dynamic GetTnaStatus(string compId, string indicationKey, string buyerRefId, string orderNo, string orderStyleRefId)
        {
            orderStyleRefId = orderStyleRefId ?? "";
            orderNo = orderNo ?? "";
            buyerRefId = buyerRefId ?? "";
            string range = "";
            string xalert = "-";
            switch (indicationKey)
            {
                case "Y":
                    range = " where DateDiffernce >= 4 ";
                    xalert = "'Having only '+ convert(varchar(10),DateDiffernce) + ' to begin' ";
                    break;
                case "A":
                    range = " where DateDiffernce >= 0 and DateDiffernce <4 ";
                    xalert = "'Having only ' + convert(varchar(10),DateDiffernce) +' to begin'";
                    break;
                case "R":
                    range = " where DateDiffernce <0 and DateDiffernce >-3 ";
                    xalert = "'Not started as per plan'";
                    break;
                case "D":
                    range = " where DateDiffernce <=-3 ";
                    xalert = "'running 3 or more days late'";
                    break;

            }
            range = range + " and " + "CompId='{0}' and ( BuyerRefId='{1}' or '{1}'='') and  (OrderStyleRefId='{2}' or '{2}'='') and (OrderRefId='{3}' or '{3}'='') ";
            string sql = String.Format(@"select *, " + xalert + " as Status  from vwTnaAlert " + range, compId, buyerRefId, orderStyleRefId, orderNo);
            DataTable tnaTable = _tnRepository.ExecuteQuery(sql);
            return tnaTable.Todynamic();
        }

        public DataTable GetBuyerWiseTnaAlert(string compId)
        {
            string sql = String.Format(@"select BuyerRefId, BuyerName, 
                        (select Count(*) as Y  from vwTnaAlert as A  where AlertType='Y' and CompId='{0}' and BuyerRefId= P.BuyerRefId ) as Yellow 
                        , (select Count(*) as Y  from vwTnaAlert as B  where AlertType='A' and CompId='{0}' and BuyerRefId= P.BuyerRefId ) as Amber 
                        , (select Count(*) as Y  from vwTnaAlert as B  where AlertType='R' and CompId='{0}' and BuyerRefId= P.BuyerRefId ) as Red
                        , (select Count(*) as Y  from vwTnaAlert as B  where AlertType='D' and CompId='{0}' and BuyerRefId= P.BuyerRefId ) as DoubleRed
                        From (select distinct BuyerRefId, BuyerName From vwTnaAlert) as P
                        order by p.BuyerName", compId);
            return _tnRepository.ExecuteQuery(sql);
        }

        public DataTable GetBuyerWiseActive(string compId, string buyerRefId, string alertType)
        {
            string sql = String.Format(@"SELECT      *
                                FROM            vwTnaAlert
                                WHERE        (BuyerRefId = '{0}') AND (AlertType = '{1}' and CompId='{2}')
                                ORDER BY DateDiffernce DESC", buyerRefId, alertType, compId);
            return _tnRepository.ExecuteQuery(sql);
        }

        public dynamic GetStyleWiseTna(string orderStyleRefId, string compId)
        {
            string sql = String.Format(@"EXEC SpMisGetStyleWiseTnaOperations @OrderStyleRefId='{0}',@CompId='{1}'", orderStyleRefId, compId);
            DataTable tnaTable = _tnRepository.ExecuteQuery(sql);
            return tnaTable.Todynamic();
        }

        public int CreateTnaByActivityTemplate(string orderStyleRefId)
        {
            List<OM_TnaActivity> tnaActivities = _tnaActivityRepository.Filter(x => x.XStatus == "g").ToList();
            List<OM_TNA> tnas = tnaActivities.Select(x => new OM_TNA()
            {
                SerialId = x.SlNo,
                ActivityName = x.Name,
                OrderStyleRefId = orderStyleRefId,
                ShortName = x.ShortName,
                Responsible = x.Responsible,
                CreatedBy = PortalContext.CurrentUser.UserId,
                CreatedDate = DateTime.Now,
                CompId = PortalContext.CurrentUser.CompId,

            }).ToList();
            return _tnRepository.SaveList(tnas);
        }

        public List<OM_TnaActivity> GetTaActivityList()
        {
            return _tnaActivityRepository.All().OrderBy(x => x.SlNo).ToList();
        }

        public IEnumerable<string> GetTnaRespobslibles(string compId)
        {
            return _tnaActivityRepository.All().Select(x => x.Responsible).Distinct();
        }

        public List<string> GetTnaActivity(string searchString)
        {
            return _tnaActivityRepository.Filter(x => x.Name.Replace(" ", "")
                .ToLower()
                .Contains(searchString.Replace(" ", "").ToLower())).Select(x => x.Name).Distinct().ToList();

        }

        public List<string> GetTnaResponsible(string searchString)
        {
            return _tnaActivityRepository.Filter(x => x.Responsible.Replace(" ", "")
                .ToLower()
                .Contains(searchString.Replace(" ", "").ToLower())).Select(x => x.Responsible).Distinct().ToList();
        }

        public int CreateTnaByBuyerTemplate(string compId, string buyerRefId, int defaultTemplate, string orderStyleRefId, DateTime orderDate)
        {

            return _buyerTnaTemplateRepository.CreateTnaByBuyerTemplate(compId, buyerRefId, defaultTemplate, orderStyleRefId, String.Format("{0:dd/MM/yyyy}", orderDate));
        }

        public int UpdateActualStartDate(string keyValue, DateTime actualStartDate, string orderStyleRefId, string compId)
        {
            int status = 0;
            string shortName = keyValue;
            string actualDate = actualStartDate.ToString("dd/M/yyyy");
            OM_TNA tNA = _tnRepository.FindOne(x => x.OrderStyleRefId == orderStyleRefId && x.ShortName == shortName && x.CompId == compId);
            if (tNA != null&&String.IsNullOrEmpty(tNA.ASDate))
            {
                tNA.ASDate = actualDate;
                status = _tnRepository.Edit(tNA);
            }
            return status;
        }

        public int UpdateActualEndDate(string shortName, DateTime? endDate, string orderStyleRefId, string compId)
        {
            int status = 0;
            if (endDate != null)
            {
                string actualDate = endDate.GetValueOrDefault().ToString("dd/M/yyyy");
                OM_TNA tNA = _tnRepository.FindOne(x => x.OrderStyleRefId == orderStyleRefId && x.ShortName == shortName && x.CompId == compId);
                if (tNA != null && String.IsNullOrEmpty(tNA.AEDate))
                {
                    tNA.AEDate = actualDate;
                    status = _tnRepository.Edit(tNA);
                }
            }
         
            return status;
        }

        public DataTable GetTnaActivityLog(long id, string keyName)
        {
            string sql = String.Format(@"select tn.ActivityName,al.ValueText AS [Last Update], FORMAT(al.EditedDate, 'dd/MM/yyyy, hh:mm:ss ') AS [Last Updated Daate],e.Name AS Employee,e.Department  from OM_TnaActivityLog AS al
                                inner join OM_TNA as tn on al.TnaId=tn.TnaRowId
                                inner join VEmployee as e on al.EditedBy=e.EmployeeId
                                where tn.TnaRowId={0} and KeyName='{1}'
                                order by al.EditedDate", id, keyName);
            return _tnRepository.ExecuteQuery(sql);
        }
    }
}
