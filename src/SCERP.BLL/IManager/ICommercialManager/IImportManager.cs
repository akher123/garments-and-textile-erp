using SCERP.Model.CommercialModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.IManager.ICommercialManager
{
    public interface  IImportManager
    {
        List<CommImport> GetAllImportsByPaging(out int totalRecords, CommImport model);

        List<CommImport> GetAllImports();

        CommImport GetImportById(int? id);
        List<CommImport> GetImportByLcId(int? id);
        int SaveImport(CommImport model);

        int EditImport(CommImport model);

        int DeleteImport(CommImport model);

        bool CheckExistingImport(CommImport model);

        CommImport GetExportByLcNo(string lcNo);
        
    }
}
