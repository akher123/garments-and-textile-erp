using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.DAL.Repository.Planning;
using SCERP.Model;
using SCERP.Model.InventoryModel;
using SCERP.Model.Planning;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.PlanningManager
{
    public class ProgramManager : IProgramManager
    {
        private readonly IProgramRepository _programRepository;
        private readonly IProgramDetailRepository _programDetail;
        private readonly string _compId;
        public ProgramManager(IProgramRepository programRepository, IProgramDetailRepository programDetailRepository)
        {
            _compId = PortalContext.CurrentUser.CompId;
            _programRepository = programRepository;
            _programDetail = programDetailRepository;
        }

        public string GetNewProgramRefId(string prefix,string processRefId)
        {
           // return _programRepository.GetNewProgramRefId(_compId);


            var refString = _programRepository.Filter(x => x.CompId == _compId && x.ProcessRefId == processRefId && x.ProgramRefId.Substring(0, prefix.Length) == prefix)
                    .Max(x => x.ProgramRefId.Substring(prefix.Length, 10)) ?? "0";
               var programRefId = prefix + refString.IncrementOne().PadZero(8);
            return programRefId;


        }

        public List<PLAN_Program> GetPrograms(string orderStyleRefId)
        {

            return _programRepository.Filter(x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId).ToList();
        }

        public int SaveProgram(PLAN_Program model, List<VProgramDetail> inPutProgramDetails)
        {
            model.CompId = _compId;
            model.xStatus = "R";
            model.PLAN_ProgramDetail = inPutProgramDetails.Select(x => new PLAN_ProgramDetail
            {
                CompId = _compId,
                PrgramRefId = model.ProgramRefId,
                ColorRefId = x.ColorRefId,
                SizeRefId = x.SizeRefId,
                MType = x.MType,
                ItemCode = x.ItemCode,

                Quantity = x.Quantity
            }).ToList();
            return _programRepository.Save(model);
        }

        public List<VProgramDetail> GetInPutProgramDetails(long programId)
        {
            return _programDetail.GetVProgramDetails(x => x.ProgramId == programId && x.MType == "I").ToList();
        }

        public List<VProgramDetail> GetOutPutProgramDetails(long programId)
        {
            return _programDetail.GetVProgramDetails(x => x.ProgramId == programId && x.MType == "O").ToList();
        }

        public PLAN_Program GetProgramById(long programId)
        {
            return _programRepository.FindOne(x => x.CompId == _compId && x.ProgramId == programId);
        }

        public bool IsExistProgram(string orderStyleRefId, string processRefId)
        {
            return
                _programRepository.Exists(
                    x => x.CompId == _compId && x.OrderStyleRefId == orderStyleRefId && x.ProcessRefId == processRefId);
        }

        public int EditProgram(PLAN_Program program, List<VProgramDetail> inPutProgramDetails)
        {
            int saveIndex = 0;
            using (var transaction = new TransactionScope())
            {
                var programObj =
                    _programRepository.FindOne(x => x.CompId == _compId && x.ProgramId == program.ProgramId && x.ProcessRefId == program.ProcessRefId && x.OrderStyleRefId == program.OrderStyleRefId);
                programObj.OrderStyleRefId = program.OrderStyleRefId;
                programObj.ExpDate = program.ExpDate;
                programObj.PrgDate = program.PrgDate;
                programObj.ProcessRefId = program.ProcessRefId;
                programObj.Rmks = program.Rmks;
                _programRepository.Edit(programObj);
                _programDetail.Delete(x => x.CompId == _compId && x.ProgramId == program.ProgramId && x.PrgramRefId == program.ProcessRefId);
                var programDetails = inPutProgramDetails.Select(x => new PLAN_ProgramDetail
                {
                    CompId = _compId,
                    PrgramRefId = program.ProgramRefId,
                    ColorRefId = x.ColorRefId,
                    SizeRefId = x.SizeRefId,
                    MType = x.MType,
                    ItemCode = x.ItemCode,
                    Quantity = x.Quantity
                }).ToList();
                saveIndex += _programDetail.SaveList(programDetails);
                transaction.Complete();
            }
            return saveIndex;
        }

        public List<VwProgram> GetVwProgramsByPaging(string processRefId, out int totalRecords)
        {
            var vwPrograms = _programRepository.GetVwProgramList(x => x.ProcessRefId == processRefId && x.CompId == _compId);
            totalRecords = vwPrograms.Count();
            return vwPrograms.ToList();

        }

        public decimal DeleteProgram(string programRefId, string orderStyleRefId)
        {
            int deletedRows = 0;
            using (var transaction = new TransactionScope())
            {
                deletedRows += _programDetail.Delete(x => x.CompId == _compId && x.PrgramRefId == programRefId);
                deletedRows += _programRepository.Delete(
                    x => x.CompId == _compId && x.ProgramRefId == programRefId);
                transaction.Complete();
            }
            return deletedRows;

        }

        public List<VwAssignedProgram> GetVwPrograms(string searchString)
        {
            string sewingProcessRefId = ProcessCode.SEWING;
            List<VwAssignedProgram> vwPrograms = _programRepository.GetVwAssignedProgramList(searchString, sewingProcessRefId, _compId);
            return vwPrograms.ToList();
        }

        public VwAssignedProgram GetAssignedProgram(string programRefId, string orderStyleRefId)
        {
            return _programRepository.GetAssignedProgram(programRefId, orderStyleRefId, _compId);
        }

        public List<PLAN_Program> GetProgramsByPatins(string searchString, out int totalRecords)
        {
            var programs = _programRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId);
            totalRecords = programs.Count();
            return programs.ToList();
        }

        public int SaveProgram(PLAN_Program program)
        {
            program.PLAN_ProgramDetail = program.PLAN_ProgramDetail.Select(x =>
            {
                if (x.MType == "I")
                {
                    x.YRatio = Convert.ToDouble(x.Quantity / program.PLAN_ProgramDetail.Where(I => I.MType == "I").Sum(I => I.Quantity));
                }
                return x;
            }).ToList();
            return _programRepository.Save(program);
        }

        public int EditProgram(PLAN_Program program)
        {
            int saveIndex = 0;
            using (var transaction = new TransactionScope())
            {
                var programObj = _programRepository.FindOne(x => x.CompId == _compId && x.ProgramId == program.ProgramId);
                programObj.ExpDate = program.ExpDate;
                programObj.PrgDate = program.PrgDate;
                programObj.ProcessRefId = program.ProcessRefId;
                programObj.Rmks = program.Rmks;
                programObj.BuyerRefId = program.BuyerRefId;
                programObj.OrderNo = program.OrderNo;
                programObj.OrderStyleRefId = program.OrderStyleRefId;
                programObj.ProcessRefId = program.ProcessRefId;
                programObj.ProcessorRefId = program.ProcessorRefId;
                programObj.PartyId = program.PartyId;
                programObj.CGRID = program.CGRID;
                programObj.CID = program.CID;
                programObj.Attention = program.Attention;
                programObj.ProgramType = program.ProgramType;
                _programRepository.Edit(programObj);
                _programDetail.Delete(x => x.CompId == _compId && x.ProgramId == program.ProgramId);
                program.PLAN_ProgramDetail = program.PLAN_ProgramDetail.Select(x =>
                {
                    if (x.MType == "I")
                    {
                        x.YRatio = Convert.ToDouble(x.Quantity / program.PLAN_ProgramDetail.Where(I => I.MType == "I").Sum(I => I.Quantity));
                    }
                    return x;
                }).ToList();
                saveIndex = _programDetail.SaveList(program.PLAN_ProgramDetail.ToList());
                transaction.Complete();
            }
            return saveIndex;
        }

        public List<VwProgram> GetVwProgramsPaging(string searchString, string processRefId, int pageIndex, DateTime? fromDate, DateTime? toDate, out int totalRecords)
        {
            var vwPrograms =
                _programRepository.GetVwProgramList(x => x.CompId == _compId && x.ProcessRefId == processRefId &&
                ((x.ProgramRefId.Contains(searchString) || String.IsNullOrEmpty(searchString)) || (x.StyleName.Contains(searchString) || String.IsNullOrEmpty(searchString))) && ((x.PrgDate >= fromDate || fromDate == null) && (x.ExpDate <= toDate || toDate == null)));
            var pageSize = AppConfig.PageSize;
            totalRecords = vwPrograms.Count();
            vwPrograms = vwPrograms
                    .OrderByDescending(r => r.ProgramId)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize);
            return vwPrograms.ToList();
        }

        public int DeleteProgramById(long programId)
        {
            int deletedRows = 0;
            using (var transaction = new TransactionScope())
            {
                deletedRows += _programDetail.Delete(x => x.CompId == _compId && x.ProgramId == programId);
                deletedRows += _programRepository.Delete(
                    x => x.CompId == _compId && x.ProgramId == programId);
                transaction.Complete();
            }
            return deletedRows;
        }

        public List<VwProgram> GetApprovedKnittingProgramByPaging(string porcessRefId, int pageIndex, string sort, string sortdir, bool isApproved, string compId, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var approvedKnittingProgramList = _programRepository.GetApprovedKnittingProgramList(porcessRefId, compId, isApproved);
            totalRecords = approvedKnittingProgramList.Count();
            switch (sort)
            {
                case "Buyer":
                    switch (sortdir)
                    {
                        case "DESC":
                            approvedKnittingProgramList = approvedKnittingProgramList
                                .OrderByDescending(r => r.ProgramId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            approvedKnittingProgramList = approvedKnittingProgramList
                                .OrderBy(r => r.ProgramId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    approvedKnittingProgramList = approvedKnittingProgramList
                        .OrderByDescending(r => r.ProgramId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return approvedKnittingProgramList.ToList();
        }

        public int ApprovedKnittingProgram(long programId, string compId)
        {
            PLAN_Program planProgram = _programRepository.FindOne(x => x.CompId == compId && x.ProgramId == programId);
            planProgram.IsApproved = planProgram.IsApproved != true;
            planProgram.ApprovedBy = planProgram.IsApproved == true ? PortalContext.CurrentUser.UserId : null;
            return _programRepository.Edit(planProgram);
        }

        public List<VwProgram> GetVProgramById(long programId)
        {
            var vwPrograms = _programRepository.GetVwProgramList(x => x.ProgramId == programId).ToList();
            return vwPrograms;
        }

        public List<VwProgram> GetKnittingProgramStatus(DateTime? fromDate, DateTime? toDate,string processRefId, long partyId, string searchString, string compId)
        {
            return _programRepository.GetKnittingProgramStatus(fromDate, toDate,processRefId, partyId, searchString, compId);
        }


        public IEnumerable GetProgramAutocomplite(string serachString,long partyId, string compId, string processRefId)
        {

            return _programRepository.GetVwProgramList(
                x =>
                    x.ProcessRefId == processRefId && (x.PartyId == partyId||partyId==0) && x.ProgramRefId.Contains(serachString) &&
                    x.CompId == compId).Take(15).OrderByDescending(x=>x.ProgramRefId)
                .Select(x => new
                {
                    ProgramRefId = x.ProgramRefId,
                    ProgramId = x.ProgramId,
                    PartyId=x.PartyId,
                    PartyName= x.PartyName,
                    BuyerRefId = x.BuyerRefId,
                    BuyerName= x.Buyer,
                    OrderNo=x.OrderNo,
                    OrderStyleRefId=x.OrderStyleRefId,
                    OrderName=x.RefNo,
                    StyleName=x.StyleName,
                    ProcessName= x.ProcessName
                }).ToList();
        }

        public DataTable GetPartyWiseKnittingBalance(string processorRefId, string compId, long partyId)
        {
            return _programRepository.GetPartyWiseKnittingBalance(processorRefId, compId, partyId);
        }

        public int LockedProgram(long programId, string compId)
        {
            PLAN_Program planProgram = _programRepository.FindOne(x => x.CompId == compId&&x.IsApproved && x.ProgramId == programId);
            planProgram.IsLock = planProgram.IsLock != true;
            return _programRepository.Edit(planProgram);
        }

        public List<VwProgram> GetLokedProgramByPaging(string serarchString, int pageIndex, string sort, string sortdir, bool locked, string compId,
            out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var approvedKnittingProgramList =
                _programRepository.GetVwProgramList(x => x.IsApproved && (x.ProgramRefId.Contains(serarchString)||serarchString==null) && x.IsLock == locked && x.CompId == compId);
            totalRecords = approvedKnittingProgramList.Count();
            switch (sort)
            {
                case "Buyer":
                    switch (sortdir)
                    {
                        case "DESC":
                            approvedKnittingProgramList = approvedKnittingProgramList
                                .OrderByDescending(r => r.ProgramId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            approvedKnittingProgramList = approvedKnittingProgramList
                                .OrderBy(r => r.ProgramId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    approvedKnittingProgramList = approvedKnittingProgramList
                        .OrderByDescending(r => r.ProgramId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return approvedKnittingProgramList.ToList();
        }

        public bool ProgramIsLoked(long programId)
        {
            return _programRepository.Exists(x => x.ProgramId == programId && x.IsLock == true);
        }

        public List<PLAN_Program> GetProgramsByProcessType(string pocessRefId, string compId)
        {
            return _programRepository.GetWithInclude(x => x.CompId == compId && (x.ProcessRefId == pocessRefId) && x.IsApproved, "Party")
                .OrderByDescending(x => x.ProgramId)
                .ToList();
        }

        public VwProgram GetProgramByProgramRefId(string piBookingRefId, string compId)
        {
            return _programRepository.GetVwProgramList(x => x.ProgramRefId == piBookingRefId && x.CompId == compId).FirstOrDefault();
        }

        public IEnumerable GetProgramAutocomplite(string serachString, string compId, string processRefId)
        {
            return _programRepository.GetWithInclude(x => x.ProcessRefId == processRefId  && x.ProgramRefId.Contains(serachString) && x.CompId == compId,"Party")

               .Select(x => new
               {
                   ProgramRefId = x.ProgramRefId,
                   ProgramId = x.ProgramId,
                   PartyName=x.Party.Name,
                   PartyId = x.PartyId

               }).ToList();
        }

        public IEnumerable GetProgramAutocomplite(string serachString, string compId)
        {

            return _programRepository.GetWithInclude(x => (x.ProcessRefId == "010" || x.ProcessRefId == "002" ) && x.ProgramRefId.Contains(serachString) && x.CompId == compId, "Party").Take(20)

               .Select(x => new
               {
                   ProgramRefId = x.ProgramRefId,
                   ProgramId = x.ProgramId,
                   PartyName = x.Party.Name,
                   PartyId = x.PartyId,
                   BuyerRefId=x.BuyerRefId,
                   OrderStyleRefId=x.OrderStyleRefId,
                   OrderNo=x.OrderNo,

               }).ToList();
        }

        public IEnumerable GetProgramAllAutocomplite(string serachString, string compId)
        {
            return _programRepository.GetWithInclude(x => (x.ProcessRefId == "010" || x.ProcessRefId == "002" || x.ProcessRefId == "009") && x.ProgramRefId.Contains(serachString) && x.CompId == compId, "Party").Take(20)

               .Select(x => new
               {
                   ProgramRefId = x.ProgramRefId,
                   ProgramId = x.ProgramId,
                   PartyName = x.Party.Name,
                   PartyId = x.PartyId,
                   BuyerRefId = x.BuyerRefId,
                   OrderStyleRefId = x.OrderStyleRefId,
                   OrderNo = x.OrderNo,

               }).ToList();
        }

        public IEnumerable GetProgramCollarCuffAutocomplite(string serachString, string compId)
        {

            return _programRepository.GetWithInclude(x => (x.ProcessRefId == "009") && x.ProgramRefId.Contains(serachString) && x.CompId == compId, "Party").Take(20)

               .Select(x => new
               {
                   ProgramRefId = x.ProgramRefId,
                   ProgramId = x.ProgramId,
                   PartyName = x.Party.Name,
                   PartyId = x.PartyId,
                   BuyerRefId = x.BuyerRefId,
                   OrderStyleRefId = x.OrderStyleRefId,
                   OrderNo = x.OrderNo,

               }).ToList();
        }

        public List<Dropdown> GetConumptionFabrics(string orderStyleRefId,string colorRefId,string compId)
        {
            return _programRepository.GetConumptionFabrics(orderStyleRefId,colorRefId,compId);
        }

        public List<Dropdown> GetFabricColorNameByStyle(string orderStyleRefId, string compId)
        {
            return _programRepository.GetFabricColorNameByStyle(orderStyleRefId, compId);
        }

        public int UpdateProgramRate(List<VProgramDetail> values)
        {
            int updated = 0;
            foreach (var item in values)
            {
                var prgDtl =_programDetail.FindOne(x => x.ProgramDetailId == item.ProgramDetailId);
                prgDtl.Rate = item.Rate;
                updated+=_programDetail.Edit(prgDtl);
            }
            return updated;
        }

        public List<ProgramYarnRetur> GetProgramYarnReturn(long programId)
        {
            return _programRepository.GetProgramYarnReturn(programId);
        }
    }
}
