using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IEmployeeCompanyInfoRepository : IRepository<EmployeeCompanyInfo>
    {
        EmployeeCompanyInfo GetEmployeeLatestCompanyInfoByEmployeeGuidId(EmployeeCompanyInfo employeeCompanyInfo);

        List<EmployeeCompanyInfo> GetEmployeeCompanyInfosByEmployeeGuidId(EmployeeCompanyInfo employeeCompanyInfo);

        IList<EmployeeCompanyInfo> GetEmployeCompanyInfos(SearchFieldModel searchFieldModel);

        VEmployeeCompanyInfoDetail GetEmployeeCompanyInfoByEmployeeCardId(string employeeCardId);

        EmployeeCompanyInfo GetEmployeeCompanyInfoById(Guid? employeeId, int id);

        int UpdateEmployeeCompanyInfoDate(EmployeeCompanyInfo employeeCompanyInfo);

        EmployeeCompanyInfo GetEmployeeLatestJobInfoByEmployeeId(EmployeeCompanyInfo employeeCompanyInfo);

        List<VEmployeeCompanyInfoDetail> AutocompliteGetEmployeeInfo(string employeeCardId, int? branchUnitDepartmentId);

        List<EmployeeCompanyInfoModel> GetEmployeesForIndividualHoliday(EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel);

        List<EmployeeCompanyInfoModel> GetEmployeesLatestCompanyInfo(int startPage, int pageSize, EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel);

        List<EmployeeCompanyInfoModel> GetEmployeeIndividualHoliday(int startPage, int pageSize, EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel);

        List<EmployeeCompanyInfoModel> GetEmployeesForAssigingLine(EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel);

        int AssignBulkEmployeeLine(SearchFieldModel searchFieldModel, EmployeeCompanyInfoModel model);

        List<EmployeeCompanyInfoModel> GetEmployeesForAssigingJobType(EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel);

        int AssignBulkEmployeeJobType(SearchFieldModel searchFieldModel, EmployeeCompanyInfoModel model);

        VEmployeeCompanyInfoDetail GetEmployeeByEmployeeId(Guid employeeId);

        int AssignIndividualHoliday(SearchFieldModel searchFieldModel, EmployeeCompanyInfoModel model);

        int AssignIndividualWeekend(SearchFieldModel searchFieldModel, EmployeeCompanyInfoModel model);
    }
}
