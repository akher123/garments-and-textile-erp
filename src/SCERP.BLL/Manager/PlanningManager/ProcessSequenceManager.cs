using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.PlanningManager
{
    public class ProcessSequenceManager : IProcessSequenceManager
    {
        private readonly IProcessSequenceRepository _processSequenceRepository;
        private readonly IYarnConsumptionRepository _yarnConsumptionRepository;
        private readonly ICompConsumptionDetailRepository _compConsumptionDetailRepository;
        private readonly IBuyOrdShipDetailRepository _ordShipDetail;
        private readonly IProcessSequenceDefaultRepository _processSequenceDefaultRepository;

        private readonly string _compId;
        public ProcessSequenceManager(IProcessSequenceRepository processSequenceRepository
            , IYarnConsumptionRepository yarnConsumptionRepository
            , ICompConsumptionDetailRepository compConsumptionDetailRepository
            , IBuyOrdShipDetailRepository ordShipDetail
            , IProcessSequenceDefaultRepository processSequenceDefaultRepository)
        {
            _ordShipDetail = ordShipDetail;
            _compId = PortalContext.CurrentUser.CompId;
            _yarnConsumptionRepository = yarnConsumptionRepository;
            _processSequenceRepository = processSequenceRepository;
            _processSequenceDefaultRepository = processSequenceDefaultRepository;
            _compConsumptionDetailRepository = compConsumptionDetailRepository;
        }

        public List<VProcessSequence> GetProcessSequence(string orderStyleRefId)
        {


            return _processSequenceRepository.GetProcessSequence(_compId, orderStyleRefId);
        }

        public PLAN_ProcessSequence GetProcessSequenceById(long processSequenceId, string orderStyleRefId)
        {
            return _processSequenceRepository.FindOne(x => x.ProcessSequenceId == processSequenceId && x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
        }

        public int SaveProcessSquence(PLAN_ProcessSequence model)
        {
            model.CompId = _compId;
            return _processSequenceRepository.Save(model);
        }

        public int DeleteProcessSequence(long processSequenceId, string orderStyleRefId)
        {
            var editIndex = 0;
            using (var transaction = new TransactionScope())
            {
                var deleteRows = _processSequenceRepository.Delete(x => x.ProcessSequenceId == processSequenceId && x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId);
                var psList =
                    _processSequenceRepository.Filter((x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId)).OrderBy(x => x.ProcessRow);
                var index = 1;
                foreach (var sequence in psList)
                {
                    sequence.ProcessRow = index;
                    editIndex += _processSequenceRepository.Edit(sequence);
                    index++;
                }
                transaction.Complete();
            }

            return editIndex;
        }

        public int GetProcessRow(string orderStyleRefId)
        {
            return (_processSequenceRepository.Filter(x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId).Max(x => (int?)x.ProcessRow) ?? 0) + 1;
        }

        public int SaveDefaultProcessSquence(PLAN_ProcessSequence model)
        {
            var effRows = 0;
            using (var transaction = new TransactionScope())
            {

                effRows = _processSequenceRepository.Delete(
                     x => x.CompId == _compId && x.OrderStyleRefId == model.OrderStyleRefId);
                var psLsit =
                    _processSequenceDefaultRepository.Filter(x => x.CompId == _compId).ToList()
                        .Select(x => new PLAN_ProcessSequence()
                        {
                            CompId = x.CompId,
                            ProcessRow = x.ProcessRow,
                            ProcessRefId = x.ProcessRefId,
                            OrderStyleRefId = model.OrderStyleRefId,

                        }).ToList();
                effRows += _processSequenceRepository.SaveList(psLsit);
                transaction.Complete();
            }
            return effRows;
        }

        public List<VProgramDetail> GetInPutProgramDetails(string orderStyleRefId, string processRefId)
        {
            var programDetails = new List<VProgramDetail>();
            switch (processRefId)
            {
                case ProcessType.KNITTING:   //KNITTINNG
                    programDetails = _yarnConsumptionRepository.GetVYarnConsumptions(
                         x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId).ToList().Select(x => new VProgramDetail()
                         {
                             ColorRefId = x.KColorRefId,
                             SizeRefId = x.KSizeRefId,
                             Quantity = x.KQty.GetValueOrDefault(),
                             ItemCode = x.ItemCode,
                             ItemName = x.ItemName,
                             SizeName = x.KSizeName,
                             ColorName = x.GColorName,
                             UnitName = x.UnitName,
                             MType = "I"
                         }).ToList();
                    break;
                case ProcessType.DYEING: //DYEING
                    var comConsDetails = _compConsumptionDetailRepository.GetVCompConsumptionDetails(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
                    programDetails = comConsDetails.GroupBy(x => new { x.ConsRefId, x.OrderStyleRefId, x.GColorRefId }).ToList()
                          .Select(g => new VProgramDetail()
                          {
                              Quantity = g.Sum(x => x.TQty.GetValueOrDefault()),
                              ItemCode = g.First().ItemCode,
                              ItemName = g.First().ItemName,
                              ColorName = g.First().GColorName,
                              ColorRefId = g.First().GColorRefId,
                              SizeName = g.First().GSizeName,
                              SizeRefId = g.First().GSizeRefId,
                              UnitName = g.First().UnitName,
                              MType = "I"
                          }).ToList();
                    break;
                case ProcessType.CUTTING://CUTTING
                    var finishFabricList = _compConsumptionDetailRepository.GetVCompConsumptionDetails(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
                    programDetails = finishFabricList.GroupBy(x => new { x.ConsRefId, x.OrderStyleRefId, x.PColorRefId }).ToList()
                   .Select(g => new VProgramDetail()
                   {
                       Quantity = g.Sum(x => x.TQty.GetValueOrDefault()),
                       ItemCode = g.First().ItemCode,
                       ItemName = g.First().ItemName,
                       ColorName = g.First().PColorName,
                       ColorRefId = g.First().PColorRefId,
                       SizeRefId = g.First().PSizeRefId,
                       SizeName = g.First().PSizeName,
                       UnitName = g.First().UnitName,
                       MType = "I"

                   }).ToList();
                    break;
                case ProcessType.SEWING://SEWING
                    var sewInputList = _ordShipDetail.GetVOrderStyleDetails(x => x.CompId == _compId & x.OrderStyleRefId == orderStyleRefId).ToList().Select(x => new VProgramDetail()
                   {
                       Quantity = x.Quantity,
                       ItemCode = x.ItemCode,
                       ItemName = x.ItemName,
                       ColorName = x.ColorName,
                       ColorRefId = x.ColorRefId,
                       SizeName = x.SizeName,
                       SizeRefId = x.SizeRefId,
                       UnitName = "PICES",
                       MType = "I"

                   }).ToList();
                    programDetails = sewInputList;
                    break;
                case ProcessType.FINISHING://FINISHING
                    var ffInputList = _ordShipDetail.GetVOrderStyleDetails(x => x.CompId == _compId & x.OrderStyleRefId == orderStyleRefId).ToList().Select(x => new VProgramDetail()
                    {
                        Quantity = x.Quantity,
                        ItemCode = x.ItemCode,
                        ItemName = x.ItemName,
                        ColorName = x.ColorName,
                        ColorRefId = x.ColorRefId,
                        SizeName = x.SizeName,
                        SizeRefId = x.SizeRefId,
                        UnitName = "PICES",
                        MType = "I"

                    }).ToList();
                    programDetails = ffInputList;
                    break;

            }
            return programDetails;
        }

        public List<VProgramDetail> GetOutPutProgramDetails(string orderStyleRefId, string processRefId)
        {

            var programDetails = new List<VProgramDetail>();
            switch (processRefId)
            {
                case ProcessType.KNITTING:
                    var comConsDetails = _compConsumptionDetailRepository.GetVCompConsumptionDetails(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
                    programDetails = comConsDetails.GroupBy(x => new { x.ConsRefId, x.OrderStyleRefId, x.GColorRefId }).ToList()
                          .Select(g => new VProgramDetail()
                          {
                              Quantity = g.Sum(x => x.TQty.GetValueOrDefault()),
                              ItemCode = g.First().ItemCode,
                              ItemName = g.First().ItemName,
                              ColorName = g.First().GColorName,
                              ColorRefId = g.First().GColorRefId,
                              SizeName = g.First().GSizeName,
                              SizeRefId = g.First().GSizeRefId,
                              UnitName = g.First().UnitName,
                              MType = "O"

                          }).ToList();
                    break;
                case ProcessType.DYEING:
                    var finishFabricList = _compConsumptionDetailRepository.GetVCompConsumptionDetails(x => x.OrderStyleRefId == orderStyleRefId && x.CompId == _compId);
                    programDetails = finishFabricList.GroupBy(x => new { x.ConsRefId, x.OrderStyleRefId, x.PColorRefId }).ToList()
                   .Select(g => new VProgramDetail()
                   {
                       Quantity = g.Sum(x => x.TQty.GetValueOrDefault()),
                       ItemCode = g.First().ItemCode,
                       ItemName = g.First().ItemName,
                       ColorName = g.First().PColorName,
                       ColorRefId = g.First().PColorRefId,
                       SizeRefId = g.First().PSizeRefId,
                       SizeName = g.First().PSizeName,
                       MType = "O",
                       UnitName = g.First().UnitName,

                   }).ToList();
                    break;
                case ProcessType.CUTTING:
                    var cuttingOutputList = _ordShipDetail.GetVOrderStyleDetails(x => x.CompId == _compId & x.OrderStyleRefId == orderStyleRefId).ToList().Select(x => new VProgramDetail()
                  {
                      Quantity = x.Quantity,
                      ItemCode = x.ItemCode,
                      ItemName = x.ItemName,
                      ColorName = x.ColorName,
                      SizeName = x.SizeName,
                      SizeRefId = x.SizeRefId,
                      ColorRefId = x.ColorRefId,
                      UnitName = "PICES",
                      MType = "O"

                  }).ToList();
                    programDetails = cuttingOutputList;
                    break;
                case ProcessType.SEWING:
                    var sewOutputList = _ordShipDetail.GetVOrderStyleDetails(x => x.CompId == _compId & x.OrderStyleRefId == orderStyleRefId).ToList().Select(x => new VProgramDetail()
                    {
                        Quantity = x.Quantity,
                        ItemCode = x.ItemCode,
                        ItemName = x.ItemName,
                        ColorName = x.ColorName,
                        SizeName = x.SizeName,
                        SizeRefId = x.SizeRefId,
                        ColorRefId = x.ColorRefId,
                        UnitName = "PICES",
                        MType = "O"

                    }).ToList();
                    programDetails = sewOutputList;
                    break;
                case ProcessType.FINISHING:
                    var ffOutputList = _ordShipDetail.GetVOrderStyleDetails(x => x.CompId == _compId & x.OrderStyleRefId == orderStyleRefId).ToList().Select(x => new VProgramDetail()
                    {
                        Quantity = x.Quantity,
                        ItemCode = x.ItemCode,
                        ItemName = x.ItemName,
                        SizeName = x.SizeName,
                        SizeRefId = x.SizeRefId,
                        ColorName = x.ColorName,
                        ColorRefId = x.ColorRefId,
                        UnitName = "PICES",
                        MType = "O"

                    }).ToList();
                    programDetails = ffOutputList;
                    break;

            }
            return programDetails;

        }



    }
}
