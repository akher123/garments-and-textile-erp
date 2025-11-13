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
    public class CommTNAManager : ICommTNAManager
    {

        private readonly ICommTNARepository _commTNARepository;
        private readonly string _compId;
        public CommTNAManager(ICommTNARepository commTNARepository)
        {
            _commTNARepository = commTNARepository;
            _compId = PortalContext.CurrentUser.CompId;
        }


        public int AddRows(int rowNumber, int LCNoRefId, string compId)
        {
            List<CommTNA> teList = new List<CommTNA>();
            Guid userId = PortalContext.CurrentUser.UserId.GetValueOrDefault();

            for (int i = 1; i <= rowNumber; i++)
            {
                teList.Add(new CommTNA()
                {
                    SerialId = i,
                    LCRefId = LCNoRefId,
                    CompId = compId
                });
            }
            return _commTNARepository.SaveList(teList);


        }

        public int RemoveRow(int rowNumber, int LCNoRefId, string compId)
        {
            List<CommTNA> teList = _commTNARepository.Filter(x => x.LCRefId == LCNoRefId && x.SerialId == rowNumber).ToList();

            foreach (CommTNA tna in teList)
            {
                _commTNARepository.Delete(tna);
            }
            return 1;
        }

        public int CreateTnaByActivityTemplate(string orderStyleRefId)
        {
            throw new NotImplementedException();
        }

        public int Delete(string orderStyleRefId, string compId)
        {
            throw new NotImplementedException();
        }

        public bool Exist(string orderStyleRefId, string compId)
        {
            throw new NotImplementedException();
        }

        public DataTable GetBuyerWiseActive(string compId, string buyerRefId, string alertType)
        {
            throw new NotImplementedException();
        }

        public DataTable GetBuyerWiseTnaAlert(string compId)
        {
            throw new NotImplementedException();
        }

        public List<CommTNA> GetLCWiseTna(int LCNoRefId)
        {
            return _commTNARepository.Filter(x => x.LCRefId == LCNoRefId).ToList();
        }

        public dynamic GetLCWiseTna(string orderStyleRefId, string compId)
        {
            throw new NotImplementedException();
        }

        public DataTable GetTnAReport(string orderStyleRefId, string compId)
        {
            throw new NotImplementedException();
        }

        public dynamic GetTnaStatus(string compId, string indicationKey, string buyerRefId, string orderNo, string orderStyleRefId)
        {
            throw new NotImplementedException();
        }

        public bool IsExistTnA(string copyOrderStyleRefId)
        {
            throw new NotImplementedException();
        }

       

        public bool TnACopyAndPast(string orderStyleRefId, string copyOrderStyleRefId)
        {
            throw new NotImplementedException();
        }

        public int UpdateTna(string compId, int commTnaRowId, string key, string value)
        {
            return _commTNARepository.UpdateTna(compId, commTnaRowId, key, value);
        }
    }
}
