using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.DAL.Repository.Planning;
using SCERP.Model;
using System.Linq;
using SCERP.Model.Planning;

namespace SCERP.BLL.Manager.PlanningManager
{
    public class TNAHorizontalManager : BaseManager, ITNAHorizontalManager
    {
        private readonly ITNAHorizontalRepository tnahorizentalRepository = null;

        public TNAHorizontalManager(ITNAHorizontalRepository tnahorizentalRepository)
        {
            this.tnahorizentalRepository = tnahorizentalRepository;
        }

        public List<PLAN_TNAHorizontal> GetAllTnaHorizontalByPaging(int startPage, int pageSize, out int totalRecords, PLAN_TNAHorizontal tna)
        {
            return tnahorizentalRepository.GetAllTnaHorizontalByPaging(startPage, pageSize, out totalRecords, tna);
        }
         
        public int SaveTnaHorizontal(PLAN_TNA Tna)
        {
            var result = 0;

            var exist = tnahorizentalRepository.Exists(p => p.OrderStyleRefId == Tna.OrderStyleRefId && p.ActivityId == Tna.ActivityId && p.IsActive);

            result = exist ? tnahorizentalRepository.Edit(Tna) : tnahorizentalRepository.Save(Tna);

            if (result > 0)
            {
                result = tnahorizentalRepository.SaveTnaHorizontal(Tna);
            }
            return result;
        }

        public PLAN_TNA GetTnaByRefActivity(string buyerOrderRef, int activityId)
        {
            return tnahorizentalRepository.GetTnaByRefActivity(buyerOrderRef, activityId);
        }

        public int DeleteTnaHorizontal(PLAN_TNA Tna)
        {
            return tnahorizentalRepository.DeleteTnaHorizontal(Tna);
        }

        public List<PLAN_TNAHorizontal> GetAllTnaUpdateHorizontalByPaging(int startPage, int pageSize, out int totalRecords, PLAN_TNAHorizontal tnaHorizonal)
        {
            return tnahorizentalRepository.GetAllTnaUpdateHorizontalByPaging(startPage, pageSize, out totalRecords, tnaHorizonal);
        }
    }
}