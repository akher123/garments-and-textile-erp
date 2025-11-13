using SCERP.BLL.IManager.ICommercialManager;
using SCERP.Common;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.Model.CommercialModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.Manager.CommercialManager
{
    public class LCBBLCInfoDataManager : ILCBBLCInfoDataManager
    {

        private ILCBBLCInfoDataRepository _lCBBLCInfoDataRepository;

        public LCBBLCInfoDataManager(ILCBBLCInfoDataRepository lCBBLCInfoDataRepository)
        {
            _lCBBLCInfoDataRepository = lCBBLCInfoDataRepository;
        }

        public dynamic GetStyleWiseTna(string orderStyleRefId, string compId)
        {
            string sql = String.Format(@"select OM_TNA.TnaRowId, VOM_BuyOrdStyle.BuyerName,VOM_BuyOrdStyle.RefNo as OrderNo,VOM_BuyOrdStyle.StyleName,Quantity,VOM_BuyOrdStyle.Merchandiser,OM_TNA.SerialId,OM_TNA.ActivityName,OM_TNA.PSDate,OM_TNA.PEDate,OM_TNA.Rmks,OM_TNA.XWho,OM_TNA.XWhen,Convert(varchar,OM_TNA.ASDate,103) as ASDate,Convert(varchar,OM_TNA.AEDate,101) as AEDate,OM_TNA.Responsible,OM_TNA.UpdateRemarks from OM_TNA
                        inner join VOM_BuyOrdStyle on OM_TNA.OrderStyleRefId=VOM_BuyOrdStyle.OrderStyleRefId and OM_TNA.CompId=VOM_BuyOrdStyle.CompId
                        where OM_TNA.OrderStyleRefId='{0}' and OM_TNA.CompId='{1}' order by OM_TNA.SerialId ", orderStyleRefId, compId);
            DataTable tnaTable = _lCBBLCInfoDataRepository.ExecuteQuery(sql);
            return tnaTable.Todynamic();
        }

        public int UpdateTna(string compId, int tnaRowId, string key, string value)
        {
            return _lCBBLCInfoDataRepository.UpdateTna(compId, tnaRowId, key, value);
        }

        public int AddRows(int rowNumber, string orderStyleRefId, string compId)
        {
            List<CommLCBBLCInfoData> teList = new List<CommLCBBLCInfoData>();
            Guid userId = PortalContext.CurrentUser.UserId.GetValueOrDefault();

            for (int i = 1; i <= rowNumber; i++)
            {
                teList.Add(new CommLCBBLCInfoData()
                {
                    //SerialId = i,
                    //OrderStyleRefId = orderStyleRefId,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    CompId = compId
                });
            }
            return _lCBBLCInfoDataRepository.SaveList(teList);


        }

        public DataTable GetTnAReport(string orderStyleRefId, string compId)
        {
            return _lCBBLCInfoDataRepository.ExecuteQuery(String.Format("exec spTnAReport '{0}','{1}'", compId, orderStyleRefId));
        }

        public bool IsExistTnA(string copyOrderStyleRefId)
        {
            return _lCBBLCInfoDataRepository.Exists(x => x.BBLCNo == copyOrderStyleRefId);
        }

        public bool TnACopyAndPast(string orderStyleRefId, string copyOrderStyleRefId)
        {
            List<CommLCBBLCInfoData> teList = _lCBBLCInfoDataRepository.Filter(x => x.BBLCNo == orderStyleRefId).ToList();
            teList = teList.Select(x =>
            {
                //x.OrderStyleRefId = copyOrderStyleRefId;
                x.CreatedDate = DateTime.Now;
                return x;
            }).ToList();
            return _lCBBLCInfoDataRepository.SaveList(teList) > 0;

        }

        public int RemoveRow(int rowNumber, string orderStyleRefId, string compId)
        {
            List<CommLCBBLCInfoData> teList = _lCBBLCInfoDataRepository.Filter(x => x.BBLCNo == orderStyleRefId && x.LCBBLCInfoDataId == rowNumber).ToList();

            foreach (CommLCBBLCInfoData tna in teList)
            {
                _lCBBLCInfoDataRepository.Delete(tna);
            }
            return 1;
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
            DataTable tnaTable = _lCBBLCInfoDataRepository.ExecuteQuery(sql);
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
            return _lCBBLCInfoDataRepository.ExecuteQuery(sql);
        }

        public DataTable GetBuyerWiseActive(string compId, string buyerRefId, string alertType)
        {
            string sql = String.Format(@"SELECT      *
                                FROM            vwTnaAlert
                                WHERE        (BuyerRefId = '{0}') AND (AlertType = '{1}' and CompId='{2}')
                                ORDER BY DateDiffernce DESC", buyerRefId, alertType, compId);
            return _lCBBLCInfoDataRepository.ExecuteQuery(sql);
        }
    }
}
