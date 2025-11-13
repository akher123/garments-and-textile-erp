using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class CuttingSequenceManager : ICuttingSequenceManager
    {
        private readonly ICuttingSequenceRepository _cuttingSequenceRepository;

        public CuttingSequenceManager(ICuttingSequenceRepository cuttingSequenceRepository)
        {
            _cuttingSequenceRepository = cuttingSequenceRepository;
        }
        public string GetNewComponentRefId()
        {
            var maxRefId = _cuttingSequenceRepository.Filter(x=>x.CompId==PortalContext.CurrentUser.CompId).Max(x => x.CuttingSequenceRefId);
            return maxRefId.IncrementOne().PadZero(8);
        }

        public int SaveCuttingSequenceLis(List<PROD_CuttingSequence> cuttingSequences, long cuttingSequenceId, string colorRefId)
        {
            int saveIndex = 0;
            #region
            //if (cuttingSequences.Any())
            //{
            //    using (var transaction =new TransactionScope())
            //    {
            //        var cutting = cuttingSequences.FirstOrDefault();

            //        var cuttingList =
            //            _cuttingSequenceRepository.Filter(
            //                x =>
            //                    x.CompId == PortalContext.CurrentUser.CompId && x.OrderStyleRefId == cutting.OrderStyleRefId &&
            //                    x.OrderNo == cutting.OrderNo && x.BuyerRefId == cutting.BuyerRefId && (x.ColorRefId == colorRefId || colorRefId == "0000")).ToList();
            //        foreach (var sequence in cuttingList)
            //        {
            //            _cuttingSequenceRepository.Delete(x => x.CuttingSequenceId == sequence.CuttingSequenceId);
            //        }
            //        saveIndex = _cuttingSequenceRepository.SaveList(cuttingSequences); 
            //        transaction.Complete();
            //    }

            //}
            #endregion
            foreach (var cuttingSequence in cuttingSequences)
            {
                if (cuttingSequence.CuttingSequenceId>0)
                {
                    bool isExist =_cuttingSequenceRepository.Exists(x => x.CuttingSequenceId == cuttingSequence.CuttingSequenceId);
                    if (isExist)
                    {
                       var cutSeq= _cuttingSequenceRepository.FindOne(x => x.CuttingSequenceId == cuttingSequence.CuttingSequenceId);
                        cutSeq.BuyerRefId = cuttingSequence.BuyerRefId;
                        cutSeq.OrderNo = cuttingSequence.OrderNo;
                        cutSeq.OrderStyleRefId = cuttingSequence.OrderStyleRefId;
                        cutSeq.ColorRefId = cuttingSequence.ColorRefId;
                        cutSeq.ComponentRefId = cuttingSequence.ComponentRefId;
                        cutSeq.SlNo = cuttingSequence.SlNo;
                       saveIndex+= _cuttingSequenceRepository.Edit(cutSeq);
                    }
                }
                else
                {
                    saveIndex += _cuttingSequenceRepository.Save(cuttingSequence);
                }
              
           
            }
       
            return saveIndex;

        }

        public IEnumerable GetComponentsByColor(string colorRefId, string orderStyleRefId)
        {
            return _cuttingSequenceRepository.GetComponentsByColor(colorRefId, orderStyleRefId, PortalContext.CurrentUser.CompId);
        }

       

        public List<VwCuttingSequence> GetCuttingSequenceByParam(string colorRefId,  string orderNo, string buyerRefId,
            string orderStyleRefId)
        {
            string compId = PortalContext.CurrentUser.CompId;
          return  _cuttingSequenceRepository.GetCuttingSequenceByParam(compId,  colorRefId, orderNo, buyerRefId,
                orderStyleRefId);
        }

        public List<VwCuttingSequence> GetCuttingSequenceByPaging(int pageIndex, string sort, string sortdir, out int totalRecords, string colorRefId,
            string orderNo, string buyerRefId, string orderStyleRefId)
        {
           
            string compId = PortalContext.CurrentUser.CompId;
            List<VwCuttingSequence> cuttingSequenceList = _cuttingSequenceRepository.GetCuttingSequenceByPaging(compId, colorRefId, orderNo, buyerRefId,orderStyleRefId);
            totalRecords = cuttingSequenceList.Count();
            return cuttingSequenceList.ToList();
        }

        public IEnumerable GetCuttingSequenceOrderStyle(string orderStyleRefId, string orderNo)
        {
          return  _cuttingSequenceRepository.GetCuttingSequenceOrderStyle(PortalContext.CurrentUser.CompId, orderStyleRefId,
                orderNo);
        }

        public int DeleteCuttingSequence(long cuttingSequenceId)
        {
            return _cuttingSequenceRepository.Delete(x => x.CuttingSequenceId == cuttingSequenceId);
        }

       
    }
}
