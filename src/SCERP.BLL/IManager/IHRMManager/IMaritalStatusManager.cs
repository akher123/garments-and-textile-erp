using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Linq;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IMaritalStatusManager
    {
        List<MaritalState> GetAllMaritalStatusesByPaging(int startPage, int pageSize, out int totalRecords, MaritalState maritalStaus);

        MaritalState GetMaritalStatusById(int maritalStateId);

        int SaveMaritalStatus(MaritalState maritalStatus);

        int EditMaritalStatus(MaritalState maritalStatus);

        int DeleteMaritalStatus(MaritalState maritalStatus);

        bool CheckExistingMaritalStatus(MaritalState maritalStatus);

        List<MaritalState> GetMaritalStatusBySearchKey(string searchKey);
        List<MaritalState> GetAllMaritalStatuses();
    }
}
