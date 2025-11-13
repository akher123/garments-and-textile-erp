using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.Maintenance;

namespace SCERP.BLL.IManager.IMaintenance
{
   public interface IReturnableChallanManager  
    {
       List<Maintenance_ReturnableChallan> GetAllReturnableChallanByPaging(int pageIndex, string sort, string sortdir, DateTime? dateFrom, DateTime? dateTo, int challanStatus, string searchString, string compId, string challanType, out int totalRecords);
       Maintenance_ReturnableChallan GetReturnableChallanByReturnableChallanId(long returnableChallanId, string compId);
       int EditReturnableChallan(Maintenance_ReturnableChallan model);
       int SaveReturnableChallan(Maintenance_ReturnableChallan model);
       int DeleteReturnableChallan(long returnableChallanId, string compId);
       List<VwReturnableChallan> GetReturnableChallanForReport(long returnableChallanId, string compId);
       string GetReturnableChallanRefId(string challanType, string preefix);
       Maintenance_ReturnableChallan GetReturnableChallanByReturnableChallanRefId(string returnableChallanRefId, string compId);
       object GetRefNoBySearchCharacter(string searchCharacter,string challanType);
       List<Maintenance_ReturnableChallan> GetApprovedReturnableChallanByPaging(int pageIndex, string sort, string sortdir, bool? isApproved, string compId,string challanType, out int totalRecords);
       int ApprovedReturnableChallan(long returnableChallanId, string compId);
       DataTable GetReturnableChallanInfo(DateTime? dateFrom, DateTime? dateTo, string challanType, int challanStatus, string compId); 
    }
}
