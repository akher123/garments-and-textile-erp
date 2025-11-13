using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class GroupSubProcessManager : IGroupSubProcessManager
    {
        private readonly IGroupSubProcessRepository _groupSubProcessRepository;

        public GroupSubProcessManager(IGroupSubProcessRepository groupSubProcessRepository)
        {
            _groupSubProcessRepository = groupSubProcessRepository;
        }

        public List<PROD_GroupSubProcess> GetAllGroupSubProcess(string compId)
        {
            return _groupSubProcessRepository.Filter(x => x.CompId == compId).OrderBy(x=>x.GroupName).ToList();
        }

        public List<PROD_GroupSubProcess> GetGroupSubProcessByPaging(int pageIndex, string searchString, string compId, out int totalRecords)
        {

            var pageSize = AppConfig.PageSize;
            var groupSps = _groupSubProcessRepository.Filter(
                 x =>
                     x.CompId == compId &&
                     (x.GroupName.ToLower().Contains(searchString.ToLower()) || String.IsNullOrEmpty(searchString)));
            totalRecords = groupSps.Count();
            groupSps = groupSps.OrderByDescending(x => x.GroupSubProcessId).Skip(pageIndex * pageSize).Take(pageSize);
            return groupSps.ToList();
        }

        public int SaveGroupSubProcess(PROD_GroupSubProcess groupSubProcess)
        {
            groupSubProcess.SpCode = "0";
            groupSubProcess.CompId = PortalContext.CurrentUser.CompId;
            groupSubProcess.GroupType = "EX";
            return _groupSubProcessRepository.Save(groupSubProcess);

        }

        public int EditGroupSubProcess(PROD_GroupSubProcess groupSubProcess)
        {
            var groupSp = _groupSubProcessRepository.FindOne(x => x.GroupSubProcessId == groupSubProcess.GroupSubProcessId);
            groupSp.GroupName = groupSubProcess.GroupName;
            return _groupSubProcessRepository.Edit(groupSp);
        }

        public PROD_GroupSubProcess GetGroupSubProcessById(int groupSubProcessId)
        {
            return _groupSubProcessRepository.FindOne(x => x.GroupSubProcessId == groupSubProcessId);
        }

        public int DeleteGroupSubProcess(int groupSubProcessId)
        {
          return  _groupSubProcessRepository.Delete(x => x.GroupSubProcessId == groupSubProcessId);
        }
    }
}
