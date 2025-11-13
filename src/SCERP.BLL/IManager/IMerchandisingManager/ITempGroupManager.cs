using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
   public interface ITempGroupManager
    {
       List<OM_TempGroup> GetTempGroupByPaging(string tempGroupName, int pageIndex, string sort, string sortdir, out int totalRecords);
       OM_TempGroup GeTempGroupById(int tempGroupId, string compId);
       int DeleteTemplateGroup(int tempGroupId, string compId);
       bool IsTempGroupExist(OM_TempGroup model);
       int EditTempGroup(OM_TempGroup model);
       int SaveTempGroup(OM_TempGroup model);
       List<OM_TempGroup> GetAllTempGroup(string compId);
    }
}
