using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IEmployeeCompanyInfoManager
    {
        EmployeeCompanyInfo GetEmployeeLatestCompanyInfoByEmployeeGuidId(EmployeeCompanyInfo employeeCompanyInfo);

        List<EmployeeCompanyInfo> GetEmployeeCompanyInfosByEmployeeGuidId(EmployeeCompanyInfo employeeCompanyInfo);

        int SaveEmployeeCompanyInfo(EmployeeCompanyInfo employeeCompanyInfo);

        int EditEmployeeCompanyInfo(EmployeeCompanyInfo employeeCompanyInfo);

        int DeleteEmployeeCompanyInfo(EmployeeCompanyInfo employeeCompanyInfo);

        EmployeeCompanyInfo GetEmployeeCompanyInfoById(Guid? employeeId, int id);

        bool CheckExistingEmployeeCompanyInfo(Model.Custom.EmployeeCompanyInfoCustomModel employeeCompanyInfo);

        int UpdateEmployeeCompanyInfoDate(EmployeeCompanyInfo employeeCompanyInfo);

        EmployeeCompanyInfo GetEmployeeLatestJobInfoByEmployeeId(EmployeeCompanyInfo employeeCompanyInfo);

        List<VEmployeeCompanyInfoDetail> AutocompliteGetEmployeeInfo(string employeeCardId, int? branchUnitDepartmentId);

        List<EmployeeCompanyInfoModel> GetEmployeesLatestCompanyInfo(int startPage, int pageSize, EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel);

        List<EmployeeCompanyInfoModel> GetEmployeeIndividualHoliday(int startPage, int pageSize, EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel);

        List<EmployeeCompanyInfoModel> GetEmployeesForIndividualHoliday(EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel);

        List<EmployeeCompanyInfoModel> GetEmployeesForAssigingLine(EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel);

        int AssignBulkEmployeeLine(SearchFieldModel searchFieldModel, EmployeeCompanyInfoModel model);

        Company GetCompanyRefIdByEmployeeId(Guid? compId);

        VEmployeeCompanyInfoDetail GetEmployeeByEmployeeCardId(string employeeCardId);

        List<EmployeeCompanyInfoModel> GetEmployeesForAssigingJobType(EmployeeCompanyInfoModel model, SearchFieldModel searchFieldModel);

        int AssignBulkEmployeeJobType(SearchFieldModel searchFieldModel, EmployeeCompanyInfoModel model);

        VEmployeeCompanyInfoDetail GetEmployeeByEmployeeId(Guid employeeId);

        int AssignIndividualHoliday(SearchFieldModel searchFieldModel, EmployeeCompanyInfoModel model);

        int AssignIndividualWeekend(SearchFieldModel searchFieldModel, EmployeeCompanyInfoModel model);
    }
}
