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
    public interface ITNAHorizontalManager
    {
        List<PLAN_TNAHorizontal> GetAllTnaHorizontalByPaging(int startPage, int pageSize, out int totalRecords, PLAN_TNAHorizontal tna);
        int SaveTnaHorizontal(PLAN_TNA Tna);
        PLAN_TNA GetTnaByRefActivity(string buyerOrderRef, int activityId);
        int DeleteTnaHorizontal(PLAN_TNA Tna);
        List<PLAN_TNAHorizontal> GetAllTnaUpdateHorizontalByPaging(int startPage, int pageSize, out int totalRecords, PLAN_TNAHorizontal tnaHorizonal);
    }
}
