using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Planning;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IPlanningRepository
{
    public interface ITNAHorizontalRepository : IRepository<PLAN_TNA>
    {
        List<PLAN_TNAHorizontal> GetAllTnaHorizontalByPaging(int startPage, int pageSize, out int totalRecords, PLAN_TNAHorizontal tna);
        List<PLAN_TNAHorizontal> GetAllTnaUpdateHorizontalByPaging(int startPage, int pageSize, out int totalRecords, PLAN_TNAHorizontal tnaHorizonal);
        int SaveTnaHorizontal(PLAN_TNA Tna);
        PLAN_TNA GetTnaByRefActivity(string buyerOrderRef, int activityId);
        int DeleteTnaHorizontal(PLAN_TNA Tna);
    }
}
