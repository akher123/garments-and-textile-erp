using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.DAL.IRepository.IHRMRepository
{
    public interface IMaritalStatusRepository:IRepository<MaritalState>
    {
        MaritalState GetMaritalStatusById(int maritalStateId);
        List<MaritalState> GetAllMaritalStatusesByPaging(int startPage, int pageSize, out int totalRecords, MaritalState maritalState);
        List<MaritalState> GetAllMaritalStatuses();
    }
}
