using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface IOmSizeManager
    {
        object SizeAutoComplite(string serachString,string typeId);
        List<OM_Size> GetOmSizesByPaging(OM_Size model, out int totalRecords);
        OM_Size GetOmSizeById(int sizeId);
        string GetNewOmSizeRefId();
        int EditOmSize(OM_Size model);
        int SaveOmSize(OM_Size model);
        int DeleteOmSize(string sizeRefId);

        bool CheckSizeExist(OM_Size size);
        List<OM_Size> GetOmSizes();
    }
}
