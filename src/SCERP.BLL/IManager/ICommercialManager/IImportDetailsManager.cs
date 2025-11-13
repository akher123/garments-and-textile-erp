using SCERP.Model.CommercialModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.IManager.ICommercialManager
{
    public interface IImportDetailsManager
    {
        List<CommImportDetails> GetAllImportsDetailsByPaging(out int totalRecords, CommImportDetails model);

        List<CommImportDetails> GetAllImportDetails();

        CommImportDetails GetImportDetailsById(int? id);
        List<CommImportDetails> GetImportDetailsByLcId(int? id);
        int SaveImportDetails(CommImportDetails model);

        int EditImportDetails(CommImportDetails model);

        int DeleteImportDetails(CommImportDetails model);

        bool CheckExistingImportDetails(CommImportDetails model);

        
        
    }
}
